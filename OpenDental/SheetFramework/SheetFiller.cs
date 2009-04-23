﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenDentBusiness;

namespace OpenDental{
	public class SheetFiller {
		///<summary>Gets the data from the database and fills the fields.</summary>
		public static void FillFields(Sheet sheet){
			foreach(SheetParameter param in sheet.Parameters){
				if(param.IsRequired && param.ParamValue==null){
					throw new ApplicationException(Lan.g("Sheet","Parameter not specified for sheet: ")+param.ParamName);
				}
			}
			Patient pat=null;
			Referral refer=null;
			switch(sheet.SheetType) {
				case SheetTypeEnum.LabelPatient:
					pat=Patients.GetPat((int)GetParamByName(sheet,"PatNum").ParamValue);
					FillFieldsForLabelPatient(sheet,pat);
					break;
				case SheetTypeEnum.LabelCarrier:
					Carrier carrier=Carriers.GetCarrier((int)GetParamByName(sheet,"CarrierNum").ParamValue);
					FillFieldsForLabelCarrier(sheet,carrier);
					break;
				case SheetTypeEnum.LabelReferral:
					refer=ReferralL.GetReferral((int)GetParamByName(sheet,"ReferralNum").ParamValue);
					FillFieldsForLabelReferral(sheet,refer);
					break;
				case SheetTypeEnum.ReferralSlip:
					pat=Patients.GetPat((int)GetParamByName(sheet,"PatNum").ParamValue);
					refer=ReferralL.GetReferral((int)GetParamByName(sheet,"ReferralNum").ParamValue);
					FillFieldsForReferralSlip(sheet,pat,refer);
					break;
				case SheetTypeEnum.LabelAppointment:
					Appointment appt=Appointments.GetOneApt((int)GetParamByName(sheet,"AptNum").ParamValue);
					pat=Patients.GetPat(appt.PatNum);
					FillFieldsForLabelAppointment(sheet,appt,pat);
					break;
				case SheetTypeEnum.Rx:
					RxPat rx=RxPats.GetRx((int)GetParamByName(sheet,"RxNum").ParamValue);
					pat=Patients.GetPat(rx.PatNum);
					Provider prov=Providers.GetProv(rx.ProvNum);
					FillFieldsForRx(sheet,rx,pat,prov);
					break;
				case SheetTypeEnum.Consent:
					pat=Patients.GetPat((int)GetParamByName(sheet,"PatNum").ParamValue);
					FillFieldsForConsent(sheet,pat);
					break;
				case SheetTypeEnum.PatientLetter:
					pat=Patients.GetPat((int)GetParamByName(sheet,"PatNum").ParamValue);
					FillFieldsForPatientLetter(sheet,pat);
					break;
				case SheetTypeEnum.ReferralLetter:
					pat=Patients.GetPat((int)GetParamByName(sheet,"PatNum").ParamValue);
					refer=ReferralL.GetReferral((int)GetParamByName(sheet,"ReferralNum").ParamValue);
					FillFieldsForReferralLetter(sheet,pat,refer);
					break;
			}
			FillFieldsInStaticText(sheet,pat);
			FillPatientImages(sheet,pat);
		}

		private static SheetParameter GetParamByName(Sheet sheet,string paramName){
			foreach(SheetParameter param in sheet.Parameters){
				if(param.ParamName==paramName){
					return param;
				}
			}
			return null;
		}

