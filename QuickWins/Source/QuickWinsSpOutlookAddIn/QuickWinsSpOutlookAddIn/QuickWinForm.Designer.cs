using System.Collections.Generic;

namespace QuickWinsSpOutlookAddIn
{
    
partial class QuickWinForm
    {

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        

        private void InitializeComponent()
        {
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.txtRequestedBy = new DevExpress.XtraEditors.ButtonEdit();
            this.comboResolution = new System.Windows.Forms.ComboBox();
            this.comboSystem = new System.Windows.Forms.ComboBox();
            this.comboSource = new System.Windows.Forms.ComboBox();
            this.comboIssue = new System.Windows.Forms.ComboBox();
            this.txtOtherResolution = new DevExpress.XtraEditors.MemoEdit();
            this.txtOherIssue = new DevExpress.XtraEditors.MemoEdit();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRequestedBy.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOtherResolution.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOherIssue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.txtRequestedBy);
            this.layoutControl1.Controls.Add(this.comboResolution);
            this.layoutControl1.Controls.Add(this.comboSystem);
            this.layoutControl1.Controls.Add(this.comboSource);
            this.layoutControl1.Controls.Add(this.comboIssue);
            this.layoutControl1.Controls.Add(this.txtOtherResolution);
            this.layoutControl1.Controls.Add(this.txtOherIssue);
            this.layoutControl1.Controls.Add(this.dateTimePicker1);
            this.layoutControl1.Controls.Add(this.simpleButton1);
            this.layoutControl1.Controls.Add(this.simpleButton2);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(660, 319, 923, 522);
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(568, 510);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // txtRequestedBy
            // 
            this.txtRequestedBy.Location = new System.Drawing.Point(93, 36);
            this.txtRequestedBy.Name = "txtRequestedBy";
            this.txtRequestedBy.Properties.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.txtRequestedBy.Properties.Appearance.Options.UseBackColor = true;
            this.txtRequestedBy.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtRequestedBy.Size = new System.Drawing.Size(463, 20);
            this.txtRequestedBy.StyleController = this.layoutControl1;
            this.txtRequestedBy.TabIndex = 1;
            this.txtRequestedBy.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtRequestedBy_ButtonClick);
            this.txtRequestedBy.EditValueChanged += new System.EventHandler(this.txtRequestedBy_EditValueChanged);
            this.txtRequestedBy.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtRequestedBy_KeyUp);
            // 
            // comboResolution
            // 
            this.comboResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboResolution.FormattingEnabled = true;
            this.comboResolution.Location = new System.Drawing.Point(93, 261);
            this.comboResolution.Name = "comboResolution";
            this.comboResolution.Size = new System.Drawing.Size(463, 21);
            this.comboResolution.TabIndex = 5;
            this.comboResolution.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // comboSystem
            // 
            this.comboSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSystem.FormattingEnabled = true;
            this.comboSystem.Location = new System.Drawing.Point(93, 85);
            this.comboSystem.Name = "comboSystem";
            this.comboSystem.Size = new System.Drawing.Size(463, 21);
            this.comboSystem.TabIndex = 3;
            this.comboSystem.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // comboSource
            // 
            this.comboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSource.FormattingEnabled = true;
            this.comboSource.Items.AddRange(new object[] {
            "Phone",
            "Email",
            "Desk Visit",
            "Skype"});
            this.comboSource.Location = new System.Drawing.Point(93, 60);
            this.comboSource.Name = "comboSource";
            this.comboSource.Size = new System.Drawing.Size(463, 21);
            this.comboSource.TabIndex = 2;
            this.comboSource.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // comboIssue
            // 
            this.comboIssue.AllowDrop = true;
            this.comboIssue.BackColor = System.Drawing.SystemColors.ControlLight;
            this.comboIssue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboIssue.FormattingEnabled = true;
            this.comboIssue.Location = new System.Drawing.Point(93, 110);
            this.comboIssue.Name = "comboIssue";
            this.comboIssue.Size = new System.Drawing.Size(463, 21);
            this.comboIssue.TabIndex = 4;
            this.comboIssue.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // txtOtherResolution
            // 
            this.txtOtherResolution.Location = new System.Drawing.Point(93, 286);
            this.txtOtherResolution.Name = "txtOtherResolution";
            this.txtOtherResolution.Properties.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.txtOtherResolution.Properties.Appearance.Options.UseBackColor = true;
            this.txtOtherResolution.Size = new System.Drawing.Size(463, 186);
            this.txtOtherResolution.StyleController = this.layoutControl1;
            this.txtOtherResolution.TabIndex = 15;
            this.txtOtherResolution.Visible = false;
            this.txtOtherResolution.EditValueChanged += new System.EventHandler(this.memoEdit2_EditValueChanged);
            // 
            // txtOherIssue
            // 
            this.txtOherIssue.Location = new System.Drawing.Point(93, 135);
            this.txtOherIssue.Name = "txtOherIssue";
            this.txtOherIssue.Properties.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.txtOherIssue.Properties.Appearance.Options.UseBackColor = true;
            this.txtOherIssue.Size = new System.Drawing.Size(463, 122);
            this.txtOherIssue.StyleController = this.layoutControl1;
            this.txtOherIssue.TabIndex = 14;
            this.txtOherIssue.Visible = false;
            this.txtOherIssue.EditValueChanged += new System.EventHandler(this.memoEdit1_EditValueChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarMonthBackground = System.Drawing.SystemColors.ButtonShadow;
            this.dateTimePicker1.CalendarTitleBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.dateTimePicker1.Location = new System.Drawing.Point(93, 12);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(463, 20);
            this.dateTimePicker1.TabIndex = 10;
            this.dateTimePicker1.TabStop = false;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(201, 476);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(163, 22);
            this.simpleButton1.StyleController = this.layoutControl1;
            this.simpleButton1.TabIndex = 6;
            this.simpleButton1.Text = "Submit";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(368, 476);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(188, 22);
            this.simpleButton2.StyleController = this.layoutControl1;
            this.simpleButton2.TabIndex = 13;
            this.simpleButton2.Text = "Cancel";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem10,
            this.layoutControlItem9,
            this.emptySpaceItem1,
            this.layoutControlItem11,
            this.layoutControlItem6,
            this.layoutControlItem8,
            this.layoutControlItem5,
            this.layoutControlItem12,
            this.layoutControlItem13,
            this.layoutControlItem2});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(568, 510);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.BackColor = System.Drawing.Color.Transparent;
            this.layoutControlItem1.AppearanceItemCaption.BackColor2 = System.Drawing.Color.Transparent;
            this.layoutControlItem1.AppearanceItemCaption.Options.UseBackColor = true;
            this.layoutControlItem1.Control = this.dateTimePicker1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(548, 24);
            this.layoutControlItem1.Text = "Requested Date";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(78, 13);
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.simpleButton2;
            this.layoutControlItem10.Location = new System.Drawing.Point(356, 464);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(192, 26);
            this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem10.TextVisible = false;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.simpleButton1;
            this.layoutControlItem9.Location = new System.Drawing.Point(189, 464);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(167, 26);
            this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem9.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 464);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(189, 26);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.txtOherIssue;
            this.layoutControlItem11.Location = new System.Drawing.Point(0, 123);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(548, 126);
            this.layoutControlItem11.Text = "Other";
            this.layoutControlItem11.TextSize = new System.Drawing.Size(78, 13);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.txtOtherResolution;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 274);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(548, 190);
            this.layoutControlItem6.Text = "Other";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(78, 13);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.comboIssue;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 98);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(548, 25);
            this.layoutControlItem8.Text = "Problem/ Issue";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(78, 13);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.comboSource;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(548, 25);
            this.layoutControlItem5.Text = "Source";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(78, 13);
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this.comboSystem;
            this.layoutControlItem12.Location = new System.Drawing.Point(0, 73);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Size = new System.Drawing.Size(548, 25);
            this.layoutControlItem12.Text = "System";
            this.layoutControlItem12.TextSize = new System.Drawing.Size(78, 13);
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this.comboResolution;
            this.layoutControlItem13.Location = new System.Drawing.Point(0, 249);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Size = new System.Drawing.Size(548, 25);
            this.layoutControlItem13.Text = "Resolution";
            this.layoutControlItem13.TextSize = new System.Drawing.Size(78, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txtRequestedBy;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(548, 24);
            this.layoutControlItem2.Text = "Requested By";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(78, 13);
            // 
            // QuickWinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(568, 510);
            this.Controls.Add(this.layoutControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickWinForm";
            this.ShowIcon = false;
            this.Text = "QuickWinForm";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtRequestedBy.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOtherResolution.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOherIssue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraEditors.MemoEdit txtOtherResolution;
        private DevExpress.XtraEditors.MemoEdit txtOherIssue;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private System.Windows.Forms.ComboBox comboIssue;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private System.Windows.Forms.ComboBox comboResolution;
        private System.Windows.Forms.ComboBox comboSystem;
        private System.Windows.Forms.ComboBox comboSource;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem12;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem13;
        private DevExpress.XtraEditors.ButtonEdit txtRequestedBy;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}