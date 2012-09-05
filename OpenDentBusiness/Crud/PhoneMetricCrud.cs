//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class PhoneMetricCrud {
		///<summary>Gets one PhoneMetric object from the database using the primary key.  Returns null if not found.</summary>
		internal static PhoneMetric SelectOne(long phoneMetricNum){
			string command="SELECT * FROM phonemetric "
				+"WHERE PhoneMetricNum = "+POut.Long(phoneMetricNum);
			List<PhoneMetric> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PhoneMetric object from the database using a query.</summary>
		internal static PhoneMetric SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PhoneMetric> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PhoneMetric objects from the database using a query.</summary>
		internal static List<PhoneMetric> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PhoneMetric> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<PhoneMetric> TableToList(DataTable table){
			List<PhoneMetric> retVal=new List<PhoneMetric>();
			PhoneMetric phoneMetric;
			for(int i=0;i<table.Rows.Count;i++) {
				phoneMetric=new PhoneMetric();
				phoneMetric.PhoneMetricNum= PIn.Long  (table.Rows[i]["PhoneMetricNum"].ToString());
				phoneMetric.DateTimeEntry = PIn.DateT (table.Rows[i]["DateTimeEntry"].ToString());
				phoneMetric.VoiceMails    = PIn.Int   (table.Rows[i]["VoiceMails"].ToString());
				phoneMetric.Triages       = PIn.Int   (table.Rows[i]["Triages"].ToString());
				phoneMetric.MinutesBehind = PIn.Int   (table.Rows[i]["MinutesBehind"].ToString());
				retVal.Add(phoneMetric);
			}
			return retVal;
		}

		///<summary>Inserts one PhoneMetric into the database.  Returns the new priKey.</summary>
		internal static long Insert(PhoneMetric phoneMetric){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				phoneMetric.PhoneMetricNum=DbHelper.GetNextOracleKey("phonemetric","PhoneMetricNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(phoneMetric,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							phoneMetric.PhoneMetricNum++;
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
				return Insert(phoneMetric,false);
			}
		}

		///<summary>Inserts one PhoneMetric into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(PhoneMetric phoneMetric,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				phoneMetric.PhoneMetricNum=ReplicationServers.GetKey("phonemetric","PhoneMetricNum");
			}
			string command="INSERT INTO phonemetric (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PhoneMetricNum,";
			}
			command+="DateTimeEntry,VoiceMails,Triages,MinutesBehind) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(phoneMetric.PhoneMetricNum)+",";
			}
			command+=
				     POut.DateT (phoneMetric.DateTimeEntry)+","
				+    POut.Int   (phoneMetric.VoiceMails)+","
				+    POut.Int   (phoneMetric.Triages)+","
				+    POut.Int   (phoneMetric.MinutesBehind)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				phoneMetric.PhoneMetricNum=Db.NonQ(command,true);
			}
			return phoneMetric.PhoneMetricNum;
		}

		///<summary>Updates one PhoneMetric in the database.</summary>
		internal static void Update(PhoneMetric phoneMetric){
			string command="UPDATE phonemetric SET "
				+"DateTimeEntry =  "+POut.DateT (phoneMetric.DateTimeEntry)+", "
				+"VoiceMails    =  "+POut.Int   (phoneMetric.VoiceMails)+", "
				+"Triages       =  "+POut.Int   (phoneMetric.Triages)+", "
				+"MinutesBehind =  "+POut.Int   (phoneMetric.MinutesBehind)+" "
				+"WHERE PhoneMetricNum = "+POut.Long(phoneMetric.PhoneMetricNum);
			Db.NonQ(command);
		}

		///<summary>Updates one PhoneMetric in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(PhoneMetric phoneMetric,PhoneMetric oldPhoneMetric){
			string command="";
			if(phoneMetric.DateTimeEntry != oldPhoneMetric.DateTimeEntry) {
				if(command!=""){ command+=",";}
				command+="DateTimeEntry = "+POut.DateT(phoneMetric.DateTimeEntry)+"";
			}
			if(phoneMetric.VoiceMails != oldPhoneMetric.VoiceMails) {
				if(command!=""){ command+=",";}
				command+="VoiceMails = "+POut.Int(phoneMetric.VoiceMails)+"";
			}
			if(phoneMetric.Triages != oldPhoneMetric.Triages) {
				if(command!=""){ command+=",";}
				command+="Triages = "+POut.Int(phoneMetric.Triages)+"";
			}
			if(phoneMetric.MinutesBehind != oldPhoneMetric.MinutesBehind) {
				if(command!=""){ command+=",";}
				command+="MinutesBehind = "+POut.Int(phoneMetric.MinutesBehind)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE phonemetric SET "+command
				+" WHERE PhoneMetricNum = "+POut.Long(phoneMetric.PhoneMetricNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one PhoneMetric from the database.</summary>
		internal static void Delete(long phoneMetricNum){
			string command="DELETE FROM phonemetric "
				+"WHERE PhoneMetricNum = "+POut.Long(phoneMetricNum);
			Db.NonQ(command);
		}

	}
}