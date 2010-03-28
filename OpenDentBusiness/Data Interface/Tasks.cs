using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary>Not part of cache refresh.</summary>
	public class Tasks {
		///<summary>Only used from UI.</summary>
		public static ArrayList LastOpenList;
		///<summary>Only used from UI.</summary>
		public static int LastOpenGroup;
		///<summary>Only used from UI.</summary>
		public static DateTime LastOpenDate;

		/*
		///<summary>There are NO tasks on the user trunk, so this is not needed.</summary>
		public static List<Task> RefreshUserTrunk(int userNum) {
			string command="SELECT task.* FROM tasksubscription "
				+"LEFT JOIN task ON task.TaskNum=tasksubscription.TaskNum "
				+"WHERE tasksubscription.UserNum="+POut.PInt(userNum)
				+" AND tasksubscription.TaskNum!=0 "
				+"ORDER BY DateTimeEntry";
			return RefreshAndFill(command);
		}*/

		///<summary>Gets one Task from database.</summary>
		public static Task GetOne(long TaskNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Task>(MethodBase.GetCurrentMethod(),TaskNum);
			}
			string command=
				"SELECT * FROM task"
				+" WHERE TaskNum = "+POut.Long(TaskNum);
			List<Task> taskList=RefreshAndFill(Db.GetTable(command));
			if(taskList.Count==0) {
				return null;
			}
			return taskList[0];
		}

		///<summary>Gets all tasks for the main trunk.</summary>
		public static List<Task> RefreshMainTrunk(bool showDone,DateTime startDate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod(),showDone,startDate);
			}
			//startDate only applies if showing Done tasks.
			string command="SELECT * FROM task "
				+"WHERE TaskListNum=0 "
				+"AND DateTask < '1880-01-01' "
				+"AND IsRepeating=0";
			if(showDone){
				command+=" AND (TaskStatus !="+POut.Long((int)TaskStatusEnum.Done)
					+" OR DateTimeFinished > "+POut.Date(startDate)+")";//of if done, then restrict date
			}
			else{
				command+=" AND TaskStatus !="+POut.Long((int)TaskStatusEnum.Done);
			}
			command+=" ORDER BY DateTimeEntry";
			return RefreshAndFill(Db.GetTable(command));
		}

		///<summary>Gets all tasks for the repeating trunk.  Always includes "done".</summary>
		public static List<Task> RefreshRepeatingTrunk() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM task "
				+"WHERE TaskListNum=0 "
				+"AND DateTask < '1880-01-01' "
				+"AND IsRepeating=1 "
				+"ORDER BY DateTimeEntry";
			return RefreshAndFill(Db.GetTable(command));
		}

		///<summary>0 is not allowed, because that would be a trunk.</summary>
		public static List<Task> RefreshChildren(long listNum,bool showDone,DateTime startDate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod(),listNum,showDone,startDate);
			}
			//startDate only applies if showing Done tasks.
			string command=
				"SELECT * FROM task "
				+"WHERE TaskListNum="+POut.Long(listNum);
			if(showDone){
				command+=" AND (TaskStatus !="+POut.Long((int)TaskStatusEnum.Done)
					+" OR DateTimeFinished > "+POut.Date(startDate)+")";//of if done, then restrict date
			}
			else{
				command+=" AND TaskStatus !="+POut.Long((int)TaskStatusEnum.Done);
			}
			command+=" ORDER BY DateTimeEntry";
			return RefreshAndFill(Db.GetTable(command));
		}

		///<summary>All repeating items for one date type with no heirarchy.</summary>
		public static List<Task> RefreshRepeating(TaskDateType dateType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod(),dateType);
			}
			string command=
				"SELECT * FROM task "
				+"WHERE IsRepeating=1 "
				+"AND DateType="+POut.Long((int)dateType)+" "
				+"ORDER BY DateTimeEntry";
			return RefreshAndFill(Db.GetTable(command));
		}

		///<summary>Gets all tasks for one of the 3 dated trunks. startDate only applies if showing Done.</summary>
		public static List<Task> RefreshDatedTrunk(DateTime date,TaskDateType dateType,bool showDone,DateTime startDate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod(),date,dateType,showDone,startDate);
			}
			DateTime dateFrom=DateTime.MinValue;
			DateTime dateTo=DateTime.MaxValue;
			if(dateType==TaskDateType.Day) {
				dateFrom=date;
				dateTo=date;
			}
			else if(dateType==TaskDateType.Week) {
				dateFrom=date.AddDays(-(int)date.DayOfWeek);
				dateTo=dateFrom.AddDays(6);
			}
			else if(dateType==TaskDateType.Month) {
				dateFrom=new DateTime(date.Year,date.Month,1);
				dateTo=dateFrom.AddMonths(1).AddDays(-1);
			}
			string command=
				"SELECT * FROM task "
				+"WHERE DateTask >= "+POut.Date(dateFrom)
				+" AND DateTask <= "+POut.Date(dateTo)
				+" AND DateType="+POut.Long((int)dateType);
			if(showDone){
				command+=" AND (TaskStatus !="+POut.Long((int)TaskStatusEnum.Done)
					+" OR DateTimeFinished > "+POut.Date(startDate)+")";//of if done, then restrict date
			}
			else{
				command+=" AND TaskStatus !="+POut.Long((int)TaskStatusEnum.Done);
			}
			command+=" ORDER BY DateTimeEntry";
			return RefreshAndFill(Db.GetTable(command));
		}

		///<summary>Only used once when first synching all the tasks for taskAncestors.  Then, never used again.</summary>
		public static List<Task> RefreshAll(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM task WHERE TaskListNum != 0";
			return RefreshAndFill(Db.GetTable(command));
		}

		public static List<Task> RefreshAndFill(DataTable table){
			//No need to check RemotingRole; no call to db.
			List<Task> retVal=new List<Task>();
			Task task;
			for(int i=0;i<table.Rows.Count;i++) {
				task=new Task();
				task.TaskNum        = PIn.Long(table.Rows[i][0].ToString());
				task.TaskListNum    = PIn.Long(table.Rows[i][1].ToString());
				task.DateTask       = PIn.Date(table.Rows[i][2].ToString());
				task.KeyNum         = PIn.Long(table.Rows[i][3].ToString());
				task.Descript       = PIn.String(table.Rows[i][4].ToString());
				task.TaskStatus     = (TaskStatusEnum)PIn.Long(table.Rows[i][5].ToString());
				task.IsRepeating    = PIn.Bool(table.Rows[i][6].ToString());
				task.DateType       = (TaskDateType)PIn.Long(table.Rows[i][7].ToString());
				task.FromNum        = PIn.Long(table.Rows[i][8].ToString());
				task.ObjectType     = (TaskObjectType)PIn.Long(table.Rows[i][9].ToString());
				task.DateTimeEntry  = PIn.DateT(table.Rows[i][10].ToString());
				task.UserNum        = PIn.Long(table.Rows[i][11].ToString());
				task.DateTimeFinished= PIn.DateT(table.Rows[i][12].ToString());
				retVal.Add(task);
			}
			return retVal;
		}

		///<summary>Must supply the supposedly unaltered oldTask.  The update will fail if oldTask does not exactly match the database state.  Keeps users from overwriting each other's changes.</summary>
		public static void Update(Task task,Task oldTask){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),task,oldTask);
				return;
			}
			if(task.IsRepeating && task.DateTask.Year>1880) {
				throw new Exception(Lans.g("Tasks","Task cannot be tagged repeating and also have a date."));
			}
			if(task.IsRepeating && task.TaskStatus!=TaskStatusEnum.New) {//and any status but new
				throw new Exception(Lans.g("Tasks","Tasks that are repeating must have a status of New."));
			}
			if(task.IsRepeating && task.TaskListNum!=0 && task.DateType!=TaskDateType.None) {//In repeating, children not allowed to repeat.
				throw new Exception(Lans.g("Tasks","In repeating tasks, only the main parents can have a task status."));
			}
			if(WasTaskAltered(oldTask)){
				throw new Exception(Lans.g("Tasks","Not allowed to save changes because the task has been altered by someone else."));
			}
			string command= "UPDATE task SET " 
				+"TaskListNum = '"    +POut.Long   (task.TaskListNum)+"'"
				+",DateTask = "       +POut.Date  (task.DateTask)
				+",KeyNum = '"        +POut.Long   (task.KeyNum)+"'"
				+",Descript = '"      +POut.String(task.Descript)+"'"
				+",TaskStatus = '"    +POut.Long   ((int)task.TaskStatus)+"'"
				+",IsRepeating = '"   +POut.Bool  (task.IsRepeating)+"'"
				+",DateType = '"      +POut.Long   ((int)task.DateType)+"'"
				+",FromNum = '"       +POut.Long   (task.FromNum)+"'"
				+",ObjectType = '"    +POut.Long   ((int)task.ObjectType)+"'"
				+",DateTimeEntry = "  +POut.DateT (task.DateTimeEntry)
				+",UserNum = '"       +POut.Long   (task.UserNum)+"'"
				+",DateTimeFinished ="+POut.DateT (task.DateTimeFinished)
				+" WHERE TaskNum = '" +POut.Long(task.TaskNum)+"'";
 			Db.NonQ(command);
			//need to optimize this later to skip unless TaskListNumChanged
			TaskAncestors.Synch(task);
		}

		///<summary></summary>
		public static long Insert(Task task) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				task.TaskNum=Meth.GetLong(MethodBase.GetCurrentMethod(),task);
				return task.TaskNum;
			}
			if(task.IsRepeating && task.DateTask.Year>1880) {
				throw new Exception(Lans.g("Tasks","Task cannot be tagged repeating and also have a date."));
			}
			if(task.IsRepeating && task.TaskStatus!=TaskStatusEnum.New) {//and any status but new
				throw new Exception(Lans.g("Tasks","Tasks that are repeating must have a status of New."));
			}
			if(task.IsRepeating && task.TaskListNum!=0 && task.DateType!=TaskDateType.None) {//In repeating, children not allowed to repeat.
				throw new Exception(Lans.g("Tasks","In repeating tasks, only the main parents can have a task status."));
			}
			if(PrefC.RandomKeys){
				task.TaskNum=ReplicationServers.GetKey("task","TaskNum");
			}
			string command= "INSERT INTO task (";
			if(PrefC.RandomKeys){
				command+="TaskNum,";
			}
			command+="TaskListNum,DateTask,KeyNum,Descript,TaskStatus,"
				+"IsRepeating,DateType,FromNum,ObjectType,DateTimeEntry,UserNum,DateTimeFinished) VALUES(";
			if(PrefC.RandomKeys){
				command+="'"+POut.Long(task.TaskNum)+"', ";
			}
			command+=
				 "'"+POut.Long   (task.TaskListNum)+"', "
				+POut.Date  (task.DateTask)+", "
				+"'"+POut.Long   (task.KeyNum)+"', "
				+"'"+POut.String(task.Descript)+"', "
				+"'"+POut.Long   ((int)task.TaskStatus)+"', "
				+"'"+POut.Bool  (task.IsRepeating)+"', "
				+"'"+POut.Long   ((int)task.DateType)+"', "
				+"'"+POut.Long   (task.FromNum)+"', "
				+"'"+POut.Long   ((int)task.ObjectType)+"', "
				+POut.DateT (task.DateTimeEntry)+","
				+"'"+POut.Long   (task.UserNum)+"',"
				+POut.DateT (task.DateTimeFinished)+")";
 			if(PrefC.RandomKeys){
				Db.NonQ(command);
			}
			else{
 				task.TaskNum=Db.NonQ(command,true);
			}
			TaskAncestors.Synch(task);
			return task.TaskNum;
		}

		///<summary></summary>
		public static bool WasTaskAltered(Task task){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),task);
			}
			string command="SELECT * FROM task WHERE TaskNum="+POut.Long(task.TaskNum);
			Task oldtask=RefreshAndFill(Db.GetTable(command))[0];
			if(oldtask.DateTask!=task.DateTask
					|| oldtask.DateType!=task.DateType
					|| oldtask.Descript!=task.Descript
					|| oldtask.FromNum!=task.FromNum
					|| oldtask.IsRepeating!=task.IsRepeating
					|| oldtask.KeyNum!=task.KeyNum
					|| oldtask.ObjectType!=task.ObjectType
					|| oldtask.TaskListNum!=task.TaskListNum
					|| oldtask.TaskStatus!=task.TaskStatus
					|| oldtask.UserNum!=task.UserNum
					|| oldtask.DateTimeEntry!=task.DateTimeEntry
					|| oldtask.DateTimeFinished!=task.DateTimeFinished)
			{
				return true;
			}
			return false;
		}

		///<summary>Deleting a task never causes a problem, so no dependencies are checked.</summary>
		public static void Delete(Task task){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),task);
				return;
			}
			string command= "DELETE from task WHERE TaskNum = "+POut.Long(task.TaskNum);
 			Db.NonQ(command);
			command="DELETE from taskancestor WHERE TaskNum = "+POut.Long(task.TaskNum);
			Db.NonQ(command);
		}

		///<summary>Gets a count of New tasks to notify user when first logging in.</summary>
		public static int UserTasksCount(long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT COUNT(*) FROM taskancestor,task,tasklist,tasksubscription "
				+"WHERE taskancestor.TaskListNum=tasklist.TaskListNum "
				+"AND task.TaskNum=taskancestor.TaskNum "
				+"AND tasksubscription.TaskListNum=tasklist.TaskListNum "
				+"AND tasksubscription.UserNum="+POut.Long(userNum)
				+" AND task.TaskStatus="+POut.Long((int)TaskStatusEnum.New);
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Appends a carriage return as well as the text to any task.  If a taskListNum is specified, then it also changes the taskList.</summary>
		public static void Append(long taskNum,string text) {
			//No need to check RemotingRole; no call to db.
			Append(taskNum,text,-1);
		}

		///<summary>Appends a carriage return as well as the text to any task.  If a taskListNum is specified, then it also changes the taskList.</summary>
		public static void Append(long taskNum,string text,long taskListNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),taskNum,text,taskListNum);
				return;
			}
			string command;
			if(taskListNum==-1) {
				command="UPDATE task SET Descript=CONCAT(Descript,'"+POut.String("\r\n"+text)+"') WHERE TaskNum="+POut.Long(taskNum);
			}
			else {
				command="UPDATE task SET Descript=CONCAT(Descript,'"+POut.String("\r\n"+text)+"'), "
					+"TaskListNum="+POut.Long(taskListNum)+" "
					+"WHERE TaskNum="+POut.Long(taskNum);
			}
			Db.NonQ(command);
		}
	
	

	
	}

	

	


}




