		private static void FillFieldsInStaticText(Sheet sheet,Patient pat) {
			if(pat==null){
				return;
			}
			string fldval;
			string address=pat.Address;
			if(pat.Address2!=""){
				address+=", "+pat.Address2;
			}
			string birthdate=pat.Birthdate.ToShortDateString();
			if(pat.Birthdate.Year<1880){
				birthdate="";
			}
			List <PatPlan> patPlanList=PatPlans.Refresh(pat.PatNum);
			int planNum=PatPlans.GetPlanNum(patPlanList,1);
			InsPlan plan=InsPlans.GetPlan(planNum,new List <InsPlan> ());
			Carrier carrier=null;
			string carrierName="";
			string carrierAddress="";
			string carrierCityStZip="";
			string subscriberId="";
			string subscriberNameFL="";
			if(plan!=null){
				carrier=Carriers.GetCarrier(plan.CarrierNum);
				carrierName=carrier.CarrierName;
				carrierAddress=carrier.Address;
				if(carrier.Address2!=""){
					carrierAddress+=", "+carrier.Address2;
				}
				carrierCityStZip=carrier.City+", "+carrier.State+"  "+carrier.Zip;
				subscriberId=plan.SubscriberID;
				subscriberNameFL=Patients.GetLim(plan.Subscriber).GetNameFL();
			}
			TreatPlan[] treatPlanList=TreatPlans.Refresh(pat.PatNum);
			TreatPlan treatPlan=null;
			string dateOfLastSavedTP="";
			string tpResponsPartyAddress="";
			string tpResponsPartyCityStZip="";
			string tpResponsPartyNameFL="";
			if(treatPlanList.Length>0){
				treatPlan=treatPlanList[treatPlanList.Length-1].Copy();
				dateOfLastSavedTP=treatPlan.DateTP.ToShortDateString();
				Patient patRespParty=Patients.GetPat(treatPlan.ResponsParty);
				if(patRespParty!=null){
					tpResponsPartyAddress=patRespParty.Address;
					if(patRespParty.Address2!=""){
						tpResponsPartyAddress+=", "+patRespParty.Address2;
					}
					tpResponsPartyCityStZip=patRespParty.City+", "+patRespParty.State+"  "+patRespParty.Zip;
					tpResponsPartyNameFL=patRespParty.GetNameFL();
				}
			}
			Recall recall=Recalls.GetRecallProphyOrPerio(pat.PatNum);
			string dateRecallDue="";
			string recallInterval="";
			if(recall!=null){
				if(recall.DateDue.Year>1880){
					dateRecallDue=recall.DateDue.ToShortDateString();
				}
				recallInterval=recall.RecallInterval.ToString();
			}
			List<Appointment> apptList=Appointments.GetFutureSchedApts(pat.PatNum);
			string nextSchedApptDateT="";
			if(apptList.Count>0){
				nextSchedApptDateT=apptList[0].AptDateTime.ToShortDateString()+"  "+apptList[0].AptDateTime.ToShortTimeString();
			}
			Provider priProv=Providers.GetProv(Patients.GetProvNum(pat));//guaranteed to work
			foreach(SheetField field in sheet.SheetFields) {
				if(field.FieldType!=SheetFieldType.StaticText) {
					continue;
				}
				fldval=field.FieldValue;
				fldval=fldval.Replace("[address]",address);
				fldval=fldval.Replace("[Birthdate]",birthdate);
				fldval=fldval.Replace("[carrierName]",carrierName);
				fldval=fldval.Replace("[ChartNumber]",pat.ChartNumber);
				fldval=fldval.Replace("[carrierAddress]",carrierAddress);
				fldval=fldval.Replace("[carrierCityStZip]",carrierCityStZip);
				fldval=fldval.Replace("[cityStateZip]",pat.City+", "+pat.State+"  "+pat.Zip);
				fldval=fldval.Replace("[dateOfLastSavedTP]",dateOfLastSavedTP);
				fldval=fldval.Replace("[dateRecallDue]",dateRecallDue);
				fldval=fldval.Replace("[Email]",pat.Email);
				fldval=fldval.Replace("[HmPhone]",StripPhoneBeyondSpace(pat.HmPhone));
				fldval=fldval.Replace("[nameFL]",pat.GetNameFL());
				fldval=fldval.Replace("[nextSchedApptDateT]",nextSchedApptDateT);
				fldval=fldval.Replace("[PatNum]",pat.PatNum.ToString());
				fldval=fldval.Replace("[priProvNameFormal]",priProv.GetFormalName());
				fldval=fldval.Replace("[recallInterval]",recallInterval);
				fldval=fldval.Replace("[salutation]",pat.GetSalutation());
				fldval=fldval.Replace("[siteDescription]",Sites.GetDescription(pat.SiteNum));
				fldval=fldval.Replace("[subscriberID]",subscriberId);
				fldval=fldval.Replace("[subscriberNameFL]",subscriberNameFL);
				fldval=fldval.Replace("[tpResponsPartyAddress]",tpResponsPartyAddress);
				fldval=fldval.Replace("[tpResponsPartyCityStZip]",tpResponsPartyCityStZip);
				fldval=fldval.Replace("[tpResponsPartyNameFL]",tpResponsPartyNameFL);
				fldval=fldval.Replace("[WirelessPhone]",StripPhoneBeyondSpace(pat.WirelessPhone));
				fldval=fldval.Replace("[WkPhone]",StripPhoneBeyondSpace(pat.WkPhone));
				field.FieldValue=fldval;
			}
		}

