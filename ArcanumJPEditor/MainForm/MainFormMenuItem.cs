// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArcanumJPEditor {
    public partial class MainForm {

        void toolStripMenuItemFile_DropDownOpening( object sender, EventArgs e ) {
            TabPage tab = tabControlFile.SelectedTab;
            if ( tab != null ) {
                // modify check?
                //toolStripMenuItemSave.Enabled = true;
                toolStripMenuItemTabClose.Text = tab.Text + " �����(&C)";
                toolStripMenuItemSave.Text = tab.Text + " ��ۑ�(&S)";
            } else {
                //toolStripMenuItemSave.Enabled = false;
                toolStripMenuItemTabClose.Text = "�^�u�����(&C)";
                toolStripMenuItemSave.Text = "�ۑ�(&S)";
            }


            if ( history.Path.Count > 0 ) {
                ToolStripMenuItem[] items = new ToolStripMenuItem[ history.Path.Count ];
                for ( int i = 0; i < history.Path.Count; ++i ) {
                    string path = history.Path[ history.Path.Count - 1 - i ];
                    items[ i ] = new ToolStripMenuItem( "&" + ( i + 1 ) + ": " + path );
                    items[ i ].Tag = path;
                    items[ i ].Click += new EventHandler( Recent_Click );
                }

                toolStripMenuItemRecent.DropDownItems.Clear();
                toolStripMenuItemRecent.DropDownItems.AddRange( items );

                toolStripMenuItemRecent.Enabled = true;
            } else {
                toolStripMenuItemRecent.Enabled = false;
            }
        }

        void Recent_Click( object sender, EventArgs e ) {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if ( item != null ) {
                string path = item.Tag as string;
                if ( path != null ) {
                    // fullpath�Ō����ł��Ȃ��Ƃ��������
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
                        //setStatus("find");
                        treeViewFile.SelectedNode = find;
                        open( find );
                    } else {
                        setStatus( "�ŋߕҏW�����t�@�C��: �t�@�C����������Ȃ�����" );
                    }

                }
            }
        }
        TreeNode searchNodes( TreeNodeCollection nodes , string text ) {
            foreach ( TreeNode node in nodes ) {
                if ( node.Text == text ) {
                    return node;
                }
            }
            return null;
        }


        void Save_Click( object sender, EventArgs e ) {
            Editor edit = getSelectedEditor();
            if ( edit != null ) {
                if ( edit.checkModify() ) {
                    save( edit );
                }
            }
        }

        void SaveAll_Click( object sender, EventArgs e ) {
            foreach ( TabPage tab in tabControlFile.TabPages ) {
                // modify check?
                Editor edit = tab.Tag as Editor;
                if ( edit != null ) {
                    if ( edit.checkModify() ) {
                        save( edit );
                    }
                }
            }
        }

        void Exit_Click( object sender, EventArgs e ) {
            this.Close();
        }

        void Undo_Click( object sender, EventArgs e ) {
            // textbox�ɂ��邯�ǎg�����ɂ͂Ȃ���@���O������richtextbox�܂ł̋C�x��
            Editor edit = getSelectedEditor();
            if ( edit != null ) {
                edit.Undo_Click( sender, e );
            }
        }

        void Redo_Click( object sender, EventArgs e ) {
            // textbox�ɂȂ���I
            Editor edit = getSelectedEditor();
            if ( edit != null ) {
                edit.Redo_Click( sender, e );
            }
        }

        void Cut_Click( object sender, EventArgs e ) {
            if ( treeViewFile.Focused ) {
                FileNameCopy_Click( sender, e );
            } else {
                Editor edit = getSelectedEditor();
                if ( edit != null ) {
                    edit.Cut_Click( sender, e );
                }
            }
        }

        void Copy_Click( object sender, EventArgs e ) {
            if ( treeViewFile.Focused ) {
                FileNameCopy_Click( sender, e );
            } else {
                Editor edit = getSelectedEditor();
                if ( edit != null ) {
                    edit.Copy_Click( sender, e );
                }
            }
        }

        void Paste_Click( object sender, EventArgs e ) {
            Editor edit = getSelectedEditor();
            if ( edit != null ) {
                edit.Paste_Click( sender, e );
            }
        }

        void SelectAll_Click( object sender, EventArgs e ) {
            Editor edit = getSelectedEditor();
            if ( edit != null ) {
                edit.SelectAll_Click( sender, e );
            }
        }

        void Find_Click( object sender, EventArgs e ) {
            // �J���ĂȂ��ꍇ�̓t�@�C������
            if ( tabControlFile.TabPages.Count == 0 ) {
                search_option.range = Search.Option.Range.ByFileName;
            }

            if ( search != null && search.Visible ) {
                search.Focus();
            } else {
                // �N���b�v�{�[�h���甽�f
#if false
                if ( search_option.search_word != null && search_option.search_word.Length > 0 ) {
                } else {
                    // ��̏ꍇ�̂ݓ����悤�ɂ��Ă݂邩�[
                    string word = System.Windows.Forms.Clipboard.GetText();
                    if ( word != null ) {
                        word = word.Replace( "\r", "" );
                        word = word.Replace( "\n", "" );


                        search_option.search_word = word; // �ۗ�
                        //���s��菜�����ق����ǂ�����
                        // ���s��������del�ł������ł����ɂ͂܂�
                        // dao�ł��΂����Ă���
                    }
                }
#endif
                search = new Search( search_option ); // ��蒼���Ȃ��ƃC���X�^���X�����Ă��j������Ă�Ƃ������₪�镜�A������ɂ́H
                search.FormClosing += new FormClosingEventHandler( Search_FormClosing );
                search.ButtonSearch.Click += new EventHandler( ButtonSearch_Click );
                search.ButtonSearchPrev.Click += new EventHandler( ButtonSearchPrev_Click );
                search.TextBoxSearch.KeyDown += new KeyEventHandler( TextBoxSearch_KeyDown );
                // �ǂ������̈ʒu�ɏo�������̂����A��ʊO�ɏo��Ƌl�ނ��Ȃ���������m���鏈�������̂͌ゾ
                search.Show( this );
                // ����Location�̃Z�b�g�͕\���ザ��Ȃ��ƗL���ɂȂ�Ȃ���S�~�������邩��
            }
        }
        void Search_FormClosing( object sender, FormClosingEventArgs e ) {
            search.restore( search_option );
        }
        void TextBoxSearch_KeyDown( object sender, KeyEventArgs e ) {
            if ( e.KeyCode == System.Windows.Forms.Keys.Enter ) {
                ButtonSearch_Click( sender, null );
            }
        }

        void ButtonSearch_Click( object sender, EventArgs e ) {
            search.restore( search_option );
            //menuItemSearchNext_Click( sender, e );
            find( true );
        }

        void ButtonSearchPrev_Click( object sender, EventArgs e ) {
            search.restore( search_option );
            //menuItemSearchPrev_Click( sender, e );
            find( false );
        }
        void find( bool forward ) {
            TreeNodeUtill.GetNode getfunc = TreeNodeUtill.GetPrev;
            if ( forward ) {
                getfunc = TreeNodeUtill.GetNext;
            }
            Search.Option option = new Search.Option( search_option );
            TabPage tab = tabControlFile.SelectedTab;
            int tabindex = tabControlFile.SelectedIndex;
            switch ( option.range ) {
                //case Search.Option.Range.ByAll:
                //    break;
                //case Search.Option.Range.ByMes:
                //    break;
                //case Search.Option.Range.ByDlg:
                //    break;
                case Search.Option.Range.BySelected:
                    if ( tab != null ) {
                        Editor edit = tab.Tag as Editor;
                        setStatus( "" );
                        if ( edit.search( option, getfunc ) ) {
                        } else {
                            setStatus( "��������" );
                        }
                    }
                    break;
                case Search.Option.Range.ByOpened:
                    // �I���^�u����`�F�b�N
                    if ( tab != null ) {
                        int count = tabControlFile.TabCount;
                        int num = tabindex;
                        option.repeat = false; // edit�ł̓��s�[�g���Ȃ���
                        setStatus( "" );
                        do {
                            tabControlFile.SelectedIndex = num;// selected�����x���N����d���������A������Ȃ��͗l
                            TabPage t = tabControlFile.TabPages[ num ];
                            Editor edit = t.Tag as Editor;
                            bool ret = edit.search( option, getfunc );
                            if ( ret ) {
                                Control con = edit.ActiveControl;
                                //tabControlFile.SelectedIndex = num;
                                // TODO:���̏��ԂŃt�H�[�J�X�ڂ�񂩂��� �t�H�[�J�X�͈ڂ�Ȃ�
                                break;
                            }

                            if ( forward ) {
                                // next
                                ++num;
                                // �I�[�`�F�b�N
                                if ( search_option.repeat == false && num >= count ) {
                                    setStatus( "��������" );
                                    break;
                                }
                            } else {
                                // prev
                                --num;
                                // �I�[�`�F�b�N
                                if ( search_option.repeat == false && num < 0 ) {
                                    setStatus( "��������" );
                                    break;
                                }
                            }
                            num = num % count;
                            if ( num < 0 ) {
                                num += count;
                            }

                            // ���̃y�[�W�ɐ؂�ւ��Č������鎞�́Ames/dlg�̃c���[�𖢏������̖`����Ԃɂ��Ă����Ȃ��ƁA
                            // 2��ڂ���Ђ��������Ă���Ȃ��Ȃ��B
                            // �ŏ��ɖ߂鎞�̋����͂ǂ��Ȃ�񂾁H
                            // �����葱������莟�̃^�u�ɂ����Ă���Ȃ��̂ŁAoption�M���Ƃ�
                            edit.treeReset();
                        } while ( num != tabindex );
                    }
                    break;
                case Search.Option.Range.ByFileName:
                    TreeNode select = treeViewFile.SelectedNode;
                    if ( select == null ) {
                        if ( treeViewFile.Nodes.Count > 0 ) {
                            // �����select�ɂ���Ƃ����͒��ׂĂ���Ȃ�
                            // �܂����̂Ƃ���͒��ׂ�K�v�͔����̂���
                            select = treeViewFile.Nodes[ 0 ];
                        } else {
                            return;
                        }
                    }
                    setStatus( "" );
                    TreeNode node = searchFile( select, null, option, getfunc );
                    if ( node == null && option.repeat && select != treeViewFile.Nodes[ 0 ] ) {
                        node = searchFile( treeViewFile.Nodes[ 0 ], select, option, getfunc );
                    }
                    if ( node != null ) {
                        //treeViewFile.Focus();
                        treeViewFile.SelectedNode = node;
                    } else {
                        setStatus( "��������" );
                    }
                    break;
                default:
                    break;
            }
        }


        TreeNode searchFile( TreeNode node, TreeNode stop, Search.Option option, TreeNodeUtill.GetNode getfunc ) {
            TreeNode next = getfunc( node );

            while ( next != null && next != stop ) {
                string text = next.Text;
                // �ׂ����I�v�V�����͂܂�
                if ( compare( text, option ) ) {
                    return next;
                }
                next = getfunc( next );
            }
            return null;
        }


        bool compare( string text, Search.Option option ) {
            string a = text;
            string b = option.search_word; // �������͎��O�ɍ���Ă����ƌy�����Ȃ񂾂�
            if ( option.regex ) {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex( b );
                return regex.IsMatch( a );
            }

            if ( option.charactor == false ) {
                a = a.ToLower();
                b = b.ToLower();
            }
            if ( option.complete ) {
                if ( a.Length == b.Length ) {
                    if ( a == b ) {
                        return true;
                    }
                }
            }

            // normal
            if ( a.Contains( b ) ) {
                return true;
            }

            return false;
        }


        void Contract_CheckedChanged( object sender, EventArgs e ) {
            ToolStripButton button = sender as ToolStripButton;
            if ( button != null ) {
                splitContainer1.Panel1Collapsed = button.Checked;
            }
        }

        void Editor_Click( object sender, EventArgs e ) {

            // �c���[�E�N���Ƃ����Ƃ܂��ʂ̘b
            TabPage tab = tabControlFile.SelectedTab;
            if ( tab != null ) {
                Editor edit = tab.Tag as Editor;
                //if ( edit.Tag != null ) {
                //string path = edit.Tag as string; // ����������ƈ�U����܂ł͌Â��܂܂���
                string path = edit.ModPath;
                if ( System.IO.File.Exists( path ) == false ) {
                    path = edit.OriginalPath;
                }
                runEditor( path );
            }
        }
        void EditorOriginal_Click( object sender, EventArgs e ) {
            TabPage tab = tabControlFile.SelectedTab;
            if ( tab != null ) {
                Editor edit = tab.Tag as Editor;
                string path = edit.OriginalPath;
                runEditor( path );
            }
        }

        
        void Dat_Click( object sender, EventArgs e ) {
            setStatus( "DAT�쐬..." );
            string exe = Path.Tools.MakerFile;
            if ( System.IO.File.Exists( exe ) == false ) {
                addStatus( exe + " �����݂��Ȃ�" );
                return;
            }

            {
                string src = Path.Data.ConvertedDirectory + Path.Data.BaseDirectory;
                if ( System.IO.Directory.Exists( src ) == false ) {
                    addStatus( src + " �����݂��Ȃ�" );
                    return;
                } 
                string name = Path.Data.MainFile;
                string path = Path.ExecutableDirectory + name;
                // �t�@�C���܂��̓f�B���N�g�����O���ƃG���[���邪�A�܂��߂ɒ��ׂ邩���H
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = Path.ExecutableDirectory + exe;
                proc.StartInfo.WorkingDirectory = src;
                proc.StartInfo.Arguments = @"-r -c0 " + path;
                bool isConvert = false;
                string dlg = src + Path.Data.DialogDirectory;
                if ( System.IO.Directory.Exists( dlg ) ) {
                    proc.StartInfo.Arguments += @" " + Path.Data.DialogDirectory + "*";
                    isConvert = true;
                }
                string mes = src + Path.Data.MessageDirectory;
                if ( System.IO.Directory.Exists( mes ) ) {
                    proc.StartInfo.Arguments += @" " + Path.Data.MessageDirectory + "*";
                    isConvert = true;
                }
                if ( isConvert ) {
                    proc.Start();
                    addStatus( name + @" �쐬..." );
                } else {
                    addStatus( name + @" ���s..." );
                }
            }
            {
                string src = Path.Data.ConvertedDirectory + Path.Data.ModuleDirectory;
                if ( System.IO.Directory.Exists( src ) == false ) {
                    addStatus( src + " �����݂��Ȃ�" );
                    return;
                }
                string name = Path.Data.ModuleFile;
                string path = Path.ExecutableDirectory + name;
                // �t�@�C���܂��̓f�B���N�g�����O���ƃG���[���邪�A�܂��߂ɒ��ׂ邩���H
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = Path.ExecutableDirectory + exe;
                proc.StartInfo.WorkingDirectory = src;
                proc.StartInfo.Arguments = @"-r -c0 " + path;
                string mes = src + Path.Data.MessageDirectory;
                if ( System.IO.Directory.Exists( mes ) ) {
                    proc.StartInfo.Arguments += @" " + Path.Data.MessageDirectory + "*";
                    proc.Start();
                    addStatus( name + @" �쐬..." );
                } else {
                    addStatus( name + @" ���s..." );
                }
            }
            // ������뎸�s���̏�������
            addStatus( "����" );
        }

        void Place_Click( object sender, EventArgs e ) {
            setStatus( "�R�s�[�J�n..." );

            if ( config.ArcanumDirectory == null ) {
                addStatus( @"Arcanum �C���X�g�[���t�H���_�����ݒ�" );
                return;
            } else if( config.ArcanumDirectory.Length <= 0 ){
                addStatus( @"Arcanum �C���X�g�[���t�H���_�����ݒ�" );
                return;
            } else if ( System.IO.Directory.Exists( config.ArcanumDirectory ) == false ) {
                addStatus( @"Arcanum �C���X�g�[���t�H���_�����݂��Ȃ�" );
                return;
            }
            {
                string name = Path.Data.MainFile;
                string dat = name;
                if ( System.IO.File.Exists( dat ) ) {
                    // copy
                    string dest = System.IO.Path.GetFullPath( config.ArcanumDirectory );
                    dest = System.IO.Path.Combine( dest, name );
                    System.IO.File.Copy( dat, dest, true );
                    addStatus( name + " ���R�s�[..." );
                } else {
                    addStatus( name + " �����݂��Ȃ�..." );
                }
            }
            {
                string name = Path.Data.ModuleFile;
                string dat = name;
                if ( System.IO.File.Exists( dat ) ) {
                    // copy
                    string dest = System.IO.Path.GetFullPath( config.ArcanumDirectory );
                    dest = System.IO.Path.Combine( dest, @"modules\" + name );
                    System.IO.File.Copy( dat, dest, true );
                    addStatus( name + " ���R�s�[..." );
                } else {
                    addStatus( name + " �����݂��Ȃ�..." );
                }
            }
            addStatus( "����" );
        
        }

        void ConvertAll_Click( object sender, EventArgs e ) {
            // convert_click�œ�d�Ƀ`�F�b�N���Ă邪�Acancel����Ă��ۂ͂�߂Ȃ���΂Ȃ�Ȃ��̂�
            if( preConvertSave() == false ) {
                return;
            }

            Convert_Click( sender, e );
            Dat_Click( sender, e );
            Place_Click( sender, e );
        }

        void Setting_Click( object sender, EventArgs e ) {
            Setting setting = new Setting( config );
            DialogResult result = setting.ShowDialog( this );
            if ( result == DialogResult.OK ) {
                setting.restore( config );
                xml.Xml.Write<Config>( Path.Xml.ConfigFile, config );
            }
            SpeechPlayer.Volume = config.SpeechVolume;
        }

        void Next_Click( object sender, EventArgs e ) {
            Editor edit = getSelectedEditor();
            if ( edit != null ) {
                edit.Next_Click( sender, e );
            }
        }
        void ToMale_Click( object sender, EventArgs e ) {
            Editor edit = getSelectedEditor();
            if ( edit != null ) {
                edit.ToMale_Click( sender, e );
            }
        }
        void ToFemale_Click( object sender, EventArgs e ) {
            Editor edit = getSelectedEditor();
            if ( edit != null ) {
                edit.ToFemale_Click( sender, e );
            }
        }

        void SpeechPlay_Click( object sender, EventArgs e ) {
            Editor edit = getSelectedEditor() as DlgEditor;
            if ( edit != null ) {
                edit.SpeechPlay_Click( sender, e );
                // �p�X�擾���Ă������ōĐ��H
                // ����virtual�̕������ꂵ��
            }
        }
        void SpeechStop_Click( object sender, EventArgs e ) {
            SpeechPlayer.stop( null ); // force stop
        }

        void Contract_Click( object sender, EventArgs e ) {
            toolStripButtonContract.Checked = !toolStripButtonContract.Checked;
        }

        void TabClose_Click( object sender, EventArgs e ) {
            // middle click���Ɣ����ɔ���Ă���
            TabPage tab = tabControlFile.SelectedTab;
            if ( tab != null ) {
                Editor edit = tab.Tag as Editor;
                
                SpeechPlayer.stop( edit );

                if( edit != null && edit.checkModify() ) {
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

                tabControlFile.TabPages.Remove( tab );
                setTitle( "" );
                // ���ɑI�������^�u�͎w�肵�Ȃ��Ɛ擪�ɂȂ�͗l
            }
        }

        void TabCloseAll_Click( object sender, EventArgs e ) {
            foreach ( TabPage tab in tabControlFile.TabPages ) {
                // modify check?
                Editor edit = tab.Tag as Editor;

                SpeechPlayer.stop( edit );

                if ( edit != null && edit.checkModify() ) {
                    DialogResult result = MessageBox.Show(
                        this,
                        tab.Text + " �͕ύX����Ă��܂��B�ۑ����܂����H",
                        "�S�Ẵ^�u�����",
                        MessageBoxButtons.YesNoCancel
                        );

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

                // �Z�[�u�L�����Z�������Ƃ��ł��Ӑ}�����Ƃ���^�C�g��������H�H
                if ( tab == tabControlFile.SelectedTab ) {
                    setTitle( "" );
                }
                // foreach�񂵂Ă���Œ��ɏ����Ă����v���ۂ�
                tabControlFile.TabPages.Remove( tab );
            }
        }

        void Convert_Click( object sender, EventArgs e ) {


            if ( preConvertSave() == false ) {
                return;
            }

            // conv\Arcanum\�̒��g������Α|������
            // conv�ŏ����Ȃ��̂͂Ȃ�ŁH
            IOController.deleteDirectory( Path.Data.ConvertedDirectory + Path.Data.BaseDirectory );

            setStatus( "�R���o�[�g�J�n..." );
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            initializeProgressBar( IOController.getNumFiles( Path.Data.ModifiedDirectory ) );

            Convert.convert( Path.Data.ModifiedDirectory );

            watch.Stop();

            completeProgressBar();
            addStatus( "����" + " (" + watch.ElapsedMilliseconds + "ms)" );
        }

        bool preConvertSave() {
            // ���̑O�ɊJ���Ă���^�u�̃Z�[�u�̊m�F
            bool isModify = false;
            foreach ( TabPage tab in tabControlFile.TabPages ) {
                // modify check?
                Editor edit = tab.Tag as Editor;
                if ( edit != null && edit.checkModify() ) {
                    isModify = true;
                    break;
                }
            }
            if ( isModify ) {
                DialogResult result = MessageBox.Show(
                    this,
                    "�ύX����Ă���t�@�C��������܂��B\n�ۑ����Ă��Ȃ��t�@�C���̕ύX�̓R���o�[�g���ɓK�p����܂���B\n�S�Ẵt�@�C����ۑ����܂����H",
                    "�R���o�[�g�O�m�F",
                    MessageBoxButtons.YesNoCancel
                    );

                switch ( result ) {
                    case DialogResult.Cancel:
                        return false;
                    case DialogResult.No:
                        break;
                    case DialogResult.Yes:
                        foreach ( TabPage tab in tabControlFile.TabPages ) {
                            // modify check?
                            Editor edit = tab.Tag as Editor;
                            if ( edit != null && edit.checkModify() ) {
                                save( edit );
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return true;
        }

        void ConvertAddingPrefix_Click( object sender, EventArgs e ) {

            // ���̑O�ɊJ���Ă���^�u�̃Z�[�u�̊m�F
            if ( preConvertSave() == false ) {
                return;
            }


            // conv\Arcanum\�̒��g������Α|������

            // temp�Ƃ�conv�w�肶��_���Ȃ�H
            IOController.deleteDirectory( Path.Data.TemporaryDirectory + Path.Data.BaseDirectory );

            IOController.deleteDirectory( Path.Data.ConvertedDirectory + Path.Data.BaseDirectory );


            setStatus( "�R���o�[�g�J�n..." );

            // prefix
            // editor�̃^�u�̕\���͕ύX�������Ȃ�����J������

            // ��Utemp�ɏo��

            // temp����conv��

            // �����Ȃ��ƁA�ʏ�R���o�[�g���Ƀt�@�C�������܂�ȁc
            // ���邢�̓R���o�[�g�O�Ɉ�Uconv��|�����邩
            // ���Əd���̂ŏ��X���X�V����Ȃ� ���낻��v���O���X�o�[�������B�ł���΃}���`�X���b�h�ł�������
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            initializeProgressBar( IOController.getNumFiles( Path.Data.BaseDirectory ) + IOController.getNumFiles( Path.Data.ModifiedDirectory ) );

            Convert.convertPrefixOriginal( Path.Data.BaseDirectory );

            Convert.convertPrefix( Path.Data.ModifiedDirectory );

            watch.Stop();

            completeProgressBar();
            addStatus( "����" + " (" + watch.ElapsedMilliseconds + "ms)" );

            // �S�~����
            IOController.deleteDirectory( Path.Data.TemporaryDirectory );


        }


        void About_Click( object sender, EventArgs e ) {
            About about = new About( title, version );
            about.ShowDialog( this );
        }



        void FileNameCopy_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewFile.SelectedNode;
            if ( node != null ) {
                System.Windows.Forms.Clipboard.SetText( node.Text );
                setStatus( @"�t�@�C�������R�s�[: " + Clipboard.GetText() );
            }
        }

        void PathCopy_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewFile.SelectedNode;
            if ( node != null ) {
                System.Windows.Forms.Clipboard.SetText( node.FullPath );
                setStatus( @"�p�X���R�s�[: " + Clipboard.GetText() );
            }
        }

        void Progress_Click( object sender, EventArgs e ) {

            initializeProgressBar( IOController.getNumFiles( @"Arcanum\" ) );

            setStatus( "�|��i�����̎Z�o��..." );

            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            // add��list�ɂł����l�߂ďW�v�͌�ɂ��������y���ȁ[
            string prog = Path.Xml.ProgressFile;
            ProgressFile pf = new ProgressFile();
            if ( System.IO.File.Exists( prog ) ) {
                pf = xml.Xml.Read<ProgressFile>( prog );
            }
            // list�̃I�[�_�[���C�ɂȂ邯�Ǐ����ĂȂ��̂ŔO�̂���hash�ɂ����
            System.Collections.Hashtable ignore = new System.Collections.Hashtable();
            foreach ( string f in pf.Ignore ) {
                ignore.Add( f, 0 );
            }
            foreach ( string f in pf.Pending ) {
                ignore.Add( f, 0 );
            }


            List<ProgressCount> progress = new List<ProgressCount>();
            calcProgress( Path.Data.BaseDirectory, progress, ignore );

            string buff = @"";
            int num = 0;
            int total = 0;
            foreach ( ProgressCount count in progress ) {
                if ( count.Exist ) {
                    buff += @"*";
                }
                if ( count.Translated < count.Total ) {
                } else {
                    buff += @"+";
                }
                float rate = 0;
                if( count.Total > 0 ) {
                    rate = ( float )count.Translated / count.Total;
                }
                buff += count.Name + ": " + count.Translated + " / " + count.Total + " (" +  rate.ToString("P0") + ")\r\n";
                num += count.Translated;
                total += count.Total;
            }
            if ( total > 0 ) {
                float totalrate = ( float )num / total;
                buff = "TOTAL: " + num + " / " + total + " (" + totalrate.ToString( "P0" ) + ")\r\n" + buff;
                string output = @"progress.txt";
                using ( System.IO.StreamWriter writer = new System.IO.StreamWriter( output, false, System.Text.Encoding.GetEncoding( "shift_jis" ) ) ) {
                    //writer.AutoFlush = true; // ������������
                    writer.Write( buff );
                    runEditor( output );
                }
            }

            completeProgressBar();

            watch.Stop();

            addStatus( "����: progress.txt" + " (" + watch.ElapsedMilliseconds + "ms)" );

            // �{�Ƃ��̃`�F�b�N�ɂЂ��������Ă�Ȃ�
            // prefix�p�Ƃ͕ʊ֐��p�ӂ��Ȃ���
        }

        void calcProgress( string name, List<ProgressCount> progress, System.Collections.Hashtable ignore ) {
            if( System.IO.Directory.Exists( name ) == false ) {
                return;
            }
            { // dir part
                string current = System.IO.Directory.GetCurrentDirectory();
                string[] dirs = System.IO.Directory.GetDirectories( name );
                foreach( string dir in dirs ) {
                    if( IOController.validDirectory( dir ) ) {
                        calcProgress( dir, progress, ignore );
                    }
                }

            }
            { // file part
                string dname = System.IO.Path.GetFileName( name );
                dname = dname.ToLower();
                if( dname == Path.Data.DialogDirectoryName || dname == Path.Data.MessageDirectoryName ) {
                    string[] files = System.IO.Directory.GetFiles( name );
                    foreach( string file in files ) {
                        // ���O�t�@�C��
                        if( IOController.validDlgMesFile( file ) && ignore.ContainsKey( file ) == false ) {

                            File ori = new File();
                            ori.open( file, true );
                            int num = 0;
                            int idnum = ori.IDs.Count;
                            string modpath = Path.Data.ModifiedDirectory + file;
                            bool exist = System.IO.File.Exists( modpath );
                            if( exist ) {
                                File mod = new File();
                                mod.open( modpath, false );

                                int count = 0;
                                // compair
                                for( int i = 0; i < idnum; ++i ) {
                                    string id = ori.IDs[ i ] as string;
                                    if( id != null ) {
                                        File.Chunk oc = ori.Chunks[ id ] as File.Chunk;
                                        File.Chunk mc = mod.Chunks[ id ] as File.Chunk;
                                        if( oc != null && mc != null ) {
                                            // ���̔�r�񐔂���肽��
                                            if( ori.Type == File.FileType.MES ) {
                                                File.Node omes = oc.Message;
                                                File.Node mmes = mc.Message;
                                                if( File.checkAvailable( omes.Original ) ) {
                                                    ++count;
                                                    if( omes.Original.CompareTo( mmes.Modify ) != 0 ) {
                                                        ++num;
                                                    }
                                                } else {
                                                    //++num;
                                                }
                                            } else {
                                                File.Node omale = oc.MaleLine;
                                                File.Node ofemale = oc.FemaleLine;
                                                File.Node mmale = mc.MaleLine;
                                                File.Node mfemale = mc.FemaleLine;
                                                // ����邵�ǂ����������ɂ��悤���Ȃ� 0.5?

                                                // ��͂ǂ����邩��

                                                // ������x���O���Ȃ��Ƃ��ꂾ���A���̕ӂ͂��傤���Ȃ��̂��˂�
                                                // ����[E:]�݂����Ȃ̂́A�ʃt�@�C���Q�ƂȂ̂ŏ��O���Ă���������
                                                
                                                // �j���ʂŃJ�E���g
                                                if( File.checkAvailable( omale.Original ) ) {
                                                    if( omale.Original.CompareTo( mmale.Modify ) != 0 ) {
                                                        if( File.checkAvailable( ofemale.Original ) ) {
                                                            ++count;
                                                            if( ofemale.Original.CompareTo( mfemale.Modify ) != 0 ) {
                                                                ++num;
                                                            }
                                                        } else {
                                                            //++num;
                                                        }
                                                    }
                                                }
                                                if( File.checkAvailable( ofemale.Original ) ) {
                                                    ++count;
                                                    if( ofemale.Original.CompareTo( mfemale.Modify ) != 0 ) {
                                                        ++num;
                                                    }
                                                } else {
                                                    //++num;
                                                }

                                            }

                                        }

                                    }
                                }
                                idnum = count;

#if false
                        if ( num < idnum ) {
                            System.Console.WriteLine( "\t" + modpath + ": " + num + " / " + idnum );
                        } else {
                            System.Console.WriteLine( modpath + ": " + num + " / " + idnum );
                        }
#endif

                            } else {
                                int count = 0;
                                for( int i = 0; i < idnum; ++i ) {
                                    string id = ori.IDs[ i ] as string;
                                    if( id != null ) {
                                        File.Chunk oc = ori.Chunks[ id ] as File.Chunk;
                                        if( oc != null ) {
                                            // ���̔�r�񐔂���肽���̂�
                                            if( ori.Type == File.FileType.MES ) {
                                                File.Node omes = oc.Message;
                                                if( File.checkAvailable( omes.Original ) ) {
                                                    ++count;
                                                }
                                            } else {
                                                File.Node omale = oc.MaleLine;
                                                File.Node ofemale = oc.FemaleLine;
                                                // ����邵�ǂ����������ɂ��悤���Ȃ� 0.5?

                                                // ��͂ǂ����邩��

                                                // ������x���O���Ȃ��Ƃ��ꂾ���A���̕ӂ͂��傤���Ȃ��̂��˂�
                                                // ����[E:]�݂����Ȃ̂́A�ʃt�@�C���Q�ƂȂ̂ŏ��O���Ă���������
                                                // �j���ʂŃJ�E���g
                                                if( File.checkAvailable( omale.Original ) ) {
                                                    ++count;
                                                }
                                                if( File.checkAvailable( ofemale.Original ) ) {
                                                    ++count;
                                                }
#if false
                                                if( File.checkAvailable( omale.Original ) || File.checkAvailable( ofemale.Original ) ) {
                                                    ++count;
                                                }
#endif
                                            }
                                        }
                                    }
                                }
                                idnum = count;

                            }

                            progress.Add( new ProgressCount( file, num, idnum, exist ) );
                        } else {
                            //System.Console.WriteLine( "invalid format:" + file );
                        }

                        incrementProgressBar();

                    }
                }
            }

        }


        void runEditor( string path ) {
            if ( System.IO.File.Exists( path ) ) {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = Path.Tools.NotepadFile;
                if ( config.TextEditorPath != null && System.IO.File.Exists( config.TextEditorPath ) ) {
                    proc.StartInfo.FileName = config.TextEditorPath;
                }
                proc.StartInfo.WorkingDirectory = Path.ExecutableDirectory; // �G�f�B�^�ɂ���Ă͂܂�������
                proc.StartInfo.Arguments = "\"" + path + "\"";
                proc.Start();
            }
        }
    }
}
