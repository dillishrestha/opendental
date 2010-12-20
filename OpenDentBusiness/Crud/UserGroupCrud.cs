//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class UserGroupCrud {
		///<summary>Gets one UserGroup object from the database using the primary key.  Returns null if not found.</summary>
		internal static UserGroup SelectOne(long userGroupNum){
			string command="SELECT * FROM usergroup "
				+"WHERE UserGroupNum = "+POut.Long(userGroupNum)+" LIMIT 1";
			List<UserGroup> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one UserGroup object from the database using a query.</summary>
		internal static UserGroup SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<UserGroup> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of UserGroup objects from the database using a query.</summary>
		internal static List<UserGroup> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<UserGroup> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<UserGroup> TableToList(DataTable table){
			List<UserGroup> retVal=new List<UserGroup>();
			UserGroup userGroup;
			for(int i=0;i<table.Rows.Count;i++) {
				userGroup=new UserGroup();
				userGroup.UserGroupNum= PIn.Long  (table.Rows[i]["UserGroupNum"].ToString());
				userGroup.Description = PIn.String(table.Rows[i]["Description"].ToString());
				retVal.Add(userGroup);
			}
			return retVal;
		}

		///<summary>Inserts one UserGroup into the database.  Returns the new priKey.</summary>
		internal static long Insert(UserGroup userGroup){
			return Insert(userGroup,false);
		}

		///<summary>Inserts one UserGroup into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(UserGroup userGroup,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				userGroup.UserGroupNum=ReplicationServers.GetKey("usergroup","UserGroupNum");
			}
			string command="INSERT INTO usergroup (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="UserGroupNum,";
			}
			command+="Description) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(userGroup.UserGroupNum)+",";
			}
			command+=
				 "'"+POut.String(userGroup.Description)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				userGroup.UserGroupNum=Db.NonQ(command,true);
			}
			return userGroup.UserGroupNum;
		}

		///<summary>Updates one UserGroup in the database.</summary>
		internal static void Update(UserGroup userGroup){
			string command="UPDATE usergroup SET "
				+"Description = '"+POut.String(userGroup.Description)+"' "
				+"WHERE UserGroupNum = "+POut.Long(userGroup.UserGroupNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Updates one UserGroup in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(UserGroup userGroup,UserGroup oldUserGroup){
			string command="";
			if(userGroup.Description != oldUserGroup.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(userGroup.Description)+"'";
			}
			if(command==""){
				return;
			}
			command="UPDATE usergroup SET "+command
				+" WHERE UserGroupNum = "+POut.Long(userGroup.UserGroupNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Deletes one UserGroup from the database.</summary>
		internal static void Delete(long userGroupNum){
			string command="DELETE FROM usergroup "
				+"WHERE UserGroupNum = "+POut.Long(userGroupNum)+" LIMIT 1";
			Db.NonQ(command);
		}

	}
}