using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary>There is no cache for clinics.  We assume they will almost never change.</summary>
	public class Clinics {
		///<summary>Clinics cannot be hidden or deleted, so there is only one list.</summary>
		private static Clinic[] list;

		public static Clinic[] List{
			//No need to check RemotingRole; no call to db.
			get {
				if(list==null) {
					RefreshCache();
				}
				return list;
			}
			set {
				list=value;
			}
		}

		///<summary>Refresh all clinics.  Not actually part of official cache.</summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM clinic";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="clinic";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			list=new Clinic[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				list[i]=new Clinic();
				list[i].ClinicNum       = PIn.Long(table.Rows[i][0].ToString());
				list[i].Description     = PIn.String(table.Rows[i][1].ToString());
				list[i].Address         = PIn.String(table.Rows[i][2].ToString());
				list[i].Address2        = PIn.String(table.Rows[i][3].ToString());
				list[i].City            = PIn.String(table.Rows[i][4].ToString());
				list[i].State           = PIn.String(table.Rows[i][5].ToString());
				list[i].Zip             = PIn.String(table.Rows[i][6].ToString());
				list[i].Phone           = PIn.String(table.Rows[i][7].ToString());
				list[i].BankNumber      = PIn.String(table.Rows[i][8].ToString());
				list[i].DefaultPlaceService=(PlaceOfService)PIn.Long(table.Rows[i][9].ToString());
				list[i].InsBillingProv  = PIn.Long(table.Rows[i][10].ToString());
			}
		}

		///<summary></summary>
		public static long Insert(Clinic clinic){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				clinic.ClinicNum=Meth.GetLong(MethodBase.GetCurrentMethod(),clinic);
				return clinic.ClinicNum;
			}
			if(PrefC.RandomKeys) {
				clinic.ClinicNum=ReplicationServers.GetKey("clinic","ClinicNum");
			}
			string command="INSERT INTO clinic (";
			if(PrefC.RandomKeys) {
				command+="ClinicNum,";
			}
			command+="Description,Address,Address2,City,State,Zip,Phone,"
				+"BankNumber,DefaultPlaceService,InsBillingProv) VALUES(";
			if(PrefC.RandomKeys) {
				command+=POut.Long(clinic.ClinicNum)+", ";
			}
			command+=
				 "'"+POut.String(clinic.Description)+"', "
				+"'"+POut.String(clinic.Address)+"', "
				+"'"+POut.String(clinic.Address2)+"', "
				+"'"+POut.String(clinic.City)+"', "
				+"'"+POut.String(clinic.State)+"', "
				+"'"+POut.String(clinic.Zip)+"', "
				+"'"+POut.String(clinic.Phone)+"', "
				+"'"+POut.String(clinic.BankNumber)+"', "
				+"'"+POut.Long   ((int)clinic.DefaultPlaceService)+"', "
				+"'"+POut.Long   (clinic.InsBillingProv)+"')";
			if(PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				clinic.ClinicNum=Db.NonQ(command,true);
			}
			return clinic.ClinicNum;
		}

		///<summary></summary>
		public static void Update(Clinic clinic){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),clinic);
				return;
			}
			string command= "UPDATE clinic SET " 
				+ "Description = '"       +POut.String(clinic.Description)+"'"
				+ ",Address = '"          +POut.String(clinic.Address)+"'"
				+ ",Address2 = '"         +POut.String(clinic.Address2)+"'"
				+ ",City = '"             +POut.String(clinic.City)+"'"
				+ ",State = '"            +POut.String(clinic.State)+"'"
				+ ",Zip = '"              +POut.String(clinic.Zip)+"'"
				+ ",Phone = '"            +POut.String(clinic.Phone)+"'"
				+ ",BankNumber = '"       +POut.String(clinic.BankNumber)+"'"
				+ ",DefaultPlaceService='"+POut.Long   ((int)clinic.DefaultPlaceService)+"'"
				+ ",InsBillingProv='"     +POut.Long   (clinic.InsBillingProv)+"'"
				+" WHERE ClinicNum = '" +POut.Long(clinic.ClinicNum)+"'";
 			Db.NonQ(command);
		}

		///<summary>Checks dependencies first.  Throws exception if can't delete.</summary>
		public static void Delete(Clinic clinic){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),clinic);
				return;
			}
			//check patients for dependencies
			string command="SELECT LName,FName FROM patient WHERE ClinicNum ="
				+POut.Long(clinic.ClinicNum);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count>0){
				string pats="";
				for(int i=0;i<table.Rows.Count;i++){
					pats+="\r";
					pats+=table.Rows[i][0].ToString()+", "+table.Rows[i][1].ToString();
				}
				throw new Exception(Lans.g("Clinics","Cannot delete clinic because it is in use by the following patients:")+pats);
			}
			//check payments for dependencies
			command="SELECT patient.LName,patient.FName FROM patient,payment "
				+"WHERE payment.ClinicNum ="+POut.Long(clinic.ClinicNum)
				+" AND patient.PatNum=payment.PatNum";
			table=Db.GetTable(command);
			if(table.Rows.Count>0){
				string pats="";
				for(int i=0;i<table.Rows.Count;i++){
					pats+="\r";
					pats+=table.Rows[i][0].ToString()+", "+table.Rows[i][1].ToString();
				}
				throw new Exception(Lans.g("Clinics","Cannot delete clinic because the following patients have payments using it:")+pats);
			}
			//check claimpayments for dependencies
			command="SELECT patient.LName,patient.FName FROM patient,claimproc,claimpayment "
				+"WHERE claimpayment.ClinicNum ="+POut.Long(clinic.ClinicNum)
				+" AND patient.PatNum=claimproc.PatNum"
				+" AND claimproc.ClaimPaymentNum=claimpayment.ClaimPaymentNum "
				+"GROUP BY claimpayment.ClaimPaymentNum";
			table=Db.GetTable(command);
			if(table.Rows.Count>0){
				string pats="";
				for(int i=0;i<table.Rows.Count;i++){
					pats+="\r";
					pats+=table.Rows[i][0].ToString()+", "+table.Rows[i][1].ToString();
				}
				throw new Exception(Lans.g("Clinics","Cannot delete clinic because the following patients have claim payments using it:")+pats);
			}
			//check appointments for dependencies
			command="SELECT patient.LName,patient.FName FROM patient,appointment "
				+"WHERE appointment.ClinicNum ="+POut.Long(clinic.ClinicNum)
				+" AND patient.PatNum=appointment.PatNum";
			table=Db.GetTable(command);
			if(table.Rows.Count>0){
				string pats="";
				for(int i=0;i<table.Rows.Count;i++){
					pats+="\r";
					pats+=table.Rows[i][0].ToString()+", "+table.Rows[i][1].ToString();
				}
				throw new Exception(Lans.g("Clinics","Cannot delete clinic because the following patients have appointments using it:")+pats);
			}
			//check procedures for dependencies
			command="SELECT patient.LName,patient.FName FROM patient,procedurelog "
				+"WHERE procedurelog.ClinicNum ="+POut.Long(clinic.ClinicNum)
				+" AND patient.PatNum=procedurelog.PatNum";
			table=Db.GetTable(command);
			if(table.Rows.Count>0){
				string pats="";
				for(int i=0;i<table.Rows.Count;i++){
					pats+="\r";
					pats+=table.Rows[i][0].ToString()+", "+table.Rows[i][1].ToString();
				}
				throw new Exception(Lans.g("Clinics","Cannot delete clinic because the following patients have procedures using it:")+pats);
			}
			//check operatories for dependencies
			command="SELECT OpName FROM operatory "
				+"WHERE ClinicNum ="+POut.Long(clinic.ClinicNum);
			table=Db.GetTable(command);
			if(table.Rows.Count>0){
				string ops="";
				for(int i=0;i<table.Rows.Count;i++){
					ops+="\r";
					ops+=table.Rows[i][0].ToString();
				}
				throw new Exception(Lans.g("Clinics","Cannot delete clinic because the following operatories are using it:")+ops);
			}
			//delete
			command= "DELETE FROM clinic" 
				+" WHERE ClinicNum = "+POut.Long(clinic.ClinicNum);
 			Db.NonQ(command);
		}

		///<summary>Returns null if clinic not found.</summary>
		public static Clinic GetClinic(long clinicNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<List.Length;i++){
				if(List[i].ClinicNum==clinicNum){
					return List[i].Copy();
				}
			}
			return null;
		}

		///<summary>Returns an empty string for invalid clinicNums.</summary>
		public static string GetDesc(long clinicNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<List.Length;i++){
				if(List[i].ClinicNum==clinicNum){
					return List[i].Description;
				}
			}
			return "";
		}
	
		///<summary>Returns practice default for invalid clinicNums.</summary>
		public static PlaceOfService GetPlaceService(long clinicNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<List.Length;i++){
				if(List[i].ClinicNum==clinicNum){
					return List[i].DefaultPlaceService;
				}
			}
			return (PlaceOfService)PrefC.GetLong(PrefName.DefaultProcedurePlaceService);
			//return PlaceOfService.Office;
		}

		///<summary>Clinics cannot be hidden, so if clinicNum=0, this will return -1.</summary>
		public static int GetIndex(long clinicNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<List.Length;i++) {
				if(List[i].ClinicNum==clinicNum) {
					return i;
				}
			}
			return -1;
		}

	}
	


}













