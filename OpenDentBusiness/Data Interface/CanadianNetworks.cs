using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenDentBusiness{
	///<summary></summary>
	public class CanadianNetworks{
		private static List<CanadianNetwork> listt;

		public static List<CanadianNetwork> Listt{
			//No need to check RemotingRole; no call to db.
			get{
				if(listt==null){
					Refresh();
				}
				return listt;
			}
		}
	
		///<summary></summary>
		public static void Refresh(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			string command="SELECT * FROM canadiannetwork";
			DataTable table=Db.GetTable(command);
			listt=new List<CanadianNetwork>();
			CanadianNetwork network;
			for(int i=0;i<table.Rows.Count;i++){
				network=new CanadianNetwork();
				network.CanadianNetworkNum=PIn.Long   (table.Rows[i][0].ToString());
				network.Abbrev            =PIn.String(table.Rows[i][1].ToString());
				network.Descript          =PIn.String(table.Rows[i][2].ToString());
				listt.Add(network);
			}
		}

		///<summary></summary>
		public static long Insert(CanadianNetwork network) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				network.CanadianNetworkNum=Meth.GetLong(MethodBase.GetCurrentMethod(),network);
				return network.CanadianNetworkNum;
			}
			if(PrefC.RandomKeys) {
				network.CanadianNetworkNum=ReplicationServers.GetKey("canadiannetwork","CanadianNetworkNum");
			}
			string command="INSERT INTO canadiannetwork (";
			if(PrefC.RandomKeys) {
				command+="CanadianNetworkNum,";
			}
			command+="Abbrev, Descript) VALUES(";
			if(PrefC.RandomKeys) {
				command+="'"+POut.Long(network.CanadianNetworkNum)+"', ";
			}
			command+=
				 "'"+POut.String(network.Abbrev)+"', "
				+"'"+POut.String(network.Descript)+"')";
			if(PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				network.CanadianNetworkNum=Db.NonQ(command,true);
			}
			return network.CanadianNetworkNum;
		}

		///<summary></summary>
		public static void Update(CanadianNetwork Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Cur);
				return;
			}
			string command="UPDATE canadiannetwork SET "
				+ "Abbrev = '"+POut.String(Cur.Abbrev)+"' "
				+ ",Descript='"+POut.String(Cur.Descript)+"' "
				+"WHERE CanadianNetworkNum = '"+POut.Long(Cur.CanadianNetworkNum)+"'";
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void Delete(int networkNum){

		}

		///<summary></summary>
		public static CanadianNetwork GetNetwork(long networkNum) {
			//No need to check RemotingRole; no call to db.
			if(listt==null) {
				Refresh();
			}
			for(int i=0;i<listt.Count;i++){
				if(listt[i].CanadianNetworkNum==networkNum){
					return listt[i];
				}
			}
			return null;
		}


	


		 
		 
	}
}













