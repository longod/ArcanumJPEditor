// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArcanumJPEditor {
    public partial class TranslateBrowser : Form {
        static TranslateBrowser instance = null;

        readonly object sync = new object();

        static public TranslateBrowser Create( WebTranslate.TranslateBase trans, System.Windows.Forms.IWin32Window parent ) {
            if ( instance != null ) {
                instance.translate( trans );
                return instance;
            }
            TranslateBrowser browser = new TranslateBrowser( trans );
            //browser.Show( parent );
            browser.Show(); // 親よりも後ろに配置できる
            return browser;
        }

        TranslateBrowser( WebTranslate.TranslateBase trans ) {
            InitializeComponent();
            this.Shown += new EventHandler( TranslateBrowser_Shown );
            this.FormClosed += new FormClosedEventHandler( TranslateBrowser_FormClosed );
            this.webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler( webBrowser_DocumentCompleted );
            this.webBrowser.Navigating += new WebBrowserNavigatingEventHandler( webBrowser_Navigating );

            this.translate( trans );
        }

        void webBrowser_Navigating( object sender, WebBrowserNavigatingEventArgs e ) {
            //e.Cancel = true;
        }


        TranslateBrowser() {
            InitializeComponent();
        }

        void TranslateBrowser_Shown( object sender, EventArgs e ) {
            // とりあえずこのタイミングでシングル化
            if ( instance != null ) {
                // この場合はassertでもいいが
                lock ( instance.sync ) {
                    instance = this;
                }
            } else {
                lock ( this.sync ) {
                    instance = this;
                }
            }
        }

        void TranslateBrowser_FormClosed( object sender, FormClosedEventArgs e ) {
            // とりあえずこのタイミングで破棄
            lock ( instance.sync ) {
                instance = null;
            }
        }

        void webBrowser_DocumentCompleted( object sender, WebBrowserDocumentCompletedEventArgs e ) {
            // 前面にもってくる
            this.Activate();
        }

        void translate( WebTranslate.TranslateBase trans ) {
            trans.AsyncDone += new WebTranslate.TranslateBase.CallbackFunc( trans_AsyncDone );
            trans.GetResponseASync();
        }

        static void trans_AsyncDone( WebTranslate.TranslateBase trans ) {
            // 通信完了までにウインドウがとじられていると、存在しない場所にアクセスしてエラーを起こす。
            // 何か手立てが欲しいが disposeかclose時にチェックして動いていたら、中断させるしかないか？
            // いやこの関数はstaticであるべきか、これは単一インスタンス化した場合に可能であるがdisposeでinstance=null?再利用？

            // 複数同時に来たらどうするか…critical section?
            // 別スレッドからのあくせすなのでやはりCSか
            if ( instance == null ) {
                return;
            }
            lock ( instance.sync ) {
                switch ( trans.Result ) {
                    case WebTranslate.TranslateBase.ConnectionResult.FailureTimeOut:
                        instance.webBrowser.DocumentText = "タイムアウトしました";
                        break;
                    case WebTranslate.TranslateBase.ConnectionResult.FailureResponce:
                        instance.webBrowser.DocumentText = "通信に失敗しました";
                        break;
                    case WebTranslate.TranslateBase.ConnectionResult.FailureUnknown:
                        instance.webBrowser.DocumentText = "不明なエラーが発生しました";
                        break;
                    case WebTranslate.TranslateBase.ConnectionResult.Success:
                        //instance.webBrowser.Url = new System.Uri( trans.URL );
                        instance.webBrowser.DocumentText = trans.OutputText;
                        break;
                    default:
                        break;
                }
            }
            //instance.Activate();
            //Console.WriteLine( "done!" );
        }
    }
}