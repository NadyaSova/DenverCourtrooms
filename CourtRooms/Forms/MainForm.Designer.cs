namespace CourtRooms
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnStart = new System.Windows.Forms.Button();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.lblLastProcessedTitle = new System.Windows.Forms.Label();
            this.lblLastProcessedCase = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabCaseNumber = new System.Windows.Forms.TabPage();
            this.tabCalendar = new System.Windows.Forms.TabPage();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.ddlRoom = new System.Windows.Forms.ComboBox();
            this.lblRoom = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.chkManualCaptcha = new System.Windows.Forms.CheckBox();
            this.tabMain.SuspendLayout();
            this.tabCaseNumber.SuspendLayout();
            this.tabCalendar.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStart.Location = new System.Drawing.Point(14, 139);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(90, 34);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(7, 12);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(128, 17);
            this.lblFrom.TabIndex = 1;
            this.lblFrom.Text = "Case number from:";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(251, 12);
            this.lblTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(24, 17);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "to:";
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(143, 9);
            this.txtFrom.Margin = new System.Windows.Forms.Padding(4);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(100, 23);
            this.txtFrom.TabIndex = 3;
            this.txtFrom.Text = "19GS000001";
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(282, 9);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(100, 23);
            this.txtTo.TabIndex = 4;
            this.txtTo.Text = "19GS000050";
            this.txtTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTo_KeyDown);
            // 
            // lblLastProcessedTitle
            // 
            this.lblLastProcessedTitle.AutoSize = true;
            this.lblLastProcessedTitle.Location = new System.Drawing.Point(12, 191);
            this.lblLastProcessedTitle.Name = "lblLastProcessedTitle";
            this.lblLastProcessedTitle.Size = new System.Drawing.Size(146, 17);
            this.lblLastProcessedTitle.TabIndex = 5;
            this.lblLastProcessedTitle.Text = "Last Processed Case:";
            // 
            // lblLastProcessedCase
            // 
            this.lblLastProcessedCase.AutoSize = true;
            this.lblLastProcessedCase.Location = new System.Drawing.Point(164, 191);
            this.lblLastProcessedCase.Name = "lblLastProcessedCase";
            this.lblLastProcessedCase.Size = new System.Drawing.Size(0, 17);
            this.lblLastProcessedCase.TabIndex = 6;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(11, 223);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(536, 296);
            this.txtLog.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(132, 139);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 34);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabMain
            // 
            this.tabMain.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabMain.Controls.Add(this.tabCaseNumber);
            this.tabMain.Controls.Add(this.tabCalendar);
            this.tabMain.Location = new System.Drawing.Point(3, 12);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(549, 79);
            this.tabMain.TabIndex = 9;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            // 
            // tabCaseNumber
            // 
            this.tabCaseNumber.BackColor = System.Drawing.SystemColors.Control;
            this.tabCaseNumber.Controls.Add(this.lblFrom);
            this.tabCaseNumber.Controls.Add(this.lblTo);
            this.tabCaseNumber.Controls.Add(this.txtFrom);
            this.tabCaseNumber.Controls.Add(this.txtTo);
            this.tabCaseNumber.Location = new System.Drawing.Point(4, 28);
            this.tabCaseNumber.Name = "tabCaseNumber";
            this.tabCaseNumber.Padding = new System.Windows.Forms.Padding(3);
            this.tabCaseNumber.Size = new System.Drawing.Size(541, 47);
            this.tabCaseNumber.TabIndex = 0;
            this.tabCaseNumber.Text = "Search by Case Number";
            // 
            // tabCalendar
            // 
            this.tabCalendar.BackColor = System.Drawing.SystemColors.Control;
            this.tabCalendar.Controls.Add(this.dtTo);
            this.tabCalendar.Controls.Add(this.ddlRoom);
            this.tabCalendar.Controls.Add(this.lblRoom);
            this.tabCalendar.Controls.Add(this.label2);
            this.tabCalendar.Controls.Add(this.dtFrom);
            this.tabCalendar.Controls.Add(this.label3);
            this.tabCalendar.Location = new System.Drawing.Point(4, 28);
            this.tabCalendar.Name = "tabCalendar";
            this.tabCalendar.Padding = new System.Windows.Forms.Padding(3);
            this.tabCalendar.Size = new System.Drawing.Size(541, 47);
            this.tabCalendar.TabIndex = 1;
            this.tabCalendar.Text = "Search by Date";
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "MM/dd/yyyy";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(422, 9);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(105, 23);
            this.dtTo.TabIndex = 11;
            this.dtTo.Value = new System.DateTime(2019, 11, 30, 0, 0, 0, 0);
            this.dtTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtTo_KeyDown);
            // 
            // ddlRoom
            // 
            this.ddlRoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRoom.FormattingEnabled = true;
            this.ddlRoom.Location = new System.Drawing.Point(62, 9);
            this.ddlRoom.Name = "ddlRoom";
            this.ddlRoom.Size = new System.Drawing.Size(140, 24);
            this.ddlRoom.TabIndex = 10;
            // 
            // lblRoom
            // 
            this.lblRoom.AutoSize = true;
            this.lblRoom.Location = new System.Drawing.Point(7, 11);
            this.lblRoom.Name = "lblRoom";
            this.lblRoom.Size = new System.Drawing.Size(49, 17);
            this.lblRoom.TabIndex = 9;
            this.lblRoom.Text = "Room:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(216, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Date from:";
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "MM/dd/yyyy";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(291, 9);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(105, 23);
            this.dtFrom.TabIndex = 10;
            this.dtFrom.Value = new System.DateTime(2019, 11, 1, 0, 0, 0, 0);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(398, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "to:";
            // 
            // chkManualCaptcha
            // 
            this.chkManualCaptcha.AutoSize = true;
            this.chkManualCaptcha.Location = new System.Drawing.Point(18, 93);
            this.chkManualCaptcha.Name = "chkManualCaptcha";
            this.chkManualCaptcha.Size = new System.Drawing.Size(176, 21);
            this.chkManualCaptcha.TabIndex = 10;
            this.chkManualCaptcha.Text = "Solve captcha manually";
            this.chkManualCaptcha.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 531);
            this.Controls.Add(this.chkManualCaptcha);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblLastProcessedTitle);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblLastProcessedCase);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Denver Courtrooms";
            this.tabMain.ResumeLayout(false);
            this.tabCaseNumber.ResumeLayout(false);
            this.tabCaseNumber.PerformLayout();
            this.tabCalendar.ResumeLayout(false);
            this.tabCalendar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.Label lblLastProcessedTitle;
        private System.Windows.Forms.Label lblLastProcessedCase;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabCaseNumber;
        private System.Windows.Forms.TabPage tabCalendar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddlRoom;
        private System.Windows.Forms.Label lblRoom;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.CheckBox chkManualCaptcha;
    }
}

