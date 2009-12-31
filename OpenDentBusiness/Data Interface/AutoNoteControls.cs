using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;

namespace OpenDentBusiness {
	public class AutoNoteControls {
		/// <summary>A list of all the Prompts.  Caching could be handled better for fewer refreshes.</summary>
		public static List<AutoNoteControl> Listt;

		///<summary></summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM autonotecontrol ORDER BY Descript";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="AutoNoteControl";
			FillCache(table);
			return table;
		}

		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			Listt=new List<AutoNoteControl>();
			AutoNoteControl noteCont;
			for (int i=0;i<table.Rows.Count;i++){
				noteCont = new AutoNoteControl();
				noteCont.AutoNoteControlNum = PIn.Long(table.Rows[i][0].ToString());
				noteCont.Descript = PIn.String(table.Rows[i]["Descript"].ToString());
				noteCont.ControlType = PIn.String(table.Rows[i]["ControlType"].ToString());
				noteCont.ControlLabel =PIn.String(table.Rows[i]["ControlLabel"].ToString());
				noteCont.ControlOptions = PIn.String(table.Rows[i]["ControlOptions"].ToString());
				Listt.Add(noteCont);
			}
		}

		public static long Insert(AutoNoteControl autonotecontrol) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				autonotecontrol.AutoNoteControlNum=Meth.GetLong(MethodBase.GetCurrentMethod(),autonotecontrol);
				return autonotecontrol.AutoNoteControlNum;
			}
			if(PrefC.RandomKeys) {
				autonotecontrol.AutoNoteControlNum=ReplicationServers.GetKey("autonotecontrol","AutoNoteControlNum");
			}
			string command="INSERT INTO autonotecontrol (";
			if(PrefC.RandomKeys) {
				command+="AutoNoteControlNum,";
			}
			command+="Descript,ControlType,ControlLabel,ControlOptions) VALUES(";
			if(PrefC.RandomKeys) {
				command+=POut.Long(autonotecontrol.AutoNoteControlNum)+", ";
			}
			command+=		
				 "'"+POut.String(autonotecontrol.Descript)+"', " 
				+"'"+POut.String(autonotecontrol.ControlType)+"', "
				+"'"+POut.String(autonotecontrol.ControlLabel)+"' ,"			
				+"'"+POut.String(autonotecontrol.ControlOptions)+"')";
			if(PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				autonotecontrol.AutoNoteControlNum=Db.NonQ(command,true);
			}
			return autonotecontrol.AutoNoteControlNum;
		}


		public static void Update(AutoNoteControl autonotecontrol) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),autonotecontrol);
				return;
			}
			string command="UPDATE autonotecontrol SET "
				+"ControlType = '"+POut.String(autonotecontrol.ControlType)+"', "
				+"Descript = '"+POut.String(autonotecontrol.Descript)+"', "
				+"ControlLabel = '"+POut.String(autonotecontrol.ControlLabel)+"', "
				+"ControlOptions = '"+POut.String(autonotecontrol.ControlOptions)+"' "
				+"WHERE AutoNoteControlNum = '"+POut.Long(autonotecontrol.AutoNoteControlNum)+"'";
			Db.NonQ(command);
		}

		public static void Delete(long autoNoteControlNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),autoNoteControlNum);
				return;
			}
			//no validation for now.
			string command="DELETE FROM autonotecontrol WHERE AutoNoteControlNum="+POut.Long(autoNoteControlNum);
			Db.NonQ(command);
		}

		///<summary>Will return null if can't match.</summary>
		public static AutoNoteControl GetByDescript(string descript) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<Listt.Count;i++) {
				if(Listt[i].Descript==descript) {
					return Listt[i];
				}
			}
			return null;
		}

		/*
		/// <summary>Returns all the control info about the selected control</summary>
		public static void RefreshControl(string ControlNumToShow) {
			string command = "SELECT * FROM autonotecontrol "
				+"WHERE AutoNoteControlNum = '"+ControlNumToShow+"'";
			DataTable table = Db.GetTable(command);
			Listt=new List<AutoNoteControl>();
			//List = new AutoNote[table.Rows.Count];
			AutoNoteControl noteCont;
			noteCont = new AutoNoteControl();
			for (int i = 0; i < table.Rows.Count; i++) {
				noteCont.AutoNoteControlNum = PIn.PInt(table.Rows[i][0].ToString());
				noteCont.Descript = PIn.PString(table.Rows[i]["Descript"].ToString());
				noteCont.ControlType = PIn.PString(table.Rows[i]["ControlType"].ToString());
				noteCont.ControlLabel = PIn.PString(table.Rows[i]["ControlLabel"].ToString());
				noteCont.PrefaceText = PIn.PString(table.Rows[i]["PrefaceText"].ToString());
				noteCont.MultiLineText = PIn.PString(table.Rows[i]["MultiLineText"].ToString());
				noteCont.ControlOptions = PIn.PString(table.Rows[i]["ControlOptions"].ToString());
				Listt.Add(noteCont);
			}
		}*/

		/*
		///<summary>Converts the Num of the control to it's Name.
		public static List<AutoNoteControl> ControlNumToName(string controlNum) {
			string command="SELECT Descript FROM autonotecontrol "
			+"WHERE AutoNoteControlNum = "+"'"+controlNum+"'";
			DataTable table=Db.GetTable(command);
			Listt=new List<AutoNoteControl>();
			AutoNoteControl note;
			note = new AutoNoteControl();
			for (int i = 0; i < table.Rows.Count; i++) {
				note.Descript=PIn.PString(table.Rows[0]["Descript"].ToString());
				Listt.Add(note);
			}
			return Listt;
		}*/

		/*
		/// <summary>Checks to see if the Control Name Already Exists.  If you are editing a control you would specify the original name. This name will be ignored in the search. If else set to NULL</summary>
		public static bool ControlNameUsed(string ControlName, string OriginalControlName) {
			string command="SELECT Descript FROM autonotecontrol WHERE "
			+"Descript = '"+POut.PString(ControlName)+"' AND Descript != '"+POut.PString(OriginalControlName)+"'";
			DataTable table=Db.GetTable(command);
			bool isUsed=false;
			if (table.Rows.Count!=0) {//found duplicate control name				
				isUsed=true;
			}
			return isUsed;
		}*/
	}
}

