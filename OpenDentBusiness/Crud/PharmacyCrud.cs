//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class PharmacyCrud {
		///<summary>Gets one Pharmacy object from the database using the primary key.  Returns null if not found.</summary>
		internal static Pharmacy SelectOne(long pharmacyNum){
			string command="SELECT * FROM pharmacy "
				+"WHERE PharmacyNum = "+POut.Long(pharmacyNum)+" LIMIT 1";
			List<Pharmacy> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Pharmacy object from the database using a query.</summary>
		internal static Pharmacy SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Pharmacy> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Pharmacy objects from the database using a query.</summary>
		internal static List<Pharmacy> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Pharmacy> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<Pharmacy> TableToList(DataTable table){
			List<Pharmacy> retVal=new List<Pharmacy>();
			Pharmacy pharmacy;
			for(int i=0;i<table.Rows.Count;i++) {
				pharmacy=new Pharmacy();
				pharmacy.PharmacyNum= PIn.Long  (table.Rows[i]["PharmacyNum"].ToString());
				pharmacy.PharmID    = PIn.String(table.Rows[i]["PharmID"].ToString());
				pharmacy.StoreName  = PIn.String(table.Rows[i]["StoreName"].ToString());
				pharmacy.Phone      = PIn.String(table.Rows[i]["Phone"].ToString());
				pharmacy.Fax        = PIn.String(table.Rows[i]["Fax"].ToString());
				pharmacy.Address    = PIn.String(table.Rows[i]["Address"].ToString());
				pharmacy.Address2   = PIn.String(table.Rows[i]["Address2"].ToString());
				pharmacy.City       = PIn.String(table.Rows[i]["City"].ToString());
				pharmacy.State      = PIn.String(table.Rows[i]["State"].ToString());
				pharmacy.Zip        = PIn.String(table.Rows[i]["Zip"].ToString());
				pharmacy.Note       = PIn.String(table.Rows[i]["Note"].ToString());
				retVal.Add(pharmacy);
			}
			return retVal;
		}

		///<summary>Inserts one Pharmacy into the database.  Returns the new priKey.</summary>
		internal static long Insert(Pharmacy pharmacy){
			return Insert(pharmacy,false);
		}

		///<summary>Inserts one Pharmacy into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(Pharmacy pharmacy,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				pharmacy.PharmacyNum=ReplicationServers.GetKey("pharmacy","PharmacyNum");
			}
			string command="INSERT INTO pharmacy (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PharmacyNum,";
			}
			command+="PharmID,StoreName,Phone,Fax,Address,Address2,City,State,Zip,Note) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(pharmacy.PharmacyNum)+",";
			}
			command+=
				 "'"+POut.String(pharmacy.PharmID)+"',"
				+"'"+POut.String(pharmacy.StoreName)+"',"
				+"'"+POut.String(pharmacy.Phone)+"',"
				+"'"+POut.String(pharmacy.Fax)+"',"
				+"'"+POut.String(pharmacy.Address)+"',"
				+"'"+POut.String(pharmacy.Address2)+"',"
				+"'"+POut.String(pharmacy.City)+"',"
				+"'"+POut.String(pharmacy.State)+"',"
				+"'"+POut.String(pharmacy.Zip)+"',"
				+"'"+POut.String(pharmacy.Note)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				pharmacy.PharmacyNum=Db.NonQ(command,true);
			}
			return pharmacy.PharmacyNum;
		}

		///<summary>Updates one Pharmacy in the database.</summary>
		internal static void Update(Pharmacy pharmacy){
			string command="UPDATE pharmacy SET "
				+"PharmID    = '"+POut.String(pharmacy.PharmID)+"', "
				+"StoreName  = '"+POut.String(pharmacy.StoreName)+"', "
				+"Phone      = '"+POut.String(pharmacy.Phone)+"', "
				+"Fax        = '"+POut.String(pharmacy.Fax)+"', "
				+"Address    = '"+POut.String(pharmacy.Address)+"', "
				+"Address2   = '"+POut.String(pharmacy.Address2)+"', "
				+"City       = '"+POut.String(pharmacy.City)+"', "
				+"State      = '"+POut.String(pharmacy.State)+"', "
				+"Zip        = '"+POut.String(pharmacy.Zip)+"', "
				+"Note       = '"+POut.String(pharmacy.Note)+"' "
				+"WHERE PharmacyNum = "+POut.Long(pharmacy.PharmacyNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Updates one Pharmacy in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(Pharmacy pharmacy,Pharmacy oldPharmacy){
			string command="";
			if(pharmacy.PharmID != oldPharmacy.PharmID) {
				if(command!=""){ command+=",";}
				command+="PharmID = '"+POut.String(pharmacy.PharmID)+"'";
			}
			if(pharmacy.StoreName != oldPharmacy.StoreName) {
				if(command!=""){ command+=",";}
				command+="StoreName = '"+POut.String(pharmacy.StoreName)+"'";
			}
			if(pharmacy.Phone != oldPharmacy.Phone) {
				if(command!=""){ command+=",";}
				command+="Phone = '"+POut.String(pharmacy.Phone)+"'";
			}
			if(pharmacy.Fax != oldPharmacy.Fax) {
				if(command!=""){ command+=",";}
				command+="Fax = '"+POut.String(pharmacy.Fax)+"'";
			}
			if(pharmacy.Address != oldPharmacy.Address) {
				if(command!=""){ command+=",";}
				command+="Address = '"+POut.String(pharmacy.Address)+"'";
			}
			if(pharmacy.Address2 != oldPharmacy.Address2) {
				if(command!=""){ command+=",";}
				command+="Address2 = '"+POut.String(pharmacy.Address2)+"'";
			}
			if(pharmacy.City != oldPharmacy.City) {
				if(command!=""){ command+=",";}
				command+="City = '"+POut.String(pharmacy.City)+"'";
			}
			if(pharmacy.State != oldPharmacy.State) {
				if(command!=""){ command+=",";}
				command+="State = '"+POut.String(pharmacy.State)+"'";
			}
			if(pharmacy.Zip != oldPharmacy.Zip) {
				if(command!=""){ command+=",";}
				command+="Zip = '"+POut.String(pharmacy.Zip)+"'";
			}
			if(pharmacy.Note != oldPharmacy.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(pharmacy.Note)+"'";
			}
			if(command==""){
				return;
			}
			command="UPDATE pharmacy SET "+command
				+" WHERE PharmacyNum = "+POut.Long(pharmacy.PharmacyNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Deletes one Pharmacy from the database.</summary>
		internal static void Delete(long pharmacyNum){
			string command="DELETE FROM pharmacy "
				+"WHERE PharmacyNum = "+POut.Long(pharmacyNum)+" LIMIT 1";
			Db.NonQ(command);
		}

	}
}