using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ClaimPayments {

		///<summary></summary>
		public static DataTable GetForClaim(int claimNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),claimNum);
			}
			DataTable table=new DataTable();
			DataRow row;
			table.Columns.Add("amount");
			table.Columns.Add("BankBranch");
			table.Columns.Add("ClaimPaymentNum");
			table.Columns.Add("checkDate");
			table.Columns.Add("CheckNum");
			table.Columns.Add("Note");
			List<DataRow> rows=new List<DataRow>();
			string command="SELECT BankBranch,claimpayment.ClaimPaymentNum,CheckNum,CheckDate,"
				+"SUM(claimproc.InsPayAmt) amount,Note "
				+"FROM claimpayment,claimproc "
				+"WHERE claimpayment.ClaimPaymentNum = claimproc.ClaimPaymentNum "
				+"AND claimproc.ClaimNum = '"+POut.PInt(claimNum)+"' "
				+"GROUP BY claimpayment.ClaimPaymentNum, BankBranch, CheckDate, CheckNum, Note";//required by Oracle
			DataTable rawT=Db.GetTable(command);
			DateTime date;
			for(int i=0;i<rawT.Rows.Count;i++) {
				row=table.NewRow();
				row["amount"]=PIn.PDouble(rawT.Rows[i]["amount"].ToString()).ToString("F");
				row["BankBranch"]=rawT.Rows[i]["BankBranch"].ToString();
				row["ClaimPaymentNum"]=rawT.Rows[i]["ClaimPaymentNum"].ToString();
				date=PIn.PDate(rawT.Rows[i]["CheckDate"].ToString());
				row["checkDate"]=date.ToShortDateString();
				row["CheckNum"]=rawT.Rows[i]["CheckNum"].ToString();
				row["Note"]=rawT.Rows[i]["Note"].ToString();
				rows.Add(row);
			}
			for(int i=0;i<rows.Count;i++) {
				table.Rows.Add(rows[i]);
			}
			return table;
		}

		///<summary>Gets all unattached claimpayments for display in a new deposit.  Excludes payments before dateStart.</summary>
		public static ClaimPayment[] GetForDeposit(DateTime dateStart,int clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ClaimPayment[]>(MethodBase.GetCurrentMethod(),dateStart,clinicNum);
			}
			string command=
				"SELECT ClaimPaymentNum,CheckDate,CheckAmt,"
				+"Checknum,BankBranch,Note,"
				+"ClinicNum,DepositNum,CarrierName "
				+"FROM claimpayment "
				+"WHERE DepositNum = 0 "
				+"AND CheckDate >= "+POut.PDate(dateStart);
			if(clinicNum!=0){
				command+=" AND ClinicNum="+POut.PInt(clinicNum);
			}
			return RefreshAndFill(Db.GetTable(command));
		}

		///<summary>Gets all claimpayments for one specific deposit.</summary>
		public static ClaimPayment[] GetForDeposit(int depositNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ClaimPayment[]>(MethodBase.GetCurrentMethod(),depositNum);
			}
			string command=
				"SELECT ClaimPaymentNum,CheckDate,CheckAmt,"
				+"Checknum,BankBranch,Note,"
				+"ClinicNum,DepositNum,CarrierName "
				+"FROM claimpayment "
				+"WHERE DepositNum = "+POut.PInt(depositNum);
			return RefreshAndFill(Db.GetTable(command));
		}

		///<summary>Gets one claimpayment directly from database.</summary>
		public static ClaimPayment GetOne(int claimPaymentNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ClaimPayment>(MethodBase.GetCurrentMethod(),claimPaymentNum);
			}
			string command=
				"SELECT * FROM claimpayment "
				+"WHERE ClaimPaymentNum = "+POut.PInt(claimPaymentNum);
			return RefreshAndFill(Db.GetTable(command))[0];
		}

		private static ClaimPayment[] RefreshAndFill(DataTable table) {
			//No need to check RemotingRole; no call to db.
			ClaimPayment[] List=new ClaimPayment[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				List[i]=new ClaimPayment();
				List[i].ClaimPaymentNum= PIn.PInt(table.Rows[i][0].ToString());
				List[i].CheckDate      = PIn.PDate(table.Rows[i][1].ToString());
				List[i].CheckAmt       = PIn.PDouble(table.Rows[i][2].ToString());
				List[i].CheckNum       = PIn.PString(table.Rows[i][3].ToString());
				List[i].BankBranch     = PIn.PString(table.Rows[i][4].ToString());
				List[i].Note           = PIn.PString(table.Rows[i][5].ToString());
				List[i].ClinicNum      = PIn.PInt(table.Rows[i][6].ToString());
				List[i].DepositNum     = PIn.PInt(table.Rows[i][7].ToString());
				List[i].CarrierName    = PIn.PString(table.Rows[i][8].ToString());
			}
			return List;
		}


		///<summary></summary>
		public static int Insert(ClaimPayment cp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				cp.ClaimPaymentNum=Meth.GetInt(MethodBase.GetCurrentMethod(),cp);
				return cp.ClaimPaymentNum;
			}
			if(PrefC.RandomKeys){
				cp.ClaimPaymentNum=MiscData.GetKey("claimpayment","ClaimPaymentNum");
			}
			string command= "INSERT INTO claimpayment (";
			if(PrefC.RandomKeys){
				command+="ClaimPaymentNum,";
			}
			command+="CheckDate,CheckAmt,CheckNum,"
				+"BankBranch,Note,ClinicNum,DepositNum,CarrierName) VALUES(";
			if(PrefC.RandomKeys){
				command+="'"+POut.PInt(cp.ClaimPaymentNum)+"', ";
			}
			command+=
				 POut.PDate  (cp.CheckDate)+", "
				+"'"+POut.PDouble(cp.CheckAmt)+"', "
				+"'"+POut.PString(cp.CheckNum)+"', "
				+"'"+POut.PString(cp.BankBranch)+"', "
				+"'"+POut.PString(cp.Note)+"', "
				+"'"+POut.PInt   (cp.ClinicNum)+"', "
				+"'"+POut.PInt   (cp.DepositNum)+"', "
				+"'"+POut.PString(cp.CarrierName)+"')";
			if(PrefC.RandomKeys){
				Db.NonQ(command);
			}
			else{
 				cp.ClaimPaymentNum=Db.NonQ(command,true);
			}
			return cp.ClaimPaymentNum;
		}

		///<summary>If trying to change the amount and attached to a deposit, it will throw an error, so surround with try catch.</summary>
		public static void Update(ClaimPayment cp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),cp);
				return;
			}
			string command="SELECT DepositNum,CheckAmt FROM claimpayment "
				+"WHERE ClaimPaymentNum="+POut.PInt(cp.ClaimPaymentNum);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){
				return;
			}
			if(table.Rows[0][0].ToString()!="0"//if claimpayment is already attached to a deposit
				&& PIn.PDouble(table.Rows[0][1].ToString())!=cp.CheckAmt)//and checkAmt changes
			{
				throw new ApplicationException(Lans.g("ClaimPayments","Not allowed to change the amount on checks attached to deposits."));
			}
			command="UPDATE claimpayment SET "
				+"checkdate = "   +POut.PDate  (cp.CheckDate)+" "
				+",checkamt = '"   +POut.PDouble(cp.CheckAmt)+"' "
				+",checknum = '"   +POut.PString(cp.CheckNum)+"' "
				+",bankbranch = '" +POut.PString(cp.BankBranch)+"' "
				+",note = '"       +POut.PString(cp.Note)+"' "
				+",ClinicNum = '"  +POut.PInt   (cp.ClinicNum)+"' "
				+",DepositNum = '" +POut.PInt   (cp.DepositNum)+"' "
				+",CarrierName = '"+POut.PString(cp.CarrierName)+"' "
				+"WHERE ClaimPaymentnum = '"+POut.PInt   (cp.ClaimPaymentNum)+"'";
			//MessageBox.Show(string command);
 			Db.NonQ(command);
		}

		///<summary>Surround by try catch, because it will throw an exception if trying to delete a claimpayment attached to a deposit.</summary>
		public static void Delete(ClaimPayment cp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),cp);
				return;
			}
			string command="SELECT DepositNum FROM claimpayment "
				+"WHERE ClaimPaymentNum="+POut.PInt(cp.ClaimPaymentNum);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){
				return;
			}
			if(table.Rows[0][0].ToString()!="0"){//if claimpayment is already attached to a deposit
				#if !DEBUG
				throw new ApplicationException(Lans.g("ClaimPayments","Not allowed to delete a payment attached to a deposit."));
				#endif
			}
			command= "UPDATE claimproc SET "
				+"ClaimPaymentNum=0 "
				+"WHERE claimpaymentNum="+POut.PInt(cp.ClaimPaymentNum);
			//MessageBox.Show(string command);
			Db.NonQ(command);
			command= "DELETE FROM claimpayment "
				+"WHERE ClaimPaymentnum ="+POut.PInt(cp.ClaimPaymentNum);
			//MessageBox.Show(string command);
 			Db.NonQ(command);
		}


	
		
	}

	

	


}









