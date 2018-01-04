namespace ArcanumJPEditor {
    partial class DlgEditor {
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( bool disposing ) {
            if ( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( DlgEditor ) );
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxID = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControlLine = new System.Windows.Forms.TabControl();
            this.tabPageMale = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.textBoxOriginalM = new System.Windows.Forms.TextBox();
            this.textBoxModifyM = new System.Windows.Forms.TextBox();
            this.tabPageFemale = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.textBoxOriginalF = new System.Windows.Forms.TextBox();
            this.textBoxModifyF = new System.Windows.Forms.TextBox();
            this.treeViewInfo = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonToMale = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonToFemale = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRestore = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSpeechPlay = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSpeechStop = new System.Windows.Forms.ToolStripButton();
            this.treeViewLine = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList( this.components );
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxID.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControlLine.SuspendLayout();
            this.tabPageMale.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabPageFemale.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.groupBoxID );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.treeViewLine );
            this.splitContainer1.Size = new System.Drawing.Size( 640, 480 );
            this.splitContainer1.SplitterDistance = 363;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBoxID
            // 
            this.groupBoxID.AutoSize = true;
            this.groupBoxID.Controls.Add( this.splitContainer2 );
            this.groupBoxID.Controls.Add( this.toolStrip1 );
            this.groupBoxID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxID.Location = new System.Drawing.Point( 0, 0 );
            this.groupBoxID.Name = "groupBoxID";
            this.groupBoxID.Padding = new System.Windows.Forms.Padding( 3, 3, 3, 0 );
            this.groupBoxID.Size = new System.Drawing.Size( 640, 363 );
            this.groupBoxID.TabIndex = 0;
            this.groupBoxID.TabStop = false;
            this.groupBoxID.Text = "ID";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point( 3, 15 );
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add( this.tabControlLine );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.treeViewInfo );
            this.splitContainer2.Size = new System.Drawing.Size( 634, 323 );
            this.splitContainer2.SplitterDistance = 195;
            this.splitContainer2.TabIndex = 3;
            // 
            // tabControlLine
            // 
            this.tabControlLine.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControlLine.Controls.Add( this.tabPageMale );
            this.tabControlLine.Controls.Add( this.tabPageFemale );
            this.tabControlLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlLine.Location = new System.Drawing.Point( 0, 0 );
            this.tabControlLine.Name = "tabControlLine";
            this.tabControlLine.SelectedIndex = 0;
            this.tabControlLine.Size = new System.Drawing.Size( 634, 195 );
            this.tabControlLine.TabIndex = 0;
            // 
            // tabPageMale
            // 
            this.tabPageMale.Controls.Add( this.splitContainer3 );
            this.tabPageMale.Location = new System.Drawing.Point( 4, 4 );
            this.tabPageMale.Name = "tabPageMale";
            this.tabPageMale.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabPageMale.Size = new System.Drawing.Size( 626, 170 );
            this.tabPageMale.TabIndex = 0;
            this.tabPageMale.Text = "Male Line";
            this.tabPageMale.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point( 3, 3 );
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add( this.textBoxOriginalM );
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add( this.textBoxModifyM );
            this.splitContainer3.Size = new System.Drawing.Size( 620, 164 );
            this.splitContainer3.SplitterDistance = 80;
            this.splitContainer3.TabIndex = 1;
            // 
            // textBoxOriginalM
            // 
            this.textBoxOriginalM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOriginalM.HideSelection = false;
            this.textBoxOriginalM.Location = new System.Drawing.Point( 0, 0 );
            this.textBoxOriginalM.Multiline = true;
            this.textBoxOriginalM.Name = "textBoxOriginalM";
            this.textBoxOriginalM.ReadOnly = true;
            this.textBoxOriginalM.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOriginalM.Size = new System.Drawing.Size( 620, 80 );
            this.textBoxOriginalM.TabIndex = 0;
            // 
            // textBoxModifyM
            // 
            this.textBoxModifyM.AcceptsReturn = true;
            this.textBoxModifyM.AcceptsTab = true;
            this.textBoxModifyM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxModifyM.HideSelection = false;
            this.textBoxModifyM.Location = new System.Drawing.Point( 0, 0 );
            this.textBoxModifyM.Multiline = true;
            this.textBoxModifyM.Name = "textBoxModifyM";
            this.textBoxModifyM.ReadOnly = true;
            this.textBoxModifyM.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxModifyM.Size = new System.Drawing.Size( 620, 80 );
            this.textBoxModifyM.TabIndex = 1;
            // 
            // tabPageFemale
            // 
            this.tabPageFemale.Controls.Add( this.splitContainer4 );
            this.tabPageFemale.Location = new System.Drawing.Point( 4, 4 );
            this.tabPageFemale.Name = "tabPageFemale";
            this.tabPageFemale.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabPageFemale.Size = new System.Drawing.Size( 626, 170 );
            this.tabPageFemale.TabIndex = 1;
            this.tabPageFemale.Text = "Female Line";
            this.tabPageFemale.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point( 3, 3 );
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add( this.textBoxOriginalF );
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add( this.textBoxModifyF );
            this.splitContainer4.Size = new System.Drawing.Size( 620, 164 );
            this.splitContainer4.SplitterDistance = 80;
            this.splitContainer4.TabIndex = 1;
            // 
            // textBoxOriginalF
            // 
            this.textBoxOriginalF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOriginalF.HideSelection = false;
            this.textBoxOriginalF.Location = new System.Drawing.Point( 0, 0 );
            this.textBoxOriginalF.Multiline = true;
            this.textBoxOriginalF.Name = "textBoxOriginalF";
            this.textBoxOriginalF.ReadOnly = true;
            this.textBoxOriginalF.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOriginalF.Size = new System.Drawing.Size( 620, 80 );
            this.textBoxOriginalF.TabIndex = 0;
            // 
            // textBoxModifyF
            // 
            this.textBoxModifyF.AcceptsReturn = true;
            this.textBoxModifyF.AcceptsTab = true;
            this.textBoxModifyF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxModifyF.HideSelection = false;
            this.textBoxModifyF.Location = new System.Drawing.Point( 0, 0 );
            this.textBoxModifyF.Multiline = true;
            this.textBoxModifyF.Name = "textBoxModifyF";
            this.textBoxModifyF.ReadOnly = true;
            this.textBoxModifyF.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxModifyF.Size = new System.Drawing.Size( 620, 80 );
            this.textBoxModifyF.TabIndex = 1;
            // 
            // treeViewInfo
            // 
            this.treeViewInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewInfo.Location = new System.Drawing.Point( 0, 0 );
            this.treeViewInfo.Name = "treeViewInfo";
            this.treeViewInfo.Size = new System.Drawing.Size( 634, 124 );
            this.treeViewInfo.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNext,
            this.toolStripSeparator1,
            this.toolStripButtonToMale,
            this.toolStripButtonToFemale,
            this.toolStripButtonRestore,
            this.toolStripSeparator2,
            this.toolStripButtonSpeechPlay,
            this.toolStripButtonSpeechStop} );
            this.toolStrip1.Location = new System.Drawing.Point( 3, 338 );
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size( 634, 25 );
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonNext
            // 
            this.toolStripButtonNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNext.Image = global::ArcanumJPEditor.Properties.Resources.resultset_next;
            this.toolStripButtonNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNext.Name = "toolStripButtonNext";
            this.toolStripButtonNext.Size = new System.Drawing.Size( 23, 22 );
            this.toolStripButtonNext.Text = "次のラインへ";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size( 6, 25 );
            // 
            // toolStripButtonToMale
            // 
            this.toolStripButtonToMale.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonToMale.Image = global::ArcanumJPEditor.Properties.Resources.arrow_left;
            this.toolStripButtonToMale.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonToMale.Name = "toolStripButtonToMale";
            this.toolStripButtonToMale.Size = new System.Drawing.Size( 23, 22 );
            this.toolStripButtonToMale.Text = "Female LineをMale Lineにコピー";
            // 
            // toolStripButtonToFemale
            // 
            this.toolStripButtonToFemale.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonToFemale.Image = global::ArcanumJPEditor.Properties.Resources.arrow_right;
            this.toolStripButtonToFemale.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonToFemale.Name = "toolStripButtonToFemale";
            this.toolStripButtonToFemale.Size = new System.Drawing.Size( 23, 22 );
            this.toolStripButtonToFemale.Text = "Male LineをFemale Lineにコピー";
            // 
            // toolStripButtonRestore
            // 
            this.toolStripButtonRestore.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonRestore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRestore.Image = global::ArcanumJPEditor.Properties.Resources.arrow_rotate_anticlockwise;
            this.toolStripButtonRestore.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRestore.Name = "toolStripButtonRestore";
            this.toolStripButtonRestore.Size = new System.Drawing.Size( 23, 22 );
            this.toolStripButtonRestore.Text = "編集前に戻す";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size( 6, 25 );
            // 
            // toolStripButtonSpeechPlay
            // 
            this.toolStripButtonSpeechPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSpeechPlay.Image = global::ArcanumJPEditor.Properties.Resources.control_play_blue;
            this.toolStripButtonSpeechPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSpeechPlay.Name = "toolStripButtonSpeechPlay";
            this.toolStripButtonSpeechPlay.Size = new System.Drawing.Size( 23, 22 );
            this.toolStripButtonSpeechPlay.Text = "再生";
            // 
            // toolStripButtonSpeechStop
            // 
            this.toolStripButtonSpeechStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSpeechStop.Image = global::ArcanumJPEditor.Properties.Resources.control_stop_blue;
            this.toolStripButtonSpeechStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSpeechStop.Name = "toolStripButtonSpeechStop";
            this.toolStripButtonSpeechStop.Size = new System.Drawing.Size( 23, 22 );
            this.toolStripButtonSpeechStop.Text = "停止";
            // 
            // treeViewLine
            // 
            this.treeViewLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewLine.HideSelection = false;
            this.treeViewLine.Location = new System.Drawing.Point( 0, 0 );
            this.treeViewLine.Name = "treeViewLine";
            this.treeViewLine.Size = new System.Drawing.Size( 640, 113 );
            this.treeViewLine.TabIndex = 1;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ( ( System.Windows.Forms.ImageListStreamer )( resources.GetObject( "imageList1.ImageStream" ) ) );
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName( 0, "bullet_red.png" );
            this.imageList1.Images.SetKeyName( 1, "bullet_orange.png" );
            this.imageList1.Images.SetKeyName( 2, "bullet_green.png" );
            this.imageList1.Images.SetKeyName( 3, "script.png" );
            // 
            // DlgEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.splitContainer1 );
            this.Name = "DlgEditor";
            this.Size = new System.Drawing.Size( 640, 480 );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.groupBoxID.ResumeLayout( false );
            this.groupBoxID.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.ResumeLayout( false );
            this.tabControlLine.ResumeLayout( false );
            this.tabPageMale.ResumeLayout( false );
            this.splitContainer3.Panel1.ResumeLayout( false );
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout( false );
            this.splitContainer3.Panel2.PerformLayout();
            this.splitContainer3.ResumeLayout( false );
            this.tabPageFemale.ResumeLayout( false );
            this.splitContainer4.Panel1.ResumeLayout( false );
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout( false );
            this.splitContainer4.Panel2.PerformLayout();
            this.splitContainer4.ResumeLayout( false );
            this.toolStrip1.ResumeLayout( false );
            this.toolStrip1.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewLine;
        private System.Windows.Forms.GroupBox groupBoxID;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControlLine;
        private System.Windows.Forms.TabPage tabPageMale;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox textBoxOriginalM;
        private System.Windows.Forms.TextBox textBoxModifyM;
        private System.Windows.Forms.TabPage tabPageFemale;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.TextBox textBoxOriginalF;
        private System.Windows.Forms.TextBox textBoxModifyF;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonNext;
        private System.Windows.Forms.ToolStripButton toolStripButtonRestore;
        private System.Windows.Forms.ToolStripButton toolStripButtonToMale;
        private System.Windows.Forms.ToolStripButton toolStripButtonToFemale;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TreeView treeViewInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonSpeechPlay;
        private System.Windows.Forms.ToolStripButton toolStripButtonSpeechStop;
    }
}
