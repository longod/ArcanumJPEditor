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
                MainForm.setStatus( @"�I�[�v��: " + Original.FilePath );

                string mod_path = Path.Data.ModifiedDirectory + path;
                System.IO.FileInfo info = new System.IO.FileInfo( mod_path );
                if ( info.Exists ) {
                    ret = Mod.open( mod_path, false );
                    if ( ret ) {
                        this.Tag = Mod.FilePath;
                        MainForm.setStatus( @"�I�[�v��: " + Mod.FilePath );
                    }
                } else {
                    //Mod = Original;
                    ret = Mod.open( origin_path, true );
                    Mod.FilePath = mod_path;
                    // �����ŉ��������ɓ�񃍁[�h
                }

                file_name = info.Name; // mod�������Ă�file���͎���
            }

        }
        public void save() {
            System.IO.FileInfo info = new System.IO.FileInfo( Mod.FilePath );
            if ( info.Exists == false ) {
                System.IO.Directory.CreateDirectory( info.DirectoryName );
                string path = Original.FilePath;
            }
            Mod.save( Mod.FilePath );
            // mod���܂��Ȃ�original=mod�̏ꍇ�ǂݒ����Ȃ���original�̕\�������������Ȃ�
            // �������P���ɓǂݒ������̂ł͑ʖڂ��Ȃ��B

            // �����[�h�Ƃ�����ori��mod�̒l�𓯈�ɂ���
            MainForm.setStatus( @"�Z�[�u: " + Mod.FilePath );

        }
        public bool checkModify() {
            return Mod.checkModify();
        }

        protected void Highlights( RichTextBox box ) {
            if ( box.Text.Length > 0 ) {
                int start = box.SelectionStart;
                int len = box.SelectionLength;
                System.Text.RegularExpressions.Regex regx = new System.Text.RegularExpressions.Regex( @"\t|{|}|@|�@" );
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

        // �����ɂ������Ȃ�΃o���o���ɂ���interface�N���X��邵��
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
                        // ���̕ӃX�}�[�g�ɂ�������
                        // ���z���C�g�X�y�[�X�łȂ�
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
            items[ 0 ] = new MenuItem( "�R�s�[(&C)", Copy_Click );
            items[ 1 ] = new MenuItem( "-" );
            items[ 2 ] = new MenuItem( "�S�đI��(&A)", SelectAll_Click );
            items[ 3 ] = new MenuItem( "-" );
            items[ 4 ] = new MenuItem( "�p���Y�Ŗ|��(��)(&E)", Eijirou_Click );
            items[ 5 ] = new MenuItem( "�p���Y�Ŗ|��(��) - Web(&W)", EijirouWeb_Click );
            ContextMenu menu = new ContextMenu( items );
            menu.Popup += new EventHandler( ContextMenu_Popup );
            return menu;
        }


        public ContextMenu createMenuModify() {
            MenuItem[] items = new MenuItem[ 11 ];
            items[ 0 ] = new MenuItem( "���ɖ߂�(&U)", Undo_Click );
            items[ 1 ] = new MenuItem( "��蒼��(&Y)", Redo_Click );
            items[ 2 ] = new MenuItem( "-" );
            items[ 3 ] = new MenuItem( "�؂���(&T)", Cut_Click );
            items[ 4 ] = new MenuItem( "�R�s�[(&C)", Copy_Click );
            items[ 5 ] = new MenuItem( "�\��t��(&P)", Paste_Click );
            items[ 6 ] = new MenuItem( "-" );
            items[ 7 ] = new MenuItem( "�S�đI��(&A)", SelectAll_Click );
            items[ 8 ] = new MenuItem( "-" );
            items[ 9 ] = new MenuItem( "�p���Y�Ŗ|��(��)(&E)", Eijirou_Click );
            items[10 ] = new MenuItem( "�p���Y�Ŗ|��(��) - Web(&W)", EijirouWeb_Click );
            ContextMenu menu = new ContextMenu( items );
            menu.Popup += new EventHandler( ContextMenu_Popup );
            return menu;
        }
    }
}
