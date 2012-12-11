package com.opendental.odweb.client.tabletypes;

import com.google.gwt.xml.client.Document;
import com.opendental.odweb.client.remoting.Serializing;
import com.google.gwt.i18n.client.DateTimeFormat;
import java.util.Date;

public class Schedule {
		/** Primary key. */
		public int ScheduleNum;
		/** Date for this timeblock. */
		public Date SchedDate;
		/** Start time for this timeblock. */
		public String StartTime;
		/** Stop time for this timeblock. */
		public String StopTime;
		/** Enum:ScheduleType 0=Practice,1=Provider,2=Blockout,3=Employee.  Practice is used as a way to indicate holidays and as a way to put a note in for the entire practice for one day.  But whenever type is Practice, times will be ignored. */
		public ScheduleType SchedType;
		/** FK to provider.ProvNum if a provider type. */
		public int ProvNum;
		/** FK to definition.DefNum if blockout.  eg. HighProduction, RCT Only, Emerg. */
		public int BlockoutType;
		/** This contains various types of text entered by the user. */
		public String Note;
		/** Enum:SchedStatus enumeration 0=Open,1=Closed,2=Holiday.  All blocks have a status of Open, but user doesn't see the status.  The "closed" status was previously used to override the defaults when the last timeblock was deleted.  But it's nearly phased out now.  Still used by blockouts.  Holidays are a special type of practice schedule item which do not have providers attached. */
		public SchedStatus Status;
		/** FK to employee.EmployeeNum. */
		public int EmployeeNum;
		/** Last datetime that this row was inserted or updated. */
		public Date DateTStamp;

		/** Deep copy of object. */
		public Schedule Copy() {
			Schedule schedule=new Schedule();
			schedule.ScheduleNum=this.ScheduleNum;
			schedule.SchedDate=this.SchedDate;
			schedule.StartTime=this.StartTime;
			schedule.StopTime=this.StopTime;
			schedule.SchedType=this.SchedType;
			schedule.ProvNum=this.ProvNum;
			schedule.BlockoutType=this.BlockoutType;
			schedule.Note=this.Note;
			schedule.Status=this.Status;
			schedule.EmployeeNum=this.EmployeeNum;
			schedule.DateTStamp=this.DateTStamp;
			return schedule;
		}

		/** Serialize the object into XML. */
		public String SerializeToXml() {
			StringBuilder sb=new StringBuilder();
			sb.append("<Schedule>");
			sb.append("<ScheduleNum>").append(ScheduleNum).append("</ScheduleNum>");
			sb.append("<SchedDate>").append(DateTimeFormat.getFormat("yyyyMMddHHmmss").format(SchedDate)).append("</SchedDate>");
			sb.append("<StartTime>").append(Serializing.EscapeForXml(StartTime)).append("</StartTime>");
			sb.append("<StopTime>").append(Serializing.EscapeForXml(StopTime)).append("</StopTime>");
			sb.append("<SchedType>").append(SchedType.ordinal()).append("</SchedType>");
			sb.append("<ProvNum>").append(ProvNum).append("</ProvNum>");
			sb.append("<BlockoutType>").append(BlockoutType).append("</BlockoutType>");
			sb.append("<Note>").append(Serializing.EscapeForXml(Note)).append("</Note>");
			sb.append("<Status>").append(Status.ordinal()).append("</Status>");
			sb.append("<EmployeeNum>").append(EmployeeNum).append("</EmployeeNum>");
			sb.append("<DateTStamp>").append(DateTimeFormat.getFormat("yyyyMMddHHmmss").format(DateTStamp)).append("</DateTStamp>");
			sb.append("</Schedule>");
			return sb.toString();
		}

		/** Sets all the variables on this object based on the values in the XML document.  Variables that are not in the XML document will be null or their default values.
		 * @param doc A parsed XML document.  Must be valid XML.  Does not need to contain a node for every variable on this object.
		 * @throws Exception DeserializeFromXml is entirely encased in a try catch and will throw exceptions if anything goes wrong. */
		public void DeserializeFromXml(Document doc) throws Exception {
			try {
				if(Serializing.GetXmlNodeValue(doc,"ScheduleNum")!=null) {
					ScheduleNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"ScheduleNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"SchedDate")!=null) {
					SchedDate=DateTimeFormat.getFormat("yyyyMMddHHmmss").parseStrict(Serializing.GetXmlNodeValue(doc,"SchedDate"));
				}
				if(Serializing.GetXmlNodeValue(doc,"StartTime")!=null) {
					StartTime=Serializing.GetXmlNodeValue(doc,"StartTime");
				}
				if(Serializing.GetXmlNodeValue(doc,"StopTime")!=null) {
					StopTime=Serializing.GetXmlNodeValue(doc,"StopTime");
				}
				if(Serializing.GetXmlNodeValue(doc,"SchedType")!=null) {
					SchedType=ScheduleType.values()[Integer.valueOf(Serializing.GetXmlNodeValue(doc,"SchedType"))];
				}
				if(Serializing.GetXmlNodeValue(doc,"ProvNum")!=null) {
					ProvNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"ProvNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"BlockoutType")!=null) {
					BlockoutType=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"BlockoutType"));
				}
				if(Serializing.GetXmlNodeValue(doc,"Note")!=null) {
					Note=Serializing.GetXmlNodeValue(doc,"Note");
				}
				if(Serializing.GetXmlNodeValue(doc,"Status")!=null) {
					Status=SchedStatus.values()[Integer.valueOf(Serializing.GetXmlNodeValue(doc,"Status"))];
				}
				if(Serializing.GetXmlNodeValue(doc,"EmployeeNum")!=null) {
					EmployeeNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"EmployeeNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"DateTStamp")!=null) {
					DateTStamp=DateTimeFormat.getFormat("yyyyMMddHHmmss").parseStrict(Serializing.GetXmlNodeValue(doc,"DateTStamp"));
				}
			}
			catch(Exception e) {
				throw e;
			}
		}

		/** For schedule timeblocks. */
		public enum ScheduleType {
			/** 0 */
			Practice,
			/** 1 */
			Provider,
			/** 2 */
			Blockout,
			/** 3 */
			Employee
		}

		/** Schedule status.  Open=0,Closed=1,Holiday=2. */
		public enum SchedStatus {
			/** 0 */
			Open,
			/** 1 */
			Closed,
			/** 2 */
			Holiday
		}


}