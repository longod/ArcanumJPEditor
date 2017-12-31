namespace ArcanumJPEditor {
    partial class Search {
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxResID = new System.Windows.Forms.CheckBox();
            this.checkBoxResult = new System.Windows.Forms.CheckBox();
            this.checkBoxTest = new System.Windows.Forms.CheckBox();
            this.checkBoxInt = new System.Windows.Forms.CheckBox();
            this.checkBoxLine = new System.Windows.Forms.CheckBox();
            this.checkBoxID = new System.Windows.Forms.CheckBox();
            this.comboBoxRange = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxComplete = new System.Windows.Forms.CheckBox();
            this.checkBoxChar = new System.Windows.Forms.CheckBox();
            this.checkBoxRepeat = new System.Windows.Forms.CheckBox();
            this.checkBoxRegex = new System.Windows.Forms.CheckBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.buttonSearchPrev = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.checkBoxOrigin = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 12, 15 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 67, 12 );
            this.label1.TabIndex = 0;
            this.label1.Text = "検索文字列:";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point( 85, 12 );
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size( 200, 19 );
            this.textBoxSearch.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.checkBoxResID );
            this.groupBox1.Controls.Add( this.checkBoxResult );
            this.groupBox1.Controls.Add( this.checkBoxTest );
            this.groupBox1.Controls.Add( this.checkBoxInt );
            this.groupBox1.Controls.Add( this.checkBoxLine );
            this.groupBox1.Controls.Add( this.checkBoxID );
            this.groupBox1.Location = new System.Drawing.Point( 12, 63 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 100, 148 );
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "検索対象";
            // 
            // checkBoxResID
            // 
            this.checkBoxResID.AutoSize = true;
            this.checkBoxResID.Location = new System.Drawing.Point( 6, 128 );
            this.checkBoxResID.Name = "checkBoxResID";
            this.checkBoxResID.Size = new System.Drawing.Size( 89, 16 );
            this.checkBoxResID.TabIndex = 9;
            this.checkBoxResID.Text = "Response ID";
            this.checkBoxResID.UseVisualStyleBackColor = true;
            // 
            // checkBoxResult
            // 
            this.checkBoxResult.AutoSize = true;
            this.checkBoxResult.Location = new System.Drawing.Point( 6, 106 );
            this.checkBoxResult.Name = "checkBoxResult";
            this.checkBoxResult.Size = new System.Drawing.Size( 63, 16 );
            this.checkBoxResult.TabIndex = 8;
            this.checkBoxResult.Text = "Results";
            this.checkBoxResult.UseVisualStyleBackColor = true;
            // 
            // checkBoxTest
            // 
            this.checkBoxTest.AutoSize = true;
            this.checkBoxTest.Location = new System.Drawing.Point( 6, 84 );
            this.checkBoxTest.Name = "checkBoxTest";
            this.checkBoxTest.Size = new System.Drawing.Size( 83, 16 );
            this.checkBoxTest.TabIndex = 7;
            this.checkBoxTest.Text = "Test Codes";
            this.checkBoxTest.UseVisualStyleBackColor = true;
            // 
            // checkBoxInt
            // 
            this.checkBoxInt.AutoSize = true;
            this.checkBoxInt.Location = new System.Drawing.Point( 6, 62 );
            this.checkBoxInt.Name = "checkBoxInt";
            this.checkBoxInt.Size = new System.Drawing.Size( 73, 16 );
            this.checkBoxInt.TabIndex = 6;
            this.checkBoxInt.Text = "Int Check";
            this.checkBoxInt.UseVisualStyleBackColor = true;
            // 
            // checkBoxLine
            // 
            this.checkBoxLine.AutoSize = true;
            this.checkBoxLine.Location = new System.Drawing.Point( 6, 40 );
            this.checkBoxLine.Name = "checkBoxLine";
            this.checkBoxLine.Size = new System.Drawing.Size( 72, 16 );
            this.checkBoxLine.TabIndex = 5;
            this.checkBoxLine.Text = "Text Line";
            this.checkBoxLine.UseVisualStyleBackColor = true;
            // 
            // checkBoxID
            // 
            this.checkBoxID.AutoSize = true;
            this.checkBoxID.Location = new System.Drawing.Point( 6, 18 );
            this.checkBoxID.Name = "checkBoxID";
            this.checkBoxID.Size = new System.Drawing.Size( 35, 16 );
            this.checkBoxID.TabIndex = 4;
            this.checkBoxID.Text = "ID";
            this.checkBoxID.UseVisualStyleBackColor = true;
            // 
            // comboBoxRange
            // 
            this.comboBoxRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRange.FormattingEnabled = true;
            this.comboBoxRange.Location = new System.Drawing.Point( 85, 37 );
            this.comboBoxRange.Name = "comboBoxRange";
            this.comboBoxRange.Size = new System.Drawing.Size( 200, 20 );
            this.comboBoxRange.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 24, 40 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 55, 12 );
            this.label2.TabIndex = 0;
            this.label2.Text = "検索範囲:";
            // 
            // checkBoxComplete
            // 
            this.checkBoxComplete.AutoSize = true;
            this.checkBoxComplete.Location = new System.Drawing.Point( 118, 147 );
            this.checkBoxComplete.Name = "checkBoxComplete";
            this.checkBoxComplete.Size = new System.Drawing.Size( 72, 16 );
            this.checkBoxComplete.TabIndex = 13;
            this.checkBoxComplete.Text = "完全一致";
            this.checkBoxComplete.UseVisualStyleBackColor = true;
            // 
            // checkBoxChar
            // 
            this.checkBoxChar.AutoSize = true;
            this.checkBoxChar.Location = new System.Drawing.Point( 118, 103 );
            this.checkBoxChar.Name = "checkBoxChar";
            this.checkBoxChar.Size = new System.Drawing.Size( 166, 16 );
            this.checkBoxChar.TabIndex = 11;
            this.checkBoxChar.Text = "大文字と小文字の区別をする";
            this.checkBoxChar.UseVisualStyleBackColor = true;
            // 
            // checkBoxRepeat
            // 
            this.checkBoxRepeat.AutoSize = true;
            this.checkBoxRepeat.Location = new System.Drawing.Point( 118, 81 );
            this.checkBoxRepeat.Name = "checkBoxRepeat";
            this.checkBoxRepeat.Size = new System.Drawing.Size( 161, 16 );
            this.checkBoxRepeat.TabIndex = 10;
            this.checkBoxRepeat.Text = "終端に到達したら先頭に戻る";
            this.checkBoxRepeat.UseVisualStyleBackColor = true;
            // 
            // checkBoxRegex
            // 
            this.checkBoxRegex.AutoSize = true;
            this.checkBoxRegex.Location = new System.Drawing.Point( 118, 169 );
            this.checkBoxRegex.Name = "checkBoxRegex";
            this.checkBoxRegex.Size = new System.Drawing.Size( 100, 16 );
            this.checkBoxRegex.TabIndex = 14;
            this.checkBoxRegex.Text = "正規表現を使う";
            this.checkBoxRegex.UseVisualStyleBackColor = true;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point( 291, 10 );
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size( 75, 23 );
            this.buttonSearch.TabIndex = 15;
            this.buttonSearch.Text = "次を検索(&S)";
            this.buttonSearch.UseVisualStyleBackColor = true;
            // 
            // buttonSearchPrev
            // 
            this.buttonSearchPrev.Location = new System.Drawing.Point( 291, 39 );
            this.buttonSearchPrev.Name = "buttonSearchPrev";
            this.buttonSearchPrev.Size = new System.Drawing.Size( 75, 23 );
            this.buttonSearchPrev.TabIndex = 16;
            this.buttonSearchPrev.Text = "前を検索(&P)";
            this.buttonSearchPrev.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point( 291, 188 );
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size( 75, 23 );
            this.buttonClose.TabIndex = 17;
            this.buttonClose.Text = "閉じる(&W)";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // checkBoxOrigin
            // 
            this.checkBoxOrigin.AutoSize = true;
            this.checkBoxOrigin.Location = new System.Drawing.Point( 118, 125 );
            this.checkBoxOrigin.Name = "checkBoxOrigin";
            this.checkBoxOrigin.Size = new System.Drawing.Size( 130, 16 );
            this.checkBoxOrigin.TabIndex = 12;
            this.checkBoxOrigin.Text = "原文のみを対象にする";
            this.checkBoxOrigin.UseVisualStyleBackColor = true;
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 380, 224 );
            this.Controls.Add( this.checkBoxOrigin );
            this.Controls.Add( this.buttonClose );
            this.Controls.Add( this.buttonSearchPrev );
            this.Controls.Add( this.buttonSearch );
            this.Controls.Add( this.checkBoxRegex );
            this.Controls.Add( this.checkBoxRepeat );
            this.Controls.Add( this.checkBoxChar );
            this.Controls.Add( this.checkBoxComplete );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.comboBoxRange );
            this.Controls.Add( this.groupBox1 );
            this.Controls.Add( this.textBoxSearch );
            this.Controls.Add( this.label1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Search";
            this.ShowInTaskbar = false;
            this.Text = "Search";
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxID;
        private System.Windows.Forms.CheckBox checkBoxResult;
        private System.Windows.Forms.CheckBox checkBoxTest;
        private System.Windows.Forms.CheckBox checkBoxInt;
        private System.Windows.Forms.CheckBox checkBoxLine;
        private System.Windows.Forms.ComboBox comboBoxRange;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxComplete;
        private System.Windows.Forms.CheckBox checkBoxChar;
        private System.Windows.Forms.CheckBox checkBoxRepeat;
        private System.Windows.Forms.CheckBox checkBoxRegex;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Button buttonSearchPrev;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.CheckBox checkBoxOrigin;
        private System.Windows.Forms.CheckBox checkBoxResID;
    }
}