// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ArcanumJPEditor {
    public class Editor : System.Windows.Forms.UserControl {
        protected File Original = new File();
        protected File Mod = new File();
        protected string file_name = "";
        protected HistoryManager history = new HistoryManager();

        public string OriginalPath {
            get { return Original.FilePath; }
        }
        public string ModPath {
            get { return Mod.FilePath; }
        }
        public string FileName {
            get { return file_name; }
        }

        protected void open( string path ) {
            //string origin_path = Path.convineFullPath( path );
            string origin_path = path;
            bool ret = Original.open( origin_path, true );
            if ( ret ) {
                this.Tag = Original.FilePath;
                MainForm.setStatus( @"オープン: " + Original.FilePath );

                string mod_path = Path.Data.ModifiedDirectory + path;
                System.IO.FileInfo info = new System.IO.FileInfo( mod_path );
                if ( info.Exists ) {
                    ret = Mod.open( mod_path, false );
                    if ( ret ) {
                        this.Tag = Mod.FilePath;
                        MainForm.setStatus( @"オープン: " + Mod.FilePath );
                    }
                } else {
                    //Mod = Original;
                    ret = Mod.open( origin_path, true );
                    Mod.FilePath = mod_path;
                    // ここで横着せずに二回ロード
                }

                file_name = info.Name; // modが無くてもfile名は取れる
            }

        }
        public void save() {
            System.IO.FileInfo info = new System.IO.FileInfo( Mod.FilePath );
            if ( info.Exists == false ) {
                System.IO.Directory.CreateDirectory( info.DirectoryName );
                string path = Original.FilePath;
            }
            Mod.save( Mod.FilePath );
            // modがまだなくoriginal=modの場合読み直さないとoriginalの表示がおかしくなる
            // しかし単純に読み直したのでは駄目だなあ。

            // リロードというかoriとmodの値を同一にする
            MainForm.setStatus( @"セーブ: " + Mod.FilePath );

        }
        public bool checkModify() {
            return Mod.checkModify();
        }

        protected void Highlights( RichTextBox box ) {
            if ( box.Text.Length > 0 ) {
                int start = box.SelectionStart;
                int len = box.SelectionLength;
                System.Text.RegularExpressions.Regex regx = new System.Text.RegularExpressions.Regex( @"\t|{|}|@|　" );
                System.Text.RegularExpressions.MatchCollection matches = regx.Matches( box.Text );
                if ( matches.Count > 0 ) {
                    box.SelectAll();
                    box.SelectionBackColor = System.Drawing.SystemColors.Window;

                    foreach ( System.Text.RegularExpressions.Match match in matches ) {
                        box.Select( match.Index, match.Length );
                        box.SelectionBackColor = Color.Aquamarine;
                    }

                    box.SelectionStart = start;
                    box.SelectionLength = len;
                }

            }
        }

        static protected string round( string text, int max ) {
            string ret = text;
            if ( text.Length > max ) {
                string sub = text.Substring( 0, max );
                ret = sub + "...";
            }
            return ret;
        }

        // 純粋にしたいならばバラバラにしてinterfaceクラス作るしか
        public virtual void SelectAll_Click( object sender, EventArgs e ) { }
        public virtual void Copy_Click( object sender, EventArgs e ) { }
        public virtual void Paste_Click( object sender, EventArgs e ) { }
        public virtual void Cut_Click( object sender, EventArgs e ) { }
        public virtual void Undo_Click( object sender, EventArgs e ) { }
        public virtual void Redo_Click( object sender, EventArgs e ) { }
        public virtual void Eijirou_Click( object sender, EventArgs e ) { }
        public virtual void EijirouWeb_Click( object sender, EventArgs e ) { }
        
        public virtual void Next_Click( object sender, EventArgs e ) { }
        public virtual void ToMale_Click( object sender, EventArgs e ) { }
        public virtual void ToFemale_Click( object sender, EventArgs e ) { }
        public virtual void SpeechPlay_Click( object sender, EventArgs e ) { }
        public virtual void SpeechStop_Click( object sender, EventArgs e ) { }
       
        public void ContextMenu_Popup( object sender, EventArgs e ) {
            ContextMenu menu = sender as ContextMenu;
            if ( menu != null ) {
                TextBox box = menu.SourceControl as TextBox;
                if ( box != null ) {
                    string text = box.SelectedText;
                    if ( text != null && text.Length > 0 ) {
                        // この辺スマートにしたいな
                        // かつホワイトスペースでない
                        menu.MenuItems[ menu.MenuItems.Count - 3 ].Visible = true;
                        menu.MenuItems[ menu.MenuItems.Count - 2 ].Visible = true;
                        menu.MenuItems[ menu.MenuItems.Count - 1 ].Visible = true;
                    } else {
                        menu.MenuItems[ menu.MenuItems.Count - 3 ].Visible = false;
                        menu.MenuItems[ menu.MenuItems.Count - 2 ].Visible = false;
                        menu.MenuItems[ menu.MenuItems.Count - 1 ].Visible = false;
                    }
                }
            }
        }

        public virtual bool search( Search.Option option, TreeNodeUtill.GetNode getfunc ) {
            return false;
        }

        public virtual void treeReset() {}

        public ContextMenu createMenuOriginal() {
            MenuItem[] items = new MenuItem[ 6 ];
            items[ 0 ] = new MenuItem( "コピー(&C)", Copy_Click );
            items[ 1 ] = new MenuItem( "-" );
            items[ 2 ] = new MenuItem( "全て選択(&A)", SelectAll_Click );
            items[ 3 ] = new MenuItem( "-" );
            items[ 4 ] = new MenuItem( "英次郎で翻訳(仮)(&E)", Eijirou_Click );
            items[ 5 ] = new MenuItem( "英次郎で翻訳(仮) - Web(&W)", EijirouWeb_Click );
            ContextMenu menu = new ContextMenu( items );
            menu.Popup += new EventHandler( ContextMenu_Popup );
            return menu;
        }


        public ContextMenu createMenuModify() {
            MenuItem[] items = new MenuItem[ 11 ];
            items[ 0 ] = new MenuItem( "元に戻す(&U)", Undo_Click );
            items[ 1 ] = new MenuItem( "やり直し(&Y)", Redo_Click );
            items[ 2 ] = new MenuItem( "-" );
            items[ 3 ] = new MenuItem( "切り取り(&T)", Cut_Click );
            items[ 4 ] = new MenuItem( "コピー(&C)", Copy_Click );
            items[ 5 ] = new MenuItem( "貼り付け(&P)", Paste_Click );
            items[ 6 ] = new MenuItem( "-" );
            items[ 7 ] = new MenuItem( "全て選択(&A)", SelectAll_Click );
            items[ 8 ] = new MenuItem( "-" );
            items[ 9 ] = new MenuItem( "英次郎で翻訳(仮)(&E)", Eijirou_Click );
            items[10 ] = new MenuItem( "英次郎で翻訳(仮) - Web(&W)", EijirouWeb_Click );
            ContextMenu menu = new ContextMenu( items );
            menu.Popup += new EventHandler( ContextMenu_Popup );
            return menu;
        }
    }
}
