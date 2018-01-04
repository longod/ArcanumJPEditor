namespace ArcanumJPEditor {
    partial class Setting {
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

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.groupBoxArcanum = new System.Windows.Forms.GroupBox();
            this.buttonArcanumSelector = new System.Windows.Forms.Button();
            this.textBoxArcanum = new System.Windows.Forms.TextBox();
            this.groupBoxEditor = new System.Windows.Forms.GroupBox();
            this.buttonEditorSelector = new System.Windows.Forms.Button();
            this.textBoxEditor = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxSpeechVolume = new System.Windows.Forms.GroupBox();
            this.trackBarSpeechVolume = new System.Windows.Forms.TrackBar();
            this.groupBoxArcanum.SuspendLayout();
            this.groupBoxEditor.SuspendLayout();
            this.groupBoxSpeechVolume.SuspendLayout();
            ( ( System.ComponentModel.ISupportInitialize )( this.trackBarSpeechVolume ) ).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxArcanum
            // 
            this.groupBoxArcanum.Controls.Add( this.buttonArcanumSelector );
            this.groupBoxArcanum.Controls.Add( this.textBoxArcanum );
            this.groupBoxArcanum.Location = new System.Drawing.Point( 12, 12 );
            this.groupBoxArcanum.Name = "groupBoxArcanum";
            this.groupBoxArcanum.Size = new System.Drawing.Size( 242, 47 );
            this.groupBoxArcanum.TabIndex = 0;
            this.groupBoxArcanum.TabStop = false;
            this.groupBoxArcanum.Text = "Arcanum インストールフォルダ";
            // 
            // buttonArcanumSelector
            // 
            this.buttonArcanumSelector.Location = new System.Drawing.Point( 212, 14 );
            this.buttonArcanumSelector.Name = "buttonArcanumSelector";
            this.buttonArcanumSelector.Size = new System.Drawing.Size( 23, 23 );
            this.buttonArcanumSelector.TabIndex = 1;
            this.buttonArcanumSelector.Text = "...";
            this.buttonArcanumSelector.UseVisualStyleBackColor = true;
            // 
            // textBoxArcanum
            // 
            this.textBoxArcanum.Location = new System.Drawing.Point( 6, 18 );
            this.textBoxArcanum.Name = "textBoxArcanum";
            this.textBoxArcanum.Size = new System.Drawing.Size( 200, 19 );
            this.textBoxArcanum.TabIndex = 0;
            // 
            // groupBoxEditor
            // 
            this.groupBoxEditor.Controls.Add( this.buttonEditorSelector );
            this.groupBoxEditor.Controls.Add( this.textBoxEditor );
            this.groupBoxEditor.Location = new System.Drawing.Point( 12, 65 );
            this.groupBoxEditor.Name = "groupBoxEditor";
            this.groupBoxEditor.Size = new System.Drawing.Size( 242, 47 );
            this.groupBoxEditor.TabIndex = 1;
            this.groupBoxEditor.TabStop = false;
            this.groupBoxEditor.Text = "テキストエディタ パス";
            // 
            // buttonEditorSelector
            // 
            this.buttonEditorSelector.Location = new System.Drawing.Point( 212, 14 );
            this.buttonEditorSelector.Name = "buttonEditorSelector";
            this.buttonEditorSelector.Size = new System.Drawing.Size( 23, 23 );
            this.buttonEditorSelector.TabIndex = 1;
            this.buttonEditorSelector.Text = "...";
            this.buttonEditorSelector.UseVisualStyleBackColor = true;
            // 
            // textBoxEditor
            // 
            this.textBoxEditor.Location = new System.Drawing.Point( 6, 18 );
            this.textBoxEditor.Name = "textBoxEditor";
            this.textBoxEditor.Size = new System.Drawing.Size( 200, 19 );
            this.textBoxEditor.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point( 260, 12 );
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size( 75, 23 );
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point( 260, 41 );
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size( 75, 23 );
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // groupBoxSpeechVolume
            // 
            this.groupBoxSpeechVolume.Controls.Add( this.trackBarSpeechVolume );
            this.groupBoxSpeechVolume.Location = new System.Drawing.Point( 12, 118 );
            this.groupBoxSpeechVolume.Name = "groupBoxSpeechVolume";
            this.groupBoxSpeechVolume.Size = new System.Drawing.Size( 242, 62 );
            this.groupBoxSpeechVolume.TabIndex = 2;
            this.groupBoxSpeechVolume.TabStop = false;
            this.groupBoxSpeechVolume.Text = "ボリューム: ";
            // 
            // trackBarSpeechVolume
            // 
            this.trackBarSpeechVolume.Location = new System.Drawing.Point( 6, 18 );
            this.trackBarSpeechVolume.Maximum = 100;
            this.trackBarSpeechVolume.Name = "trackBarSpeechVolume";
            this.trackBarSpeechVolume.Size = new System.Drawing.Size( 229, 42 );
            this.trackBarSpeechVolume.TabIndex = 0;
            this.trackBarSpeechVolume.TickFrequency = 5;
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 347, 193 );
            this.Controls.Add( this.groupBoxSpeechVolume );
            this.Controls.Add( this.buttonCancel );
            this.Controls.Add( this.buttonOK );
            this.Controls.Add( this.groupBoxEditor );
            this.Controls.Add( this.groupBoxArcanum );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Setting";
            this.ShowInTaskbar = false;
            this.Text = "Setting";
            this.groupBoxArcanum.ResumeLayout( false );
            this.groupBoxArcanum.PerformLayout();
            this.groupBoxEditor.ResumeLayout( false );
            this.groupBoxEditor.PerformLayout();
            this.groupBoxSpeechVolume.ResumeLayout( false );
            this.groupBoxSpeechVolume.PerformLayout();
            ( ( System.ComponentModel.ISupportInitialize )( this.trackBarSpeechVolume ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxArcanum;
        private System.Windows.Forms.Button buttonArcanumSelector;
        private System.Windows.Forms.TextBox textBoxArcanum;
        private System.Windows.Forms.GroupBox groupBoxEditor;
        private System.Windows.Forms.Button buttonEditorSelector;
        private System.Windows.Forms.TextBox textBoxEditor;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxSpeechVolume;
        private System.Windows.Forms.TrackBar trackBarSpeechVolume;

    }
}