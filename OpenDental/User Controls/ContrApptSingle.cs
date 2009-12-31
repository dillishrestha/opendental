/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{

	///<summary></summary>
	public class ContrApptSingle : System.Windows.Forms.UserControl{
		private System.ComponentModel.Container components = null;// Required designer variable.
		///<summary>Set on mouse down or from Appt module</summary>
		public static long ClickedAptNum;
		/// <summary>This is not the best place for this, but changing it now would cause bugs.  Set manually</summary>
		public static long SelectedAptNum;
		///<summary>True if this control is on the pinboard</summary>
		public bool ThisIsPinBoard;
		///<summary>Stores the shading info for the provider bars on the left of the appointments module</summary>
		public static int[][] ProvBar;
		///<summary>Stores the background bitmap for this control</summary>
		public Bitmap Shadow;
		private Font baseFont=new Font("Arial",8);
		private Font boldFont=new Font("Arial",8,FontStyle.Bold);
		private string patternShowing;
		///<summary>This is a datarow that stores all info necessary to draw appt.  Replacing Info.</summary>
		public DataRow DataRoww;
		///<summary>Indicator that account has procedures with no ins claim</summary>
		public bool FamHasInsNotSent;
		///<Summary>Will show the highlight around the edges.  For now, this is only used for pinboard.  The ordinary selected appt is set with SelectedAptNum.</Summary>
		public bool IsSelected;


		///<summary></summary>
		public ContrApptSingle(){
			InitializeComponent();// This call is required by the Windows.Forms Form Designer.
			//Info=new InfoApt();
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
				if(Shadow!=null) {
					Shadow.Dispose();
				}
				if(baseFont!=null) {
					baseFont.Dispose();
				}
				if(boldFont!=null) {
					boldFont.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code

		private void InitializeComponent(){
			// 
			// ContrApptSingle
			// 
			this.Name = "ContrApptSingle";
			this.Size = new System.Drawing.Size(85, 72);
			this.Load += new System.EventHandler(this.ContrApptSingle_Load);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ContrApptSingle_MouseDown);

		}
		#endregion
		
		///<summary></summary>
		protected override void OnPaint(PaintEventArgs pea){
			//Graphics grfx=pea.Graphics;
			//grfx.DrawImage(shadow,0,0);
		}

		
		///<summary>This is only called when viewing appointments on the Appt module.  For Planned apt and pinboard, use SetSize instead so that the location won't change.</summary>
		public void SetLocation(){
			if(ContrApptSheet.IsWeeklyView) {
				Width=(int)ContrApptSheet.ColAptWidth;
				Location=new Point(ConvertToX(),ConvertToY());
			}
			else{
				Location=new Point(ConvertToX()+2,ConvertToY());
				Width=ContrApptSheet.ColWidth-5;
			}
			SetSize();
		}

		///<summary>Used from SetLocation. Also used for Planned apt and pinboard instead of SetLocation so that the location won't be altered.</summary>
		public void SetSize(){
			patternShowing=GetPatternShowing(DataRoww["Pattern"].ToString());
			//height is based on original 5 minute pattern. Might result in half-rows
			Height=DataRoww["Pattern"].ToString().Length*ContrApptSheet.Lh*ContrApptSheet.RowsPerIncr;
			//if(ContrApptSheet.TwoRowsPerIncrement){
			//	Height=Height*2;
			//}
			if(PrefC.GetLong(PrefName.AppointmentTimeIncrement)==10){
				Height=Height/2;
			}
			else{//15 minute increments
				Height=Height/3;
			}
			//if(ThisIsPinBoard){
				//hack:  it's at 99x96 right now
				//if(Height > ContrAppt.PinboardSize.Height-4)
				//	Height=ContrAppt.PinboardSize.Height-4;
				//if(Width > 99-2){
			//	Width=99-2;
				//}
			//}
		}
		
		///<summary>Called from SetLocation to establish X position of control.</summary>
		private int ConvertToX(){
			if(ContrApptSheet.IsWeeklyView) {
				//the next few lines are because we start on Monday instead of Sunday
				int dayofweek=(int)PIn.DateT(DataRoww["AptDateTime"].ToString()).DayOfWeek-1;
				if(dayofweek==-1) {
					dayofweek=6;
				}
				return ContrApptSheet.TimeWidth
					+ContrApptSheet.ColDayWidth*(dayofweek)+1
					+(int)(ContrApptSheet.ColAptWidth*(float)ApptViewItemL.GetIndexOp(PIn.Long(DataRoww["Op"].ToString())));
			}
			else {
				return ContrApptSheet.TimeWidth+ContrApptSheet.ProvWidth*ContrApptSheet.ProvCount
					+ContrApptSheet.ColWidth*(ApptViewItemL.GetIndexOp(PIn.Long(DataRoww["Op"].ToString())))+1;
					//Info.MyApt.Op))+1;
			}
		}

		///<summary>Called from SetLocation to establish Y position of control.  Also called from ContrAppt.RefreshDay when determining provBar markings. Does not round to the nearest row.</summary>
		public int ConvertToY(){
			DateTime aptDateTime=PIn.DateT(DataRoww["AptDateTime"].ToString());
			int retVal=(int)(((double)aptDateTime.Hour*(double)60
				/(double)PrefC.GetLong(PrefName.AppointmentTimeIncrement)
				+(double)aptDateTime.Minute
				/(double)PrefC.GetLong(PrefName.AppointmentTimeIncrement)
				)*(double)ContrApptSheet.Lh*ContrApptSheet.RowsPerIncr);
			return retVal;
		}

		///<summary>This converts the dbPattern in 5 minute interval into the pattern that will be viewed based on RowsPerIncrement and AppointmentTimeIncrement.  So it will always depend on the current view.Therefore, it should only be used for visual display purposes rather than within the FormAptEdit. If height of appointment allows a half row, then this includes an increment for that half row.</summary>
		public static string GetPatternShowing(string dbPattern){
			StringBuilder strBTime=new StringBuilder();
			for(int i=0;i<dbPattern.Length;i++){
				for(int j=0;j<ContrApptSheet.RowsPerIncr;j++){
					strBTime.Append(dbPattern.Substring(i,1));
				}
				i++;//skip
				if(PrefC.GetLong(PrefName.AppointmentTimeIncrement)==15){
					i++;//skip another
				}
			}
			return strBTime.ToString();
		}

		///<summary>It is planned to move some of this logic to OnPaint and use a true double buffer.</summary>
		public void CreateShadow(){
			if(this.Parent is ContrApptSheet) {
				bool isVisible=false;
				for(int j=0;j<ApptViewItemL.VisOps.Count;j++) {
					if(this.DataRoww["Op"].ToString()==OperatoryC.ListShort[ApptViewItemL.VisOps[j]].OperatoryNum.ToString()){
						isVisible=true;
					}
				}
				if(!isVisible){
					return;
				}
			}
			if(Shadow!=null){
				Shadow=null;
			}
			if(Width<4){
				return;
			}
			if(Height<4){
				return;
			}
			Shadow=new Bitmap(Width,Height);
			Graphics g=Graphics.FromImage(Shadow);
			Pen penB=new Pen(Color.Black);
			Pen penW=new Pen(Color.White);
			Pen penGr=new Pen(Color.SlateGray);
			Pen penDG=new Pen(Color.DarkSlateGray);
			Pen penO;//provider outline color
			Color backColor;
			Color provColor;
			Color confirmColor;
			confirmColor=DefC.GetColor(DefCat.ApptConfirmed,PIn.Long(DataRoww["Confirmed"].ToString()));
			if(DataRoww["ProvNum"].ToString()!="0" && DataRoww["IsHygiene"].ToString()=="0"){//dentist
				provColor=Providers.GetColor(PIn.Long(DataRoww["ProvNum"].ToString()));
				penO=new Pen(Providers.GetOutlineColor(PIn.Long(DataRoww["ProvNum"].ToString())));
			}
			else if(DataRoww["ProvHyg"].ToString()!="0" && DataRoww["IsHygiene"].ToString()=="1"){//hygienist
				provColor=Providers.GetColor(PIn.Long(DataRoww["ProvHyg"].ToString()));
				penO=new Pen(Providers.GetOutlineColor(PIn.Long(DataRoww["ProvHyg"].ToString())));
			}
			else{//unknown
				provColor=Color.White;
				penO=new Pen(Color.Black);
			}
			if(PIn.Long(DataRoww["AptStatus"].ToString())==(int)ApptStatus.Complete){
				backColor=DefC.Long[(int)DefCat.AppointmentColors][3].ItemColor;
			}
			else if (PIn.Long(DataRoww["AptStatus"].ToString())==(int)ApptStatus.PtNote) {
				backColor=DefC.Long[(int)DefCat.AppointmentColors][7].ItemColor;
			}
			else if (PIn.Long(DataRoww["AptStatus"].ToString()) == (int)ApptStatus.PtNoteCompleted) {
				backColor = DefC.Long[(int)DefCat.AppointmentColors][10].ItemColor;
			}
			else {
				backColor=provColor;
				//We might want to do something interesting here.
			}
			g.FillRectangle(new SolidBrush(backColor),7,0,Width-7,Height);
			g.FillRectangle(Brushes.White,0,0,7,Height);
			g.DrawLine(penB,7,0,7,Height);
			//Highlighting border
			if(IsSelected	|| 
				(!ThisIsPinBoard && DataRoww["AptNum"].ToString()==SelectedAptNum.ToString()))
			{
				//Left
				g.DrawLine(penO,8,1,8,Height-2);
				g.DrawLine(penO,9,1,9,Height-3);
				//Right
				g.DrawLine(penO,Width-2,1,Width-2,Height-2);
				g.DrawLine(penO,Width-3,2,Width-3,Height-3);
				//Top
				g.DrawLine(penO,8,1,Width-2,1);
				g.DrawLine(penO,8,2,Width-3,2);
				//bottom
				g.DrawLine(penO,9,Height-2,Width-2,Height-2);
				g.DrawLine(penO,10,Height-3,Width-3,Height-3);
			}
			Pen penTimediv=Pens.Silver;
			//g.TextRenderingHint=TextRenderingHint.SingleBitPerPixelGridFit;//to make printing clearer
			for(int i=0;i<patternShowing.Length;i++){//Info.MyApt.Pattern.Length;i++){
				if(patternShowing.Substring(i,1)=="X"){	
					g.FillRectangle(new SolidBrush(provColor),1,i*ContrApptSheet.Lh+1,6,ContrApptSheet.Lh);
				}
				else{
				}
				if(Math.IEEERemainder((double)i,(double)ContrApptSheet.RowsPerIncr)==0){//0/1
					g.DrawLine(penTimediv,1,i*ContrApptSheet.Lh,6,i*ContrApptSheet.Lh);
				}	
			}
			//elements=new string[] {"PatientName","Note","Lab","Procs"};
			int row=0;
			int elementI=0;
			while(row<patternShowing.Length && elementI<ApptViewItemL.ApptRows.Count) {
				row+=OnDrawElement(g,elementI,row);
				elementI++;
			}
			//Main outline
			g.DrawRectangle(new Pen(Color.Black),0,0,Width-1,Height-1);
			//Credit and ins
			if(!ContrApptSheet.IsWeeklyView) {
				g.FillRectangle(new SolidBrush(confirmColor),Width-13,1,12,ContrApptSheet.Lh-2);
				g.DrawRectangle(new Pen(Color.Black),Width-13,0,13,ContrApptSheet.Lh-1);
				//if note, then draw note symbol ♫
				string strNote="";
				if (PIn.Long(DataRoww["AptStatus"].ToString()) == (int)ApptStatus.PtNote 
					|| PIn.Long(DataRoww["AptStatus"].ToString()) == (int)ApptStatus.PtNoteCompleted) 
				{
					strNote = "♫";
					g.DrawString(strNote, baseFont, new SolidBrush(Color.DarkBlue), Width - 13, -1);//0,-1);
				}
				else {
					Color color=Color.Black;
					int xpos=Width-14;
					Font font=baseFont;
					if(DataRoww["creditIns"].ToString().Contains("!")) {
						color=Color.Red;
						font=boldFont;
					}
					if((DataRoww["creditIns"].ToString().Equals("!")) | (DataRoww["creditIns"].ToString().Equals("I"))) {
						xpos=Width-12;
					}
					g.DrawString(strNote+DataRoww["creditIns"].ToString(),font,new SolidBrush(color),xpos,-1);
				}
				//assistant box
				if(DataRoww["Assistant"].ToString()!="0"){
					g.FillRectangle(new SolidBrush(Color.White),Width-18,Height-ContrApptSheet.Lh,17,ContrApptSheet.Lh-1);
					g.DrawLine(Pens.Gray,Width-18,Height-ContrApptSheet.Lh,Width,Height-ContrApptSheet.Lh);
					g.DrawLine(Pens.Gray,Width-18,Height-ContrApptSheet.Lh,Width-18,Height);
					g.DrawString(Employees.GetAbbr(PIn.Long(DataRoww["Assistant"].ToString()))
						,baseFont,new SolidBrush(Color.Black),Width-18,Height-ContrApptSheet.Lh-1);
				}
			}
			if(DataRoww["AptStatus"].ToString()==((int)ApptStatus.Broken).ToString()){
				g.DrawLine(new Pen(Color.Black),8,1,Width-1,Height-1);
				g.DrawLine(new Pen(Color.Black),8,Height-1,Width-1,1);
			}
			this.BackgroundImage=Shadow;
			//Shadow=null;
			g.Dispose();
		}

		///<summary>Called from createShadow once for each element. The rowI is specified so that this method knows where to start drawing.  Returns the number of rows that this element fills.</summary>
		private int OnDrawElement(Graphics g,int elementI,int rowI){
			int xPos=9;
			int yPos=rowI*ContrApptSheet.Lh;
			SolidBrush brush;
			/*
			if (PIn.PLong(DataRoww["AptStatus"].ToString()) == (int)ApptStatus.PtNote) {
				brush = new SolidBrush(DefC.Long[(int)DefCat.AppointmentColors][6].ItemColor);
			}
			else if (PIn.PLong(DataRoww["AptStatus"].ToString()) == (int)ApptStatus.PtNoteCompleted) {
				brush= new SolidBrush(DefC.Long[(int)DefCat.AppointmentColors][9].ItemColor);
			}
			else {//appointment*/
				brush= new SolidBrush(ApptViewItemL.ApptRows[elementI].ElementColor);
			//}
			SolidBrush noteTitlebrush = new SolidBrush(DefC.Long[(int)DefCat.AppointmentColors][8].ItemColor);
			StringFormat format=new StringFormat();
			//Font notetitleFont=new Font("Arial",7);
			format.Alignment=StringAlignment.Near;
			int charactersFitted;//not used
			int linesFilled;
			SizeF noteSize;
			RectangleF rect;
			string text;
			bool isNote=false;
			if(PIn.Long(DataRoww["AptStatus"].ToString()) == (int)ApptStatus.PtNote
				|| PIn.Long(DataRoww["AptStatus"].ToString()) == (int)ApptStatus.PtNoteCompleted) 
			{
				isNote=true;
			}
			/*
				switch(ApptViewItemL.ApptRows[elementI].ElementDesc) {
					case "Note":
						text = DataRoww["Note"].ToString();
						if(rowI == 0)
							noteSize = g.MeasureString(text,baseFont,Width - 9 - 4);
						else
							noteSize = g.MeasureString(text,baseFont,Width - 9);
						g.MeasureString(text,baseFont,noteSize,format,out charactersFitted,out linesFilled);
						rect = new RectangleF(new PointF(xPos,yPos),noteSize);//-3), noteSize);
						g.DrawString(text,baseFont,brush,rect,format);
						return linesFilled;
					case "PatientName":
						g.DrawString(DataRoww["patientName"].ToString(),baseFont, //notetitleFont, 
							noteTitlebrush,xPos,yPos);//+1);
						return 1;
				}
			}
			else{*/
				switch(ApptViewItemL.ApptRows[elementI].ElementDesc){
					case "Address":
						text=DataRoww["address"].ToString();
						noteSize=g.MeasureString(text,baseFont,Width-9);
						g.MeasureString(text,baseFont,noteSize,format,out charactersFitted,out linesFilled);
						rect=new RectangleF(new PointF(xPos,yPos),noteSize);
						g.DrawString(text,baseFont,brush,rect,format);
						return linesFilled;
					case "AddrNote":
						text=DataRoww["addrNote"].ToString();
						if(rowI==0)
							noteSize=g.MeasureString(text,baseFont,Width-9-4);
						else
							noteSize=g.MeasureString(text,baseFont,Width-9);
						g.MeasureString(text,baseFont,noteSize,format,out charactersFitted,out linesFilled);
						rect=new RectangleF(new PointF(xPos,yPos),noteSize);
						g.DrawString(text,baseFont,brush,rect,format);
						return linesFilled;
					case "Age":
						g.DrawString(DataRoww["age"].ToString(),baseFont,brush,xPos,yPos);
						return 1;
					case "ASAP":
						if(DataRoww["AptStatus"].ToString()==((int)ApptStatus.ASAP).ToString()){
							text=Lan.g("enumApptStatus","ASAP");
							g.DrawString(text,baseFont,brush,xPos,yPos);
							return 1;
						}
						else{
							return 0;
						}
					case "ChartNumAndName":
						text=DataRoww["chartNumAndName"].ToString();
						g.DrawString(text,baseFont,brush,xPos,yPos);
						return 1;
					case "ChartNumber":
						g.DrawString(DataRoww["chartNumber"].ToString(),baseFont,brush,xPos,yPos);
						return 1;
					case "HmPhone":
						g.DrawString(DataRoww["hmPhone"].ToString(),baseFont,brush,xPos,yPos);
						return 1;
					case "Lab":
						text=DataRoww["lab"].ToString();
						if(text==""){
							return 0;
						}
						else {
							g.DrawString(text,baseFont,brush,xPos,yPos);
							return 1;
						}
					case "MedUrgNote":
						text=DataRoww["MedUrgNote"].ToString();
						if(rowI==0)
							noteSize=g.MeasureString(text,baseFont,Width-9-4);
						else
							noteSize=g.MeasureString(text,baseFont,Width-9);
						g.MeasureString(text,baseFont,noteSize,format,out charactersFitted,out linesFilled);
						rect=new RectangleF(new PointF(xPos,yPos),noteSize);
						g.DrawString(text,baseFont,brush,rect,format);
						return linesFilled;
					case "Note":
						text=DataRoww["Note"].ToString();
						if(rowI==0)
							noteSize=g.MeasureString(text,baseFont,Width-9-4);
						else
							noteSize=g.MeasureString(text,baseFont,Width-9);
						g.MeasureString(text,baseFont,noteSize,format,out charactersFitted,out linesFilled);
						rect=new RectangleF(new PointF(xPos,yPos),noteSize);
						g.DrawString(text,baseFont,brush,rect,format);
						return linesFilled;
					case "PatientName":
						g.DrawString(DataRoww["patientName"].ToString(),baseFont,brush,xPos,yPos);
						return 1;
					case "PatNum":
						g.DrawString(DataRoww["patNum"].ToString(),baseFont,brush,xPos,yPos);
						return 1;
					case "PatNumAndName":
						g.DrawString(DataRoww["patNumAndName"].ToString(),baseFont,brush,xPos,yPos);
						return 1;
					case "PremedFlag":
						if(DataRoww["preMedFlag"].ToString()==""){
							return 0;
						}
						else{
							g.DrawString(DataRoww["preMedFlag"].ToString(),baseFont,brush,xPos,yPos);
							return 1;
						}
					case "Procs":
						text=DataRoww["procs"].ToString();
						if(rowI==0)
							noteSize=g.MeasureString(text,baseFont,Width-9-4);
						else
							noteSize=g.MeasureString(text,baseFont,Width-9);
						g.MeasureString(text,baseFont,noteSize,format,out charactersFitted,out linesFilled);
						rect=new RectangleF(new PointF(xPos,yPos),noteSize);
						g.DrawString(text,baseFont,brush,rect,format);
						return linesFilled;
					case "Production":
						if(isNote) {
							return 0;
						}
						g.DrawString(DataRoww["production"].ToString(),baseFont,brush,xPos,yPos);
						return 1;
					case "Provider":
						g.DrawString(DataRoww["provider"].ToString(),baseFont,brush,xPos,yPos);
						return 1;
					case "WirelessPhone":
						g.DrawString(DataRoww["wirelessPhone"].ToString(),baseFont,brush,xPos,yPos);
						return 1;
					case "WkPhone":
						g.DrawString(DataRoww["wkPhone"].ToString(),baseFont,brush,xPos,yPos);
						return 1;
				}
			//}
			return 0;
		}

		private void ContrApptSingle_Load(object sender, System.EventArgs e){
			/*
			if(Info.IsNext){
				Width=110;
				//don't change location
			}
			else{
				Location=new Point(ConvertToX(),ConvertToY());
				Width=ContrApptSheet.ColWidth-1;
				Height=Info.MyApt.Pattern.Length*ContrApptSheet.Lh;
			}
			*/
		}

		private void ContrApptSingle_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			ClickedAptNum=PIn.Long(DataRoww["AptNum"].ToString());
		}




	}//end class
}//end namespace
