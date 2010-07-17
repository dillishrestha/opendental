//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class SupplyNeededCrud {
		///<summary>Gets one SupplyNeeded object from the database using the primary key.  Returns null if not found.</summary>
		internal static SupplyNeeded SelectOne(long supplyNeededNum){
			string command="SELECT * FROM supplyneeded "
				+"WHERE SupplyNeededNum = "+POut.Long(supplyNeededNum)+" LIMIT 1";
			List<SupplyNeeded> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one SupplyNeeded object from the database using a query.</summary>
		internal static SupplyNeeded SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SupplyNeeded> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of SupplyNeeded objects from the database using a query.</summary>
		internal static List<SupplyNeeded> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SupplyNeeded> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<SupplyNeeded> TableToList(DataTable table){
			List<SupplyNeeded> retVal=new List<SupplyNeeded>();
			SupplyNeeded supplyNeeded;
			for(int i=0;i<table.Rows.Count;i++) {
				supplyNeeded=new SupplyNeeded();
				supplyNeeded.SupplyNeededNum= PIn.Long  (table.Rows[i]["SupplyNeededNum"].ToString());
				supplyNeeded.Description    = PIn.String(table.Rows[i]["Description"].ToString());
				supplyNeeded.DateAdded      = PIn.Date  (table.Rows[i]["DateAdded"].ToString());
				retVal.Add(supplyNeeded);
			}
			return retVal;
		}

		///<summary>Inserts one SupplyNeeded into the database.  Returns the new priKey.</summary>
		internal static long Insert(SupplyNeeded supplyNeeded){
			return Insert(supplyNeeded,false);
		}

		///<summary>Inserts one SupplyNeeded into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(SupplyNeeded supplyNeeded,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				supplyNeeded.SupplyNeededNum=ReplicationServers.GetKey("supplyneeded","SupplyNeededNum");
			}
			string command="INSERT INTO supplyneeded (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="SupplyNeededNum,";
			}
			command+="Description,DateAdded) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(supplyNeeded.SupplyNeededNum)+",";
			}
			command+=
				 "'"+POut.String(supplyNeeded.Description)+"',"
				+    POut.Date  (supplyNeeded.DateAdded)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				supplyNeeded.SupplyNeededNum=Db.NonQ(command,true);
			}
			return supplyNeeded.SupplyNeededNum;
		}

		///<summary>Updates one SupplyNeeded in the database.</summary>
		internal static void Update(SupplyNeeded supplyNeeded){
			string command="UPDATE supplyneeded SET "
				+"Description    = '"+POut.String(supplyNeeded.Description)+"', "
				+"DateAdded      =  "+POut.Date  (supplyNeeded.DateAdded)+" "
				+"WHERE SupplyNeededNum = "+POut.Long(supplyNeeded.SupplyNeededNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Updates one SupplyNeeded in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(SupplyNeeded supplyNeeded,SupplyNeeded oldSupplyNeeded){
			string command="";
			if(supplyNeeded.Description != oldSupplyNeeded.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(supplyNeeded.Description)+"'";
			}
			if(supplyNeeded.DateAdded != oldSupplyNeeded.DateAdded) {
				if(command!=""){ command+=",";}
				command+="DateAdded = "+POut.Date(supplyNeeded.DateAdded)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE supplyneeded SET "+command
				+" WHERE SupplyNeededNum = "+POut.Long(supplyNeeded.SupplyNeededNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Deletes one SupplyNeeded from the database.</summary>
		internal static void Delete(long supplyNeededNum){
			string command="DELETE FROM supplyneeded "
				+"WHERE SupplyNeededNum = "+POut.Long(supplyNeededNum)+" LIMIT 1";
			Db.NonQ(command);
		}

	}
}