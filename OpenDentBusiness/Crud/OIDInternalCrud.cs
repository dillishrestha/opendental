//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class OIDInternalCrud {
		///<summary>Gets one OIDInternal object from the database using the primary key.  Returns null if not found.</summary>
		public static OIDInternal SelectOne(long ehrOIDNum){
			string command="SELECT * FROM oidinternal "
				+"WHERE EhrOIDNum = "+POut.Long(ehrOIDNum);
			List<OIDInternal> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one OIDInternal object from the database using a query.</summary>
		public static OIDInternal SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<OIDInternal> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of OIDInternal objects from the database using a query.</summary>
		public static List<OIDInternal> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<OIDInternal> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<OIDInternal> TableToList(DataTable table){
			List<OIDInternal> retVal=new List<OIDInternal>();
			OIDInternal oIDInternal;
			for(int i=0;i<table.Rows.Count;i++) {
				oIDInternal=new OIDInternal();
				oIDInternal.EhrOIDNum= PIn.Long  (table.Rows[i]["EhrOIDNum"].ToString());
				string iDType=table.Rows[i]["IDType"].ToString();
				if(iDType==""){
					oIDInternal.IDType =(IdentifierType)0;
				}
				else try{
					oIDInternal.IDType =(IdentifierType)Enum.Parse(typeof(IdentifierType),iDType);
				}
				catch{
					oIDInternal.IDType =(IdentifierType)0;
				}
				oIDInternal.IDRoot   = PIn.String(table.Rows[i]["IDRoot"].ToString());
				retVal.Add(oIDInternal);
			}
			return retVal;
		}

		///<summary>Inserts one OIDInternal into the database.  Returns the new priKey.</summary>
		public static long Insert(OIDInternal oIDInternal){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				oIDInternal.EhrOIDNum=DbHelper.GetNextOracleKey("oidinternal","EhrOIDNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(oIDInternal,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							oIDInternal.EhrOIDNum++;
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
				return Insert(oIDInternal,false);
			}
		}

		///<summary>Inserts one OIDInternal into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(OIDInternal oIDInternal,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				oIDInternal.EhrOIDNum=ReplicationServers.GetKey("oidinternal","EhrOIDNum");
			}
			string command="INSERT INTO oidinternal (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EhrOIDNum,";
			}
			command+="IDType,IDRoot) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(oIDInternal.EhrOIDNum)+",";
			}
			command+=
				 "'"+POut.String(oIDInternal.IDType.ToString())+"',"
				+"'"+POut.String(oIDInternal.IDRoot)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				oIDInternal.EhrOIDNum=Db.NonQ(command,true);
			}
			return oIDInternal.EhrOIDNum;
		}

		///<summary>Updates one OIDInternal in the database.</summary>
		public static void Update(OIDInternal oIDInternal){
			string command="UPDATE oidinternal SET "
				+"IDType   = '"+POut.String(oIDInternal.IDType.ToString())+"', "
				+"IDRoot   = '"+POut.String(oIDInternal.IDRoot)+"' "
				+"WHERE EhrOIDNum = "+POut.Long(oIDInternal.EhrOIDNum);
			Db.NonQ(command);
		}

		///<summary>Updates one OIDInternal in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		public static void Update(OIDInternal oIDInternal,OIDInternal oldOIDInternal){
			string command="";
			if(oIDInternal.IDType != oldOIDInternal.IDType) {
				if(command!=""){ command+=",";}
				command+="IDType = '"+POut.String(oIDInternal.IDType.ToString())+"'";
			}
			if(oIDInternal.IDRoot != oldOIDInternal.IDRoot) {
				if(command!=""){ command+=",";}
				command+="IDRoot = '"+POut.String(oIDInternal.IDRoot)+"'";
			}
			if(command==""){
				return;
			}
			command="UPDATE oidinternal SET "+command
				+" WHERE EhrOIDNum = "+POut.Long(oIDInternal.EhrOIDNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one OIDInternal from the database.</summary>
		public static void Delete(long ehrOIDNum){
			string command="DELETE FROM oidinternal "
				+"WHERE EhrOIDNum = "+POut.Long(ehrOIDNum);
			Db.NonQ(command);
		}

	}
}