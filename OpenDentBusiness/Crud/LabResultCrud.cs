//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class LabResultCrud {
		///<summary>Gets one LabResult object from the database using the primary key.  Returns null if not found.</summary>
		internal static LabResult SelectOne(long labResultNum){
			string command="SELECT * FROM labresult "
				+"WHERE LabResultNum = "+POut.Long(labResultNum);
			List<LabResult> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one LabResult object from the database using a query.</summary>
		internal static LabResult SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<LabResult> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of LabResult objects from the database using a query.</summary>
		internal static List<LabResult> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<LabResult> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<LabResult> TableToList(DataTable table){
			List<LabResult> retVal=new List<LabResult>();
			LabResult labResult;
			for(int i=0;i<table.Rows.Count;i++) {
				labResult=new LabResult();
				labResult.LabResultNum = PIn.Long  (table.Rows[i]["LabResultNum"].ToString());
				labResult.LabPanelNum  = PIn.Long  (table.Rows[i]["LabPanelNum"].ToString());
				labResult.DateTest     = PIn.Date  (table.Rows[i]["DateTest"].ToString());
				labResult.TestPerformed= PIn.String(table.Rows[i]["TestPerformed"].ToString());
				labResult.DateTStamp   = PIn.DateT (table.Rows[i]["DateTStamp"].ToString());
				retVal.Add(labResult);
			}
			return retVal;
		}

		///<summary>Inserts one LabResult into the database.  Returns the new priKey.</summary>
		internal static long Insert(LabResult labResult){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				labResult.LabResultNum=DbHelper.GetNextOracleKey("labresult","LabResultNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(labResult,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							labResult.LabResultNum++;
							loopcount++;
						}
						else{
							throw ex;
						}
					}
				}
				throw new ApplicationException("Insert failed.  Could not generate primary key.");
			}
			else {
				return Insert(labResult,false);
			}
		}

		///<summary>Inserts one LabResult into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(LabResult labResult,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				labResult.LabResultNum=ReplicationServers.GetKey("labresult","LabResultNum");
			}
			string command="INSERT INTO labresult (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="LabResultNum,";
			}
			command+="LabPanelNum,DateTest,TestPerformed) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(labResult.LabResultNum)+",";
			}
			command+=
				     POut.Long  (labResult.LabPanelNum)+","
				+    POut.Date  (labResult.DateTest)+","
				+"'"+POut.String(labResult.TestPerformed)+"')";
				//DateTStamp can only be set by MySQL
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				labResult.LabResultNum=Db.NonQ(command,true);
			}
			return labResult.LabResultNum;
		}

		///<summary>Updates one LabResult in the database.</summary>
		internal static void Update(LabResult labResult){
			string command="UPDATE labresult SET "
				+"LabPanelNum  =  "+POut.Long  (labResult.LabPanelNum)+", "
				+"DateTest     =  "+POut.Date  (labResult.DateTest)+", "
				+"TestPerformed= '"+POut.String(labResult.TestPerformed)+"' "
				//DateTStamp can only be set by MySQL
				+"WHERE LabResultNum = "+POut.Long(labResult.LabResultNum);
			Db.NonQ(command);
		}

		///<summary>Updates one LabResult in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(LabResult labResult,LabResult oldLabResult){
			string command="";
			if(labResult.LabPanelNum != oldLabResult.LabPanelNum) {
				if(command!=""){ command+=",";}
				command+="LabPanelNum = "+POut.Long(labResult.LabPanelNum)+"";
			}
			if(labResult.DateTest != oldLabResult.DateTest) {
				if(command!=""){ command+=",";}
				command+="DateTest = "+POut.Date(labResult.DateTest)+"";
			}
			if(labResult.TestPerformed != oldLabResult.TestPerformed) {
				if(command!=""){ command+=",";}
				command+="TestPerformed = '"+POut.String(labResult.TestPerformed)+"'";
			}
			//DateTStamp can only be set by MySQL
			if(command==""){
				return;
			}
			command="UPDATE labresult SET "+command
				+" WHERE LabResultNum = "+POut.Long(labResult.LabResultNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one LabResult from the database.</summary>
		internal static void Delete(long labResultNum){
			string command="DELETE FROM labresult "
				+"WHERE LabResultNum = "+POut.Long(labResultNum);
			Db.NonQ(command);
		}

	}
}