﻿using System;
using System.Collections;
using System.Data;
using OpenDental.DataAccess;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace OpenDentBusiness{

	///<summary></summary>
	public class AnestheticRecords{
		///<summary>List of all anesthetic records for the current patient.</summary>
		public static AnestheticRecord[] List;

		///<summary>Stores the string of the command that will be sent to the database.</summary>
		public MySqlCommand cmd;
		///<summary>This is the connection that is used by the data adapter for all queries.</summary>
		private static MySqlConnection con;


		///<summary>Most recent date *first*. </summary>
		public static void Refresh(int patNum){	
			string command =
				"SELECT * FROM anestheticrecord"
				+ " WHERE PatNum = '"+patNum.ToString()+"'"
				+ " ORDER BY anestheticrecord.AnestheticDate DESC";
			DataTable table = General.GetTable(command);
			List = new AnestheticRecord[table.Rows.Count];
			for (int i = 0; i < table.Rows.Count; i++){
				List[i] = new AnestheticRecord();
				List[i].AnestheticRecordNum = PIn.PInt(table.Rows[i][0].ToString());
				List[i].PatNum = PIn.PInt(table.Rows[i][1].ToString());
				List[i].AnestheticDate = PIn.PDateT(table.Rows[i][2].ToString());
				List[i].ProvNum = PIn.PInt(table.Rows[i][3].ToString());
			} 
			
		}

		///<summary></summary>
		public static void Update(AnestheticRecord Cur){
			string command = "UPDATE anestheticrecord SET "
				+ "PatNum = '" + POut.PInt(Cur.PatNum) + "'"
				+ ",AnestheticDate = " + POut.PDateT(Cur.AnestheticDate) + "'"
				+ ",ProvNum = '" + POut.PInt(Cur.ProvNum) + "'"
				+ " WHERE AnestheticRecordNum = '" + POut.PInt(Cur.AnestheticRecordNum) + "'";
			General.NonQ(command);
		}

		///<summary>Creates a new AnestheticRecord in the db</summary>
		public static void Insert(AnestheticRecord Cur){
			if (PrefC.RandomKeys){
				Cur.AnestheticRecordNum = MiscData.GetKey("anestheticrecord", "AnestheticRecordNum");
			}
			string command = "INSERT INTO anestheticrecord (";
			if(PrefC.RandomKeys){
				command += "AnestheticRecordNum,";
			}
			command += "PatNum,AnestheticDate,ProvNum"
				+ ") VALUES(";
			if (PrefC.RandomKeys){
				command += "'" + POut.PInt(Cur.AnestheticRecordNum) + "', ";
			}
			command +=
				"'" + POut.PInt(Cur.PatNum) + "', "
				+ POut.PDateT(Cur.AnestheticDate) + ", "
				+ "'" + POut.PInt(Cur.ProvNum) + "')";
			if (PrefC.RandomKeys){
				General.NonQ(command);
			}
			else{
				Cur.AnestheticRecordNum = General.NonQ(command, true);
			}
		}

		///<summary>Creates a corresponding AnestheticData record in the db</summary>
		public static void InsertAnestheticData(AnestheticRecord Cur){

			if (PrefC.RandomKeys)
			{
				Cur.AnestheticRecordNum = MiscData.GetKey("anestheticrecord", "AnestheticRecordNum");
			}
			string command = "INSERT INTO anestheticdata (";
			if (PrefC.RandomKeys)
			{
				command += "AnestheticRecordNum,";
			}
			command += "AnestheticRecordNum"
				+ ") VALUES(";
			if (PrefC.RandomKeys)
			{
				command += "'" + POut.PInt(Cur.AnestheticRecordNum) + "', ";
			}
			command +=
				"" + POut.PInt(Cur.AnestheticRecordNum) + ")";
			if (PrefC.RandomKeys)
			{
				General.NonQ(command);
			}
			else
			{
				Cur.AnestheticRecordNum = General.NonQ(command, true);
			}
		}
		///<summary>Deletes an Anesthetic Record and the corresponding Anesthetic Data</summary>
		public static void Delete(AnestheticRecord Cur) {
			string command = "DELETE FROM anestheticrecord WHERE AnestheticRecordNum = '" + Cur.AnestheticRecordNum.ToString() + "'";
			General.NonQ(command);
			string command2 = "DELETE FROM anestheticdata WHERE AnestheticRecordNum = '" + Cur.AnestheticRecordNum.ToString() + "'";
			General.NonQ(command2);
		}

		/// <summary>/// Gets the Anesthetic Record number from the anestheticrecord table./// </summary>
		
		public static int GetRecordNum(int patnum)
		{
			MySqlCommand cmd = new MySqlCommand();
			con = new MySqlConnection(DataSettings.ConnectionString);
			cmd.Connection = con;
			if (con.State == ConnectionState.Open)
				con.Close();
			con.Open();
			cmd.CommandText = "SELECT AnestheticRecordNum FROM anestheticrecord WHERE PatNum = '" + patnum.ToString() + "'";    /*"SELECT Max(AnestheticRecordNum) FROM anestheticrecord a, patient p where a.Patnum = p.Patnum and p.patnum = " + patnum + "";*/
			cmd.Connection = con;
			int anestheticRecordNum = Convert.ToInt32(cmd.ExecuteScalar());
			con.Close();
			return anestheticRecordNum;
			
		}

		/// <summary>/// Returns the date shown in the listAnesthetic.SelectedItem so it can be used to pull the correct AnestheticRecordCur from the db/// </summary>
		
		public static int GetRecordNumByDate(string AnestheticDateCur)
		{
			DateTime anestheticDate = Convert.ToDateTime(AnestheticDateCur);
			//need to format so it matches DateTime format as that's what's in the db; yyyy/MM/dd hh:mm:ss tt is what's displayed in listAnesthetic.SelectedItem
			string newdate = anestheticDate.ToString("yyyy-MM-dd HH:mm:ss");
			
			MySqlCommand cmd = new MySqlCommand();
			con = new MySqlConnection(DataSettings.ConnectionString);
			cmd.Connection = con;
			if (con.State == ConnectionState.Open)
				con.Close();
			con.Open();
			cmd.CommandText = "SELECT AnestheticRecordNum FROM anestheticrecord WHERE AnestheticDate = '" + (newdate) + "'";    /*"SELECT Max(AnestheticRecordNum) FROM anestheticrecord a, patient p where a.Patnum = p.Patnum and p.patnum = " + patnum + "";*/
			cmd.Connection = con;
			int anestheticRecordNum = Convert.ToInt32(cmd.ExecuteScalar());
			con.Close();
			return anestheticRecordNum;

		}

	}



}