		private static string StripPhoneBeyondSpace(string phone) {
			if(!phone.Contains(" ")) {
				return phone;
			}
			int idx=phone.IndexOf(" ");
			return phone.Substring(0,idx);
		}

		private static void FillPatientImages(Sheet sheet,Patient pat){
			if(pat==null){
				return;
			}
			Document[] docList=Documents.GetAllWithPat(pat.PatNum);
			int category;
			string fieldVal;//zoom and pan
			int x;
			int y;
			int w;
			int h;
			float ratioObject;
			float ratioImage;
			Image img;
			foreach(SheetField field in sheet.SheetFields){
				if(field.FieldType!=SheetFieldType.PatImage){
					continue;
				}
				category=PIn.PInt(field.FieldName);
				field.FieldName="0";//in case we can't find an image, this will be 0.
				field.FieldValue="";
				//go backwards to find the latest date
				for(int i=docList.Length-1;i>=0;i--){
					if(docList[i].DocCategory!=category){
						continue;
					}
					field.FieldName=docList[i].DocNum.ToString();
					ratioObject=(float)field.Width/(float)field.Height;
					img=Image.FromFile(  docList[i].FileName);
					//ratioImage=(float)docList[i].wid  field.Width/(float)field.Height;
//incomplete

					field.FieldValue="";
					break;
				}
			}
		}

		private static void FillFieldsForLabelPatient(Sheet sheet,Patient pat){
			foreach(SheetField field in sheet.SheetFields){
				switch(field.FieldName){
					case "nameFL":
						field.FieldValue=pat.GetNameFLFormal();
						break;
					case "nameLF":
						field.FieldValue=pat.GetNameLF();
						break;
					case "address":
						field.FieldValue=pat.Address;
						if(pat.Address2!=""){
							field.FieldValue+="\r\n"+pat.Address2;
						}
						break;
					case "cityStateZip":
						field.FieldValue=pat.City+", "+pat.State+" "+pat.Zip;
						break;
					case "ChartNumber":
						field.FieldValue=pat.ChartNumber;
						break;
					case "PatNum":
						field.FieldValue=pat.PatNum.ToString();
						break;
					case "dateTime.Today":
						field.FieldValue=DateTime.Today.ToShortDateString();
						break;
					case "birthdate":
						//only a temporary workaround:
						field.FieldValue="BD: "+pat.Birthdate.ToShortDateString();
						break;
					case "priProvName":
						field.FieldValue=Providers.GetLongDesc(pat.PriProv);
						break;
				}
			}
		}

		private static void FillFieldsForLabelCarrier(Sheet sheet,Carrier carrier) {
			foreach(SheetField field in sheet.SheetFields) {
				switch(field.FieldName) {
					case "CarrierName":
						field.FieldValue=carrier.CarrierName;
						break;
					case "address":
						field.FieldValue=carrier.Address;
						if(carrier.Address2!="") {
							field.FieldValue+="\r\n"+carrier.Address2;
						}
						break;
					case "cityStateZip":
						field.FieldValue=carrier.City+", "+carrier.State+" "+carrier.Zip;
						break;
				}
			}
		}

		private static void FillFieldsForLabelReferral(Sheet sheet,Referral refer) {
			foreach(SheetField field in sheet.SheetFields) {
				switch(field.FieldName) {
					case "nameFL":
						field.FieldValue=Referrals.GetNameFL(refer.ReferralNum);
						break;
					case "address":
						field.FieldValue=refer.Address;
						if(refer.Address2!="") {
							field.FieldValue+="\r\n"+refer.Address2;
						}
						break;
					case "cityStateZip":
						field.FieldValue=refer.City+", "+refer.ST+" "+refer.Zip;
						break;
				}
			}
		}

