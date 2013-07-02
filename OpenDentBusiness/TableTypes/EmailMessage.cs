using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	///<summary>An outgoing email message is stored here.</summary>
	[Serializable]
	public class EmailMessage:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long EmailMessageNum;
		///<summary>FK to patient.PatNum</summary>
		public long PatNum;
		///<summary>Single valid email address. Bcc field might be added later, although it won't be very useful.  We will never allow visible cc for privacy reasons.</summary>
		public string ToAddress;
		///<summary>Valid email address.</summary>
		public string FromAddress;
		///<summary>Subject line.</summary>
		public string Subject;
		///<summary>Body of the email</summary>
		public string BodyText;
		///<summary>Date and time the message was sent. Automated at the UI level.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime MsgDateTime;
		///<summary>0=neither, 1=sent, 2=received.</summary>
		public EmailSentOrReceived SentOrReceived;
		///<summary>Not a database column.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public List<EmailAttach> Attachments;

		///<summary>Constructor</summary>
		public EmailMessage(){
			Attachments=new List<EmailAttach>();
		}

		public EmailMessage Copy() {
			EmailMessage e=(EmailMessage)this.MemberwiseClone();
			e.Attachments=new List<EmailAttach>();
			for(int i=0;i<Attachments.Count;i++) {
				e.Attachments.Add(Attachments[i].Copy());
			}
			return e;
		}
	}

	///<summary>0=Neither, 1=Sent, 2=Receivedceived. 3=Read, 4=WebMailReceived, 5=WebMailRecdRead, 6=WebMailSent, 7=WebMailSentRead</summary>
	public enum EmailSentOrReceived {
		///<summary>0 Unsent</summary>
		Neither,
		///<summary>1</summary>
		Sent,
		///<summary>2 For regular email and email encrypted using Direct.  Shows in Inbox.  Once it's attached to a patient it will also show in Chart module.</summary>
		Received,
		///<summary>3 For regular email and email encrypted using Direct.  Has been read.  Shows in Inbox.  Once it's attached to a patient it will also show in Chart module.</summary>
		Read,
		///<summary>4 WebMail received from patient portal.  Shows in OD Inbox and in pt Chart module.  Also shows in PP as a sent and unread WebMail msg.</summary>
		WebMailReceived,
		///<summary>5 WebMail received from patient portal that has been marked read.  Shows in the OD Inbox and in pt Chart module.  Also shows in PP as a sent and read WebMail.</summary>
		WebMailRecdRead,
		///<summary>6 Webmail sent from provider to patient.  Shows in Chart module and also shows in PP as a received and unread WebMail msg.</summary>
		WebMailSent,
		///<summary>7 Webmail sent from provider to patient and read by patient.  Shows in Chart module and also shows in PP as a received and read WebMail msg.</summary>
		WebMailSentRead
	}


}
