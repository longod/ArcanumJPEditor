namespace ArcanumJPEditor {
    partial class MesEditor {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( MesEditor ) );
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxID = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.textBoxOriginal = new System.Windows.Forms.TextBox();
            this.textBoxModify = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRestore = new System.Windows.Forms.ToolStripButton();
            this.treeViewLine = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList( this.components );
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxID.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
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
            this.splitContainer1.SplitterDistance = 204;
            this.splitContainer1.TabIndex = 1;
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
            this.groupBoxID.Size = new System.Drawing.Size( 640, 204 );
            this.groupBoxID.TabIndex = 0;
            this.groupBoxID.TabStop = false;
            this.groupBoxID.Text = "ID";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point( 3, 15 );
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add( this.textBoxOriginal );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.textBoxModify );
            this.splitContainer2.Size = new System.Drawing.Size( 634, 164 );
            this.splitContainer2.SplitterDistance = 80;
            this.splitContainer2.TabIndex = 3;
            // 
            // textBoxOriginal
            // 
            this.textBoxOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOriginal.HideSelection = false;
            this.textBoxOriginal.Location = new System.Drawing.Point( 0, 0 );
            this.textBoxOriginal.Multiline = true;
            this.textBoxOriginal.Name = "textBoxOriginal";
            this.textBoxOriginal.ReadOnly = true;
            this.textBoxOriginal.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOriginal.Size = new System.Drawing.Size( 634, 80 );
            this.textBoxOriginal.TabIndex = 0;
            // 
            // textBoxModify
            // 
            this.textBoxModify.AcceptsReturn = true;
            this.textBoxModify.AcceptsTab = true;
            this.textBoxModify.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxModify.HideSelection = false;
            this.textBoxModify.Location = new System.Drawing.Point( 0, 0 );
            this.textBoxModify.Multiline = true;
            this.textBoxModify.Name = "textBoxModify";
            this.textBoxModify.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxModify.Size = new System.Drawing.Size( 634, 80 );
            this.textBoxModify.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNext,
            this.toolStripSeparator1,
            this.toolStripButtonRestore} );
            this.toolStrip1.Location = new System.Drawing.Point( 3, 179 );
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size( 634, 25 );
            this.toolStrip1.TabIndex = 1;
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
            // treeViewLine
            // 
            this.treeViewLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewLine.HideSelection = false;
            this.treeViewLine.Location = new System.Drawing.Point( 0, 0 );
            this.treeViewLine.Name = "treeViewLine";
            this.treeViewLine.Size = new System.Drawing.Size( 640, 272 );
            this.treeViewLine.TabIndex = 1;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ( ( System.Windows.Forms.ImageListStreamer )( resources.GetObject( "imageList1.ImageStream" ) ) );
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName( 0, "bullet_red.png" );
            this.imageList1.Images.SetKeyName( 1, "bullet_green.png" );
            this.imageList1.Images.SetKeyName( 2, "script.png" );
            // 
            // MesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.splitContainer1 );
            this.Name = "MesEditor";
            this.Size = new System.Drawing.Size( 640, 480 );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.groupBoxID.ResumeLayout( false );
            this.groupBoxID.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout( false );
            this.toolStrip1.ResumeLayout( false );
            this.toolStrip1.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewLine;
        private System.Windows.Forms.GroupBox groupBoxID;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox textBoxOriginal;
        private System.Windows.Forms.TextBox textBoxModify;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonNext;
        private System.Windows.Forms.ToolStripButton toolStripButtonRestore;
        private System.Windows.Forms.ImageList imageList1;


    }
}
