package com.opendental.odweb.client.tabletypes;

import com.google.gwt.xml.client.Document;
import com.opendental.odweb.client.remoting.Serializing;
import com.google.gwt.i18n.client.DateTimeFormat;
import java.util.Date;

public class TaskNote {
		/** Primary key. */
		public int TaskNoteNum;
		/** FK to task. The task this tasknote is attached to. */
		public int TaskNum;
		/** FK to user. The user who created this tasknote. */
		public int UserNum;
		/** Date and time the note was created (editable). */
		public Date DateTimeNote;
		/** Note. Text that the user wishes to show on the task. */
		public String Note;

		/** Deep copy of object. */
		public TaskNote Copy() {
			TaskNote tasknote=new TaskNote();
			tasknote.TaskNoteNum=this.TaskNoteNum;
			tasknote.TaskNum=this.TaskNum;
			tasknote.UserNum=this.UserNum;
			tasknote.DateTimeNote=this.DateTimeNote;
			tasknote.Note=this.Note;
			return tasknote;
		}

		/** Serialize the object into XML. */
		public String SerializeToXml() {
			StringBuilder sb=new StringBuilder();
			sb.append("<TaskNote>");
			sb.append("<TaskNoteNum>").append(TaskNoteNum).append("</TaskNoteNum>");
			sb.append("<TaskNum>").append(TaskNum).append("</TaskNum>");
			sb.append("<UserNum>").append(UserNum).append("</UserNum>");
			sb.append("<DateTimeNote>").append(DateTimeFormat.getFormat("yyyyMMddHHmmss").format(DateTimeNote)).append("</DateTimeNote>");
			sb.append("<Note>").append(Serializing.EscapeForXml(Note)).append("</Note>");
			sb.append("</TaskNote>");
			return sb.toString();
		}

		/** Sets all the variables on this object based on the values in the XML document.  Variables that are not in the XML document will be null or their default values.
		 * @param doc A parsed XML document.  Must be valid XML.  Does not need to contain a node for every variable on this object.
		 * @throws Exception DeserializeFromXml is entirely encased in a try catch and will throw exceptions if anything goes wrong. */
		public void DeserializeFromXml(Document doc) throws Exception {
			try {
				if(Serializing.GetXmlNodeValue(doc,"TaskNoteNum")!=null) {
					TaskNoteNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"TaskNoteNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"TaskNum")!=null) {
					TaskNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"TaskNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"UserNum")!=null) {
					UserNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"UserNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"DateTimeNote")!=null) {
					DateTimeNote=DateTimeFormat.getFormat("yyyyMMddHHmmss").parseStrict(Serializing.GetXmlNodeValue(doc,"DateTimeNote"));
				}
				if(Serializing.GetXmlNodeValue(doc,"Note")!=null) {
					Note=Serializing.GetXmlNodeValue(doc,"Note");
				}
			}
			catch(Exception e) {
				throw e;
			}
		}


}