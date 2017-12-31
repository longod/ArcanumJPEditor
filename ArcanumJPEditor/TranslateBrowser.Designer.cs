namespace ArcanumJPEditor {
    partial class TranslateBrowser {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( TranslateBrowser ) );
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point( 0, 0 );
            this.webBrowser.MinimumSize = new System.Drawing.Size( 20, 20 );
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size( 632, 613 );
            this.webBrowser.TabIndex = 0;
            // 
            // TranslateBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 632, 613 );
            this.Controls.Add( this.webBrowser );
            this.Icon = ( ( System.Drawing.Icon )( resources.GetObject( "$this.Icon" ) ) );
            this.Name = "TranslateBrowser";
            this.Text = "TranslateBrowser";
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
    }
}