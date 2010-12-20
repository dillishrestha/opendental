//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class PatientNoteCrud {
		///<summary>Gets one PatientNote object from the database using the primary key.  Returns null if not found.</summary>
		internal static PatientNote SelectOne(long patNum){
			string command="SELECT * FROM patientnote "
				+"WHERE PatNum = "+POut.Long(patNum)+" LIMIT 1";
			List<PatientNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PatientNote object from the database using a query.</summary>
		internal static PatientNote SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PatientNote> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PatientNote objects from the database using a query.</summary>
		internal static List<PatientNote> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PatientNote> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<PatientNote> TableToList(DataTable table){
			List<PatientNote> retVal=new List<PatientNote>();
			PatientNote patientNote;
			for(int i=0;i<table.Rows.Count;i++) {
				patientNote=new PatientNote();
				patientNote.PatNum      = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				patientNote.FamFinancial= PIn.String(table.Rows[i]["FamFinancial"].ToString());
				patientNote.ApptPhone   = PIn.String(table.Rows[i]["ApptPhone"].ToString());
				patientNote.Medical     = PIn.String(table.Rows[i]["Medical"].ToString());
				patientNote.Service     = PIn.String(table.Rows[i]["Service"].ToString());
				patientNote.MedicalComp = PIn.String(table.Rows[i]["MedicalComp"].ToString());
				patientNote.Treatment   = PIn.String(table.Rows[i]["Treatment"].ToString());
				patientNote.CCNumber    = PIn.String(table.Rows[i]["CCNumber"].ToString());
				patientNote.CCExpiration= PIn.Date  (table.Rows[i]["CCExpiration"].ToString());
				retVal.Add(patientNote);
			}
			return retVal;
		}

		///<summary>Inserts one PatientNote into the database.  Returns the new priKey.</summary>
		internal static long Insert(PatientNote patientNote){
			return Insert(patientNote,false);
		}

		///<summary>Inserts one PatientNote into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(PatientNote patientNote,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				patientNote.PatNum=ReplicationServers.GetKey("patientnote","PatNum");
			}
			string command="INSERT INTO patientnote (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PatNum,";
			}
			command+="FamFinancial,ApptPhone,Medical,Service,MedicalComp,Treatment,CCNumber,CCExpiration) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(patientNote.PatNum)+",";
			}
			command+=
				 "'"+POut.String(patientNote.FamFinancial)+"',"
				+"'"+POut.String(patientNote.ApptPhone)+"',"
				+"'"+POut.String(patientNote.Medical)+"',"
				+"'"+POut.String(patientNote.Service)+"',"
				+"'"+POut.String(patientNote.MedicalComp)+"',"
				+"'"+POut.String(patientNote.Treatment)+"',"
				+"'"+POut.String(patientNote.CCNumber)+"',"
				+    POut.Date  (patientNote.CCExpiration)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				patientNote.PatNum=Db.NonQ(command,true);
			}
			return patientNote.PatNum;
		}

		///<summary>Updates one PatientNote in the database.</summary>
		internal static void Update(PatientNote patientNote){
			string command="UPDATE patientnote SET "
				+"FamFinancial= '"+POut.String(patientNote.FamFinancial)+"', "
				+"ApptPhone   = '"+POut.String(patientNote.ApptPhone)+"', "
				+"Medical     = '"+POut.String(patientNote.Medical)+"', "
				+"Service     = '"+POut.String(patientNote.Service)+"', "
				+"MedicalComp = '"+POut.String(patientNote.MedicalComp)+"', "
				+"Treatment   = '"+POut.String(patientNote.Treatment)+"', "
				+"CCNumber    = '"+POut.String(patientNote.CCNumber)+"', "
				+"CCExpiration=  "+POut.Date  (patientNote.CCExpiration)+" "
				+"WHERE PatNum = "+POut.Long(patientNote.PatNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Updates one PatientNote in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(PatientNote patientNote,PatientNote oldPatientNote){
			string command="";
			if(patientNote.FamFinancial != oldPatientNote.FamFinancial) {
				if(command!=""){ command+=",";}
				command+="FamFinancial = '"+POut.String(patientNote.FamFinancial)+"'";
			}
			if(patientNote.ApptPhone != oldPatientNote.ApptPhone) {
				if(command!=""){ command+=",";}
				command+="ApptPhone = '"+POut.String(patientNote.ApptPhone)+"'";
			}
			if(patientNote.Medical != oldPatientNote.Medical) {
				if(command!=""){ command+=",";}
				command+="Medical = '"+POut.String(patientNote.Medical)+"'";
			}
			if(patientNote.Service != oldPatientNote.Service) {
				if(command!=""){ command+=",";}
				command+="Service = '"+POut.String(patientNote.Service)+"'";
			}
			if(patientNote.MedicalComp != oldPatientNote.MedicalComp) {
				if(command!=""){ command+=",";}
				command+="MedicalComp = '"+POut.String(patientNote.MedicalComp)+"'";
			}
			if(patientNote.Treatment != oldPatientNote.Treatment) {
				if(command!=""){ command+=",";}
				command+="Treatment = '"+POut.String(patientNote.Treatment)+"'";
			}
			if(patientNote.CCNumber != oldPatientNote.CCNumber) {
				if(command!=""){ command+=",";}
				command+="CCNumber = '"+POut.String(patientNote.CCNumber)+"'";
			}
			if(patientNote.CCExpiration != oldPatientNote.CCExpiration) {
				if(command!=""){ command+=",";}
				command+="CCExpiration = "+POut.Date(patientNote.CCExpiration)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE patientnote SET "+command
				+" WHERE PatNum = "+POut.Long(patientNote.PatNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Deletes one PatientNote from the database.</summary>
		internal static void Delete(long patNum){
			string command="DELETE FROM patientnote "
				+"WHERE PatNum = "+POut.Long(patNum)+" LIMIT 1";
			Db.NonQ(command);
		}

	}
}