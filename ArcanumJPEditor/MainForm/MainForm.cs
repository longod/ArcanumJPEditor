// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArcanumJPEditor {
    public partial class MainForm : Form {
        static MainForm Instance = null;
        string title = "ArcanumJPEditor";
        string version = "0.7.1";
        Config config = new Config();
        Search search;
        Search.Option search_option = new Search.Option();

        History history = new History();

#if _DEBUG
        ConsoleLog console = null;
#else
        //ConsoleLog console = new ConsoleLog();
        ConsoleLog console = null;
#endif

        public MainForm() {
            InitializeComponent();

            Instance = this;

            setStatus( "" );
            //this.Text = title + " " + version;
            setTitle( "" );

            Path.ExecutablePath = Application.ExecutablePath;
            Path.ExecutableFile = System.IO.Path.GetFileName( Path.ExecutablePath );
            Path.ExecutableDirectory = System.IO.Path.GetDirectoryName( Path.ExecutablePath ) + Path.Backslash;

            //System.IO.Directory.SetCurrentDirectory( Path.convineFullPath( "" ) );
            System.IO.Directory.SetCurrentDirectory( Path.ExecutableDirectory );


            string conf = Path.Xml.ConfigFile;
            if ( System.IO.File.Exists( conf ) ) {
                config = xml.Xml.Read<Config>( conf );
            }
            SpeechPlayer.Volume = config.SpeechVolume;
            string hist = Path.Xml.HistoryFile;
            if ( System.IO.File.Exists( hist ) ) {
                history = xml.Xml.Read<History>( hist );
            }


            treeViewFile.ImageList = imageList1;

            bool ret = createFileTree();

            // can not find dlg or mes
            if ( ret == false ) {
                if ( System.IO.File.Exists( Path.Tools.UnpackerFile ) ) {

                    if ( System.IO.File.Exists( Path.Tools.MakerFile ) ) {
                        DialogResult yn = MessageBox.Show( "�Ώۃt�@�C����������܂���ł����B\nArcanum�{�̂���A���p�b�N���܂����H\n�i���΂炭���Ԃ�v���܂��j", title, MessageBoxButtons.YesNo );
                        if ( yn == DialogResult.Yes ) {
                            if ( System.IO.Directory.Exists( config.ArcanumDirectory ) == false || System.IO.File.Exists( config.ArcanumDirectory + @"\Arcanum.exe" ) == false ) {
                                // �Â�
                                MessageBox.Show( "Arcanum �C���X�g�[���t�H���_���w�肵�ĉ������B", title );

                                //Setting setting = new Setting( config );

                                Setting_Click( null, null );
                            }
                            if ( System.IO.Directory.Exists( config.ArcanumDirectory ) && System.IO.File.Exists( config.ArcanumDirectory + @"\Arcanum.exe" ) ) {
                                // make
                                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                                proc.StartInfo.FileName = Path.Tools.UnpackerFile;
                                proc.StartInfo.WorkingDirectory = Path.ExecutableDirectory;
                                proc.StartInfo.Arguments = "\"" + Path.ExecutableDirectory + "\" \"" + config.ArcanumDirectory + "\"";
                                proc.Start();
                                proc.WaitForExit();
                                // ����I���������`�F�b�N��������
                                createFileTree();
                            } else {
                                // �s���ȃp�X
                                MessageBox.Show( "�A���p�b�N�Ɏ��s���܂����B\nArcanum�{�̂�dbmaker.exe���K�v�ł��B", title );
                            }
                        }
                    } else {
                        MessageBox.Show( "�Ώۃt�@�C����������܂���ł����B\nArcanum�{�̂�dbmaker.exe���K�v�ł��B", title );
                        // ���̌�ă`�F�b�N���Ă��������A�ʓ|�Ȃ̂Ŗ����ő��s
                    }
                } else {
                    // unpack.bat��������ł����c
                }
            }


            this.FormClosing += new FormClosingEventHandler( MainForm_FormClosing );

            toolStripMenuItemContract.Click += new EventHandler( Contract_Click );
            toolStripButtonContract.CheckedChanged += new EventHandler( Contract_CheckedChanged );
            toolStripMenuItemTabClose.Click += new EventHandler( TabClose_Click );
            toolStripButtonTabClose.Click += new EventHandler( TabClose_Click );
            toolStripMenuItemTabCloseAll.Click += new EventHandler( TabCloseAll_Click );

            toolStripMenuItemFile.DropDownOpening += new EventHandler( toolStripMenuItemFile_DropDownOpening );
            toolStripMenuItemSave.Click += new EventHandler( Save_Click );
            toolStripButtonSave.Click += new EventHandler( Save_Click );
            toolStripMenuItemSaveAll.Click += new EventHandler( SaveAll_Click );
            toolStripButtonSaveAll.Click += new EventHandler( SaveAll_Click );
            toolStripMenuItemExit.Click += new EventHandler( Exit_Click );

            toolStripMenuItemUndo.Click += new EventHandler( Undo_Click );
            toolStripButtonUndo.Click += new EventHandler( Undo_Click );
            toolStripMenuItemRedo.Click += new EventHandler( Redo_Click );
            toolStripButtonRedo.Click += new EventHandler( Redo_Click );
            toolStripMenuItemCut.Click += new EventHandler( Cut_Click );
            toolStripButtonCut.Click += new EventHandler( Cut_Click );
            toolStripMenuItemCopy.Click += new EventHandler( Copy_Click );
            toolStripButtonCopy.Click += new EventHandler( Copy_Click );
            toolStripMenuItemPaste.Click += new EventHandler( Paste_Click );
            toolStripButtonPaste.Click += new EventHandler( Paste_Click );
            toolStripMenuItemSelectAll.Click += new EventHandler( SelectAll_Click );

            toolStripMenuItemConvert.Click += new EventHandler( Convert_Click );
            toolStripButtonConvert.Click += new EventHandler( Convert_Click );
            toolStripMenuItemDat.Click += new EventHandler( Dat_Click );
            toolStripButtonDat.Click += new EventHandler( Dat_Click );
            toolStripMenuItemPlace.Click += new EventHandler( Place_Click );
            toolStripButtonPlace.Click += new EventHandler( Place_Click );
            toolStripMenuItemConvertAll.Click += new EventHandler( ConvertAll_Click );
            toolStripButtonConvertAll.Click += new EventHandler( ConvertAll_Click );

            toolStripMenuItemConvertAddingPrefix.Click += new EventHandler( ConvertAddingPrefix_Click );

            toolStripMenuItemFind.Click += new EventHandler( Find_Click );
            toolStripButtonFind.Click += new EventHandler( Find_Click );

            toolStripMenuItemEditor.Click += new EventHandler( Editor_Click );
            toolStripButtonEditor.Click += new EventHandler( Editor_Click );
            toolStripMenuItemEditorOriginal.Click += new EventHandler( EditorOriginal_Click );
            toolStripButtonEditorOriginal.Click += new EventHandler( EditorOriginal_Click );
            toolStripMenuItemSetting.Click += new EventHandler( Setting_Click );
            toolStripButtonSetting.Click += new EventHandler( Setting_Click );


            toolStripMenuItemAbout.Click += new EventHandler( About_Click );

            toolStripMenuItemNext.Click += new EventHandler( Next_Click );
            toolStripMenuItemToMale.Click += new EventHandler( ToMale_Click );
            toolStripMenuItemToFemale.Click += new EventHandler( ToFemale_Click );
            toolStripMenuItemSpeechPlay.Click += new EventHandler( SpeechPlay_Click );
            toolStripMenuItemSpeechStop.Click += new EventHandler( SpeechStop_Click );

            toolStripMenuItemProgress.Click += new EventHandler( Progress_Click );
            toolStripButtonProgress.Click += new EventHandler( Progress_Click );


            treeViewFile.MouseDown += new MouseEventHandler( treeViewFile_MouseDown );
            treeViewFile.MouseDoubleClick += new MouseEventHandler( treeViewFile_MouseDoubleClick );
            treeViewFile.KeyPress += new KeyPressEventHandler( treeViewFile_KeyPress );

            tabControlFile.MouseClick += new MouseEventHandler( tabControlFile_MouseClick );
            tabControlFile.MouseWheel += new MouseEventHandler( tabControlFile_MouseWheel );

            MenuItem[] items = new MenuItem[ 11 ];
            items[ 0 ] = new MenuItem( "�J��(&O)", treeViewFileOpen_Click );
            items[ 1 ] = new MenuItem( "�e�L�X�g�G�f�B�^�ŊJ��(&T)", treeViewFileEditor_Click );
            items[ 2 ] = new MenuItem( "�e�L�X�g�G�f�B�^�ŃI���W�i�����J��(&Y)", treeViewFileEditorOriginal_Click );
            items[ 3 ] = new MenuItem( "-" );
            items[ 4 ] = new MenuItem( "�t�@�C�������R�s�[(&F)", FileNameCopy_Click );
            items[ 5 ] = new MenuItem( "�p�X���R�s�[(&P)", PathCopy_Click );
            items[ 6 ] = new MenuItem( "-" );
            items[ 7 ] = new MenuItem( "�W�J(&E)", Expand_Click );
            items[ 8 ] = new MenuItem( "�S�W�J(&X)", ExpandAll_Click );
            items[ 9 ] = new MenuItem( "�܂���(&C)", Collapse_Click );
            items[ 10 ] = new MenuItem( "�S�܂���(&O)", CollapseAll_Click );
            treeViewFile.ContextMenu = new ContextMenu( items );

            if ( console != null ) {
                console.Show( this );
            }

        }

        void treeViewFileEditor_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewFile.SelectedNode;
            if ( node != null ) {
                string path = Path.Data.ModifiedDirectory + node.FullPath;
                if ( System.IO.File.Exists( path ) == false ) {
                    path = node.FullPath;
                }
                runEditor( path );
            }
        }
        void treeViewFileEditorOriginal_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewFile.SelectedNode;
            if ( node != null ) {
                string path = node.FullPath;
                runEditor( path );
            }
        }

        void Expand_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewFile.SelectedNode;
            if ( node != null ) {
                node.Expand();
            }
        }

        void ExpandAll_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewFile.SelectedNode;
            if ( node != null ) {
                node.ExpandAll();
            }
        }

        void Collapse_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewFile.SelectedNode;
            if ( node != null ) {
                node.Collapse( true );
            }
        }

        void CollapseAll_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewFile.SelectedNode;
            if ( node != null ) {
                node.Collapse( false );
            }
        }

        void MainForm_FormClosing( object sender, FormClosingEventArgs e ) {
            foreach ( TabPage tab in tabControlFile.TabPages ) {
                // modify check?
                Editor edit = tab.Tag as Editor;
                if ( edit != null && edit.checkModify() ) {
                    DialogResult result = MessageBox.Show(
                        this,
                        tab.Text + " �͕ύX����Ă��܂��B�ۑ����܂����H",
                        "�I���m�F",
                        MessageBoxButtons.YesNoCancel
                        );

                    // foreach�񂵂Ă���Œ��ɏ����Ă����v�Ȃ̂�����H
                    switch ( result ) {
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            return;
                        case DialogResult.No:
                            break;
                        case DialogResult.Yes:
                            save( edit );
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        // �^�u�̒��Ƀt�H�[�J�X���������ׂŏ�肭�@�\���Ă��Ȃ�
        void tabControlFile_MouseWheel( object sender, MouseEventArgs e ) {
            //System.Console.WriteLine( e.Delta.ToString() );
            int index = tabControlFile.SelectedIndex; // �Ȃ��Ƃ���-1
            int count = tabControlFile.TabCount;
            // 2�ȏ�
            if ( count > 1 ) {
                // �^�u��ɂ��邩�ǂ�������
                // ���ꂾ�ƃ^�u������\�蕔���ł͗L���ɂȂ��̂��
                // rect��tabControlFile�̃T�C�Y�����Ɍv�Z�����
                bool onTab = false;
                Point p = tabControlFile.PointToClient( Cursor.Position );
                for ( int i = 0; i < tabControlFile.TabPages.Count; ++i ) {
                    Rectangle rect = tabControlFile.GetTabRect( i );
                    if ( rect.Contains( p ) ) {
                        onTab = true;
                        break;
                    }
                }

                if ( onTab ) {
                    int delta = e.Delta / 100; // 臒l������ɂ���ł����� 120���݂炵�����A�ق��̃}�E�X���Ƃǂ�����
                    index -= delta; // �t
                    index = index % count; // mod���}�C�i�X�܂ł����񂩂�
                    //while ( index < 0 ) {
                    if ( index < 0 ) { // mod�ς݂�����if�ŏ\���̂͂�
                        index = count + index;
                    }
                    tabControlFile.SelectedIndex = index;
                }
            }
        }


        void tabControlFile_MouseClick( object sender, MouseEventArgs e ) {
            if ( e.Button == MouseButtons.Middle ) {
                TabControl tc = sender as TabControl;
                // �I���^�u�ł͂Ȃ��āA�}�E�X�̏�ɂ���^�u����
                if ( tc != null && tc.SelectedTab != null ) {
                    Point p = tabControlFile.PointToClient( Cursor.Position );
                    for ( int i = 0; i < tabControlFile.TabPages.Count; ++i ) {
                        Rectangle rect = tabControlFile.GetTabRect( i );
                        if ( rect.Contains( p ) ) {
                            // tab Close
                            // conform save?
                            TabPage tab = tabControlFile.TabPages[ i ];
                            Editor edit = tab.Tag as Editor;

                            SpeechPlayer.stop( edit );
                            
                            if ( edit != null && edit.checkModify() ) {
                                DialogResult result = MessageBox.Show(
                                    this,
                                    tab.Text + " �͕ύX����Ă��܂��B�ۑ����܂����H",
                                    "�^�u�����",
                                    MessageBoxButtons.YesNoCancel
                                    );

                                // foreach�񂵂Ă���Œ��ɏ����Ă����v�Ȃ̂�����H
                                switch ( result ) {
                                    case DialogResult.Cancel:
                                        return;
                                    case DialogResult.No:
                                        break;
                                    case DialogResult.Yes:
                                        save( edit );
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if ( tc.SelectedTab == tab ) {
                                setTitle( "" );
                            }
                            tc.TabPages.RemoveAt( i );
                            break;
                        }
                    }
                }
            }
        }

        void treeViewFile_KeyPress( object sender, KeyPressEventArgs e ) {
            if ( e.KeyChar == '\r' ) {
                TreeView tree = sender as TreeView;
                if ( tree != null ) {
                    if ( tree.SelectedNode != null ) {
                        // open
                        open( tree.SelectedNode );
                    }
                }
            }
        }

        void treeViewFile_MouseDown( object sender, MouseEventArgs e ) {
            // ���E�ǂ���ł������͓����ɂ��悤 ���N���b�N�͖���
            if ( e.Button == MouseButtons.Left || e.Button == MouseButtons.Right ) {
                TreeView tree = sender as TreeView;
                if ( tree != null ) {
                    Point p = tree.PointToClient( Cursor.Position );
                    //TreeNode node = tree.GetNodeAt( p );
                    TreeViewHitTestInfo info = tree.HitTest( p );

                    if ( info.Node != null ) {
                        tree.SelectedNode = info.Node;
                    }
                }
            }
        }

        void treeViewFile_MouseDoubleClick( object sender, MouseEventArgs e ) {
            if ( e.Button == MouseButtons.Left ) {
                TreeView tree = sender as TreeView;
                if ( tree != null ) {
                    if ( tree.SelectedNode != null ) {
                        open( tree.SelectedNode );
                    }
                }
            }
        }

        void treeViewFileOpen_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewFile.SelectedNode;
            if ( node != null ) {
                open( node );
            }
        }

        private void open( TreeNode node ) {
            //System.IO.FileInfo info = new System.IO.FileInfo( Path.convineFullPath( node.FullPath ) );
            System.IO.FileInfo info = new System.IO.FileInfo( node.FullPath );
            if ( info.Exists ) {
                // �d���֎~
                foreach ( TabPage t in tabControlFile.TabPages ) {
                    for ( int i = 0; i < t.Controls.Count; ++i ) {
                        if ( t.Controls[ i ].Name == node.FullPath ) {
                            tabControlFile.SelectedTab = t;
                            return;
                        }
                    }
                }

                TabPage tab = new TabPage( node.Text );
                //tabControlFile.SuspendLayout();

                switch ( info.Extension.ToLower() ) {
                    case Path.Ext.DialogExtention:
                        DlgEditor dlg = new DlgEditor( node.FullPath );
                        dlg.Dock = DockStyle.Fill;
                        tab.Tag = dlg;
                        tab.Controls.Add( dlg );
                        tabControlFile.TabPages.Add( tab );
                        tabControlFile.SelectedTab = tab;
                        break;
                    case Path.Ext.MessageExtention:
                        MesEditor mes = new MesEditor( node.FullPath );
                        mes.Dock = DockStyle.Fill;
                        tab.Tag = mes;
                        tab.Controls.Add( mes );
                        tabControlFile.TabPages.Add( tab );
                        tabControlFile.SelectedTab = tab;
                        break;
                    default:
                        break;
                }

                //tabControlFile.ResumeLayout();
                //tabControlFile.PerformLayout();

                // �t�H�[�J�X���ڂ��H
                // �Ȃ񂩒ǉ������Ƃ��Ƀ`�����̂��؂ɂȂ�
            }
        }

        // current path�Z�b�g����悤�ɂ����̂ŁA���΃p�X�ł�����ق��������y���낤�Ȃ�
        bool createFileTree() {
            treeViewFile.Nodes.Clear();
            string mod_dir = @"Arcanum";
            if ( System.IO.Directory.Exists( mod_dir ) == false ) {
                return false;
            }
            TreeNode root = new TreeNode( mod_dir );
            //createFileTree( root, Path.convineFullPath( mod_dir ) );
            bool find = createFileTree( root, mod_dir );
            if ( find ) {
                root.Expand();
                treeViewFile.Nodes.Add( root );
            }
            return find;
        }
        bool createFileTree( TreeNode node, string name ) {
            bool find = false;
            { // dir part
                string current = System.IO.Directory.GetCurrentDirectory();
                string[] dirs = System.IO.Directory.GetDirectories( name );
                int num = 0;
                foreach ( string dir in dirs ) {
                    if ( IOController.validDirectory( dir ) ) {
                        ++num;
                    }
                }
                if ( num > 0 ) {
                    TreeNode[] nodes = new TreeNode[ num ];
                    int count = 0;
                    bool adding = false;
                    for ( int i = 0; i < dirs.Length; ++i ) {
                        // ���݊m�F���������A�ċN�Ȃ̂�path���Ȃ��Ă����Ȃ��Ɠ����
                        // ����addrange���Ă�null�����肾�Ƃ����邩�������
                        if ( IOController.validDirectory( dirs[ i ] ) ) {

                            string dir = System.IO.Path.GetFileName( dirs[ i ] );
                            nodes[ count ] = new TreeNode( dir );
                            nodes[ count ].ImageIndex = 0;
                            nodes[ count ].SelectedImageIndex = 0;
                            bool ret = createFileTree( nodes[ count ], dirs[ i ] );
                            if ( ret ) {
                                find = true;
                                adding = true;
                            }
                            ++count;
                        }
                    }
                    // ����dlg,mes����������ǉ�
                    if ( adding ) {
                        node.Nodes.AddRange( nodes );
                    }
                }
            }
            { // file part
                string[] files = System.IO.Directory.GetFiles( name );
                int num = 0;
                foreach ( string file in files ) {
                    if ( IOController.validDlgMesFile( file ) ) {
                        num++;
                        find = true;
                    }
                }
                if ( num > 0 ) {
                    //Uri current = new Uri( Path.convineFullPath( Path.Data.BaseDirectory ) );
                    //TreeNode[] nodes = new TreeNode[ files.Length ];
                    TreeNode[] nodes = new TreeNode[ num ];
                    int count = 0;
                    for ( int i = 0; i < files.Length; ++i ) {
                        int icon = 1;
                        if ( IOController.validDlgMesFile( files[ i ] ) ) {
                            nodes[ count ] = new TreeNode( System.IO.Path.GetFileName( files[ i ] ) );
                            string mod = Path.Data.ModifiedDirectory + files[ i ];
                            if ( System.IO.File.Exists( mod ) ) {
                                icon = 2;
                            } else {
                                icon = 1;
                            }
                            nodes[ count ].ImageIndex = icon;
                            nodes[ count ].SelectedImageIndex = icon;
                            ++count;
                        }
#if false
                        nodes[ i ] = new TreeNode( System.IO.Path.GetFileName( files[ i ] ) );
                        // �{���͐i���ł�肽���̂���
                        int icon = 1;

                        // debug
                        //File file = new File();
                        switch ( ext.ToLower() ) {
                            case Path.Ext.MessageExtention:
                                //icon = 1;
                                //file.open( files[ i ], true );
                                //file.save( files[ i ] );
                                //System.Console.WriteLine( "|-" );
                                //System.Console.WriteLine( "| " + nodes[ i ].Text + " || || " );
                                //System.Console.WriteLine( "**" + nodes[ i ].Text );
                                //System.Console.WriteLine( "    \"" + nodes[ i ].Text + "\" => \"" + System.IO.Path.GetDirectoryName( files[ i ] ) + "\"," );
                                find = true;
                                break;

                            case Path.Ext.DialogueExtention:
                                //icon = 2;
                                //file.open( files[ i ], true );
                                //file.save( files[ i ] );
                                //System.Console.WriteLine( "|-" );
                                //System.Console.WriteLine( "| " + nodes[ i ].Text + " || || " );
                                //System.Console.WriteLine( "**" + nodes[i].Text );
                                find = true;
                                break;
                        }
                        //Uri uri = current.MakeRelativeUri( new Uri( files[ i ] ) );
                        //string mod = Path.convineFullPath( Path.Data.ModifiedDirectory + uri.ToString().Replace( '/', '\\' ) );
                        string mod = Path.Data.ModifiedDirectory + files[ i ];
                        if ( System.IO.File.Exists( mod ) ) {
                            icon = 2;
                        } else {
                            icon = 1;
                        }
                        nodes[ i ].ImageIndex = icon;
                        nodes[ i ].SelectedImageIndex = icon;
#endif
                    }
                    node.Nodes.AddRange( nodes );
                } else {
                    node.Expand();
                }
                return find;
            }
        }

        Editor getSelectedEditor() {
            TabPage tab = tabControlFile.SelectedTab;
            if ( tab != null ) {
                Editor edit = tab.Tag as Editor;
                return edit;
            }
            return null;
        }

        void save( Editor edit ) {
            if ( edit != null ) {
                edit.save();
                if ( edit.Tag != null ) {
                    string path = edit.Name;
                    // �d�����Ă���ꍇ�͌Â��ق����폜
                    for ( int i = 0; i < history.Path.Count; ++i ) {
                        if ( history.Path[i] == path ) {
                            history.Path.RemoveAt( i );
                            break;
                        }
                    }
                    // �ő吔�����H
                    while ( history.Path.Count > 9 ) {
                        history.Path.RemoveAt( 0 );
                    }                    
                    history.Path.Add( path );
                    xml.Xml.Write<History>( Path.Xml.HistoryFile, history );

                    
                    // �A�C�R���̕ύX
                    string[] names = path.Split( '\\' );
                    TreeNodeCollection nodes = treeViewFile.Nodes;
                    int index = 0;
                    TreeNode find = null;

                    while ( index < names.Length ) {
                        TreeNode node = searchNodes( nodes, names[ index ] );
                        if ( node == null ) {
                            break;
                        }
                        ++index;
                        if ( index == names.Length ) {
                            find = node;
                            break;
                        }
                        nodes = node.Nodes;
                    }

                    if ( find != null ) {
                        string mod = Path.Data.ModifiedDirectory + path;
                        if ( System.IO.File.Exists( mod ) ) {
                            int icon = 2;
                            find.ImageIndex = icon;
                            find.SelectedImageIndex = icon;
                        }
                        //System.Console.WriteLine("find");
                    }
                }
            }
        }


    }
}