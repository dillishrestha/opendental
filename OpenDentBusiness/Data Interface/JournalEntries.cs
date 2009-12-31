using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class JournalEntries {

		///<summary>Used when displaying the splits for a transaction.</summary>
		public static ArrayList GetForTrans(long transactionNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ArrayList>(MethodBase.GetCurrentMethod(),transactionNum);
			}
			string command=
				"SELECT * FROM journalentry "
				+"WHERE TransactionNum="+POut.Long(transactionNum);
			List<JournalEntry> List=RefreshAndFill(Db.GetTable(command));
			ArrayList retVal=new ArrayList();
			for(int i=0;i<List.Count;i++) {
				retVal.Add(List[i]);
			}
			return retVal;
		}

		///<summary>Used to display a list of entries for one account.</summary>
		public static List <JournalEntry> GetForAccount(long accountNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List <JournalEntry>>(MethodBase.GetCurrentMethod(),accountNum);
			}
			string command=
				"SELECT * FROM journalentry "
				+"WHERE AccountNum="+POut.Long(accountNum)
				+" ORDER BY DateDisplayed";
			return RefreshAndFill(Db.GetTable(command));
		}

		///<summary>Used in reconcile window.</summary>
		public static List <JournalEntry> GetForReconcile(long accountNum,bool includeUncleared,long reconcileNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List <JournalEntry>>(MethodBase.GetCurrentMethod(),accountNum,includeUncleared,reconcileNum);
			}
			string command=
				"SELECT * FROM journalentry "
				+"WHERE AccountNum="+POut.Long(accountNum)
				+" AND (ReconcileNum="+POut.Long(reconcileNum);
			if(includeUncleared) {
				command+=" OR ReconcileNum=0)";
			}
			else {
				command+=")";
			}
			command+=" ORDER BY DateDisplayed";
			return RefreshAndFill(Db.GetTable(command));
		}

		private static List <JournalEntry> RefreshAndFill(DataTable table) {
			//No need to check RemotingRole; no call to db.
			List <JournalEntry> List=new List <JournalEntry> ();
			for(int i=0;i<table.Rows.Count;i++) {
				List.Add(new JournalEntry());
				List[i].JournalEntryNum= PIn.Long(table.Rows[i][0].ToString());
				List[i].TransactionNum = PIn.Long(table.Rows[i][1].ToString());
				List[i].AccountNum     = PIn.Long(table.Rows[i][2].ToString());
				List[i].DateDisplayed  = PIn.Date(table.Rows[i][3].ToString());
				List[i].DebitAmt       = PIn.Double(table.Rows[i][4].ToString());
				List[i].CreditAmt      = PIn.Double(table.Rows[i][5].ToString());
				List[i].Memo           = PIn.String(table.Rows[i][6].ToString());
				List[i].Splits         = PIn.String(table.Rows[i][7].ToString());
				List[i].CheckNumber    = PIn.String(table.Rows[i][8].ToString());
				List[i].ReconcileNum   = PIn.Long(table.Rows[i][9].ToString());
			}
			return List;
		}

		///<summary></summary>
		public static long Insert(JournalEntry je) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				je.JournalEntryNum=Meth.GetLong(MethodBase.GetCurrentMethod(),je);
				return je.JournalEntryNum;
			}
			if(je.DebitAmt<0 || je.CreditAmt<0){
				throw new ApplicationException(Lans.g("JournalEntries","Error. Credit and debit must both be positive."));
			}
			if(PrefC.RandomKeys) {
				je.JournalEntryNum=ReplicationServers.GetKey("journalentry","JournalEntryNum");
			}
			string command="INSERT INTO journalentry (";
			if(PrefC.RandomKeys) {
				command+="JournalEntryNum,";
			}
			command+="TransactionNum,AccountNum,DateDisplayed,DebitAmt,CreditAmt,Memo,Splits,CheckNumber,"
				+"ReconcileNum) VALUES(";
			if(PrefC.RandomKeys) {
				command+="'"+POut.Long(je.JournalEntryNum)+"', ";
			}
			command+=
				 "'"+POut.Long   (je.TransactionNum)+"', "
				+"'"+POut.Long   (je.AccountNum)+"', "
				    +POut.Date  (je.DateDisplayed)+", "
				+"'"+POut.Double(je.DebitAmt)+"', "
				+"'"+POut.Double(je.CreditAmt)+"', "
				+"'"+POut.String(je.Memo)+"', "
				+"'"+POut.String(je.Splits)+"', "
				+"'"+POut.String(je.CheckNumber)+"', "
				+"'"+POut.Long   (je.ReconcileNum)+"')";
			if(PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				je.JournalEntryNum=Db.NonQ(command,true);
			}
			return je.JournalEntryNum;
		}

		///<summary></summary>
		public static void Update(JournalEntry je) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),je);
				return;
			}
			if(je.DebitAmt<0 || je.CreditAmt<0) {
				throw new ApplicationException(Lans.g("JournalEntries","Error. Credit and debit must both be positive."));
			}
			string command= "UPDATE journalentry SET "
				+"TransactionNum = '"+POut.Long   (je.TransactionNum)+"' "
				+",AccountNum = '"   +POut.Long   (je.AccountNum)+"' "
				+",DateDisplayed = "+POut.Date  (je.DateDisplayed)+" "
				+",DebitAmt = '"     +POut.Double(je.DebitAmt)+"' "
				+",CreditAmt = '"    +POut.Double(je.CreditAmt)+"' "
				+",Memo = '"         +POut.String(je.Memo)+"' "
				+",Splits = '"       +POut.String(je.Splits)+"' "
				+",CheckNumber = '"  +POut.String(je.CheckNumber)+"' "
				+",ReconcileNum = '" +POut.Long   (je.ReconcileNum)+"' "
				+"WHERE JournalEntryNum = '"+POut.Long(je.JournalEntryNum)+"'";
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void Delete(JournalEntry je) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),je);
				return;
			}
			string command= "DELETE FROM journalentry WHERE JournalEntryNum = "+POut.Long(je.JournalEntryNum);
			Db.NonQ(command);
		}

		///<summary>Used in FormTransactionEdit to synch database with changes user made to the journalEntry list for a transaction.  Must supply an old list for comparison.  Only the differences are saved.  Surround with try/catch, because it will thrown an exception if any entries are negative.</summary>
		public static void UpdateList(ArrayList oldJournalList,ArrayList newJournalList) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<newJournalList.Count;i++){
				if(((JournalEntry)newJournalList[i]).DebitAmt<0 || ((JournalEntry)newJournalList[i]).CreditAmt<0){
					throw new ApplicationException(Lans.g("JournalEntries","Error. Credit and debit must both be positive."));
				}
			}
			JournalEntry newJournalEntry;
			for(int i=0;i<oldJournalList.Count;i++) {//loop through the old list
				newJournalEntry=null;
				for(int j=0;j<newJournalList.Count;j++) {
					if(newJournalList[j]==null || ((JournalEntry)newJournalList[j]).JournalEntryNum==0) {
						continue;
					}
					if(((JournalEntry)oldJournalList[i]).JournalEntryNum==((JournalEntry)newJournalList[j]).JournalEntryNum) {
						newJournalEntry=(JournalEntry)newJournalList[j];
						break;
					}
				}
				if(newJournalEntry==null) {
					//journalentry with matching journalEntryNum was not found, so it must have been deleted
					Delete((JournalEntry)oldJournalList[i]);
					continue;
				}
				//journalentry was found with matching journalEntryNum, so check for changes
				if(newJournalEntry.AccountNum != ((JournalEntry)oldJournalList[i]).AccountNum
					|| newJournalEntry.DateDisplayed != ((JournalEntry)oldJournalList[i]).DateDisplayed
					|| newJournalEntry.DebitAmt != ((JournalEntry)oldJournalList[i]).DebitAmt
					|| newJournalEntry.CreditAmt != ((JournalEntry)oldJournalList[i]).CreditAmt
					|| newJournalEntry.Memo != ((JournalEntry)oldJournalList[i]).Memo
					|| newJournalEntry.Splits != ((JournalEntry)oldJournalList[i]).Splits
					|| newJournalEntry.CheckNumber!= ((JournalEntry)oldJournalList[i]).CheckNumber) 
				{
					Update(newJournalEntry);
				}
			}
			for(int i=0;i<newJournalList.Count;i++) {//loop through the new list
				if(newJournalList[i]==null) {
					continue;
				}
				if(((JournalEntry)newJournalList[i]).JournalEntryNum!=0) {
					continue;
				}
				//entry with journalEntryNum=0, so it's new
				Insert((JournalEntry)newJournalList[i]);
			}
		}

		///<summary>Called from FormTransactionEdit.</summary>
		public static bool AttachedToReconcile(ArrayList journalList){
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<journalList.Count;i++){
				if(((JournalEntry)journalList[i]).ReconcileNum!=0){
					return true;
				}
			}
			return false;
		}

		///<summary>Called from FormTransactionEdit.</summary>
		public static DateTime GetReconcileDate(ArrayList journalList) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<journalList.Count;i++) {
				if(((JournalEntry)journalList[i]).ReconcileNum!=0) {
					return Reconciles.GetOne(((JournalEntry)journalList[i]).ReconcileNum).DateReconcile;
				}
			}
			return DateTime.MinValue;
		}

		///<summary>Called once from FormReconcileEdit when closing.  Saves the reconcileNum for every item in the list.</summary>
		public static void SaveList(List <JournalEntry> journalList,long reconcileNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),journalList,reconcileNum);
				return;
			}
			string command="UPDATE journalentry SET ReconcileNum=0 WHERE";
			string str="";
			for(int i=0;i<journalList.Count;i++){
				if(journalList[i].ReconcileNum==0){
					if(str!=""){
						str+=" OR";
					}
					str+=" JournalEntryNum="+POut.Long(journalList[i].JournalEntryNum);
				}
			}
			if(str!=""){
				command+=str;
				Db.NonQ(command);
			}
			command="UPDATE journalentry SET ReconcileNum="+POut.Long(reconcileNum)+" WHERE";
			str="";
			for(int i=0;i<journalList.Count;i++) {
				if(journalList[i].ReconcileNum==reconcileNum) {
					if(str!="") {
						str+=" OR";
					}
					str+=" JournalEntryNum="+POut.Long(journalList[i].JournalEntryNum);
				}
			}
			if(str!=""){
				command+=str;
				Db.NonQ(command);
			}
		}

		/*//<summary>Attempts to delete all journal entries for one transaction.  Will later throw an error if attached to any reconciles.</summary>
		public static void DeleteForTrans(int transactionNum){
			string command="DELETE FROM journalentry WHERE TransactionNum="+POut.PInt(transactionNum);
			DataConnection dcon=new DataConnection();
			Db.NonQ(command);
		}*/




	}

	
}




