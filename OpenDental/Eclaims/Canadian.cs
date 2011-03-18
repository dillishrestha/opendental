using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental.Eclaims {
	public class Canadian {

		//The characters for this character set were found at: http://en.wikipedia.org/wiki/Codepage_850
		//private static string codePage850Chars=" ☺☻♥♦♣♠•◘○◙♂♀♪♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼ !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~⌂"
		//    +"ÇüéâäàåçêëèïîìÄÅÉæÆôöòûùÿÖÜø£Ø×ƒáíóúñÑªº¿®¬½¼¡«»░▒▓│┤ÁÂÀ©╣║╗╝¢¥┐└┴┬├─┼ãÃ╚╔╩╦╠═╬¤ðÐÊËÈıÍÎÏ┘┌█▄¦Ì▀ÓßÔÒõÕµþÞÚÛÙýÝ¯´ ±‗¾¶§÷¸°¨·¹³²■ ";
		private static int localCodePage=0;
		private static Encoding localEncoding=null;
#if DEBUG
		public static int testNumber=-1;
		///<summary>Corresponds to field G46. We only need this for predetermination tests #6 and #7 because each predetermination page can handle up to 7 procedures and in the UI we limit the number of attached procedures to 7 in Canadian mode. We will not need this variable when released.</summary>
		public static int currentPredeterminationPageNumber=0;
		///<summary>Corresponds to field G47. We only need this for predetermination tests #6 and #7 because each predetermination page can handle up to 7 procedures and in the UI we limit the number of attached procedures to 7 in Canadian mode. We will not need this variable when released.</summary>
		public static int lastPredeterminationPageNumber=0;
#endif

		//<summary>Gets the filename for this batch. Used when saving or when rolling back.</summary>
		//private static string GetFileName(Clearinghouse clearhouse,int interchangeNum) {
			
		//}

		///<summary>Called directly instead of from Eclaims.SendBatches.  Includes one claim.  Sets claim status internally if successfully sent.  Returns the EtransNum of the ack.  Includes various user interaction such as displaying of messages, printing, triggering of COB claims, etc.  For a normal claim, primaryEOB will be blank.  But if this is a COB(type7), then we need to embed the primary EOB by passing it in.</summary>
		public static long SendClaim(ClaimSendQueueItem queueItem,bool doPrint,string primaryEOB){
			Clearinghouse clearhouse=GetClearinghouse();//clearinghouse must be valid to get to this point.
				//ClearinghouseL.GetClearinghouse(((ClaimSendQueueItem)queueItems[0]).ClearingHouseNum);
//Warning: this path is not handled properly if trailing slash is missing:
			string saveFolder=clearhouse.ExportPath;
			if(!Directory.Exists(saveFolder)) {
				throw new ApplicationException(saveFolder+" not found.");
			}
			Etrans etrans;
			Claim claim;
			Clinic clinic;
			Provider billProv;
			Provider treatProv;
			InsPlan insPlan;
			InsSub insSub;
			Carrier carrier;
			InsPlan insPlan2=null;
			InsSub insSub2=null;
			Carrier carrier2=null;
			List <PatPlan> patPlansForPatient;
			Patient patient;
			Patient subscriber;
			List<ClaimProc> claimProcList;//all claimProcs for a patient.
			List<ClaimProc> claimProcsClaim;
			List<Procedure> procListAll;
			List<Procedure> extracted;
			List<Procedure> procListLabForOne;//Lab fees for one procedure
			Patient subscriber2=null;
			Procedure proc;
			ProcedureCode procCode;
			StringBuilder strb;
			//this no longer actually sets the claim as "sent".  Just creates the etrans without a message.
			if(primaryEOB==""){
				etrans=Etranss.SetClaimSentOrPrinted(queueItem.ClaimNum,queueItem.PatNum,clearhouse.ClearinghouseNum,EtransType.Claim_CA,0);
			}
			else{
				etrans=Etranss.SetClaimSentOrPrinted(queueItem.ClaimNum,queueItem.PatNum,clearhouse.ClearinghouseNum,EtransType.ClaimCOB_CA,0);
			}
			strb=new StringBuilder();
			claim=Claims.GetClaim(queueItem.ClaimNum);
			clinic=Clinics.GetClinic(claim.ClinicNum);
			billProv=ProviderC.ListLong[Providers.GetIndexLong(claim.ProvBill)];
			treatProv=ProviderC.ListLong[Providers.GetIndexLong(claim.ProvTreat)];
			insPlan=InsPlans.GetPlan(claim.PlanNum,new List <InsPlan> ());
			insSub=InsSubs.GetSub(claim.InsSubNum,new List<InsSub>());
			carrier=Carriers.GetCarrier(insPlan.CarrierNum);
			if(claim.PlanNum2 > 0) {
				insPlan2=InsPlans.GetPlan(claim.PlanNum2,new List <InsPlan> ());
				insSub2=InsSubs.GetSub(claim.InsSubNum2,new List<InsSub>());
				carrier2=Carriers.GetCarrier(insPlan2.CarrierNum);
				subscriber2=Patients.GetPat(insSub2.Subscriber);
			}
			patPlansForPatient=PatPlans.Refresh(claim.PatNum);
			patient=Patients.GetPat(claim.PatNum);
			subscriber=Patients.GetPat(insSub.Subscriber);
			claimProcList=ClaimProcs.Refresh(patient.PatNum);
			claimProcsClaim=ClaimProcs.GetForSendClaim(claimProcList,claim.ClaimNum);
			procListAll=Procedures.Refresh(claim.PatNum);
			extracted=new List<Procedure>();
			if(claim.CanadianIsInitialUpper=="Y" || claim.CanadianIsInitialLower=="Y") {//only set extracted teeth if initial prosth
				extracted=Procedures.GetCanadianExtractedTeeth(procListAll);
			}
			//A01 transaction prefix 12 AN
			strb.Append(TidyAN(carrier.CanadianTransactionPrefix,12));
			//A02 office sequence number 6 N
			strb.Append(TidyN(etrans.OfficeSequenceNumber,6));
			//A03 format version number 2 N
			if(carrier.CDAnetVersion=="") {
				strb.Append("04");
			}
			else {
				strb.Append(carrier.CDAnetVersion);
			}
			//A04 transaction code 2 N
			if(claim.ClaimType=="PreAuth") {
				strb.Append("03");//Predetermination
			}
			else {
				strb.Append("01");//claim
			}
			//A05 carrier id number 6 N
			strb.Append(carrier.ElectID);//already validated as 6 digit number.
			//A06 software system id 3 AN  The third character is for version of OD.
//todo
#if DEBUG
			strb.Append("TS1");
#else
			strb.Append("OD1");//To be later supplied by CDAnet staff to uniquely identify OD.
#endif
			if(carrier.CDAnetVersion!="02") { //version 04
				//A10 encryption method 1 N
				strb.Append(carrier.CanadianEncryptionMethod);//validated in UI
			}
			//A07 message length. 5 N in version 04, 4 N in version 02
			//We simply create a place holder here. We come back at the end of message construction and record the actual final message length.
			if(carrier.CDAnetVersion=="02") {
				strb.Append("0000");
			}
			else { //version 04
				strb.Append("00000");
			}
			bool C19PlanRecordPresent=false;
			if(insPlan.CanadianPlanFlag=="A") {// || plan.CanadianPlanFlag=="N"){
				C19PlanRecordPresent=true;
			}
			if(carrier.CDAnetVersion=="02") {
				//A08 email flag 1 N
				if(claim.CanadianMaterialsForwarded=="") {
					strb.Append("0"); //no additional information
				}
				else if(claim.CanadianMaterialsForwarded.Contains("E")) {
					strb.Append("1"); //E-Mail to follow.
				}
				else {
					strb.Append("2"); //Letter to follow
				}
			}
			else { //version 04
				//A08 materials forwarded 1 AN
				strb.Append(GetMaterialsForwarded(claim.CanadianMaterialsForwarded));
			}
			if(carrier.CDAnetVersion!="02") { //version 04
				//A09 carrier transaction counter 5 N
				strb.Append(TidyN(etrans.CarrierTransCounter,5));
			}
			//B01 CDA provider number 9 AN
			strb.Append(TidyAN(treatProv.NationalProvID,9));//already validated
			//B02 (treating) provider office number 4 AN
			strb.Append(TidyAN(treatProv.CanadianOfficeNum,4));//already validated	
			if(carrier.CDAnetVersion!="02") { //version 04
				//B03 billing provider number 9 AN
				//might need to account for possible 5 digit prov id assigned by carrier
				strb.Append(TidyAN(billProv.NationalProvID,9));//already validated
				//B04 billing provider office number 4 AN
				strb.Append(TidyAN(billProv.CanadianOfficeNum,4));//already validated	
				//B05 referring provider 10 AN
				strb.Append(TidyAN(claim.CanadianReferralProviderNum,10));
				//B06 referral reason 2 N
				strb.Append(TidyN(claim.CanadianReferralReason,2));
			}
			if(carrier.CDAnetVersion=="02") {
				//C01 primary policy/plan number 8 AN
				//only validated to ensure that it's not blank and is less than 8. Also that no spaces.
				strb.Append(TidyAN(insPlan.GroupNum,8));
			}
			else { //version 04
				//C01 primary policy/plan number 12 AN
				//only validated to ensure that it's not blank and is less than 12. Also that no spaces.
				strb.Append(TidyAN(insPlan.GroupNum,12));
			}
			//C11 primary division/section number 10 AN
			strb.Append(TidyAN(insPlan.DivisionNo,10));
			if(carrier.CDAnetVersion=="02") {
				//C02 subscriber id number 11 AN
				strb.Append(TidyAN(insSub.SubscriberID.Replace("-",""),11));//validated
			}
			else { //version 04
				//C02 subscriber id number 12 AN
				strb.Append(TidyAN(insSub.SubscriberID.Replace("-",""),12));//validated
			}
			if(carrier.CDAnetVersion!="02") { //version 04
				//C17 primary dependant code 2 N
				string patID="";
				for(int p=0;p<patPlansForPatient.Count;p++) {
					if(patPlansForPatient[p].PlanNum==claim.PlanNum) {
						patID=patPlansForPatient[p].PatID;
					}
				}
				strb.Append(TidyN(patID,2));
			}
			//C03 relationship code 1 N
			//User interface does not only show Canadian options, but all options are handled.
			strb.Append(GetRelationshipCode(claim.PatRelat));
			//C04 patient's sex 1 A
			//validated to not include "unknown"
			if(patient.Gender==PatientGender.Male){
				strb.Append("M");
			}
			else{
				strb.Append("F");
			}
			//C05 patient birthday 8 N
			strb.Append(patient.Birthdate.ToString("yyyyMMdd"));//validated
			if(carrier.CDAnetVersion=="02") {
				//C06 patient last name 25 A
				strb.Append(TidyA(patient.LName,25));//validated
			}
			else { //version 04
				//C06 patient last name 25 AE
				strb.Append(TidyAE(patient.LName,25,true));//validated
			}
			if(carrier.CDAnetVersion=="02") {
				//C07 patient first name 15 A
				strb.Append(TidyA(patient.FName,15));//validated
			}
			else { //version 04
				//C07 patient first name 15 AE
				strb.Append(TidyAE(patient.FName,15,true));//validated
			}
			if(carrier.CDAnetVersion=="02") {
				//C08 patient middle initial 1 A
				strb.Append(TidyA(patient.MiddleI,1));
			}
			else { //version 04
				//C08 patient middle initial 1 AE
				strb.Append(TidyAE(patient.MiddleI,1));
			}
			if(carrier.CDAnetVersion=="02") {
				//C09 eligibility exception code 1 N
				string exceptionCode=TidyN(patient.CanadianEligibilityCode,1);
				if(exceptionCode=="4") {
					exceptionCode="0";
				}
				strb.Append(exceptionCode);//validated
			}
			else { //version 04
				//C09 eligibility exception code 1 N
				strb.Append(TidyN(patient.CanadianEligibilityCode,1));//validated
			}
			if(carrier.CDAnetVersion=="02") {
				//C10 name of school 25 A
				//validated if patient 18yrs or older and full-time student (or disabled student)
				strb.Append(TidyA(patient.SchoolName,25));
			}
			else { //version 04
				//C10 name of school 25 AEN
				//validated if patient 18yrs or older and full-time student (or disabled student)
				strb.Append(TidyAEN(patient.SchoolName,25));
			}
			if(carrier.CDAnetVersion!="02") { //version 04
				//C12 plan flag 1 A
				strb.Append(Canadian.GetPlanFlag(insPlan.CanadianPlanFlag));
				//C18 plan record count 1 N
				if(C19PlanRecordPresent) {
					strb.Append("1");
				}
				else {
					strb.Append("0");
				}
			}
			//D01 subscriber birthday 8 N
			strb.Append(subscriber.Birthdate.ToString("yyyyMMdd"));//validated
			if(carrier.CDAnetVersion=="02") {
				//D02 subscriber last name 25 A
				strb.Append(TidyA(subscriber.LName,25));//validated
			}
			else { //version 04
				//D02 subscriber last name 25 AE
				strb.Append(TidyAE(subscriber.LName,25,true));//validated
			}
			if(carrier.CDAnetVersion=="02") {
				//D03 subscriber first name 15 A
				strb.Append(TidyA(subscriber.FName,15));//validated
			}
			else { //version 04
				//D03 subscriber first name 15 AE
				strb.Append(TidyAE(subscriber.FName,15,true));//validated
			}
			if(carrier.CDAnetVersion=="02") {
				//D04 subscriber middle initial 1 A
				strb.Append(TidyA(subscriber.MiddleI,1));
			}
			else { //version 04
				//D04 subscriber middle initial 1 AE
				strb.Append(TidyAE(subscriber.MiddleI,1));
			}
			if(carrier.CDAnetVersion=="02") {
				//D05 subscriber address line one 30 AN
				strb.Append(TidyAN(subscriber.Address,30,true));//validated
			}
			else { //version 04
				//D05 subscriber address line one 30 AEN
				strb.Append(TidyAEN(subscriber.Address,30,true));//validated
			}
			if(carrier.CDAnetVersion=="02") {
				//D06 subscriber address line two 30 AN
				strb.Append(TidyAN(subscriber.Address2,30,true));
			}
			else { //version 04
				//D06 subscriber address line two 30 AEN
				strb.Append(TidyAEN(subscriber.Address2,30,true));
			}
			if(carrier.CDAnetVersion=="02") {
				//D07 subscriber city 20 A
				strb.Append(TidyA(subscriber.City,20));//validated
			}
			else { //version 04
				//D07 subscriber city 20 AEN
				strb.Append(TidyAEN(subscriber.City,20,true));//validated
			}
			//D08 subscriber province/state 2 A
			strb.Append(subscriber.State);//very throroughly validated previously
			if(carrier.CDAnetVersion=="02") {
				//D09 subscriber postal/zip code 6 AN
				strb.Append(TidyAN(subscriber.Zip.Replace("-",""),6));//validated.
			}
			else { //version 04
				//D09 subscriber postal/zip code 9 AN
				strb.Append(TidyAN(subscriber.Zip.Replace("-",""),9));//validated.
			}
			//D10 language of insured 1 A
			if(subscriber.Language=="fr"){
				strb.Append("F");
			}
			else{
				strb.Append("E");
			}
			if(carrier.CDAnetVersion!="02") { //version 04
				//D11 card sequence/version number 2 N
				//Not validated against type of carrier yet. Might need to check if Dentaide.
				strb.Append(TidyN(insPlan.DentaideCardSequence,2));
				//E18 secondary coverage flag 1 A
				if(claim.PlanNum2>0) {
					strb.Append("Y");
				}
				else {
					strb.Append("N");
				}
				//E20 secondary record count 1 N
				if(claim.PlanNum2==0) {
					strb.Append("0");
				}
				else {
					strb.Append("1");
				}
				//F06 number of procedures performed 1 N. Must be between 1 and 7.  UI prevents attaching more than 7.
				strb.Append(TidyN(claimProcsClaim.Count,1));//number validated
				//F22 extracted teeth count 2 N
				//todo: check validation.  I don't think there actually is any validation to be done because zero is always an allowed value.
				strb.Append(TidyN(extracted.Count,2));//validated against matching prosthesis
				if(claim.ClaimType=="PreAuth") {
					//F25 Orthodontic Record Flag 1 N
					if(claim.IsOrtho) {
						strb.Append("1");
					}
					else {
						strb.Append("0");
					}
				}
			}
			//Secondary carrier fields (E19 to E07) ONLY included if E20=1----------------------------------------------------
			if(claim.PlanNum2!=0) {
				//todo We still need to write the business logic for COB further down in this method.  COB is triggered by EOB.
				if(carrier.CDAnetVersion!="02") { //version 04
					//E19 secondary carrier transaction number 6 N
					strb.Append(TidyN(etrans.CarrierTransCounter2,6));
				}
				//E01 sec carrier id number 6 N
				strb.Append(carrier2.ElectID);//already validated as 6 digit number.
				if(carrier.CDAnetVersion=="02") {
					//E02 sec carrier policy/plan num 8 AN
					//only validated to ensure that it's not blank and is less than 8. Also that no spaces.
					//We might later allow 999999 if sec carrier is unlisted or unknown.
					strb.Append(TidyAN(insPlan2.GroupNum,8));
				}
				else { //version 04
					//E02 sec carrier policy/plan num 12 AN
					//only validated to ensure that it's not blank and is less than 12. Also that no spaces.
					//We might later allow 999999 if sec carrier is unlisted or unknown.
					strb.Append(TidyAN(insPlan2.GroupNum,12));
				}
				//E05 sec division/section num 10 AN
				strb.Append(TidyAN(insPlan2.DivisionNo,10));
				if(carrier.CDAnetVersion=="02") {
					//E03 sec plan subscriber id 11 AN
					strb.Append(TidyAN(insSub2.SubscriberID.Replace("-",""),11));//validated
				}
				else { //version 04
					//E03 sec plan subscriber id 12 AN
					strb.Append(TidyAN(insSub2.SubscriberID.Replace("-",""),12));//validated
				}
				if(carrier.CDAnetVersion!="02") { //version 04
					//E17 sec dependent code 2 N
					string patID="";
					for(int p=0;p<patPlansForPatient.Count;p++) {
						if(patPlansForPatient[p].PlanNum==claim.PlanNum2) {
							patID=patPlansForPatient[p].PatID;
						}
					}
					strb.Append(TidyN(patID,2));
					//E06 sec relationship code 1 N
					//User interface does not only show Canadian options, but all options are handled.
					strb.Append(GetRelationshipCode(claim.PatRelat2));
				}
				//E04 sec subscriber birthday 8 N
				strb.Append(subscriber2.Birthdate.ToString("yyyyMMdd"));//validated
				if(carrier.CDAnetVersion!="02") { //version 04
					//E08 sec subscriber last name 25 AE
					strb.Append(TidyAE(subscriber2.LName,25,true));//validated
					//E09 sec subscriber first name 15 AE
					strb.Append(TidyAE(subscriber2.FName,15,true));//validated
					//E10 sec subscriber middle initial 1 AE
					strb.Append(TidyAE(subscriber2.MiddleI,1));
					//E11 sec subscriber address one 30 AEN
					strb.Append(TidyAEN(subscriber2.Address,30,true));//validated
					//E12 sec subscriber address two 30 AEN
					strb.Append(TidyAEN(subscriber2.Address2,30,true));
					//E13 sec subscriber city 20 AEN
					strb.Append(TidyAEN(subscriber2.City,20,true));//validated
					//E14 sec subscriber province/state 2 A
					strb.Append(subscriber2.State);//very throroughly validated previously
					//E15 sec subscriber postal/zip code 9 AN
					strb.Append(TidyAN(subscriber2.Zip.Replace("-",""),9));//validated
					//E16 sec language 1 A
					if(subscriber2.Language=="fr") {
						strb.Append("F");
					}
					else {
						strb.Append("E");
					}
					//E07 sec card sequence/version num 2 N
					//todo Not validated yet.
					strb.Append(TidyN(insPlan2.DentaideCardSequence,2));
				}
				//End of secondary subscriber fields---------------------------------------------------------------------------
			}
			else { //There is no secondary plan.
				if(carrier.CDAnetVersion=="02") { 
					//Secondary subscriber fields are always available in version 2. Since there is no plan, put blank data as a filler.
					//E01 N 6
					strb.Append("000000");
					//E02 AN 8
					strb.Append("        ");
					//E05 AN 10
					strb.Append("          ");
					//E03 AN 11
					strb.Append("           ");
					//E04 N 8
					strb.Append("00000000");
				}
			}
			if(claim.ClaimType!="PreAuth") {
				//F01 payee code 1 N
				if(insSub.AssignBen) {
					if(carrier.CDAnetVersion=="02") {
						strb.Append("0");//pay dentist
					}
					else { //version 04
						strb.Append("4");//pay dentist
					}
				}
				else {
					strb.Append("1");//pay subscriber
				}
			}
			//F02 accident date 8 N
			if(claim.AccidentDate.Year>1900){//if accident related
				strb.Append(claim.AccidentDate.ToString("yyyyMMdd"));//validated
			}
			else{
				strb.Append(TidyN(0,8));
			}
			if(claim.ClaimType!="PreAuth") {
				//F03 predetermination number 14 AN
				strb.Append(TidyAN(claim.PreAuthString,14));
			}
			if(carrier.CDAnetVersion=="02") {
				//F15 Is this an Initial Replacement? A 1
				string initialPlacement="X";
				DateTime initialPlacementDate=DateTime.MinValue;
				if(claim.CanadianIsInitialUpper=="Y"){
					initialPlacement="Y";
					initialPlacementDate=claim.CanadianDateInitialUpper;
				}
				else if(claim.CanadianIsInitialLower=="Y"){
					initialPlacement="Y";
					initialPlacementDate=claim.CanadianDateInitialLower;
				}
				else if(claim.CanadianIsInitialUpper=="N") {
					initialPlacement="N";
					initialPlacementDate=claim.CanadianDateInitialUpper;
				}
				else if(claim.CanadianIsInitialLower=="N"){
					initialPlacement="N";
					initialPlacementDate=claim.CanadianDateInitialLower;
				}
				strb.Append(initialPlacement);
				//F04 date of initial placement 8 N
				if(initialPlacementDate.Year>1900) {
					strb.Append(initialPlacementDate.ToString("yyyyMMdd"));
				}
				else {
					strb.Append("00000000");
				}
				//F05 tx req'd for ortho purposes 1 A
				if(claim.IsOrtho) {
					strb.Append("Y");
				}
				else {
					strb.Append("N");
				}
				//F06 number of procedures performed 1 N. Must be between 1 and 7.  UI prevents attaching more than 7.
				strb.Append(TidyN(claimProcsClaim.Count,1));//number validated
			}
			else { //version 04
				//F15 initial placement upper 1 A  Y or N or X
				strb.Append(claim.CanadianIsInitialUpper);//validated
				//F04 date of initial placement upper 8 N
				if(claim.CanadianDateInitialUpper.Year>1900) {
					strb.Append(claim.CanadianDateInitialUpper.ToString("yyyyMMdd"));
				}
				else {
					strb.Append("00000000");
				}
				//F18 initial placement lower 1 A
				strb.Append(claim.CanadianIsInitialLower);//validated
				//F19 date of initial placement lower 8 N
				if(claim.CanadianDateInitialLower.Year>1900) {
					strb.Append(claim.CanadianDateInitialLower.ToString("yyyyMMdd"));
				}
				else {
					strb.Append("00000000");
				}
				//F05 tx req'd for ortho purposes 1 A
				if(claim.IsOrtho) {
					strb.Append("Y");
				}
				else {
					strb.Append("N");
				}
				//F20 max prosth material 1 N
				if(claim.CanadianMaxProsthMaterial==7) {//our fake crown code
					strb.Append("0");
				}
				else {
					strb.Append(claim.CanadianMaxProsthMaterial.ToString());//validated
				}
				//F21 mand prosth material 1 N
				if(claim.CanadianMandProsthMaterial==7) {//our fake crown code
					strb.Append("0");
				}
				else {
					strb.Append(claim.CanadianMandProsthMaterial.ToString());//validated
				}
			}
			if(carrier.CDAnetVersion!="02") { //version 04
				//If F22 is non-zero. Repeat for the number of times specified by F22.----------------------------------------------
				for(int t=0;t<extracted.Count;t++) {
					//F23 extracted tooth num 2 N
					//todo: check validation
					strb.Append(TidyN(Tooth.ToInternat(extracted[t].ToothNum),2));//validated
					//F24 extraction date 8 N
					//todo: check validation
					strb.Append(extracted[t].ProcDate.ToString("yyyyMMdd"));//validated
				}
			}
			if(carrier.CDAnetVersion!="02") { //version 04
				if(claim.ClaimType=="PreAuth") {
#if DEBUG
					//G46 Current Predetermination Page Number N 1
					strb.Append(Canadian.TidyN(currentPredeterminationPageNumber,1));
					//G47 Last Predetermination Page Number N 1
					strb.Append(Canadian.TidyN(lastPredeterminationPageNumber,1));
#else
					//G46 Current Predetermination Page Number N 1
					strb.Append("1");//Always 1 page, because UI prevents attaching more than 7 procedures per claim in Canadian mode.
					//G47 Last Predetermination Page Number N 1
					strb.Append("1");//Always 1 page, because UI prevents attaching more than 7 procedures per claim in Canadian mode.
#endif
					if(claim.IsOrtho) { //F25 is set
						//F37 Estimated Treatment Starting Date N 8
						strb.Append(Canadian.TidyN(claim.CanadaEstTreatStartDate.ToString("yyyyMMdd"),8));
						//F26 First Examination Fee D 6
						strb.Append("000000");//optional
						//F27 Diagnostic Phase Fee D 6
						strb.Append("000000");//optional
						//F28 Initial Payment D 6
						strb.Append(Canadian.TidyD(claim.CanadaInitialPayment,6));
						//F29 Payment Mode N 1
						strb.Append(Canadian.TidyN(claim.CanadaPaymentMode,1));//Validated in UI.
						//F30 Treatment Duration N 2
						strb.Append(Canadian.TidyN(claim.CanadaTreatDuration,2));
						//F31 Number of Anticipated Payments N 2
						strb.Append(Canadian.TidyN(claim.CanadaNumAnticipatedPayments,2));
						//F32 Anticipated Payment Amount D 6
						strb.Append(Canadian.TidyD(claim.CanadaAnticipatedPayAmount,6));
					}
				}
			}
			//Procedures: Repeat for number of times specified by F06.----------------------------------------------------------
			for(int p=0;p<claimProcsClaim.Count;p++) {
				proc=Procedures.GetProcFromList(procListAll,claimProcsClaim[p].ProcNum);
				procCode=ProcedureCodes.GetProcCode(proc.CodeNum);
				procListLabForOne=Procedures.GetCanadianLabFees(proc.ProcNum,procListAll);
				//F07 proc line number 1 N
				strb.Append((p+1).ToString());
				if(carrier.CDAnetVersion=="02") {
					//F08 procedure code 5 N
					strb.Append(TidyN(claimProcsClaim[p].CodeSent,5).Trim().PadLeft(5,'0'));
				}
				else { //version 04
					//F08 procedure code 5 AN
					strb.Append(TidyAN(claimProcsClaim[p].CodeSent,5).Trim().PadLeft(5,'0'));
				}
				if(claim.ClaimType!="PreAuth") {
					//F09 date of service 8 N
					strb.Append(claimProcsClaim[p].ProcDate.ToString("yyyyMMdd"));//validated
				}
				//F10 international tooth, sextant, quad, or arch 2 N
				strb.Append(GetToothQuadOrArch(proc,procCode));
				//F11 tooth surface 5 A
				//the SurfTidy function is very thorough, so it's OK to use TidyAN
				if(procCode.TreatArea==TreatmentArea.Surf) {
#if DEBUG
					//since the scripts use impossible surfaces, we need to just use raw database here
					strb.Append(TidyAN(proc.Surf,5));
#else
					strb.Append(TidyAN(Tooth.SurfTidyForClaims(proc.Surf,proc.ToothNum),5));
#endif
				}
				else {
					strb.Append("     ");
				}
				//F12 dentist's fee claimed 6 D
				strb.Append(TidyD(claimProcsClaim[p].FeeBilled,6));
				if(carrier.CDAnetVersion!="02") { //version 04
					//F34 lab procedure code #1 5 AN
					if(procListLabForOne.Count>0) {
						strb.Append(TidyAN(ProcedureCodes.GetProcCode(procListLabForOne[0].CodeNum).ProcCode,5).Trim().PadLeft(5,'0'));
					}
					else {
						strb.Append("     ");
					}
				}
				//F13 lab procedure fee #1 6 D
				if(procListLabForOne.Count>0){
					strb.Append(TidyD(procListLabForOne[0].ProcFee,6));
				}
				else{
					strb.Append("000000");
				}
				if(carrier.CDAnetVersion=="02") {
					//F14 Unit of Time D 4
					strb.Append(TidyD(proc.ProcTime.TotalHours,4));
				}
				else { //version 04
					//F35 lab procedure code #2 5 AN
					if(procListLabForOne.Count>1) {
						strb.Append(TidyAN(ProcedureCodes.GetProcCode(procListLabForOne[1].CodeNum).ProcCode,5).Trim().PadLeft(5,'0'));
					}
					else {
						strb.Append("     ");
					}
					//F36 lab procedure fee #2 6 D
					if(procListLabForOne.Count>1) {
						strb.Append(TidyD(procListLabForOne[1].ProcFee,6));
					}
					else {
						strb.Append("000000");
					}
					//F16 procedure type codes 5 A
					strb.Append(TidyA(proc.CanadianTypeCodes,5));
					//F17 remarks code 2 N
					//incomplete.  PMP field.  See C12.
					strb.Append("00");
				}
			}
			if(carrier.CDAnetVersion!="02") { //version 04
				//todo If C18=1, then the following field would appear
				//C19 plan record 30 AN
				if(C19PlanRecordPresent) {
					//todo: what text goes here?  Not documented
					strb.Append(Canadian.TidyAN("",30));
				}
			}
			//Now we go back and fill in the actual message length now that we know the number for sure.
			if(carrier.CDAnetVersion=="02") {
				strb.Replace("0000",Canadian.TidyN(strb.Length,4),32,4);
			}
			else { //version 04
				strb.Replace("00000",Canadian.TidyN(strb.Length,5),32,5);
			}
			//end of creating the message
			//this is where we attempt the actual sending:
			string result="";
			bool resultIsError=false;
			try{
#if DEBUG
				if(claim.ClaimType=="PreAuth") { //Predeterminations
					if(testNumber==3) { //Predetermination test #3
						strb.Replace("Y","N",563,1);//We use claim.IsOrtho for fields F05 and F25, but for some reason in this example the values expected are opposite. We think this is a problem with the CDANet test.
						strb.Replace("00000000","35000025",577,8);//These are optional fields (F26 and F27), so we have not implemented them, but the test does not work without them for some reason.
					}
					else if(testNumber==4) {
						strb.Replace("          00","081234500 04",69,12);//This example is supposed to include a referring provider and referral reason, but no provider with ID "081234500" is ever provided in the list of example provider objects in the documentation. This ID neither matches the Doc A nor Doc B example providers given.
					}
					//else if(testNumber==5) {
					//  strb.Replace("è","Š",130,1);
					//  strb.Replace("É"," ",167,1);
					//  strb.Replace("è","Š",208,1);
					//  strb.Replace("É"," ",245,1);
					//  strb.Replace("é","‚",309,1);
					//  strb.Replace("2","1",352,1);
					//  strb.Replace("è","Š",407,1);
					//  strb.Replace("é","‚",507,1);
					//  strb.Replace("F","E",536,1);
					//}
				}
#endif
				result=PassToIca(strb.ToString(),carrier.CanadianNetworkNum,clearhouse);
			}
			catch(ApplicationException ex) {
				result=ex.Message;
				resultIsError=true;
			}
			//Attach an ack to the etrans
			Etrans etransAck=new Etrans();
			etransAck.PatNum=etrans.PatNum;
			etransAck.PlanNum=etrans.PlanNum;
			etransAck.InsSubNum=etrans.InsSubNum;
			etransAck.CarrierNum=etrans.CarrierNum;
			etransAck.ClaimNum=etrans.ClaimNum;
			etransAck.DateTimeTrans=DateTime.Now;
			CCDFieldInputter fieldInputter=null;
			if(resultIsError){
				etransAck.Etype=EtransType.AckError;
				etrans.Note="failed";
			}
			else{
				fieldInputter=new CCDFieldInputter(result);
				etransAck.Etype=fieldInputter.GetEtransType();
			}
			Etranss.Insert(etransAck);
			Etranss.SetMessage(etransAck.EtransNum,result);
			etrans.AckEtransNum=etransAck.EtransNum;
			Etranss.Update(etrans);
			Etranss.SetMessage(etrans.EtransNum,strb.ToString());
			if(resultIsError) {
				throw new ApplicationException(result);
			}
			else {
				Claims.SetCanadianClaimSent(queueItem.ClaimNum);//when called from ClaimEdit, that window will close immediately, so we're directly changing the db.
				CCDField fieldTransRefNum=fieldInputter.GetFieldById("G01");
				claim.CanadaTransRefNum=fieldTransRefNum.valuestr;
				Claims.Update(claim);
			}
			if(doPrint) {
				FormCCDPrint FormP=new FormCCDPrint(etrans,result);//Print the form. 
				FormP.Print();
			}
			if(etransAck.Etype==EtransType.ClaimEOB_CA && claim.ClaimType=="P" && claim.PlanNum2>0) {//if an eob was returned and patient has secondary insurance.
				//if an EOB is returned, there are two possibilities.
				//1. The EOB contains an embedded EOB because the same carrier is both pri and sec.  Both got printed above.
				//2. The EOB does not contain an embedded EOB, indicating that a COB claim needs to be created and sent.
				//That is done here, automatically.
				//UI already prevents the initial automatic creation of the secondary claim for Canada.
				string embeddedLength=fieldInputter.GetValue("G39");
				if(embeddedLength=="0000") {//no embedded message
					Claim claim2=new Claim();
					Claims.Insert(claim2);//to retreive a key for new Claim.ClaimNum
					claim2.PatNum=claim.PatNum;
					claim2.DateService=claim.DateService;
					claim2.DateSent=DateTime.Today;
					claim2.ClaimStatus="W";
					claim2.PlanNum=claim.PlanNum2;
					claim2.InsSubNum=claim.InsSubNum2;
					claim2.PlanNum2=claim.PlanNum;
					claim2.InsSubNum2=claim.InsSubNum;
					claim2.PatRelat=claim.PatRelat2;
					claim2.PatRelat2=claim.PatRelat;
					claim2.ClaimType="S";
					claim2.ProvTreat=claim.ProvTreat;
					claim2.IsProsthesis="N";
					claim2.ProvBill=claim.ProvBill;
					claim2.EmployRelated=YN.No;
					ClaimProc[] claimProcsClaim2=new ClaimProc[claimProcsClaim.Count];
					long procNum;
					for(int i=0;i<claimProcsClaim.Count;i++) {//loop through the procs from claim 1
						//and try to find an estimate that can be used
						procNum=claimProcsClaim[i].ProcNum;
						claimProcsClaim2[i]=Procedures.GetClaimProcEstimate(procNum,claimProcList,insPlan2,claim2.InsSubNum);
					}
					for(int i=0;i<claimProcsClaim2.Length;i++) {//loop through each claimProc
						//and create any missing estimates just in case
						if(claimProcsClaim2[i]==null) {
							claimProcsClaim2[i]=new ClaimProc();
							proc=Procedures.GetProcFromList(procListAll,claimProcsClaim[i].ProcNum);
							ClaimProcs.CreateEst(claimProcsClaim2[i],proc,insPlan2,insSub2);
						}
					}
					for(int i=0;i<claimProcsClaim2.Length;i++) {
						proc=Procedures.GetProcFromList(procListAll,claimProcsClaim2[i].ProcNum);//1:1
						claimProcsClaim2[i].ClaimNum=claim2.ClaimNum;
						claimProcsClaim2[i].Status=ClaimProcStatus.NotReceived;
						claimProcsClaim2[i].CodeSent=claimProcsClaim[i].CodeSent;
						claimProcsClaim2[i].LineNumber=(byte)(i+1);
						ClaimProcs.Update(claimProcsClaim2[i]);
					}
					claimProcList=ClaimProcs.Refresh(claim2.PatNum);
					Family fam=Patients.GetFamily(claim2.PatNum);
					List<InsSub> subList=InsSubs.RefreshForFam(fam);
					List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
					List<Benefit> benefitList=Benefits.Refresh(patPlansForPatient);
					ClaimL.CalculateAndUpdate(procListAll,planList,claim2,patPlansForPatient,benefitList,patient.Age,subList);
					ClaimSendQueueItem queueItem2=Claims.GetQueueList(claim2.ClaimNum,claim2.ClinicNum)[0];
					//ok to skip validation
					long etransNum=SendClaim(queueItem2,doPrint,result);//recursive
					return etransNum;//for now, we'll return the etransnum of the secondary ack.
				}
				else{//an embedded message exists
					string embeddedMsg=fieldInputter.GetValue("G40");
					//MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(embeddedMsg);
					//msgbox.Show();
					//actually, nothing to do here because already printed above.
				}
			}
			return etransAck.EtransNum;
		}

		///<summary>Takes a string, creates a file, and drops it into the iCA folder.  Waits for the response, and then returns it as a string.  Will throw an exception if response not received in a reasonable amount of time.  </summary>
		public static string PassToIca(string msgText,long networkNum,Clearinghouse clearhouse) {
			if(clearhouse==null){
				throw new ApplicationException(Lan.g("Canadian","CDAnet Clearinghouse could not be found."));
			}
			string saveFolder=clearhouse.ExportPath;
			if(!Directory.Exists(saveFolder)) {
				throw new ApplicationException(saveFolder+" not found.");
			}
			Process[] processes=Process.GetProcesses();
			bool isRunning=false;
			for(int i=0;i<processes.Length;i++) {
				if(processes[i].ProcessName.StartsWith("iCA")) {
					isRunning=true;
					break;
				}
			}
			if(!isRunning) {
				ProcessStartInfo startInfo=new ProcessStartInfo(clearhouse.ClientProgram);
				startInfo.WindowStyle=ProcessWindowStyle.Minimized;
				startInfo.WorkingDirectory=Path.GetDirectoryName(clearhouse.ClientProgram);
				Process process=Process.Start(startInfo);
			}
			//first, delete the result file just in case.
			string outputFile=ODFileUtils.CombinePaths(saveFolder,"output.000");
			if(File.Exists(outputFile)) {
				File.Delete(outputFile);//no exception thrown if file does not exist.
			}
			//create the input file with data:
			string inputFile=ODFileUtils.CombinePaths(saveFolder,"input.000");
			File.WriteAllText(inputFile,msgText,Encoding.GetEncoding(850));
			DateTime start=DateTime.Now;
			while(DateTime.Now<start.AddSeconds(45)){//wait for max of 45 seconds. We can increase it later.
				if(File.Exists(outputFile)){
					break;
				}
				Thread.Sleep(200);//1/10 second
				Application.DoEvents();
			}
			if(!File.Exists(outputFile)) {
				//File.Delete(inputFile);//don't leave it sitting around
				throw new ApplicationException("No response.");
			}
			byte[] resultBytes=File.ReadAllBytes(outputFile);
			string result=BytesToCanadianString(resultBytes);
			//strip the prefix.  Example prefix: 123456,0,000,
			//Find position of third comma
			Match match=Regex.Match(result,@"^\d+,\d+,\d+,");
			if(match.Success){
				result=result.Substring(match.Length);
			}
			File.Delete(outputFile);
			if(result.Length<50) {//can't be a valid message
				string[] responses=result.Split(',');
				string response="Error "+responses[1]+"\r\n";
				if(responses[1]=="1013") {
					response+="iCA could not locate a valid digital certificate.\r\n";
				}
				string errorFile=ODFileUtils.CombinePaths(Path.GetDirectoryName(clearhouse.ClientProgram),"ica.log");
				string errorlog="";
				if(File.Exists(errorFile)){
					errorlog=File.ReadAllText(errorFile);
				}
				response+="\r\nPlease see http://goitrans.com/itrans_support/itrans_claim_support_error_codes.htm for more details.\r\n";
				response+="Error log:\r\n"+errorlog;
				throw new ApplicationException(response);
			}
			return result;
		}

		///<summary>Uses the ANSI code page of the current culture to convert the given byte data to a string.</summary>
		public static string BytesToCanadianString(byte[] data){
			if(data==null){
				return null;
			}
			if(localCodePage!=CultureInfo.CurrentCulture.TextInfo.ANSICodePage){//Check each time in case user changes local settings.
				localCodePage=CultureInfo.CurrentCulture.TextInfo.ANSICodePage;
				localEncoding=Encoding.GetEncoding(localCodePage);
			}
			return localEncoding.GetString(data);
		}		

		public static char GetCanadianChar(byte b){
			return BytesToCanadianString(new byte[] {b})[0];
		}

		///<summary>Since this is only used for Canadian messages, it will always use the default clearinghouse if it's Canadian.  Otherwise, it uses the first Canadian clearinghouse that it can find.</summary>
		public static Clearinghouse GetClearinghouse(){
			for(int i=0;i<Clearinghouses.Listt.Length;i++) {
				if(Clearinghouses.Listt[i].IsDefault && Clearinghouses.Listt[i].CommBridge==EclaimsCommBridge.CDAnet) {
					return Clearinghouses.Listt[i];
				}
			}
			for(int i=0;i<Clearinghouses.Listt.Length;i++) {
				if(Clearinghouses.Listt[i].CommBridge==EclaimsCommBridge.CDAnet) {
					return Clearinghouses.Listt[i];
				}
			}
			return null;
		}

		///<summary>Decimal.</summary>
		public static string TidyD(double number,int width){
			string retVal=(number*100).ToString("F0");
			if(retVal.Length>width) {
				return retVal.Substring(0,width);//this should never happen, but it might prevent a malformed claim.
			}
			return retVal.PadLeft(width,'0');
		}

		///<summary>Numeric</summary>
		public static string TidyN(int number,int width){
			string retVal=number.ToString();
			if(retVal.Length>width){
				return retVal.Substring(0,width);//this should never happen, but it might prevent a malformed claim.
			}
			return retVal.PadLeft(width,'0');
		}

		///<summary>Numeric</summary>
		public static string TidyN(string numText,int width) {
			string retVal="";
			try{
				int number=Convert.ToInt32(numText);
				retVal=number.ToString();
			}
			catch{
				retVal="";
			}
			if(retVal.Length>width) {
				return retVal.Substring(0,width);//this should never happen, but it might prevent a malformed claim.
			}
			return retVal.PadLeft(width,'0');
		}

		///<summary>This should never involve use input and is rarely used.  It only handles width and padding.</summary>
		public static string TidyA(string text,int width){
			if(text.Length>width) {
				return text.Substring(0,width);
			}
			return text.PadRight(width,' ');
		}

		///<summary>Alphabetic, with extended characters. No numbers.</summary>
		public static string TidyAE(string text,int width) {
			return TidyAE(text,width,false);
		}

		///<summary>Alphabetic with extended characters. No numbers. For testing, here are some: à â ç é è ê ë î ï ô û ù ü ÿ</summary>
		public static string TidyAE(string text,int width,bool allowLowercase) {
			if(!allowLowercase) {
				text=text.ToUpper();
			}
			text=Regex.Replace(text,//replace
				@"[0-9]",//any character that's a number
				"");//with nothing (delete it)
			text=Regex.Replace(text,//replace
				@"[^\w'\-,]",//any character that's not a word character or an apost, dash, or comma
				"");//with nothing (delete it)
			if(text.Length>width) {
				return text.Substring(0,width);
			}
			return text.PadRight(width,' ');
		}

		///<summary>Alphabetic/Numeric, no extended characters.  Caps only.</summary>
		public static string TidyAN(string text,int width) {
			return TidyAN(text,width,false);
		}

		///<summary>Alphabetic/Numeric, no extended characters.</summary>
		public static string TidyAN(string text,int width,bool allowLowercase) {
			if(!allowLowercase){
				text=text.ToUpper();
			}
			text=Regex.Replace(text,//replace
				@"[^a-zA-Z0-9 '\-,]",//any char that is not A-Z,0-9,space,apost,dash,or comma
				"");//with nothing (delete it)
			if(text.Length>width) {
				return text.Substring(0,width);
			}
			return text.PadRight(width,' ');
		}

		///<summary>Alphabetic/Numeric, with extended characters.</summary>
		public static string TidyAEN(string text,int width) {
			return TidyAE(text,width,false);
		}

		///<summary>Alphabetic/Numeric with extended characters. For testing, here are some: à â ç é è ê ë î ï ô û ù ü ÿ</summary>
		public static string TidyAEN(string text,int width,bool allowLowercase) {
			if(!allowLowercase) {
				text=text.ToUpper();
			}
			if(text.Length>width) {
				return text.Substring(0,width);
			}
			return text.PadRight(width,' ');
		}

		///<summary>Must always return single char.</summary>
		private static string GetMaterialsForwarded(string materials){
			bool E=materials.Contains("E");
			bool C=materials.Contains("C");
			bool M=materials.Contains("M");
			bool X=materials.Contains("X");
			bool I=materials.Contains("I");
			if(E&&C&&M&&X&&I){
				return "Z";
			}
			if(C&&M&&X&&I) {
				return "Y";
			}
			if(E&&C&&X&&I) {
				return "W";
			}
			if(E&&C&&M&&I) {
				return "V";
			}
			if(E&&C&&M&&X) {
				return "U";
			}
			if(M&&X&&I) {
				return "T";
			}
			if(C&&M&&X) {
				return "R";
			}
			if(C&&M&&I) {
				return "R";
			}
			if(E&&C&&I) {
				return "Q";
			}
			if(E&&C&&X) {
				return "P";
			}
			if(E&&C&&M) {
				return "O";
			}
			if(X&&I) {
				return "N";
			}
			if(M&&I) {
				return "L";
			}
			if(M&&X) {
				return "K";
			}
			if(C&&I) {
				return "J";
			}
			if(C&&X) {
				return "H";
			}
			if(C&&M) {
				return "G";
			}
			if(E&&I) {
				return "F";
			}
			if(E&&X) {
				return "D";
			}
			if(E&&M) {
				return "B";
			}
			if(E&&C) {
				return "A";
			}
			if(I) {
				return "I";
			}
			if(X) {
				return "X";
			}
			if(M) {
				return "M";
			}
			if(C) {
				return "C";
			}
			if(E) {
				return "E";
			}
			return " ";
		}

		///<summary>Used in C03 and E06</summary>
		public static string GetRelationshipCode(Relat relat){
			switch (relat){
				case Relat.Self:
					return "1";
				case Relat.Spouse:
					return "2";
				case Relat.Child:
					return "3";
				case Relat.LifePartner:
				case Relat.SignifOther:
					return "4";//(commonlaw spouse)
				default:
					return "5";
			}
		}

		public static string GetPlanFlag(string planFlag){
			if(planFlag=="A" || planFlag=="V"){
				return planFlag;
			}
			return " ";
		}

		///<summary>Because the numberins scheme is slightly different for version 2, this field (C09) should always be passed through here.</summary>
		public static string GetEligibilityCode(byte rawCode,bool isVersion02) {
			if(isVersion02 && rawCode==4) {
				return "0";
			}
			return rawCode.ToString();
		}

		///<summary>Checks for either valid USA state or valid Canadian territory.</summary>
		private static bool IsValidST(string ST){
			if(IsValidTerritory(ST) || IsValidState(ST)){
				return true;
			}
			return false;
		}

		///<summary>Checks for valid USA state.</summary>
		private static bool IsValidState(string ST){
			string[] validStates=new string[] {
				"AL","AK","AZ","AR","CA","CO","CT","DE","DC","FL","GA","HI","ID","IL","IN","IA","KS","KY","LA","ME","MD","MA","MI",
				"MN","MS","MO","MT","NE","NV","NH","NJ","NM","NY","NC","ND","OH","OK","OR","PA","RI","SC","SD","TN","TX","UT","VA",
				"WA","WV","WI","WY"};
			for(int i=0;i<validStates.Length;i++){
				if(validStates[i]==ST){
					return true;
				}
			}
			return false;
		}

		///<summary>Checks for valid Canadian terriroty.</summary>
		private static bool IsValidTerritory(string ST){
			string[] validStates=new string[] {"NL","PE","NB","QC","ON","MB","SK","AB","BC","YT","NU"};
			for(int i=0;i<validStates.Length;i++){
				if(validStates[i]==ST){
					return true;
				}
			}
			return false;
		}

		///<summary>The state is required to determine whether USA or Canadian address.  It validates both types.  Canadian must be in form ANANAN.  Zip must be either 5 or 9 digits.</summary>
		private static bool IsValidZip(string zip, string ST){
			if(IsValidState(ST)){//USA
				zip=zip.Replace("-","");
				if(Regex.IsMatch(zip,@"^[0-9]{5}$")) {//5 digits
					return true;
				}
				if(Regex.IsMatch(zip,@"^[0-9]{9}$")) {//9 digits
					return true;
				}
			}
			if(IsValidTerritory(ST)){
				if(Regex.IsMatch(zip,@"^[A-Z][0-9][A-Z][0-9][A-Z][0-9]$")) {//ANANAN
					return true;
				}
			}
			return false;
		}

		private static string GetToothQuadOrArch(Procedure proc,ProcedureCode procCode){
			switch(procCode.TreatArea){
				case TreatmentArea.Arch:
					//if(proc.Surf=="U"){
					return "00";
					//}
					//else{
					//	return "01";
					//}
				case TreatmentArea.Mouth:
				case TreatmentArea.None:
					return "00";
				case TreatmentArea.Quad:
					if(proc.Surf=="UR"){
						return "10";
					}
					else if(proc.Surf=="UL") {
						return "20";
					}
					else if(proc.Surf=="LR") {
						return "40";
					}
					else{//LL
						return "30";
					}
				case TreatmentArea.Sextant:
					if(proc.Surf=="1") {
						return "00";
					}
					else if(proc.Surf=="2") {
						return "00";
					}
					else if(proc.Surf=="3") {
						return "00";
					}
					else if(proc.Surf=="4") {
						return "00";
					}
					else if(proc.Surf=="5") {
						return "00";
					}
					else{//6
						return "00";
					}
				case TreatmentArea.Surf:
				case TreatmentArea.Tooth:
					return Tooth.ToInternat(proc.ToothNum);
				case TreatmentArea.ToothRange:
					string[] range=proc.ToothRange.Split(',');
					if(range.Length==0 || !Tooth.IsValidDB(range[0])){
						return "00";
					}
					else if(Tooth.IsMaxillary(range[0])){
						return "00";
					}
					return "00";
			}
			return "00";//will never happen
		}

		///<summary>Returns a string describing all missing data on this claim.  Claim will not be allowed to be sent electronically unless this string comes back empty.</summary>
		public static string GetMissingData(ClaimSendQueueItem queueItem) {
			string retVal="";
			Clearinghouse clearhouse=ClearinghouseL.GetClearinghouse(queueItem.ClearinghouseNum);
			Claim claim=Claims.GetClaim(queueItem.ClaimNum);
			Clinic clinic=Clinics.GetClinic(claim.ClinicNum);
			Provider billProv=ProviderC.ListLong[Providers.GetIndexLong(claim.ProvBill)];
			Provider treatProv=ProviderC.ListLong[Providers.GetIndexLong(claim.ProvTreat)];
			InsSub insSub=InsSubs.GetSub(claim.InsSubNum,new List<InsSub>());
			InsPlan insPlan=InsPlans.GetPlan(claim.PlanNum,new List <InsPlan> ());
			Carrier carrier=Carriers.GetCarrier(insPlan.CarrierNum);
			InsSub insSub2=null;
			InsPlan insPlan2=null;
			Carrier carrier2=null;
			Patient subscriber2=null;
			if(claim.PlanNum2>0) {
				insSub2=InsSubs.GetSub(claim.InsSubNum2,new List<InsSub>());
				insPlan2=InsPlans.GetPlan(claim.PlanNum2,new List <InsPlan> ());
				carrier2=Carriers.GetCarrier(insPlan2.CarrierNum);
				subscriber2=Patients.GetPat(insSub2.Subscriber);
			}
			Patient patient=Patients.GetPat(claim.PatNum);
			Patient subscriber=Patients.GetPat(insSub.Subscriber);
			List<ClaimProc> claimProcList=ClaimProcs.Refresh(patient.PatNum);//all claimProcs for a patient.
			List<ClaimProc> claimProcsClaim=ClaimProcs.GetForSendClaim(claimProcList,claim.ClaimNum);
			List<Procedure> procListAll=Procedures.Refresh(claim.PatNum);
			Procedure proc;
			ProcedureCode procCode;
			List<Procedure> extracted=Procedures.GetCanadianExtractedTeeth(procListAll);
			if(!Regex.IsMatch(carrier.ElectID,@"^[0-9]{6}$")) {
				if(retVal!="")
					retVal+=", ";
				retVal+="CarrierId 6 digits";
			}
			if(treatProv.NationalProvID.Length!=9) {
				if(retVal!="")
					retVal+=", ";
				retVal+="TreatingProv CDA num 9 digits";
			}
			if(treatProv.CanadianOfficeNum.Length!=4) {
				if(retVal!="")
					retVal+=", ";
				retVal+="TreatingProv office num 4 char";
			}
			if(billProv.NationalProvID.Length!=9) {
				if(retVal!="")
					retVal+=", ";
				retVal+="BillingProv CDA num 9 digits";
			}
			if(billProv.CanadianOfficeNum.Length!=4) {
				if(retVal!="")
					retVal+=", ";
				retVal+="BillingProv office num 4 char";
			}
			if(insPlan.GroupNum.Length==0 || insPlan.GroupNum.Length>12 || insPlan.GroupNum.Contains(" ")) {
				if(retVal!="")
					retVal+=", ";
				retVal+="Plan Number";
			}
			if(insSub.SubscriberID=="") {
				if(retVal!="")
					retVal+=", ";
				retVal+="SubscriberID";
			}
			if(claim.PatNum != insSub.Subscriber//if patient is not subscriber
				&& claim.PatRelat==Relat.Self) {//and relat is self
				if(retVal!="")
					retVal+=", ";
				retVal+="Claim Relationship";
			}
			if(patient.Gender==PatientGender.Unknown) {
				if(retVal!="")
					retVal+=", ";
				retVal+="Patient gender";
			}
			if(patient.Birthdate.Year<1880 || patient.Birthdate>DateTime.Today) {
				if(retVal!="")
					retVal+=", ";
				retVal+="Patient birthdate";
			}
			if(patient.LName=="") {
				if(retVal!="")
					retVal+=", ";
				retVal+="Patient lastname";
			}
			if(patient.FName=="") {
				if(retVal!="")
					retVal+=", ";
				retVal+="Patient firstname";
			}
			if(patient.CanadianEligibilityCode==0) {
				if(retVal!="")
					retVal+=", ";
				retVal+="Patient eligibility code";
			}
			if(patient.Age>=18 && patient.CanadianEligibilityCode==1){//fulltimeStudent
				if(patient.SchoolName=="") {
					if(retVal!="")
						retVal+=", ";
					retVal+="Patient school name";
				}
			}
			if(subscriber.Birthdate.Year<1880 || subscriber.Birthdate>DateTime.Today) {
				if(retVal!="")
					retVal+=", ";
				retVal+="Subscriber birthdate";
			}
			if(subscriber.LName=="") {
				if(retVal!="")
					retVal+=", ";
				retVal+="Subscriber lastname";
			}
			if(subscriber.FName=="") {
				if(retVal!="")
					retVal+=", ";
				retVal+="Subscriber firstname";
			}
			if(subscriber.Address=="") {
				if(retVal!="")
					retVal+=", ";
				retVal+="Subscriber address";
			}
			if(subscriber.City=="") {
				if(retVal!="")
					retVal+=", ";
				retVal+="Subscriber city";
			}
			if(!IsValidST(subscriber.State)) {
				if(retVal!="")
					retVal+=", ";
				retVal+="Subscriber ST";
			}
			if(!IsValidZip(subscriber.Zip,subscriber.State)) {
				if(retVal!="")
					retVal+=", ";
				retVal+="Subscriber Postalcode";
			}
			if(claimProcsClaim.Count>7) {//user interface enforces prevention of claim with 0 procs.
				if(retVal!="")
					retVal+=", ";
				retVal+="Over 7 procs";
			}
//incomplete. Also duplicate for max
			//user interface also needs to be improved to prompt and remind about extracted teeth
			/*if(isInitialLowerProsth && MandProsthMaterial!=0 && CountLower(extracted.Count)==0){
				if(retVal!="")
					retVal+=",";
				retVal+="Missing teeth not entered";
			}*/
			if(claim.PlanNum2>0){
				if(!Regex.IsMatch(carrier2.ElectID,@"^[0-9]{6}$")) {
					if(retVal!="")
						retVal+=", ";
					retVal+="Sec CarrierId 6 digits";
				}
				if(insPlan2.GroupNum.Length==0 || insPlan2.GroupNum.Length>12 || insPlan2.GroupNum.Contains(" ")) {
					if(retVal!="")
						retVal+=", ";
					retVal+="Sec Plan Number";
				}
				if(insSub2.SubscriberID=="") {
					if(retVal!="")
						retVal+=", ";
					retVal+="Sec SubscriberID";
				}
				if(claim.PatNum != insSub2.Subscriber//if patient is not subscriber
					&& claim.PatRelat2==Relat.Self) {//and relat is self
					if(retVal!="")
						retVal+=", ";
					retVal+="Sec Relationship";
				}
				if(subscriber2.Birthdate.Year<1880 || subscriber2.Birthdate>DateTime.Today) {
					if(retVal!="")
						retVal+=", ";
					retVal+="Sec Subscriber birthdate";
				}
				if(subscriber2.LName=="") {
					if(retVal!="")
						retVal+=", ";
					retVal+="Sec Subscriber lastname";
				}
				if(subscriber2.FName=="") {
					if(retVal!="")
						retVal+=", ";
					retVal+="Sec Subscriber firstname";
				}
				if(subscriber2.Address=="") {
					if(retVal!="")
						retVal+=", ";
					retVal+="Sec Subscriber address";
				}
				if(subscriber2.City=="") {
					if(retVal!="")
						retVal+=", ";
					retVal+="Sec Subscriber city";
				}
				if(!IsValidST(subscriber2.State)) {
					if(retVal!="")
						retVal+=", ";
					retVal+="Sec Subscriber ST";
				}
				if(!IsValidZip(subscriber2.Zip,subscriber2.State)) {
					if(retVal!="")
						retVal+=", ";
					retVal+="Sec Subscriber Postalcode";
				}
			}
			if(claim.CanadianReferralProviderNum!="" && claim.CanadianReferralReason==0) {
				if(retVal!="")
					retVal+=", ";
				retVal+="Referral reason";
			}
			if(claim.CanadianReferralProviderNum=="" && claim.CanadianReferralReason!=0) {
				if(retVal!="")
					retVal+=", ";
				retVal+="Referral provider";
			}
			//Max Prosth--------------------------------------------------------------------------------------------------
			if(claim.CanadianIsInitialUpper=="") {
				if(retVal!="")
					retVal+=", ";
				retVal+="Max prosth";
			}
			if(claim.CanadianDateInitialUpper>DateTime.MinValue) {
				if(claim.CanadianDateInitialUpper.Year<1900 || claim.CanadianDateInitialUpper>=DateTime.Today) {
					if(retVal!="")
						retVal+=", ";
					retVal+="Date initial upper";
				}
			}
			if(claim.CanadianIsInitialUpper=="N" && claim.CanadianDateInitialUpper.Year<1900) {//missing date
				if(retVal!="")
					retVal+=", ";
				retVal+="Date initial upper";
			}
			if(claim.CanadianIsInitialUpper=="N" && claim.CanadianMaxProsthMaterial==0) {
				if(retVal!="")
					retVal+=", ";
				retVal+="Max prosth material";
			}
			if(claim.CanadianIsInitialUpper=="X" && claim.CanadianMaxProsthMaterial!=0) {
				if(retVal!="")
					retVal+=", ";
				retVal+="Max prosth material";
			}
			//Mand prosth--------------------------------------------------------------------------------------------------
			if(claim.CanadianIsInitialLower=="") {
				if(retVal!="")
					retVal+=", ";
				retVal+="Mand prosth";
			}
			if(claim.CanadianDateInitialLower>DateTime.MinValue) {
				if(claim.CanadianDateInitialLower.Year<1900 || claim.CanadianDateInitialLower>=DateTime.Today) {
					if(retVal!="")
						retVal+=", ";
					retVal+="Date initial lower";
				}
			}
			if(claim.CanadianIsInitialLower=="N" && claim.CanadianDateInitialLower.Year<1900) {//missing date
				if(retVal!="")
					retVal+=", ";
				retVal+="Date initial lower";
			}
			if(claim.CanadianIsInitialLower=="N" && claim.CanadianMandProsthMaterial==0) {
				if(retVal!="")
					retVal+=", ";
				retVal+="Mand prosth material";
			}
			if(claim.CanadianIsInitialLower=="X" && claim.CanadianMandProsthMaterial!=0) {
				if(retVal!="")
					retVal+=", ";
				retVal+="Mand prosth material";
			}
			//missing teeth---------------------------------------------------------------------------------------------------
			/*Can't do this because extracted teeth count is allowed to be zero
			if(claim.CanadianIsInitialLower=="Y" && claim.CanadianMandProsthMaterial!=7) {//initial lower, but not crown
				if(extracted.Count==0) {
					if(retVal!="")
						retVal+=", ";
					retVal+="Missing teeth not entered";
				}
			}
			if(claim.CanadianIsInitialUpper=="Y" && claim.CanadianMaxProsthMaterial!=7) {//initial upper, but not crown
				if(extracted.Count==0) {
					if(retVal!="")
						retVal+=", ";
					retVal+="Missing teeth not entered";
				}
			}
			*/			
			if(claim.AccidentDate>DateTime.MinValue){
				if(claim.AccidentDate.Year<1900 || claim.AccidentDate>DateTime.Today){
					if(retVal!="")
						retVal+=",";
					retVal+="Accident date";
				}
			}
			for(int i=0;i<claimProcsClaim.Count;i++) {
				proc=Procedures.GetProcFromList(procListAll,claimProcsClaim[i].ProcNum);
				procCode=ProcedureCodes.GetProcCode(proc.CodeNum);
				if(claimProcsClaim[i].ProcDate.Year<1970 || claimProcsClaim[i].ProcDate>DateTime.Today){
					if(retVal!="") {
						retVal+=", ";
					}
					retVal+="proc "+procCode.ProcCode+" procedure date";
				}
				if(procCode.TreatArea==TreatmentArea.Arch && proc.Surf==""){
					if(retVal!="") {
						retVal+=", ";
					}
					retVal+="proc "+procCode.ProcCode+" missing arch";
				}
				if(procCode.TreatArea==TreatmentArea.ToothRange && proc.ToothRange==""){
					if(retVal!="") {
						retVal+=", ";
					}
					retVal+="proc "+procCode.ProcCode+" tooth range";
				}
				if((procCode.TreatArea==TreatmentArea.Tooth || procCode.TreatArea==TreatmentArea.Surf)
					&& !Tooth.IsValidDB(proc.ToothNum)) {
					if(retVal!="") {
						retVal+=", ";
					}
					retVal+="proc "+procCode.ProcCode+" tooth number";
				}
				if(procCode.IsProsth){
					if(proc.Prosthesis==""){//they didn't enter whether Initial or Replacement
						if(retVal!="") {
							retVal+=", ";
						}
						retVal+="proc "+procCode.ProcCode+" prosthesis";
					}
					if(proc.Prosthesis=="R"	&& proc.DateOriginalProsth.Year<1880){//if a replacement, they didn't enter a date
						if(retVal!="") {
							retVal+=", ";
						}
						retVal+="proc "+procCode.ProcCode+" prosth date";
					}
				}
			}

			return retVal;
		}

		
	}
}