		private static void FillFieldsForReferralSlip(Sheet sheet,Patient pat,Referral refer) {
			foreach(SheetField field in sheet.SheetFields) {
				switch(field.FieldName) {
					case "referral.nameFL":
						field.FieldValue=Referrals.GetNameFL(refer.ReferralNum);
						break;
					case "referral.address":
						field.FieldValue=refer.Address;
						if(refer.Address2!="") {
							field.FieldValue+="\r\n"+refer.Address2;
						}
						break;
					case "referral.cityStateZip":
						field.FieldValue=refer.City+", "+refer.ST+" "+refer.Zip;
						break;
					case "referral.phone":
						field.FieldValue="";
						if(refer.Telephone.Length==10){
							field.FieldValue="("+refer.Telephone.Substring(0,3)+")"
								+refer.Telephone.Substring(3,3)+"-"
								+refer.Telephone.Substring(6);
						}
						break;
					case "patient.nameFL":
						field.FieldValue=pat.GetNameFL();
						break;
					case "dateTime.Today":
						field.FieldValue=DateTime.Today.ToShortDateString();
						break;
					case "patient.WkPhone":
						field.FieldValue=pat.WkPhone;
						break;
					case "patient.HmPhone":
						field.FieldValue=pat.HmPhone;
						break;
					case "patient.WirelessPhone":
						field.FieldValue=pat.WirelessPhone;
						break;
					case "patient.address":
						field.FieldValue=pat.Address;
						if(pat.Address2!="") {
							field.FieldValue+="\r\n"+pat.Address2;
						}
						break;
					case "patient.cityStateZip":
						field.FieldValue=pat.City+", "+pat.State+" "+pat.Zip;
						break;
					case "patient.provider":
						field.FieldValue=Providers.GetProv(Patients.GetProvNum(pat)).GetFormalName();
						break;
					//case "notes"://an input field
				}
			}
		}

		private static void FillFieldsForLabelAppointment(Sheet sheet,Appointment appt,Patient pat) {
			foreach(SheetField field in sheet.SheetFields) {
				switch(field.FieldName) {
					case "nameFL":
						field.FieldValue=pat.GetNameFirstOrPrefL();
						break;
					case "nameLF":
						field.FieldValue=pat.GetNameLF();
						break;
					case "weekdayDateTime":
						field.FieldValue=appt.AptDateTime.ToString("ddd")+"   "
							+appt.AptDateTime.ToShortDateString()+"  "
							+appt.AptDateTime.ToShortTimeString();//  h:mm tt");
						break;
					case "length":
						int minutesTotal=appt.Pattern.Length*5;
						int hours=minutesTotal/60;//automatically rounds down
						int minutes=minutesTotal-hours*60;
						field.FieldValue="";
						if(hours>0){
							field.FieldValue=hours.ToString()+" hours, ";
						}
						field.FieldValue+=minutes.ToString()+" min";
						break;
				}
			}
		}

		private static void FillFieldsForRx(Sheet sheet,RxPat rx,Patient pat,Provider prov) {
			string text;
			foreach(SheetField field in sheet.SheetFields) {
				switch(field.FieldName) {
					case "prov.nameFL":
						field.FieldValue=prov.GetFormalName();
						break;
					case "prov.address":
						field.FieldValue=PrefC.GetString("PracticeAddress");
						if(PrefC.GetString("PracticeAddress2")!=""){
							field.FieldValue+="\r\n"+PrefC.GetString("PracticeAddress2");
						}
						break;
					case "prov.cityStateZip":
						field.FieldValue=PrefC.GetString("PracticeCity")+", "+PrefC.GetString("PracticeST")+" "+PrefC.GetString("PracticeZip");;
						break;
					case "prov.phone":
						text=PrefC.GetString("PracticePhone");
						field.FieldValue=text;
						if(text.Length==10) {
							field.FieldValue="("+text.Substring(0,3)+")"+text.Substring(3,3)+"-"+text.Substring(6);
						}
						break;
					case "RxDate":
						field.FieldValue=rx.RxDate.ToShortDateString();
						break;
					case "RxDateMonthSpelled":
						field.FieldValue=rx.RxDate.ToString("MMM dd,yyyy");
						break;
					case "prov.dEANum":
						if(rx.IsControlled){
							field.FieldValue=prov.DEANum;
						}
						else{
							field.FieldValue="";
						}
						break;
					case "pat.nameFL":
						//Can't include preferred, so:
						field.FieldValue=pat.FName+" "+pat.MiddleI+"  "+pat.LName;
						break;
					case "pat.Birthdate":
						if(pat.Birthdate.Year<1880){
							field.FieldValue="";
						}
						else{
							field.FieldValue=pat.Birthdate.ToShortDateString();
						}
						break;
					case "pat.HmPhone":
						field.FieldValue=pat.HmPhone;
						break;
					case "pat.address":
						field.FieldValue=pat.Address;
						if(pat.Address2!=""){
							field.FieldValue+="\r\n"+pat.Address2;
						}
						break;
					case "pat.cityStateZip":
						field.FieldValue=pat.City+", "+pat.State+" "+pat.Zip;
						break;
					case "Drug":
						field.FieldValue=rx.Drug;
						break;
					case "Disp":
						field.FieldValue=rx.Disp;
						break;
					case "Sig":
						field.FieldValue=rx.Sig;
						break;
					case "Refills":
						field.FieldValue=rx.Refills;
						break;					
				}
			}
		}

