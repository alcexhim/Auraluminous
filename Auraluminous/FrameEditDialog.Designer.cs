namespace Auraluminous
{
    partial class FrameEditDialog
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lvFixtures = new System.Windows.Forms.ListView();
            this.chModel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chManufacturer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmdFixturesAdd = new System.Windows.Forms.Button();
            this.cmdFixturesModify = new System.Windows.Forms.Button();
            this.cmdFixturesRemove = new System.Windows.Forms.Button();
            this.cmdFixturesClear = new System.Windows.Forms.Button();
            this.fraFixtures = new System.Windows.Forms.GroupBox();
            this.fraFrames = new System.Windows.Forms.GroupBox();
            this.cmdFramesAdd = new System.Windows.Forms.Button();
            this.cmdFramesClear = new System.Windows.Forms.Button();
            this.lvFrames = new System.Windows.Forms.ListView();
            this.chChannel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmdFramesModify = new System.Windows.Forms.Button();
            this.cmdFramesRemove = new System.Windows.Forms.Button();
            this.lblTimestamp = new System.Windows.Forms.Label();
            this.txtTimestamp = new System.Windows.Forms.TextBox();
            this.fraFixtures.SuspendLayout();
            this.fraFrames.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdCancel.Location = new System.Drawing.Point(356, 317);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdOK.Location = new System.Drawing.Point(275, 317);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 2;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // lvFixtures
            // 
            this.lvFixtures.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFixtures.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chModel,
            this.chManufacturer});
            this.lvFixtures.FullRowSelect = true;
            this.lvFixtures.GridLines = true;
            this.lvFixtures.HideSelection = false;
            this.lvFixtures.Location = new System.Drawing.Point(6, 48);
            this.lvFixtures.Name = "lvFixtures";
            this.lvFixtures.Size = new System.Drawing.Size(407, 85);
            this.lvFixtures.TabIndex = 4;
            this.lvFixtures.UseCompatibleStateImageBehavior = false;
            this.lvFixtures.View = System.Windows.Forms.View.Details;
            this.lvFixtures.SelectedIndexChanged += new System.EventHandler(this.lvFixtures_SelectedIndexChanged);
            // 
            // chModel
            // 
            this.chModel.Text = "Model";
            this.chModel.Width = 194;
            // 
            // chManufacturer
            // 
            this.chManufacturer.Text = "Manufacturer";
            this.chManufacturer.Width = 117;
            // 
            // cmdFixturesAdd
            // 
            this.cmdFixturesAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdFixturesAdd.Location = new System.Drawing.Point(6, 19);
            this.cmdFixturesAdd.Name = "cmdFixturesAdd";
            this.cmdFixturesAdd.Size = new System.Drawing.Size(75, 23);
            this.cmdFixturesAdd.TabIndex = 0;
            this.cmdFixturesAdd.Text = "&Add...";
            this.cmdFixturesAdd.UseVisualStyleBackColor = true;
            this.cmdFixturesAdd.Click += new System.EventHandler(this.cmdFixturesAdd_Click);
            // 
            // cmdFixturesModify
            // 
            this.cmdFixturesModify.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdFixturesModify.Location = new System.Drawing.Point(87, 19);
            this.cmdFixturesModify.Name = "cmdFixturesModify";
            this.cmdFixturesModify.Size = new System.Drawing.Size(75, 23);
            this.cmdFixturesModify.TabIndex = 1;
            this.cmdFixturesModify.Text = "&Modify...";
            this.cmdFixturesModify.UseVisualStyleBackColor = true;
            // 
            // cmdFixturesRemove
            // 
            this.cmdFixturesRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdFixturesRemove.Location = new System.Drawing.Point(168, 19);
            this.cmdFixturesRemove.Name = "cmdFixturesRemove";
            this.cmdFixturesRemove.Size = new System.Drawing.Size(75, 23);
            this.cmdFixturesRemove.TabIndex = 2;
            this.cmdFixturesRemove.Text = "&Remove";
            this.cmdFixturesRemove.UseVisualStyleBackColor = true;
            // 
            // cmdFixturesClear
            // 
            this.cmdFixturesClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdFixturesClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdFixturesClear.Location = new System.Drawing.Point(338, 19);
            this.cmdFixturesClear.Name = "cmdFixturesClear";
            this.cmdFixturesClear.Size = new System.Drawing.Size(75, 23);
            this.cmdFixturesClear.TabIndex = 3;
            this.cmdFixturesClear.Text = "Clear";
            this.cmdFixturesClear.UseVisualStyleBackColor = true;
            // 
            // fraFixtures
            // 
            this.fraFixtures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fraFixtures.Controls.Add(this.cmdFixturesAdd);
            this.fraFixtures.Controls.Add(this.cmdFixturesClear);
            this.fraFixtures.Controls.Add(this.lvFixtures);
            this.fraFixtures.Controls.Add(this.cmdFixturesRemove);
            this.fraFixtures.Controls.Add(this.cmdFixturesModify);
            this.fraFixtures.Location = new System.Drawing.Point(12, 38);
            this.fraFixtures.Name = "fraFixtures";
            this.fraFixtures.Size = new System.Drawing.Size(419, 139);
            this.fraFixtures.TabIndex = 0;
            this.fraFixtures.TabStop = false;
            this.fraFixtures.Text = "Fixtures";
            // 
            // fraFrames
            // 
            this.fraFrames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fraFrames.Controls.Add(this.cmdFramesAdd);
            this.fraFrames.Controls.Add(this.cmdFramesClear);
            this.fraFrames.Controls.Add(this.lvFrames);
            this.fraFrames.Controls.Add(this.cmdFramesModify);
            this.fraFrames.Controls.Add(this.cmdFramesRemove);
            this.fraFrames.Location = new System.Drawing.Point(12, 183);
            this.fraFrames.Name = "fraFrames";
            this.fraFrames.Size = new System.Drawing.Size(419, 128);
            this.fraFrames.TabIndex = 1;
            this.fraFrames.TabStop = false;
            this.fraFrames.Text = "Frames";
            // 
            // cmdFramesAdd
            // 
            this.cmdFramesAdd.Enabled = false;
            this.cmdFramesAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdFramesAdd.Location = new System.Drawing.Point(6, 19);
            this.cmdFramesAdd.Name = "cmdFramesAdd";
            this.cmdFramesAdd.Size = new System.Drawing.Size(75, 23);
            this.cmdFramesAdd.TabIndex = 0;
            this.cmdFramesAdd.Text = "&Add...";
            this.cmdFramesAdd.UseVisualStyleBackColor = true;
            this.cmdFramesAdd.Click += new System.EventHandler(this.cmdFramesAdd_Click);
            // 
            // cmdFramesClear
            // 
            this.cmdFramesClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdFramesClear.Enabled = false;
            this.cmdFramesClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdFramesClear.Location = new System.Drawing.Point(338, 19);
            this.cmdFramesClear.Name = "cmdFramesClear";
            this.cmdFramesClear.Size = new System.Drawing.Size(75, 23);
            this.cmdFramesClear.TabIndex = 3;
            this.cmdFramesClear.Text = "Clear";
            this.cmdFramesClear.UseVisualStyleBackColor = true;
            // 
            // lvFrames
            // 
            this.lvFrames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFrames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chChannel,
            this.chValue});
            this.lvFrames.Enabled = false;
            this.lvFrames.FullRowSelect = true;
            this.lvFrames.GridLines = true;
            this.lvFrames.HideSelection = false;
            this.lvFrames.Location = new System.Drawing.Point(6, 48);
            this.lvFrames.Name = "lvFrames";
            this.lvFrames.Size = new System.Drawing.Size(407, 74);
            this.lvFrames.TabIndex = 4;
            this.lvFrames.UseCompatibleStateImageBehavior = false;
            this.lvFrames.View = System.Windows.Forms.View.Details;
            // 
            // chChannel
            // 
            this.chChannel.Text = "Channel";
            this.chChannel.Width = 264;
            // 
            // chValue
            // 
            this.chValue.Text = "Value";
            this.chValue.Width = 133;
            // 
            // cmdFramesModify
            // 
            this.cmdFramesModify.Enabled = false;
            this.cmdFramesModify.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdFramesModify.Location = new System.Drawing.Point(87, 19);
            this.cmdFramesModify.Name = "cmdFramesModify";
            this.cmdFramesModify.Size = new System.Drawing.Size(75, 23);
            this.cmdFramesModify.TabIndex = 1;
            this.cmdFramesModify.Text = "&Modify...";
            this.cmdFramesModify.UseVisualStyleBackColor = true;
            // 
            // cmdFramesRemove
            // 
            this.cmdFramesRemove.Enabled = false;
            this.cmdFramesRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdFramesRemove.Location = new System.Drawing.Point(168, 19);
            this.cmdFramesRemove.Name = "cmdFramesRemove";
            this.cmdFramesRemove.Size = new System.Drawing.Size(75, 23);
            this.cmdFramesRemove.TabIndex = 2;
            this.cmdFramesRemove.Text = "&Remove";
            this.cmdFramesRemove.UseVisualStyleBackColor = true;
            // 
            // lblTimestamp
            // 
            this.lblTimestamp.AutoSize = true;
            this.lblTimestamp.Location = new System.Drawing.Point(12, 15);
            this.lblTimestamp.Name = "lblTimestamp";
            this.lblTimestamp.Size = new System.Drawing.Size(61, 13);
            this.lblTimestamp.TabIndex = 4;
            this.lblTimestamp.Text = "Timestamp:";
            // 
            // txtTimestamp
            // 
            this.txtTimestamp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTimestamp.Location = new System.Drawing.Point(79, 12);
            this.txtTimestamp.Name = "txtTimestamp";
            this.txtTimestamp.ReadOnly = true;
            this.txtTimestamp.Size = new System.Drawing.Size(352, 20);
            this.txtTimestamp.TabIndex = 5;
            // 
            // FrameEditDialog
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(443, 352);
            this.Controls.Add(this.txtTimestamp);
            this.Controls.Add(this.lblTimestamp);
            this.Controls.Add(this.fraFrames);
            this.Controls.Add(this.fraFixtures);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.MinimumSize = new System.Drawing.Size(459, 391);
            this.Name = "FrameEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Frame Edit";
            this.fraFixtures.ResumeLayout(false);
            this.fraFrames.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.ListView lvFixtures;
        private System.Windows.Forms.ColumnHeader chModel;
        private System.Windows.Forms.ColumnHeader chManufacturer;
        private System.Windows.Forms.Button cmdFixturesAdd;
        private System.Windows.Forms.Button cmdFixturesModify;
        private System.Windows.Forms.Button cmdFixturesRemove;
        private System.Windows.Forms.Button cmdFixturesClear;
        private System.Windows.Forms.GroupBox fraFixtures;
        private System.Windows.Forms.GroupBox fraFrames;
        private System.Windows.Forms.ListView lvFrames;
        private System.Windows.Forms.ColumnHeader chChannel;
        private System.Windows.Forms.ColumnHeader chValue;
        private System.Windows.Forms.Button cmdFramesAdd;
        private System.Windows.Forms.Button cmdFramesClear;
        private System.Windows.Forms.Button cmdFramesModify;
        private System.Windows.Forms.Button cmdFramesRemove;
        private System.Windows.Forms.Label lblTimestamp;
        internal System.Windows.Forms.TextBox txtTimestamp;
    }
}