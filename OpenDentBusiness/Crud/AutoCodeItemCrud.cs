//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class AutoCodeItemCrud {
		///<summary>Gets one AutoCodeItem object from the database using the primary key.  Returns null if not found.</summary>
		internal static AutoCodeItem SelectOne(long autoCodeItemNum){
			string command="SELECT * FROM autocodeitem "
				+"WHERE AutoCodeItemNum = "+POut.Long(autoCodeItemNum)+" LIMIT 1";
			List<AutoCodeItem> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one AutoCodeItem object from the database using a query.</summary>
		internal static AutoCodeItem SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<AutoCodeItem> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of AutoCodeItem objects from the database using a query.</summary>
		internal static List<AutoCodeItem> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<AutoCodeItem> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<AutoCodeItem> TableToList(DataTable table){
			List<AutoCodeItem> retVal=new List<AutoCodeItem>();
			AutoCodeItem autoCodeItem;
			for(int i=0;i<table.Rows.Count;i++) {
				autoCodeItem=new AutoCodeItem();
				autoCodeItem.AutoCodeItemNum= PIn.Long  (table.Rows[i]["AutoCodeItemNum"].ToString());
				autoCodeItem.AutoCodeNum    = PIn.Long  (table.Rows[i]["AutoCodeNum"].ToString());
				autoCodeItem.OldCode        = PIn.String(table.Rows[i]["OldCode"].ToString());
				autoCodeItem.CodeNum        = PIn.Long  (table.Rows[i]["CodeNum"].ToString());
				retVal.Add(autoCodeItem);
			}
			return retVal;
		}

		///<summary>Inserts one AutoCodeItem into the database.  Returns the new priKey.</summary>
		internal static long Insert(AutoCodeItem autoCodeItem){
			return Insert(autoCodeItem,false);
		}

		///<summary>Inserts one AutoCodeItem into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(AutoCodeItem autoCodeItem,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				autoCodeItem.AutoCodeItemNum=ReplicationServers.GetKey("autocodeitem","AutoCodeItemNum");
			}
			string command="INSERT INTO autocodeitem (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="AutoCodeItemNum,";
			}
			command+="AutoCodeNum,OldCode,CodeNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(autoCodeItem.AutoCodeItemNum)+",";
			}
			command+=
				     POut.Long  (autoCodeItem.AutoCodeNum)+","
				+"'"+POut.String(autoCodeItem.OldCode)+"',"
				+    POut.Long  (autoCodeItem.CodeNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				autoCodeItem.AutoCodeItemNum=Db.NonQ(command,true);
			}
			return autoCodeItem.AutoCodeItemNum;
		}

		///<summary>Updates one AutoCodeItem in the database.</summary>
		internal static void Update(AutoCodeItem autoCodeItem){
			string command="UPDATE autocodeitem SET "
				+"AutoCodeNum    =  "+POut.Long  (autoCodeItem.AutoCodeNum)+", "
				+"OldCode        = '"+POut.String(autoCodeItem.OldCode)+"', "
				+"CodeNum        =  "+POut.Long  (autoCodeItem.CodeNum)+" "
				+"WHERE AutoCodeItemNum = "+POut.Long(autoCodeItem.AutoCodeItemNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Updates one AutoCodeItem in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(AutoCodeItem autoCodeItem,AutoCodeItem oldAutoCodeItem){
			string command="";
			if(autoCodeItem.AutoCodeNum != oldAutoCodeItem.AutoCodeNum) {
				if(command!=""){ command+=",";}
				command+="AutoCodeNum = "+POut.Long(autoCodeItem.AutoCodeNum)+"";
			}
			if(autoCodeItem.OldCode != oldAutoCodeItem.OldCode) {
				if(command!=""){ command+=",";}
				command+="OldCode = '"+POut.String(autoCodeItem.OldCode)+"'";
			}
			if(autoCodeItem.CodeNum != oldAutoCodeItem.CodeNum) {
				if(command!=""){ command+=",";}
				command+="CodeNum = "+POut.Long(autoCodeItem.CodeNum)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE autocodeitem SET "+command
				+" WHERE AutoCodeItemNum = "+POut.Long(autoCodeItem.AutoCodeItemNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Deletes one AutoCodeItem from the database.</summary>
		internal static void Delete(long autoCodeItemNum){
			string command="DELETE FROM autocodeitem "
				+"WHERE AutoCodeItemNum = "+POut.Long(autoCodeItemNum)+" LIMIT 1";
			Db.NonQ(command);
		}

	}
}