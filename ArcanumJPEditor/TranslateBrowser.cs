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
            browser.Show(); // �e�������ɔz�u�ł���
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
            // �Ƃ肠�������̃^�C�~���O�ŃV���O����
            if ( instance != null ) {
                // ���̏ꍇ��assert�ł�������
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
            // �Ƃ肠�������̃^�C�~���O�Ŕj��
            lock ( instance.sync ) {
                instance = null;
            }
        }

        void webBrowser_DocumentCompleted( object sender, WebBrowserDocumentCompletedEventArgs e ) {
            // �O�ʂɂ����Ă���
            this.Activate();
        }

        void translate( WebTranslate.TranslateBase trans ) {
            trans.AsyncDone += new WebTranslate.TranslateBase.CallbackFunc( trans_AsyncDone );
            trans.GetResponseASync();
        }

        static void trans_AsyncDone( WebTranslate.TranslateBase trans ) {
            // �ʐM�����܂łɃE�C���h�E���Ƃ����Ă���ƁA���݂��Ȃ��ꏊ�ɃA�N�Z�X���ăG���[���N�����B
            // �����藧�Ă��~������ dispose��close���Ƀ`�F�b�N���ē����Ă�����A���f�����邵���Ȃ����H
            // ���₱�̊֐���static�ł���ׂ����A����͒P��C���X�^���X�������ꍇ�ɉ\�ł��邪dispose��instance=null?�ė��p�H

            // ���������ɗ�����ǂ����邩�ccritical section?
            // �ʃX���b�h����̂��������Ȃ̂ł�͂�CS��
            if ( instance == null ) {
                return;
            }
            lock ( instance.sync ) {
                switch ( trans.Result ) {
                    case WebTranslate.TranslateBase.ConnectionResult.FailureTimeOut:
                        instance.webBrowser.DocumentText = "�^�C���A�E�g���܂���";
                        break;
                    case WebTranslate.TranslateBase.ConnectionResult.FailureResponce:
                        instance.webBrowser.DocumentText = "�ʐM�Ɏ��s���܂���";
                        break;
                    case WebTranslate.TranslateBase.ConnectionResult.FailureUnknown:
                        instance.webBrowser.DocumentText = "�s���ȃG���[���������܂���";
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