using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Reflection;

namespace OpenDentBusiness{

	///<summary>Since users not allowed to edit, Refresh only gets run the first time it's needed.</summary>
	public class ElectIDs{
		private static ElectID[] list;

		///<summary>This is the list of all electronic IDs.</summary>
		public static ElectID[] List {
			//No need to check RemotingRole; no call to db.
			get {
				if(list==null) {
					RefreshCache();
				}
				return list;
			}
			set {
				list=value;
			}
		}

		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command = "SELECT * from electid ORDER BY CarrierName";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="ElectID";
			FillCache(table);
			return table;
		}

		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			List=new ElectID[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++){
				List[i]=new ElectID();
				List[i].ElectIDNum   = PIn.Long   (table.Rows[i][0].ToString());
				List[i].PayorID      = PIn.String(table.Rows[i][1].ToString());
				List[i].CarrierName  = PIn.String(table.Rows[i][2].ToString());
				List[i].IsMedicaid   = PIn.Bool  (table.Rows[i][3].ToString());
				List[i].ProviderTypes= PIn.String(table.Rows[i][4].ToString());
				List[i].Comments     = PIn.String(table.Rows[i][5].ToString());
			}
		}

		///<summary></summary>
		public static ProviderSupplementalID[] GetRequiredIdents(string payorID){
			//No need to check RemotingRole; no call to db.
			ElectID electID=GetID(payorID);
			if(electID==null){
				return new ProviderSupplementalID[0];
			}
			if(electID.ProviderTypes==""){
				return new ProviderSupplementalID[0];
			}
			string[] provTypes=electID.ProviderTypes.Split(',');
			if(provTypes.Length==0){
				return new ProviderSupplementalID[0];
			}
			ProviderSupplementalID[] retVal=new ProviderSupplementalID[provTypes.Length];
			for(int i=0;i<provTypes.Length;i++){
				retVal[i]=(ProviderSupplementalID)(Convert.ToInt32(provTypes[i]));
			}
			/*
			if(electID=="SB601"){//BCBS of GA
				retVal=new ProviderSupplementalID[2];
				retVal[0]=ProviderSupplementalID.BlueShield;
				retVal[1]=ProviderSupplementalID.SiteNumber;
			}*/
			return retVal;
		}

		///<summary>Gets ONE ElectID that uses the supplied payorID. Even if there are multiple payors using that ID.  So use this carefully.</summary>
		public static ElectID GetID(string payorID){
			//No need to check RemotingRole; no call to db.
			ArrayList electIDs=GetIDs(payorID);
			if(electIDs.Count==0){
				return null;
			}
			return (ElectID)electIDs[0];//simply return the first one we encounter
		}

		///<summary>Gets an arrayList of ElectID objects based on a supplied payorID. If no matches found, then returns array of 0 length. Used to display payors in FormInsPlan and also to get required idents.  This means that all payors with the same ID should have the same required idents and notes.</summary>
		public static ArrayList GetIDs(string payorID){
			//No need to check RemotingRole; no call to db.
			ArrayList retVal=new ArrayList();
			for(int i=0;i<List.Length;i++){
				if(List[i].PayorID==payorID){
					retVal.Add(List[i]);
				}
			}
			return retVal;
		}

		///<summary>Gets the names of the payors to display based on the payorID.  Since carriers sometimes share payorIDs, there will often be multiple payor names returned.</summary>
		public static string[] GetDescripts(string payorID){
			//No need to check RemotingRole; no call to db.
			if(payorID==""){
				return new string[]{};
			}
			ArrayList electIDs=GetIDs(payorID);
			string[] retVal=new string[electIDs.Count];
			for(int i=0;i<retVal.Length;i++){
				retVal[i]=((ElectID)electIDs[i]).CarrierName;
			}
			return retVal;
		}

	



	
	}
	
	

}










