//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class ClockEventCrud {
		///<summary>Gets one ClockEvent object from the database using the primary key.  Returns null if not found.</summary>
		internal static ClockEvent SelectOne(long clockEventNum){
			string command="SELECT * FROM clockevent "
				+"WHERE ClockEventNum = "+POut.Long(clockEventNum)+" LIMIT 1";
			List<ClockEvent> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ClockEvent object from the database using a query.</summary>
		internal static ClockEvent SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ClockEvent> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ClockEvent objects from the database using a query.</summary>
		internal static List<ClockEvent> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ClockEvent> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<ClockEvent> TableToList(DataTable table){
			List<ClockEvent> retVal=new List<ClockEvent>();
			ClockEvent clockEvent;
			for(int i=0;i<table.Rows.Count;i++) {
				clockEvent=new ClockEvent();
				clockEvent.ClockEventNum = PIn.Long  (table.Rows[i]["ClockEventNum"].ToString());
				clockEvent.EmployeeNum   = PIn.Long  (table.Rows[i]["EmployeeNum"].ToString());
				clockEvent.TimeEntered1  = PIn.DateT (table.Rows[i]["TimeEntered1"].ToString());
				clockEvent.TimeDisplayed1= PIn.DateT (table.Rows[i]["TimeDisplayed1"].ToString());
				clockEvent.ClockStatus   = (TimeClockStatus)PIn.Int(table.Rows[i]["ClockStatus"].ToString());
				clockEvent.Note          = PIn.String(table.Rows[i]["Note"].ToString());
				clockEvent.TimeEntered2  = PIn.DateT (table.Rows[i]["TimeEntered2"].ToString());
				clockEvent.TimeDisplayed2= PIn.DateT (table.Rows[i]["TimeDisplayed2"].ToString());
				retVal.Add(clockEvent);
			}
			return retVal;
		}

		///<summary>Inserts one ClockEvent into the database.  Returns the new priKey.</summary>
		internal static long Insert(ClockEvent clockEvent){
			return Insert(clockEvent,false);
		}

		///<summary>Inserts one ClockEvent into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(ClockEvent clockEvent,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				clockEvent.ClockEventNum=ReplicationServers.GetKey("clockevent","ClockEventNum");
			}
			string command="INSERT INTO clockevent (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ClockEventNum,";
			}
			command+="EmployeeNum,TimeEntered1,TimeDisplayed1,ClockStatus,Note,TimeEntered2,TimeDisplayed2) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(clockEvent.ClockEventNum)+",";
			}
			command+=
				     POut.Long  (clockEvent.EmployeeNum)+","
				+"NOW(),"
				+"NOW(),"
				+    POut.Int   ((int)clockEvent.ClockStatus)+","
				+"'"+POut.String(clockEvent.Note)+"',"
				+    POut.DateT (clockEvent.TimeEntered2)+","
				+    POut.DateT (clockEvent.TimeDisplayed2)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				clockEvent.ClockEventNum=Db.NonQ(command,true);
			}
			return clockEvent.ClockEventNum;
		}

		///<summary>Updates one ClockEvent in the database.</summary>
		internal static void Update(ClockEvent clockEvent){
			string command="UPDATE clockevent SET "
				+"EmployeeNum   =  "+POut.Long  (clockEvent.EmployeeNum)+", "
				//TimeEntered1 not allowed to change
				+"TimeDisplayed1=  "+POut.DateT (clockEvent.TimeDisplayed1)+", "
				+"ClockStatus   =  "+POut.Int   ((int)clockEvent.ClockStatus)+", "
				+"Note          = '"+POut.String(clockEvent.Note)+"', "
				+"TimeEntered2  =  "+POut.DateT (clockEvent.TimeEntered2)+", "
				+"TimeDisplayed2=  "+POut.DateT (clockEvent.TimeDisplayed2)+" "
				+"WHERE ClockEventNum = "+POut.Long(clockEvent.ClockEventNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Updates one ClockEvent in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(ClockEvent clockEvent,ClockEvent oldClockEvent){
			string command="";
			if(clockEvent.EmployeeNum != oldClockEvent.EmployeeNum) {
				if(command!=""){ command+=",";}
				command+="EmployeeNum = "+POut.Long(clockEvent.EmployeeNum)+"";
			}
			//TimeEntered1 not allowed to change
			if(clockEvent.TimeDisplayed1 != oldClockEvent.TimeDisplayed1) {
				if(command!=""){ command+=",";}
				command+="TimeDisplayed1 = "+POut.DateT(clockEvent.TimeDisplayed1)+"";
			}
			if(clockEvent.ClockStatus != oldClockEvent.ClockStatus) {
				if(command!=""){ command+=",";}
				command+="ClockStatus = "+POut.Int   ((int)clockEvent.ClockStatus)+"";
			}
			if(clockEvent.Note != oldClockEvent.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(clockEvent.Note)+"'";
			}
			if(clockEvent.TimeEntered2 != oldClockEvent.TimeEntered2) {
				if(command!=""){ command+=",";}
				command+="TimeEntered2 = "+POut.DateT(clockEvent.TimeEntered2)+"";
			}
			if(clockEvent.TimeDisplayed2 != oldClockEvent.TimeDisplayed2) {
				if(command!=""){ command+=",";}
				command+="TimeDisplayed2 = "+POut.DateT(clockEvent.TimeDisplayed2)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE clockevent SET "+command
				+" WHERE ClockEventNum = "+POut.Long(clockEvent.ClockEventNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Deletes one ClockEvent from the database.</summary>
		internal static void Delete(long clockEventNum){
			string command="DELETE FROM clockevent "
				+"WHERE ClockEventNum = "+POut.Long(clockEventNum)+" LIMIT 1";
			Db.NonQ(command);
		}

	}
}