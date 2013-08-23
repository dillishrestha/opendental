//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EmailAddressCrud {
		///<summary>Gets one EmailAddress object from the database using the primary key.  Returns null if not found.</summary>
		public static EmailAddress SelectOne(long emailAddressNum){
			string command="SELECT * FROM emailaddress "
				+"WHERE EmailAddressNum = "+POut.Long(emailAddressNum);
			List<EmailAddress> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EmailAddress object from the database using a query.</summary>
		public static EmailAddress SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EmailAddress> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EmailAddress objects from the database using a query.</summary>
		public static List<EmailAddress> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EmailAddress> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EmailAddress> TableToList(DataTable table){
			List<EmailAddress> retVal=new List<EmailAddress>();
			EmailAddress emailAddress;
			for(int i=0;i<table.Rows.Count;i++) {
				emailAddress=new EmailAddress();
				emailAddress.EmailAddressNum   = PIn.Long  (table.Rows[i]["EmailAddressNum"].ToString());
				emailAddress.SMTPserver        = PIn.String(table.Rows[i]["SMTPserver"].ToString());
				emailAddress.EmailUsername     = PIn.String(table.Rows[i]["EmailUsername"].ToString());
				emailAddress.EmailPassword     = PIn.String(table.Rows[i]["EmailPassword"].ToString());
				emailAddress.ServerPort        = PIn.Int   (table.Rows[i]["ServerPort"].ToString());
				emailAddress.UseSSL            = PIn.Bool  (table.Rows[i]["UseSSL"].ToString());
				emailAddress.SenderAddress     = PIn.String(table.Rows[i]["SenderAddress"].ToString());
				emailAddress.Pop3ServerIncoming= PIn.String(table.Rows[i]["Pop3ServerIncoming"].ToString());
				emailAddress.ServerPortIncoming= PIn.Int   (table.Rows[i]["ServerPortIncoming"].ToString());
				retVal.Add(emailAddress);
			}
			return retVal;
		}

		///<summary>Inserts one EmailAddress into the database.  Returns the new priKey.</summary>
		public static long Insert(EmailAddress emailAddress){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				emailAddress.EmailAddressNum=DbHelper.GetNextOracleKey("emailaddress","EmailAddressNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(emailAddress,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							emailAddress.EmailAddressNum++;
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
				return Insert(emailAddress,false);
			}
		}

		///<summary>Inserts one EmailAddress into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EmailAddress emailAddress,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				emailAddress.EmailAddressNum=ReplicationServers.GetKey("emailaddress","EmailAddressNum");
			}
			string command="INSERT INTO emailaddress (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EmailAddressNum,";
			}
			command+="SMTPserver,EmailUsername,EmailPassword,ServerPort,UseSSL,SenderAddress,Pop3ServerIncoming,ServerPortIncoming) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(emailAddress.EmailAddressNum)+",";
			}
			command+=
				 "'"+POut.String(emailAddress.SMTPserver)+"',"
				+"'"+POut.String(emailAddress.EmailUsername)+"',"
				+"'"+POut.String(emailAddress.EmailPassword)+"',"
				+    POut.Int   (emailAddress.ServerPort)+","
				+    POut.Bool  (emailAddress.UseSSL)+","
				+"'"+POut.String(emailAddress.SenderAddress)+"',"
				+"'"+POut.String(emailAddress.Pop3ServerIncoming)+"',"
				+    POut.Int   (emailAddress.ServerPortIncoming)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				emailAddress.EmailAddressNum=Db.NonQ(command,true);
			}
			return emailAddress.EmailAddressNum;
		}

		///<summary>Updates one EmailAddress in the database.</summary>
		public static void Update(EmailAddress emailAddress){
			string command="UPDATE emailaddress SET "
				+"SMTPserver        = '"+POut.String(emailAddress.SMTPserver)+"', "
				+"EmailUsername     = '"+POut.String(emailAddress.EmailUsername)+"', "
				+"EmailPassword     = '"+POut.String(emailAddress.EmailPassword)+"', "
				+"ServerPort        =  "+POut.Int   (emailAddress.ServerPort)+", "
				+"UseSSL            =  "+POut.Bool  (emailAddress.UseSSL)+", "
				+"SenderAddress     = '"+POut.String(emailAddress.SenderAddress)+"', "
				+"Pop3ServerIncoming= '"+POut.String(emailAddress.Pop3ServerIncoming)+"', "
				+"ServerPortIncoming=  "+POut.Int   (emailAddress.ServerPortIncoming)+" "
				+"WHERE EmailAddressNum = "+POut.Long(emailAddress.EmailAddressNum);
			Db.NonQ(command);
		}

		///<summary>Updates one EmailAddress in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		public static void Update(EmailAddress emailAddress,EmailAddress oldEmailAddress){
			string command="";
			if(emailAddress.SMTPserver != oldEmailAddress.SMTPserver) {
				if(command!=""){ command+=",";}
				command+="SMTPserver = '"+POut.String(emailAddress.SMTPserver)+"'";
			}
			if(emailAddress.EmailUsername != oldEmailAddress.EmailUsername) {
				if(command!=""){ command+=",";}
				command+="EmailUsername = '"+POut.String(emailAddress.EmailUsername)+"'";
			}
			if(emailAddress.EmailPassword != oldEmailAddress.EmailPassword) {
				if(command!=""){ command+=",";}
				command+="EmailPassword = '"+POut.String(emailAddress.EmailPassword)+"'";
			}
			if(emailAddress.ServerPort != oldEmailAddress.ServerPort) {
				if(command!=""){ command+=",";}
				command+="ServerPort = "+POut.Int(emailAddress.ServerPort)+"";
			}
			if(emailAddress.UseSSL != oldEmailAddress.UseSSL) {
				if(command!=""){ command+=",";}
				command+="UseSSL = "+POut.Bool(emailAddress.UseSSL)+"";
			}
			if(emailAddress.SenderAddress != oldEmailAddress.SenderAddress) {
				if(command!=""){ command+=",";}
				command+="SenderAddress = '"+POut.String(emailAddress.SenderAddress)+"'";
			}
			if(emailAddress.Pop3ServerIncoming != oldEmailAddress.Pop3ServerIncoming) {
				if(command!=""){ command+=",";}
				command+="Pop3ServerIncoming = '"+POut.String(emailAddress.Pop3ServerIncoming)+"'";
			}
			if(emailAddress.ServerPortIncoming != oldEmailAddress.ServerPortIncoming) {
				if(command!=""){ command+=",";}
				command+="ServerPortIncoming = "+POut.Int(emailAddress.ServerPortIncoming)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE emailaddress SET "+command
				+" WHERE EmailAddressNum = "+POut.Long(emailAddress.EmailAddressNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one EmailAddress from the database.</summary>
		public static void Delete(long emailAddressNum){
			string command="DELETE FROM emailaddress "
				+"WHERE EmailAddressNum = "+POut.Long(emailAddressNum);
			Db.NonQ(command);
		}

	}
}