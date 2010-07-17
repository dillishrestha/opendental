//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class ClaimPaymentCrud {
		///<summary>Gets one ClaimPayment object from the database using the primary key.  Returns null if not found.</summary>
		internal static ClaimPayment SelectOne(long claimPaymentNum){
			string command="SELECT * FROM claimpayment "
				+"WHERE ClaimPaymentNum = "+POut.Long(claimPaymentNum)+" LIMIT 1";
			List<ClaimPayment> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ClaimPayment object from the database using a query.</summary>
		internal static ClaimPayment SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ClaimPayment> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ClaimPayment objects from the database using a query.</summary>
		internal static List<ClaimPayment> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ClaimPayment> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<ClaimPayment> TableToList(DataTable table){
			List<ClaimPayment> retVal=new List<ClaimPayment>();
			ClaimPayment claimPayment;
			for(int i=0;i<table.Rows.Count;i++) {
				claimPayment=new ClaimPayment();
				claimPayment.ClaimPaymentNum= PIn.Long  (table.Rows[i]["ClaimPaymentNum"].ToString());
				claimPayment.CheckDate      = PIn.Date  (table.Rows[i]["CheckDate"].ToString());
				claimPayment.CheckAmt       = PIn.Double(table.Rows[i]["CheckAmt"].ToString());
				claimPayment.CheckNum       = PIn.String(table.Rows[i]["CheckNum"].ToString());
				claimPayment.BankBranch     = PIn.String(table.Rows[i]["BankBranch"].ToString());
				claimPayment.Note           = PIn.String(table.Rows[i]["Note"].ToString());
				claimPayment.ClinicNum      = PIn.Long  (table.Rows[i]["ClinicNum"].ToString());
				claimPayment.DepositNum     = PIn.Long  (table.Rows[i]["DepositNum"].ToString());
				claimPayment.CarrierName    = PIn.String(table.Rows[i]["CarrierName"].ToString());
				retVal.Add(claimPayment);
			}
			return retVal;
		}

		///<summary>Inserts one ClaimPayment into the database.  Returns the new priKey.</summary>
		internal static long Insert(ClaimPayment claimPayment){
			return Insert(claimPayment,false);
		}

		///<summary>Inserts one ClaimPayment into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(ClaimPayment claimPayment,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				claimPayment.ClaimPaymentNum=ReplicationServers.GetKey("claimpayment","ClaimPaymentNum");
			}
			string command="INSERT INTO claimpayment (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ClaimPaymentNum,";
			}
			command+="CheckDate,CheckAmt,CheckNum,BankBranch,Note,ClinicNum,DepositNum,CarrierName) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(claimPayment.ClaimPaymentNum)+",";
			}
			command+=
				     POut.Date  (claimPayment.CheckDate)+","
				+"'"+POut.Double(claimPayment.CheckAmt)+"',"
				+"'"+POut.String(claimPayment.CheckNum)+"',"
				+"'"+POut.String(claimPayment.BankBranch)+"',"
				+"'"+POut.String(claimPayment.Note)+"',"
				+    POut.Long  (claimPayment.ClinicNum)+","
				+    POut.Long  (claimPayment.DepositNum)+","
				+"'"+POut.String(claimPayment.CarrierName)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				claimPayment.ClaimPaymentNum=Db.NonQ(command,true);
			}
			return claimPayment.ClaimPaymentNum;
		}

		///<summary>Updates one ClaimPayment in the database.</summary>
		internal static void Update(ClaimPayment claimPayment){
			string command="UPDATE claimpayment SET "
				+"CheckDate      =  "+POut.Date  (claimPayment.CheckDate)+", "
				+"CheckAmt       = '"+POut.Double(claimPayment.CheckAmt)+"', "
				+"CheckNum       = '"+POut.String(claimPayment.CheckNum)+"', "
				+"BankBranch     = '"+POut.String(claimPayment.BankBranch)+"', "
				+"Note           = '"+POut.String(claimPayment.Note)+"', "
				+"ClinicNum      =  "+POut.Long  (claimPayment.ClinicNum)+", "
				+"DepositNum     =  "+POut.Long  (claimPayment.DepositNum)+", "
				+"CarrierName    = '"+POut.String(claimPayment.CarrierName)+"' "
				+"WHERE ClaimPaymentNum = "+POut.Long(claimPayment.ClaimPaymentNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Updates one ClaimPayment in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(ClaimPayment claimPayment,ClaimPayment oldClaimPayment){
			string command="";
			if(claimPayment.CheckDate != oldClaimPayment.CheckDate) {
				if(command!=""){ command+=",";}
				command+="CheckDate = "+POut.Date(claimPayment.CheckDate)+"";
			}
			if(claimPayment.CheckAmt != oldClaimPayment.CheckAmt) {
				if(command!=""){ command+=",";}
				command+="CheckAmt = '"+POut.Double(claimPayment.CheckAmt)+"'";
			}
			if(claimPayment.CheckNum != oldClaimPayment.CheckNum) {
				if(command!=""){ command+=",";}
				command+="CheckNum = '"+POut.String(claimPayment.CheckNum)+"'";
			}
			if(claimPayment.BankBranch != oldClaimPayment.BankBranch) {
				if(command!=""){ command+=",";}
				command+="BankBranch = '"+POut.String(claimPayment.BankBranch)+"'";
			}
			if(claimPayment.Note != oldClaimPayment.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(claimPayment.Note)+"'";
			}
			if(claimPayment.ClinicNum != oldClaimPayment.ClinicNum) {
				if(command!=""){ command+=",";}
				command+="ClinicNum = "+POut.Long(claimPayment.ClinicNum)+"";
			}
			if(claimPayment.DepositNum != oldClaimPayment.DepositNum) {
				if(command!=""){ command+=",";}
				command+="DepositNum = "+POut.Long(claimPayment.DepositNum)+"";
			}
			if(claimPayment.CarrierName != oldClaimPayment.CarrierName) {
				if(command!=""){ command+=",";}
				command+="CarrierName = '"+POut.String(claimPayment.CarrierName)+"'";
			}
			if(command==""){
				return;
			}
			command="UPDATE claimpayment SET "+command
				+" WHERE ClaimPaymentNum = "+POut.Long(claimPayment.ClaimPaymentNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Deletes one ClaimPayment from the database.</summary>
		internal static void Delete(long claimPaymentNum){
			string command="DELETE FROM claimpayment "
				+"WHERE ClaimPaymentNum = "+POut.Long(claimPaymentNum)+" LIMIT 1";
			Db.NonQ(command);
		}

	}
}