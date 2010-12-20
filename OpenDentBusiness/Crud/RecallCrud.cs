//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class RecallCrud {
		///<summary>Gets one Recall object from the database using the primary key.  Returns null if not found.</summary>
		internal static Recall SelectOne(long recallNum){
			string command="SELECT * FROM recall "
				+"WHERE RecallNum = "+POut.Long(recallNum)+" LIMIT 1";
			List<Recall> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Recall object from the database using a query.</summary>
		internal static Recall SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Recall> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Recall objects from the database using a query.</summary>
		internal static List<Recall> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Recall> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<Recall> TableToList(DataTable table){
			List<Recall> retVal=new List<Recall>();
			Recall recall;
			for(int i=0;i<table.Rows.Count;i++) {
				recall=new Recall();
				recall.RecallNum          = PIn.Long  (table.Rows[i]["RecallNum"].ToString());
				recall.PatNum             = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				recall.DateDueCalc        = PIn.Date  (table.Rows[i]["DateDueCalc"].ToString());
				recall.DateDue            = PIn.Date  (table.Rows[i]["DateDue"].ToString());
				recall.DatePrevious       = PIn.Date  (table.Rows[i]["DatePrevious"].ToString());
				recall.RecallInterval     = new Interval(PIn.Int(table.Rows[i]["RecallInterval"].ToString()));
				recall.RecallStatus       = PIn.Long  (table.Rows[i]["RecallStatus"].ToString());
				recall.Note               = PIn.String(table.Rows[i]["Note"].ToString());
				recall.IsDisabled         = PIn.Bool  (table.Rows[i]["IsDisabled"].ToString());
				recall.RecallTypeNum      = PIn.Long  (table.Rows[i]["RecallTypeNum"].ToString());
				recall.DisableUntilBalance= PIn.Double(table.Rows[i]["DisableUntilBalance"].ToString());
				recall.DisableUntilDate   = PIn.Date  (table.Rows[i]["DisableUntilDate"].ToString());
				retVal.Add(recall);
			}
			return retVal;
		}

		///<summary>Inserts one Recall into the database.  Returns the new priKey.</summary>
		internal static long Insert(Recall recall){
			return Insert(recall,false);
		}

		///<summary>Inserts one Recall into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(Recall recall,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				recall.RecallNum=ReplicationServers.GetKey("recall","RecallNum");
			}
			string command="INSERT INTO recall (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="RecallNum,";
			}
			command+="PatNum,DateDueCalc,DateDue,DatePrevious,RecallInterval,RecallStatus,Note,IsDisabled,RecallTypeNum,DisableUntilBalance,DisableUntilDate) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(recall.RecallNum)+",";
			}
			command+=
				     POut.Long  (recall.PatNum)+","
				+    POut.Date  (recall.DateDueCalc)+","
				+    POut.Date  (recall.DateDue)+","
				+    POut.Date  (recall.DatePrevious)+","
				+    POut.Int   (recall.RecallInterval.ToInt())+","
				+    POut.Long  (recall.RecallStatus)+","
				+"'"+POut.String(recall.Note)+"',"
				+    POut.Bool  (recall.IsDisabled)+","
				+    POut.Long  (recall.RecallTypeNum)+","
				+"'"+POut.Double(recall.DisableUntilBalance)+"',"
				+    POut.Date  (recall.DisableUntilDate)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				recall.RecallNum=Db.NonQ(command,true);
			}
			return recall.RecallNum;
		}

		///<summary>Updates one Recall in the database.</summary>
		internal static void Update(Recall recall){
			string command="UPDATE recall SET "
				+"PatNum             =  "+POut.Long  (recall.PatNum)+", "
				+"DateDueCalc        =  "+POut.Date  (recall.DateDueCalc)+", "
				+"DateDue            =  "+POut.Date  (recall.DateDue)+", "
				+"DatePrevious       =  "+POut.Date  (recall.DatePrevious)+", "
				+"RecallInterval     =  "+POut.Int   (recall.RecallInterval.ToInt())+", "
				+"RecallStatus       =  "+POut.Long  (recall.RecallStatus)+", "
				+"Note               = '"+POut.String(recall.Note)+"', "
				+"IsDisabled         =  "+POut.Bool  (recall.IsDisabled)+", "
				+"RecallTypeNum      =  "+POut.Long  (recall.RecallTypeNum)+", "
				+"DisableUntilBalance= '"+POut.Double(recall.DisableUntilBalance)+"', "
				+"DisableUntilDate   =  "+POut.Date  (recall.DisableUntilDate)+" "
				+"WHERE RecallNum = "+POut.Long(recall.RecallNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Updates one Recall in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(Recall recall,Recall oldRecall){
			string command="";
			if(recall.PatNum != oldRecall.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(recall.PatNum)+"";
			}
			if(recall.DateDueCalc != oldRecall.DateDueCalc) {
				if(command!=""){ command+=",";}
				command+="DateDueCalc = "+POut.Date(recall.DateDueCalc)+"";
			}
			if(recall.DateDue != oldRecall.DateDue) {
				if(command!=""){ command+=",";}
				command+="DateDue = "+POut.Date(recall.DateDue)+"";
			}
			if(recall.DatePrevious != oldRecall.DatePrevious) {
				if(command!=""){ command+=",";}
				command+="DatePrevious = "+POut.Date(recall.DatePrevious)+"";
			}
			if(recall.RecallInterval != oldRecall.RecallInterval) {
				if(command!=""){ command+=",";}
				command+="RecallInterval = "+POut.Int(recall.RecallInterval.ToInt())+"";
			}
			if(recall.RecallStatus != oldRecall.RecallStatus) {
				if(command!=""){ command+=",";}
				command+="RecallStatus = "+POut.Long(recall.RecallStatus)+"";
			}
			if(recall.Note != oldRecall.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(recall.Note)+"'";
			}
			if(recall.IsDisabled != oldRecall.IsDisabled) {
				if(command!=""){ command+=",";}
				command+="IsDisabled = "+POut.Bool(recall.IsDisabled)+"";
			}
			if(recall.RecallTypeNum != oldRecall.RecallTypeNum) {
				if(command!=""){ command+=",";}
				command+="RecallTypeNum = "+POut.Long(recall.RecallTypeNum)+"";
			}
			if(recall.DisableUntilBalance != oldRecall.DisableUntilBalance) {
				if(command!=""){ command+=",";}
				command+="DisableUntilBalance = '"+POut.Double(recall.DisableUntilBalance)+"'";
			}
			if(recall.DisableUntilDate != oldRecall.DisableUntilDate) {
				if(command!=""){ command+=",";}
				command+="DisableUntilDate = "+POut.Date(recall.DisableUntilDate)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE recall SET "+command
				+" WHERE RecallNum = "+POut.Long(recall.RecallNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Deletes one Recall from the database.</summary>
		internal static void Delete(long recallNum){
			string command="DELETE FROM recall "
				+"WHERE RecallNum = "+POut.Long(recallNum)+" LIMIT 1";
			Db.NonQ(command);
		}

	}
}