		private static void FillFieldsForConsent(Sheet sheet,Patient pat) {
			foreach(SheetField field in sheet.SheetFields) {
				switch(field.FieldName) {
					case "patient.nameFL":
						field.FieldValue=pat.GetNameFL();
						break;
					case "dateTime.Today":
						field.FieldValue=DateTime.Today.ToShortDateString();
						break;
				}
			}
		}

		private static void FillFieldsForPatientLetter(Sheet sheet,Patient pat) {
			foreach(SheetField field in sheet.SheetFields) {
				switch(field.FieldName) {
					case "PracticeTitle":
						field.FieldValue=PrefC.GetString("PracticeTitle");
						break;
					case "PracticeAddress":
						field.FieldValue=PrefC.GetString("PracticeAddress");
						if(PrefC.GetString("PracticeAddress2") != ""){
							field.FieldValue+="\r\n"+PrefC.GetString("PracticeAddress2");
						}
						break;
					case "practiceCityStateZip":
						field.FieldValue=PrefC.GetString("PracticeCity")+", "
							+PrefC.GetString("PracticeST")+"  "
							+PrefC.GetString("PracticeZip");
						break;
					case "patient.nameFL":
						field.FieldValue=pat.GetNameFLFormal();
						break;
					case "patient.address":
						field.FieldValue=pat.Address;
						if(pat.Address2!="") {
							field.FieldValue+="\r\n"+pat.Address2;
						}
						break;
					case "patient.cityStateZip":
						field.FieldValue=pat.City+", "+pat.State+" "+pat.Zip;
						break;
					case "today.DayDate":
						field.FieldValue=DateTime.Today.ToString("dddd, MM/dd/yyyy");
						break;
					case "patient.salutation":
						field.FieldValue="Dear "+pat.GetSalutation()+":";
						break;
					case "patient.priProvNameFL":
						field.FieldValue=Providers.GetFormalName(pat.PriProv);
						break;
				}
			}
		}

		private static void FillFieldsForReferralLetter(Sheet sheet,Patient pat,Referral refer) {
			foreach(SheetField field in sheet.SheetFields) {
				switch(field.FieldName) {
					case "PracticeTitle":
						field.FieldValue=PrefC.GetString("PracticeTitle");
						break;
					case "PracticeAddress":
						field.FieldValue=PrefC.GetString("PracticeAddress");
						if(PrefC.GetString("PracticeAddress2") != ""){
							field.FieldValue+="\r\n"+PrefC.GetString("PracticeAddress2");
						}
						break;
					case "practiceCityStateZip":
						field.FieldValue=PrefC.GetString("PracticeCity")+", "
							+PrefC.GetString("PracticeST")+"  "
							+PrefC.GetString("PracticeZip");
						break;
					case "referral.nameFL":
						field.FieldValue=Referrals.GetNameFL(refer.ReferralNum);
						break;
					case "referral.address":
						field.FieldValue=refer.Address;
						if(refer.Address2!="") {
							field.FieldValue+="\r\n"+refer.Address2;
						}
						break;
					case "referral.cityStateZip":
						field.FieldValue=refer.City+", "+refer.ST+" "+refer.Zip;
						break;
					case "today.DayDate":
						field.FieldValue=DateTime.Today.ToString("dddd, MM/dd/yyyy");
						break;
					case "patient.nameFL":
						field.FieldValue=pat.GetNameFL();
						break;
					case "referral.salutation":
						field.FieldValue="Dear "+refer.FName+":";
						break;
					case "patient.priProvNameFL":
						field.FieldValue=Providers.GetFormalName(pat.PriProv);
						break;
				}
			}
		}







	}
}
