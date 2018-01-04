// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ArcanumJPEditor {
    public partial class DlgEditor : Editor {
        enum InfoType {
            IntCheck = 0,
            TestCodes,
            ResponceID,
            Result,
            Reference,
            Max,
        }
        // enum toString�ł��ނ���
        string[] infoname = { "Int", "Test", "ResponseID", "Result", "Reference" };
        class NodeInfo {
            // TreeNode�͂��Ȃ炸����Ƃ͌���Ȃ�
            //public System.Collections.Generic.List<KeyArguments> Int;
            public KeyArguments MaleLine; // GeneratedDialog
            public KeyArguments FemaleLine; // GeneratedDialog or Gender

            public KeyArguments Int;
            public System.Collections.Generic.List<KeyArguments> Test;
            public System.Collections.Generic.List<KeyArguments> Result;
            public KeyArguments ResponseID;

            public System.Collections.Generic.List<TreeNode> Reference = new System.Collections.Generic.List<TreeNode>();
        }
        System.Collections.Hashtable nodeinfo;

        private DlgEditor() {
            InitializeComponent();
        }
        public DlgEditor( string path ) {
            InitializeComponent();
            treeViewLine.HideSelection = false;
            this.Name = path;

            treeViewLine.ImageList = imageList1;

            open( path );

            // dlg tree
            TreeNode root = new TreeNode( FileName );
            root.ImageIndex = 3;
            root.SelectedImageIndex = 3;

            //int size = Mod.Chunks.Count;
            int size = Mod.IDs.Count;
            // node��list���ƃZ�b�g�֌W��������Ȃ�����node��chunk��list������̂��Ȃ�
            if ( Mod.Type == File.FileType.DLG ) {
                int index = 0;
                while ( index < size ) {
                    //File.Chunk chunk = Mod.Chunks[ index ] as File.Chunk;
                    File.Chunk chunk = Mod.Chunks[ Mod.IDs[ index ] ] as File.Chunk;
                    File.Node id = chunk.ID;
                    File.Node male = chunk.MaleLine;
                    File.Node female = chunk.FemaleLine;

                    // ��͕ҏW�ł��Ȃ��悤�ɂ���
                    // �̂����l�q����
                    if ( male.Original != null && male.Original.Length == 0 &&
                        female.Original != null && female.Original.Length == 0 ) {
                        ++index;
                        continue;
                    }

                    TreeNode npc = new TreeNode( id.Original );
                    npc.Text = chunk.NodeText;

                    npc.Tag = chunk;


                    npc.ImageIndex = npc.SelectedImageIndex = getNodeImageIndex( id.Original );

                    
                    //bool ispc = true;
                    ++index;
                    //while ( index < size && ispc ) {
                    while ( index < size ) {
                        //File.Chunk child = Mod.Chunks[ index ] as File.Chunk;
                        File.Chunk child = Mod.Chunks[ Mod.IDs[ index ] ] as File.Chunk;

                        // PC��b�������
                        // int check�̗L�����ˁH
                        File.Node res = child.IntCheck;
                        if ( res.Original.Length == 0 ) {
                            //rollback
                            //--index;
                            break;
                        }

                        File.Node pcid = child.ID;
                        File.Node pcmale = child.MaleLine;
                        File.Node pcfemale = child.FemaleLine;

                        // ��͕ҏW�ł��Ȃ��悤�ɂ���
                        // �̂����l�q����
                        //if ( male.Original != null && male.Mod.Length > 0 &&
                        //    female.Original != null && female.Mod.Length > 0 ) {
                        TreeNode pc = new TreeNode( pcid.Original );
                        pc.Text = child.NodeText;

                        pc.Tag = child;

                        pc.ImageIndex = pc.SelectedImageIndex = getNodeImageIndex( pcid.Original );

                        
                        npc.Nodes.Add( pc );
                        //}

                        ++index;

                    }
                    root.Nodes.Add( npc );
                    //} else {
                    //    ++index;
                    //}

                }

                root.Expand();
                treeViewLine.Nodes.Add( root );

                // �t����
                nodeinfo = new System.Collections.Hashtable( size );
                foreach ( TreeNode node in root.Nodes ) {
                    // PC��b�͋t���������񂶂�Ȃ����H
                    // �����I�ɂ���ȊO���g�����邱�Ƃ����z���Ă������c
                    File.Chunk chunk = node.Tag as File.Chunk;
                    if ( chunk != null ) {
                        NodeInfo ni = new NodeInfo();

                        // ��Ƀp�[�X
                        if( chunk.MaleLine != null ) {
                            KeyArguments ka = Parser.getGeneratedDialog( chunk.MaleLine.Original );
                            if( ka != null ) {
                                ni.MaleLine = ka;
                            }
                        }
                        if( chunk.FemaleLine != null ) {
                            KeyArguments ka = Parser.getGeneratedDialog( chunk.MaleLine.Original );
                            if( ka != null ) {
                                ni.FemaleLine = ka;
                            }
                        }
                        if( chunk.IntCheck != null ) {
                            List<KeyArguments> ka = Parser.getKeyArguments( chunk.IntCheck.Original );
                            if ( ka != null && ka.Count > 0 ) {
                                ni.Int = ka[ 0 ];
                            }
                        }
                        if ( chunk.TestCodes != null ) {
                            List<KeyArguments> ka = Parser.getKeyArguments( chunk.TestCodes.Original );
                            if ( ka != null && ka.Count > 0 ) {
                                ni.Test = ka;
                            }
                        }
                        if ( chunk.ResponseID != null ) {
                            List<KeyArguments> ka = Parser.getKeyArguments( chunk.ResponseID.Original );
                            if ( ka != null && ka.Count > 0 ) {
                                ni.ResponseID = ka[ 0 ];
                            }
                        }
                        if ( chunk.Result != null ) {
                            List<KeyArguments> ka = Parser.getKeyArguments( chunk.Result.Original );
                            if ( ka != null && ka.Count > 0 ) {
                                ni.Result = ka;
                            }
                        }

                        File.Node id = chunk.ID;
                        if ( id != null ) { // ��������id�Ȃ��Ɛ��藧���Ȃ�
                            nodeinfo.Add( id.Original, ni );
                        }
                    }
                    foreach ( TreeNode child in node.Nodes ) {
                        chunk = child.Tag as File.Chunk;
                        if ( chunk != null ) {
                            NodeInfo ni = new NodeInfo();

                            // ��Ƀp�[�X
                            if( chunk.MaleLine != null ) {
                                KeyArguments ka = Parser.getGeneratedDialog( chunk.MaleLine.Original );
                                if( ka != null ) {
                                    ni.MaleLine = ka;
                                }
                            }
                            if( chunk.FemaleLine != null ) {
                                KeyArguments ka = Parser.getGeneratedDialog( chunk.MaleLine.Original );
                                if( ka != null ) {
                                    ni.FemaleLine = ka;
                                }
                            }
                            if( chunk.IntCheck != null ) {
                                List<KeyArguments> ka = Parser.getKeyArguments( chunk.IntCheck.Original );
                                if ( ka != null && ka.Count > 0 ) {
                                    ni.Int = ka[ 0 ];
                                }
                            }
                            if ( chunk.TestCodes != null ) {
                                List<KeyArguments> ka = Parser.getKeyArguments( chunk.TestCodes.Original );
                                if ( ka != null && ka.Count > 0 ) {
                                    ni.Test = ka;
                                }
                            }
                            if ( chunk.ResponseID != null ) {
                                List<KeyArguments> ka = Parser.getKeyArguments( chunk.ResponseID.Original );
                                if ( ka != null && ka.Count > 0 ) {
                                    ni.ResponseID = ka[ 0 ];
                                }
                            }
                            if ( chunk.Result != null ) {
                                List<KeyArguments> ka = Parser.getKeyArguments( chunk.Result.Original );
                                if ( ka != null && ka.Count > 0 ) {
                                    ni.Result = ka;
                                }
                            }

                            File.Node id = chunk.ID;
                            if ( id != null ) { // ��������id�Ȃ��Ɛ��藧���Ȃ�
                                nodeinfo.Add( id.Original, ni );
                            }
                        }
                    }
                }
                foreach ( TreeNode node in root.Nodes ) {
                    // PC��b�͋t���������񂶂�Ȃ����H
                    // �����I�ɂ���ȊO���g�����邱�Ƃ����z���Ă������c
                    File.Chunk chunk = node.Tag as File.Chunk;
                    if ( chunk != null ) {
                        File.Node id = chunk.ID;
                        NodeInfo ni = nodeinfo[ id.Original ] as NodeInfo;
                        foreach ( TreeNode n in root.Nodes ) {
                            searchReferenceID( id.Original, n, ref ni.Reference );
                        }

                    }
                    foreach ( TreeNode child in node.Nodes ) {
                        chunk = child.Tag as File.Chunk;
                        if ( chunk != null ) {
                            File.Node id = chunk.ID;
                            NodeInfo ni = nodeinfo[ id.Original ] as NodeInfo;
                            foreach ( TreeNode n in root.Nodes ) {
                                searchReferenceID( id.Original, n, ref ni.Reference );
                            }
                            
                        }
                    }
                }
#if false
                //put
                foreach ( TreeNode node in root.Nodes ) {
                    File.Chunk chunk = node.Tag as File.Chunk;
                    if ( chunk != null ) {
                        File.Node id = chunk.ID;
                        if ( nodeinfo.Contains( id.Original ) ) {
                            System.Console.WriteLine( "NPC: " + id.Original );
                            NodeInfo info = nodeinfo[ id.Original ] as NodeInfo;
                            foreach ( TreeNode refnode in info.Reference ) {
                                System.Console.WriteLine( "  " +  refnode.Text );
                            }
                        }
                    }
                    foreach ( TreeNode child in node.Nodes ) {
                        chunk = child.Tag as File.Chunk;
                        if ( chunk != null ) {
                            File.Node id = chunk.ID;
                            if ( nodeinfo.Contains( id.Original ) ) {
                                System.Console.WriteLine( "PC: " + id.Original );
                                NodeInfo info = nodeinfo[ id.Original ] as NodeInfo;
                                foreach ( TreeNode refnode in info.Reference ) {
                                    System.Console.WriteLine( "  " + refnode.Text );
                                }
                            }
                        }
                    }
                }
#endif

            }

            // �ҏW���j���[�n�͂������̃c�[���o�[�̕����y�Ȃ񂾂�Ȃ��B�ڂ�����

            // index����̔��f�ł����Ⴒ����ɂȂ肪���Ȃ̂ł�������܂��傤
            tabPageMale.Tag = textBoxModifyM;
            tabPageFemale.Tag = textBoxModifyF;

            TreeNode[] infos = new TreeNode[ infoname.Length ];
            for ( uint i = 0; i < infoname.Length; ++i ) {
                infos[ i ] = new TreeNode( infoname[ i ] );                
            }
            treeViewInfo.Nodes.AddRange( infos );
            treeViewInfo.MouseDown += new MouseEventHandler( treeViewInfo_MouseDown );
            treeViewInfo.BeforeExpand += new TreeViewCancelEventHandler( treeViewInfo_BeforeExpand );
            treeViewInfo.BeforeCollapse += new TreeViewCancelEventHandler( treeViewInfo_BeforeCollapse );
            treeViewInfo.DoubleClick += new EventHandler( treeViewInfo_DoubleClick );
#if false
            string[] types = { "Int", "Test", "Result", "Response", "Reference" };
            DataGridViewRow[] rows = new DataGridViewRow[ types.Length ];
            for ( int i = 0; i < rows.Length; ++i ) {
                rows[ i ] = new DataGridViewRow();
                DataGridViewRow row = rows[ i ];
                row.CreateCells( dataGridViewInfo );
                row.Cells[ 0 ].Value = types[ i ];
            }
            dataGridViewInfo.Rows.AddRange( rows );
            dataGridViewInfo.CurrentCell = null; // �`��J�n��
            dataGridViewInfo.CellDoubleClick += new DataGridViewCellEventHandler( dataGridViewInfo_CellDoubleClick );
            // datasource�œǂ܂��Ă邩virtualmode true 7�łȂ��Ǝg���Ȃ�
            //dataGridViewInfo.RowContextMenuStripNeeded += new DataGridViewRowContextMenuStripNeededEventHandler( dataGridViewInfo_RowContextMenuStripNeeded );
            //dataGridViewInfo.CellContextMenuStripNeeded += new DataGridViewCellContextMenuStripNeededEventHandler( dataGridViewInfo_CellContextMenuStripNeeded );
#endif
            treeViewLine.AfterSelect += new TreeViewEventHandler( treeViewLine_AfterSelect );
            treeViewLine.MouseDown += new MouseEventHandler( treeViewLine_MouseDown );

            textBoxModifyM.TextChanged += new EventHandler( textBoxModifyM_TextChanged );
            textBoxModifyF.TextChanged += new EventHandler( textBoxModifyF_TextChanged );

            tabControlLine.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlLine.DrawItem += new DrawItemEventHandler( tabControlLine_DrawItem );

            toolStripButtonNext.Click += new EventHandler( Next_Click );
            toolStripButtonRestore.Click += new EventHandler( toolStripButtonRestore_Click );

            toolStripButtonToMale.Click += new EventHandler( ToMale_Click );
            toolStripButtonToFemale.Click += new EventHandler( ToFemale_Click );

            toolStripButtonSpeechPlay.Click += new EventHandler( SpeechPlay_Click );
            toolStripButtonSpeechPlay.Enabled = false;
            toolStripButtonSpeechStop.Click += new EventHandler( SpeechStop_Click );


            textBoxOriginalM.ContextMenu = createMenuOriginal();
            textBoxModifyM.ContextMenu = createMenuModify();
            textBoxOriginalF.ContextMenu = createMenuOriginal();
            textBoxModifyF.ContextMenu = createMenuModify();


            MenuItem[] items = new MenuItem[ 4 ];
            items[ 0 ] = new MenuItem( "�W�J(&E)", Expand_Click );
            items[ 1 ] = new MenuItem( "�S�W�J(&X)", ExpandAll_Click );
            items[ 2 ] = new MenuItem( "�܂���(&C)", Collapse_Click );
            items[ 3 ] = new MenuItem( "�S�܂���(&O)", CollapseAll_Click );
            treeViewLine.ContextMenu = new ContextMenu( items );

        }

        void treeViewInfo_DoubleClick( object sender, EventArgs e ) {
            // �W�����v�\�Ȃ���s����
            TreeView tree = sender as TreeView;
            if ( tree != null && tree.SelectedNode != null && tree.SelectedNode.Tag != null ) {
                TreeNode node = tree.SelectedNode.Tag as TreeNode;
                if ( node != null ) {
                    treeViewLine.SelectedNode = node;
                    // �t�H�[�J�X�ǂ����Ɉڂ�
                    treeViewLine.Focus();
                }
            }
        }

        bool doubleclick = false;
        void treeViewInfo_BeforeCollapse( object sender, TreeViewCancelEventArgs e ) {
            TreeView tree = sender as TreeView;
            if ( tree != null && e.Node != null ) {
                // root node�̏ꍇ�͒ʏ퓮��A����ȊO�͓W�J�����Ń_�u���N���b�N�D��
                foreach ( TreeNode root in tree.Nodes ) {
                    if ( root.Equals( e.Node ) ) {
                        doubleclick = false;
                        break;
                    }
                }
                if ( doubleclick ) {
                    e.Cancel = true;
                }
            }
            doubleclick = false;
        }

        void treeViewInfo_BeforeExpand( object sender, TreeViewCancelEventArgs e ) {
            TreeView tree = sender as TreeView;
            if ( tree != null && e.Node != null ) {
                // root node�̏ꍇ�͒ʏ퓮��A����ȊO�͓W�J�����Ń_�u���N���b�N�D��
                foreach ( TreeNode root in tree.Nodes ) {
                    if ( root.Equals( e.Node ) ) {
                        doubleclick = false;
                        break;
                    }
                }
                if ( doubleclick ) {
                    e.Cancel = true;
                }
            }
            doubleclick = false;
        }

        void treeViewInfo_MouseDown( object sender, MouseEventArgs e ) {
            // ���E�ǂ���ł������͓����ɂ��悤 ���N���b�N�͖���
            if ( e.Button == MouseButtons.Left || e.Button == MouseButtons.Right ) {
                TreeView tree = sender as TreeView;
                if ( tree != null ) {
                    Point p = tree.PointToClient( Cursor.Position );
                    TreeViewHitTestInfo info = tree.HitTest( p );
                    if ( info.Node != null ) {
                        tree.SelectedNode = info.Node;
                    }
                    // double click flag
                    if ( e.Button == MouseButtons.Left && e.Clicks == 2 ) {
                        doubleclick = true;
                    }
                }
            }
        }

        void Expand_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewLine.SelectedNode;
            if ( node != null ) {
                node.Expand();
            }
        }

        void ExpandAll_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewLine.SelectedNode;
            if ( node != null ) {
                node.ExpandAll();
            }
        }

        void Collapse_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewLine.SelectedNode;
            if ( node != null ) {
                node.Collapse( true );
            }
        }

        void CollapseAll_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewLine.SelectedNode;
            if ( node != null ) {
                node.Collapse( false );
            }
        }

        public override void ToMale_Click( object sender, EventArgs e ) {
            if ( textBoxModifyM.ReadOnly == false && textBoxModifyF.ReadOnly == false ) {
                if ( textBoxModifyF.Text != null && textBoxModifyF.Text.Length > 0 ) {
                    // ����������o�Ȃ��ꍇ�͊m�F�_�C�A���O������
                    if ( textBoxOriginalM.Text != textBoxOriginalF.Text ) {
                    //if ( textBoxOriginalM.Text.CompareTo( textBoxOriginalF.Text) == 0 ) {
                        DialogResult result = MessageBox.Show( this, @"�������قȂ�܂����A�R�s�[���܂����H", @"Female Line��Male Line�ɃR�s�[", MessageBoxButtons.YesNo );
                        if ( result == DialogResult.No ) {
                            return;
                        }
                    }
                    textBoxModifyM.Text = textBoxModifyF.Text.Clone() as string;
                    MainForm.setStatus( @"Female Line��Male Line�ɃR�s�[" );
                }
            }
        }
        public override void ToFemale_Click( object sender, EventArgs e ) {
            if ( textBoxModifyM.ReadOnly == false && textBoxModifyF.ReadOnly == false ) {
                if ( textBoxModifyM.Text != null && textBoxModifyM.Text.Length > 0 ) {
                    if ( textBoxOriginalM.Text != textBoxOriginalF.Text ) {
                    //if ( textBoxOriginalM.Text.CompareTo( textBoxOriginalF.Text ) == 0 ) {
                        DialogResult result = MessageBox.Show( this, @"�������قȂ�܂����A�R�s�[���܂����H", @"Male Line��Female Line�ɃR�s�[", MessageBoxButtons.YesNo );
                        if ( result == DialogResult.No ) {
                            return;
                        }
                    }
                    textBoxModifyF.Text = textBoxModifyM.Text.Clone() as string;
                    MainForm.setStatus( @"Male Line��Female Line�ɃR�s�[" );
                }
            }
        }

        public override void SpeechPlay_Click( object sender, EventArgs e ) {
            // �����Đ�
            TreeNode node = treeViewLine.SelectedNode;
            if( node != null ) {
                File.Chunk chunk = node.Tag as File.Chunk;
                if( chunk != null ) {
                    File.Node id = chunk.ID;
                    NodeInfo info = null;
                    if( nodeinfo.Contains( id.Original ) ) {
                        info = nodeinfo[ id.Original ] as NodeInfo;
                        if( info != null && info.Test != null && info.Test.Count == 1 ) {
                            KeyArguments ka = info.Test[ 0 ];
                            if( ( ka.Key == null ) || ( ka.Key != null && ka.Key.Length == 0 ) ) {
                                if( ka.Args.Count == 1 ) {
                                    bool female = ( tabControlLine.SelectedIndex == 1 ) ? true : false;
                                    string speech_index = ka.Args[ 0 ];
                                    speech_index = Parser.removeSign( speech_index );
                                    string path = SpeechPlayer.exists( OriginalPath, speech_index, female );
                                    if( path != null ) {
                                        bool ret = SpeechPlayer.play( this, path );
                                        // �A�C�R���ς����Ȃ����ȁH
                                        // �������ς����ꍇ�́A�R�[���o�b�N�ŏI���Ƃ��ĐF�X����Ƃ�����̂ł߂�ǂ�
                                    } else {
                                        // enable=false�Ȃ�łȂ�������Ȃ񂩂���
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void SpeechStop_Click( object sender, EventArgs e ) {
            SpeechPlayer.stop( this );
        }

        void toolStripButtonRestore_Click( object sender, EventArgs e ) {
            TabPage tab = tabControlLine.SelectedTab;
            if ( tab != null ) {
                TextBox box = tab.Tag as TextBox;
                if ( box != null && box.ReadOnly == false ) {
                    TreeNode node = box.Tag as TreeNode;
                    if ( node != null ) {
                        File.Chunk chunk = node.Tag as File.Chunk;
                        File.Node text = null;
                        if ( box.Equals( textBoxModifyM ) ) {
                            text = chunk.MaleLine;
                        } else if ( box.Equals( textBoxModifyF ) ) {
                            text = chunk.FemaleLine;
                        }
                        if ( text != null ) {
                            text.Modify = text.Original.Clone() as string;
                            box.Text = text.Modify;

                            MainForm.setStatus( @"�ҏW�O�ɖ߂�" );

                        }
                    }
                }
            }

        }

        // M F��1�̊֐��ł�肽�����c
        void textBoxModifyM_TextChanged( object sender, EventArgs e ) {
            TextBox box = sender as TextBox;
            if ( box != null ) {
                TreeNode node = box.Tag as TreeNode;
                if ( node != null ) {
                    File.Chunk chunk = node.Tag as File.Chunk;
                    File.Node id = chunk.ID;
                    File.Node male = chunk.MaleLine;

                    history.TextChange( chunk.ID.Original + "M", textBoxModifyM );

                    male.Modify = box.Text; // �����ŕK�v�ȏ�ɏ����߂����̂����X�C�ɂȂ邪�E�E�E
                    node.Text = chunk.NodeText;

                    node.ImageIndex = node.SelectedImageIndex = getNodeImageIndex( id.Original );
                }
            }
        }

        void textBoxModifyF_TextChanged( object sender, EventArgs e ) {
            TextBox box = sender as TextBox;
            if ( box != null ) {
                TreeNode node = box.Tag as TreeNode;
                if ( node != null ) {
                    File.Chunk chunk = node.Tag as File.Chunk;
                    File.Node id = chunk.ID;
                    File.Node female = chunk.FemaleLine;

                    history.TextChange( chunk.ID.Original + "F", textBoxModifyF );

                    female.Modify = box.Text; // �����ŕK�v�ȏ�ɏ����߂����̂����X�C�ɂȂ邪�E�E�E
                    node.Text = chunk.NodeText;

                    node.ImageIndex = node.SelectedImageIndex = getNodeImageIndex( id.Original );

                }
            }
        }
        public override void Next_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewLine.SelectedNode; // box.tag����Ƃ邩�Ȃ�? text�Ƃ������Ăǂ����ł�������
            if ( node != null ) {
                File.Chunk chunk = node.Tag as File.Chunk;
                if ( chunk != null ) {
                    string next = chunk.ResponseID.Original;
                    next = Parser.removeSign( next ); // ��Βl
                    // ����0�Ȃǂ̓���R�[�h�ł͂Ȃ�
                    if( next != null && next.Length > 0 ) {
                        // search next
                        // Mod����Node����Ă���TreeNode���t�����ł���Ίy�Ȃ񂾂� ����܂葊�ݎQ�Ƃ���̂��˂�
                        TreeNode much = searchID( next );
                        if( much != null ) {
                            much.Expand(); // ����ʂ���������
                            treeViewLine.SelectedNode = much;
                            //treeViewLine.Focus();
                        }
                    } else {
                        TreeNode much = null;
                        if( node.Nodes.Count > 0 ) {
                            much = node.Nodes[ 0 ];
                        } else if( node.NextNode != null ) {
                            much = node.NextNode;
                        }
                        if( much != null ) {
                            much.Expand(); // ����ʂ���������
                            treeViewLine.SelectedNode = much;
                            //treeViewLine.Focus();
                        }
                    }
                }
            }
        }
        // int�ɕϊ�����̂Ƃǂ��炪�����̂�� ������5�����A���2�������x���낤����string�̕����������ȁH
        TreeNode searchID( string id ) {
            TreeNode ret = null;
            foreach ( TreeNode node in treeViewLine.Nodes ) {
                ret = searchID( id, node );
                if ( ret != null ) {
                    break;
                }
            }
            return ret;
        }

        TreeNode searchID( string id, TreeNode node ) {

            File.Chunk chunk = node.Tag as File.Chunk;
            if ( chunk != null ) {
                File.Node ID = chunk.ID;
                // ���肦�Ȃ����ꉞ
                if ( ID.Original != null && ID.Original.Length > 0 ) {
                    if ( ID.Original == id ) {
                        return node;
                    }
                }
            }
            TreeNode ret = null;
            foreach ( TreeNode n in node.Nodes ) {
                ret = searchID( id, n );
                if ( ret != null ) {
                    break;
                }
            }
            return ret;
        }


        // �t����ID
        void searchReferenceID( string id, TreeNode node, ref System.Collections.Generic.List<TreeNode> list ) {

            File.Chunk chunk = node.Tag as File.Chunk;
            if ( chunk != null ) {
                // responce id
                File.Node res = chunk.ResponseID;
                string resid = res.Original;
                // ���肦�Ȃ����ꉞ
                if( resid != null && resid.Length > 0 ) {
                    if( resid == id ) {
                        list.Add( node );
                    }
                }
                // result
                File.Node i = chunk.ID;
                NodeInfo info = nodeinfo[ i.Original ] as NodeInfo;
                if ( info != null && info.Result != null ) {
                    foreach ( KeyArguments ka in info.Result ) {
                        string ret = Parser.isReference( ka );
                        if ( ret != null && ret.Length > 0 ) {
                            if ( ret == id ) {
                                list.Add( node );
                            }
                        }
                    }
                }
            }
            foreach ( TreeNode n in node.Nodes ) {
                searchReferenceID( id, n, ref list );
            }
        }


        void tabControlLine_DrawItem( object sender, DrawItemEventArgs e ) {
            TabControl tab = sender as TabControl;
            if ( tab != null ) {
                string txt = tab.TabPages[ e.Index ].Text;

                //�^�u�̃e�L�X�g�Ɣw�i��`�悷�邽�߂̃u���V�����肷��
                Brush foreBrush = System.Drawing.SystemBrushes.ControlText;
                switch ( e.Index ) {
                    case 0:
                        // afterselect������炵���̂�readonly�t���O�Ń`�F�b�N����
                        if ( textBoxModifyM.ReadOnly ) {
                            foreBrush = System.Drawing.SystemBrushes.GrayText;
                        }
                        break;
                    case 1:
                        if ( textBoxModifyF.ReadOnly ) {
                            foreBrush = System.Drawing.SystemBrushes.GrayText;
                        }
                        break;
                    default:
                        break;
                }

                //StringFormat���쐬
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                //�w�i�̕`��
                //e.Graphics.FillRectangle( backBrush, e.Bounds );
                //Text�̕`��
                e.Graphics.DrawString( txt, e.Font, foreBrush, e.Bounds, sf );
            }
        }
        void treeViewLine_AfterSelect( object sender, TreeViewEventArgs e ) {
            if ( e.Node != null ) {
                File.Chunk chunk = e.Node.Tag as File.Chunk;
                if ( chunk != null ) {
                    textBoxModifyM.Tag = e.Node; // ��������܂�����
                    textBoxModifyF.Tag = e.Node; // ��������܂�����

                    File.Node id = chunk.ID;
                    File.Chunk origin = Original.Chunks[ id.Original ] as File.Chunk;
                    File.Node omale = origin.MaleLine;
                    File.Node ofemale = origin.FemaleLine;
                    File.Node male = chunk.MaleLine;
                    File.Node female = chunk.FemaleLine;
                    File.Node ic = chunk.IntCheck;
                    File.Node test = chunk.TestCodes;
                    File.Node res = chunk.ResponseID;
                    File.Node result = chunk.Result;

                    history.NodeChange( chunk.ID.Original + "M" );
                    history.NodeChange( chunk.ID.Original + "F" );

                    groupBoxID.Text = id.Original;

                    MainForm.setTitle( " - " + FileName + " {" + id.Original + "}" );

                    textBoxOriginalM.Text = omale.Original;
                    textBoxModifyM.Text = male.Modify;
                    textBoxOriginalF.Text = ofemale.Original;
                    textBoxModifyF.Text = female.Modify;

                    // ���ۂ̒l�̓p�[�T�ɂ����ēǂݍ��܂Ȃ���
                    // �W�J��̕ێ��A����S�W�J�ł������̂��Ȃ�
                    NodeInfo info = null;
                    if ( nodeinfo.Contains( id.Original ) ) {
                        info = nodeinfo[ id.Original ] as NodeInfo ;
                    }


                    // param
                    for ( int i = 0; i < treeViewInfo.Nodes.Count; ++i ) {
                        TreeNode root = treeViewInfo.Nodes[ i ];
                        bool expand = root.IsExpanded;
                        if ( root.Nodes.Count == 0 ) {
                            expand = true;
                        }
                        TreeNode[] nodes = null;
                        switch ( (InfoType)i ) {
                        case InfoType.IntCheck: {
#if false
                                string text = Parser.getValue( ic );
                                if ( text.Length > 0 ) {
                                    nodes = new TreeNode[ 1 ];
                                    nodes[ 0 ] = new TreeNode( text );
                                }
#endif
                                if ( info != null && info.Int != null ) {
                                    string refid = null;
                                    string text = Parser.getValue( ic, info.Int, out refid );
                                    if ( text != null && text.Length > 0 ) {
                                        nodes = new TreeNode[ 1 ];
                                        nodes[ 0 ] = new TreeNode( text );
                                    }
                                }
                            }
                            break;
                        case InfoType.TestCodes: {
#if false
                                string text = Parser.getValue( test );
                                if ( text.Length > 0 ) {
                                    nodes = new TreeNode[ 1 ]; // ��������\����
                                    nodes[ 0 ] = new TreeNode( text );
                                }
#endif
                                if ( info != null && info.Test != null ) {
                                    //1��key�󂪂���
                                    nodes = new TreeNode[ info.Test.Count ]; // �����Ȃ̂��Ȃ���΂悢��
                                    int index = 0;
                                    foreach ( KeyArguments ka in info.Test ) {
                                        string refid = null;
                                        string text = Parser.getValue( test, ka, out refid );
                                        if ( text != null && text.Length > 0 ) {
                                            nodes[ index ] = new TreeNode( text );
                                            if ( refid != null ) {
                                                // ���p�����Ԃ炳����
                                                TreeNode much = searchID( refid );
                                                if ( much != null ) {
                                                    nodes[ index ].Tag = much;
                                                    TreeNode child = new TreeNode( much.Text );
                                                    child.Tag = much;
                                                    nodes[ index ].Nodes.Add( child );
                                                }
                                            }
                                        } else {
                                            // ��͂ł��Ȃ������c ���������Ƃ��悭�������̂���ɂ���
                                            // ��ł����ɂ���\�������邩�H
                                            // FIXME ��ɗL�������J�E���g���Ċm�ۂ�����
                                            nodes[ index ] = new TreeNode( test.Original );
                                        }
                                        ++index;
                                    }
                                }

                            }
                            break;
                        case InfoType.ResponceID: {
                                if ( info != null && info.ResponseID != null ) {
                                    string refid = null;
                                    string text = Parser.getValue( res, info.ResponseID, out refid );
                                    if ( text != null && text.Length > 0 ) {
                                        nodes = new TreeNode[ 1 ];
                                        nodes[ 0 ] = new TreeNode( text );
                                        if ( refid != null ) {
                                            TreeNode much = searchID( refid );
                                            if ( much != null ) {
                                                nodes[ 0 ].Tag = much;
                                            }
                                        }
                                    }
                                }

                            }
                            break;
                        case InfoType.Result: {
#if false
                                string text = Parser.getValue( result );
                                if ( text.Length > 0 ) {
                                    nodes = new TreeNode[ 1 ]; // ��������\����
                                    nodes[ 0 ] = new TreeNode( text );
                                }
#endif
                                if ( info != null && info.Result != null ) {
                                    //1��key�󂪂���
                                    nodes = new TreeNode[ info.Result.Count ]; // �����Ȃ̂��Ȃ���΂悢��
                                    int index = 0;
                                    foreach ( KeyArguments ka in info.Result ) {
                                        string refid = null;
                                        string text = Parser.getValue( result, ka, out refid );
                                        if ( text != null && text.Length > 0 ) {
                                            nodes[ index ] = new TreeNode( text );
                                            if ( refid != null ) {
                                                TreeNode much = searchID( refid );
                                                if ( much != null ) {
                                                    nodes[ index ].Tag = much;
                                                    // ���p�����Ԃ炳����
                                                    TreeNode child = new TreeNode( much.Text );
                                                    child.Tag = much;
                                                    nodes[ index ].Nodes.Add( child );
                                                    nodes[ index ].Expand();
                                                }
                                            }
                                        } else {
                                            // ��͂ł��Ȃ������c ���������Ƃ��悭�������̂���ɂ���
                                            // ��ł����ɂ���\�������邩�H
                                            // FIXME ��ɗL�������J�E���g���Ċm�ۂ�����
                                            nodes[ index ] = new TreeNode( result.Original );
                                        }
                                        ++index;
                                    }
                                }
                            }
                            break;
                        case InfoType.Reference:
                            if ( info != null ) {
                                nodes = new TreeNode[ info.Reference.Count ];
                                for ( int j = 0; j < nodes.Length; ++j ) {
                                    string text = info.Reference[ j ].Text;
                                    nodes[ j ] = new TreeNode( text );
                                    nodes[ j ].Tag = info.Reference[ j ];
                                }
                            }
                            break;
                        default:
                            break;
                        }
                        root.Nodes.Clear();
                        if( nodes != null ) {
                            root.Nodes.AddRange( nodes );
                        }
                        if ( expand ) {
                            root.Expand();
                        }

                    }

#if false
                    dataGridViewInfo.Rows[ 0 ].Cells[ 1 ].Value = Parser.getValue( ic );
                    dataGridViewInfo.Rows[ 1 ].Cells[ 1 ].Value = Parser.getValue( test );
                    dataGridViewInfo.Rows[ 2 ].Cells[ 1 ].Value = Parser.getValue( result );
                    dataGridViewInfo.Rows[ 3 ].Cells[ 1 ].Value = Parser.getValue( res );
                    if ( nodeinfo.Contains( id.Original ) ) {

                        string value = "";
                        NodeInfo info = nodeinfo[ id.Original ] as NodeInfo;
                        if ( info != null ) {
                            foreach ( TreeNode refnode in info.Reference ) {
                                value += refnode.Text + "\n";
                            }
                        }
                        dataGridViewInfo.Rows[ 4 ].Cells[ 1 ].Value = value;
                        // 1��grid���Ƒ����ƃX�N���[����������

                    } else {
                        dataGridViewInfo.Rows[ 4 ].Cells[ 1 ].Value = null;
                    }
#endif
                    //dataGridViewInfo.Rows[ 4 ].Cells[ 1 ].Value = 
                    // �t�������ʂ̒ǉ�tag������ɔz��œ����ƃW�����v���Ɋy����
                    // datagridview���ƃX�N���[�����ɂ������疳������
                    // �R���e�L�X�g���j���[�̒ǉ�

                    // original���ċ�Ȃ�tab��
                    // editable�ɋL�^���Ă����Ă�����������
                    // �C�����Ȃ��Ƃ܂��G���o�O���邪
                    // original ��original�������̂����A���Ȃ��̂łЂƂ܂�mod����
                    // original����Ƃ�悤�ɂ�����
                    // ����Ō��������ċ�ŕۑ������ꍇ�ɕҏW�ł��Ȃ��Ȃ�͉̂���������
#if false // replace
                    bool enableM = false;
                    bool enableF = false;
                    if ( omale.Original != null && omale.Original.Length > 0 ) {
                        if ( omale.Original.Length > 1 ) {
                            if ( omale.Original[ 0 ] < 'A' || omale.Original[ 0 ] > 'Z' || omale.Original[ 1 ] != ':' ) {
                                enableM = true;
                            }
                        } else {
                            enableM = true;
                        }
                    }
                    if ( ofemale.Original != null && ofemale.Original.Length > 0 ) {
                        // 1:maleline��PC�j���䎌 0:maleline��PC�����䎌
                        if ( ofemale.Original.CompareTo( "0" ) != 0 && ofemale.Original.CompareTo( "1" ) != 0) {
                            if ( ofemale.Original.Length > 1 ) {
                                if ( ofemale.Original[ 0 ] < 'A' || ofemale.Original[ 0 ] > 'Z' || ofemale.Original[ 1 ] != ':' ) {
                                    enableF = true;
                                }
                            } else {
                                enableF = true;
                            }
                        }
                    }
#endif

                    // GeneratedDialog
                    if( info != null ) {
                        if( info.MaleLine != null ) {
                            string refid = null;
                            string ret = Parser.getValue( omale, info.MaleLine, out refid );
                            if( ret != null ) {
                                textBoxOriginalM.Text += "\r\n[" + ret + "]";
                            } else {
                                textBoxOriginalM.Text += "\r\n[" + "GeneratedDialog" + "]";
                            }
                        }
                        if( info.FemaleLine != null ) {
                            string refid = null;
                            string ret = Parser.getValue( ofemale, info.MaleLine, out refid );
                            if( ret != null ) {
                                textBoxOriginalF.Text += "\r\n[" + ret + "]";
                            } else {
                                textBoxOriginalF.Text += "\r\n[" + "GeneratedDialog" + "]";
                            }
                        }
                    } 
                    
                    
                    bool enableM = File.checkAvailable( omale.Original );
                    bool enableF = File.checkAvailable( ofemale.Original );

                    // �|��Ɍ���Ζ�����K�v�͂Ȃ��ȁB
                    // �����������肦�Ȃ����A�ϐ��̕�����󂳂ꂽ�瑦���ɔ��f����Ȃ��Ƒʖڂ�
                    // ���̑��̕]����getValue�ŕ����񐶐����Ă邩��ł��Ă���
                    // �Ȃ̂Ŏ��O�v�Z���K�v�����Ȃ�΂Ƃ������x��
                    // ���c���[��1���₵�Ă����ɂԂ牺����̂��������Ă��邪�c
#if false // pending 0.6
                    if ( !enableM ) {
                        string gen = Parser.getGeneratedDialog( omale.Original );
                        System.Console.WriteLine( "male:" + gen );
                    }
                    if ( !enableF ) {
                        string gen = Parser.getGeneratedDialog( ofemale.Original );
                        System.Console.WriteLine( "female:" + gen );
                    }
#endif

                    textBoxModifyM.ReadOnly = !enableM;
                    textBoxModifyF.ReadOnly = !enableF;

                    if ( enableM ) {
                        tabControlLine.SelectedIndex = 0;
                    } else if ( enableF ) {
                        tabControlLine.SelectedIndex = 1;
                    }

                    // ����~���蓮�Ȃ�A��ɗL�������������ꂶ�Ⴀ��������̂��ǂ����킩��񂩂�Ȃ���
                    bool enable_speech = false;
                    // test��1��key�Ȃ��Aarg����1�̏ꍇ�A����������
                    if( info != null && info.Test != null && info.Test.Count == 1 ) {
                        KeyArguments ka = info.Test[ 0 ];
                        if( ( ka.Key == null ) || ( ka.Key != null && ka.Key.Length == 0 ) ) {
                            if( ka.Args.Count == 1 ) {
                                string speech_index = ka.Args[ 0 ];
                                speech_index = Parser.removeSign( speech_index );
                                string speech = SpeechPlayer.exists( OriginalPath, speech_index, !enableM );
                                if( speech != null ) {
                                    enable_speech = true;
                                }
                            }
                        }
                    }
                    toolStripButtonSpeechPlay.Enabled = enable_speech;
                }
            }
        }

        // ���̃R���g���[���̃C�x���g�ɂ܂Ƃ߂�ƂȂ񂩖�肠��H
        void treeViewLine_MouseDown( object sender, MouseEventArgs e ) {
            // ���E�ǂ���ł������͓����ɂ��悤 ���I�͖���
            if ( e.Button == MouseButtons.Left || e.Button == MouseButtons.Right ) {
                TreeView tree = sender as TreeView;
                if ( tree != null ) {
                    Point p = tree.PointToClient( Cursor.Position );
                    TreeViewHitTestInfo info = tree.HitTest( p );
                    if ( info.Node != null ) {
                        tree.SelectedNode = info.Node;
                    }
                }
            }
        }

        public override void SelectAll_Click( object sender, EventArgs e ) {
            // original�̕����I���ł��Ȃ��ƃJ�[�\�����ǂ����邩�݂��
            TabPage tab = tabControlLine.SelectedTab;
            if ( tab != null ) {
                TextBox box = tab.Tag as TextBox;
                if ( box != null && box.Focused ) {
                    box.SelectAll();
                    MainForm.setStatus( @"�ҏW�������ׂđI��" );
                } else if ( textBoxOriginalM.Focused ) {
                    textBoxOriginalM.SelectAll();
                    MainForm.setStatus( @"���������ׂđI��" );
                } else if ( textBoxOriginalF.Focused ) {
                    textBoxOriginalF.SelectAll();
                    MainForm.setStatus( @"���������ׂđI��" );
                }
            }
        }
        public override void Cut_Click( object sender, EventArgs e ) {
            TabPage tab = tabControlLine.SelectedTab;
            if ( tab != null ) {
                TextBox box = tab.Tag as TextBox;
                if ( box != null && box.Focused && box.SelectionLength > 0 ) {
                    box.Cut();
                    MainForm.setStatus( @"�ҏW����؂���" + round( Clipboard.GetText(), 128 ) );
                } else if ( textBoxOriginalM.Focused && textBoxOriginalM.SelectionLength > 0 ) {
                    textBoxOriginalM.Copy(); // readonly�ł���
                    MainForm.setStatus( @"�������R�s�[" + round( Clipboard.GetText(), 128 ) );
                } else if ( textBoxOriginalF.Focused && textBoxOriginalF.SelectionLength > 0 ) {
                    textBoxOriginalF.Copy(); // readonly�ł���
                    MainForm.setStatus( @"�������R�s�[" + round( Clipboard.GetText(), 128 ) );
                }
            }
        }
        public override void Copy_Click( object sender, EventArgs e ) {
            TabPage tab = tabControlLine.SelectedTab;
            if ( tab != null ) {
                TextBox box = tab.Tag as TextBox;
                if ( box != null && box.Focused && box.SelectionLength > 0 ) {
                    box.Copy();
                    MainForm.setStatus( @"�ҏW�����R�s�[: " + round( Clipboard.GetText(), 128 ) );
                } else if ( textBoxOriginalM.Focused && textBoxOriginalM.SelectionLength > 0 ) {
                    textBoxOriginalM.Copy();
                    MainForm.setStatus( @"�������R�s�[: " + round( Clipboard.GetText(), 128 ) );
                } else if ( textBoxOriginalF.Focused && textBoxOriginalF.SelectionLength > 0 ) {
                    textBoxOriginalF.Copy();
                    MainForm.setStatus( @"�������R�s�[: " + round( Clipboard.GetText(), 128 ) );
                }
            }
        }
        public override void Paste_Click( object sender, EventArgs e ) {
            TabPage tab = tabControlLine.SelectedTab;
            if ( tab != null ) {
                TextBox box = tab.Tag as TextBox;
                if ( box != null && box.Focused ) {
                    box.Paste();
                    MainForm.setStatus( @"�y�[�X�g" );
                }
            }
        }


        public override void Undo_Click( object sender, EventArgs e ) {
            TabPage tab = tabControlLine.SelectedTab;
            if ( tab != null ) {
                TextBox box = tab.Tag as TextBox;
                if ( box != null ) {
                    TreeNode node = box.Tag as TreeNode;
                    if ( node != null ) {
                        File.Chunk chunk = chunk = node.Tag as File.Chunk;
                        string key = chunk.ID.Original;
                        int index = tabControlLine.SelectedIndex;

                        switch ( index ) {
                            case 0: // male
                                key += "M";
                                break;
                            case 1: //female
                                key += "F";
                                break;
                            default:
                                break;
                        }
                        history.Undo( key, box );
                        MainForm.setStatus( @"���ɖ߂�" );
                    }
                }
            }
        }
        public override void Redo_Click( object sender, EventArgs e ) {
            TabPage tab = tabControlLine.SelectedTab;
            if ( tab != null ) {
                TextBox box = tab.Tag as TextBox;
                if ( box != null ) {
                    TreeNode node = box.Tag as TreeNode;
                    if ( node != null ) {
                        File.Chunk chunk = chunk = node.Tag as File.Chunk;
                        string key = chunk.ID.Original;
                        int index = tabControlLine.SelectedIndex;

                        switch ( index ) {
                            case 0: // male
                                key += "M";
                                break;
                            case 1: //female
                                key += "F";
                                break;
                            default:
                                break;
                        }
                        history.Redo( key, box );
                        MainForm.setStatus( @"��蒼��" );
                    }
                }
            }
        }

        public override bool search( Search.Option option, TreeNodeUtill.GetNode getfunc ) {
            TreeNode select = treeViewLine.SelectedNode;
            if ( select == null ) {
                if ( treeViewLine.Nodes.Count > 0 ) {
                    // �����select�ɂ���Ƃ����͒��ׂĂ���Ȃ�
                    // �܂����̂Ƃ���͒��ׂ�K�v�͔����̂���
                    select = treeViewLine.Nodes[ 0 ];
                } else {
                    return false;
                }
            }
            TreeNode node = searchNext( select, null, option, getfunc );
            if ( node == null && option.repeat && select != treeViewLine.Nodes[ 0 ] ) {
                node = searchNext( treeViewLine.Nodes[ 0 ], select, option, getfunc );
            }
            if ( node != null ) {
                return true;
            }

            return false;
        }
        TreeNode searchNext( TreeNode node, TreeNode stop, Search.Option option, TreeNodeUtill.GetNode getfunc ) {
            TreeNode next = getfunc( node );

            while ( next != null && next != stop ) {
                File.Chunk chunk = next.Tag as File.Chunk;
                if ( option.origin ) {
                    chunk = Original.Chunks[ chunk.ID.Original ] as File.Chunk;
                }

                if ( option.line ) {
                    // tabselect�͎����H�哱�H
                    if ( compare( chunk.MaleLine.Modify, option ) ) {
                        treeViewLine.SelectedNode = next;
                        tabControlLine.SelectedIndex = 0;
                        if ( option.origin ) {
                            textBoxOriginalM.Focus();
                        } else {
                            textBoxModifyM.Focus();
                        }
                        return next;
                    }
                    if ( compare( chunk.FemaleLine.Modify, option ) ) {
                        treeViewLine.SelectedNode = next;
                        tabControlLine.SelectedIndex = 1;
                        if ( option.origin ) {
                            textBoxOriginalF.Focus();
                        } else {
                            textBoxModifyF.Focus();
                        }
                        return next;
                    }
                }
#if false
                // ���̕ӂ̓p�[�T�O�̃f�[�^�ł����̂��Ȃ�
                if ( option.integer ) {
                    if ( compare( chunk.IntCheck.Original, option ) ) {
                        // �t�H�[�J�X�Z�b�g
                        treeViewLine.SelectedNode = next;
                        dataGridViewInfo.Rows[ 0 ].Cells[ 1 ].Selected = true;
                        dataGridViewInfo.Focus();
                        return next;
                    }
                }
                if ( option.test ) {
                    if ( compare( chunk.TestCodes.Original, option ) ) {
                        // �t�H�[�J�X�Z�b�g
                        treeViewLine.SelectedNode = next;
                        dataGridViewInfo.Rows[ 1 ].Cells[ 1 ].Selected = true;
                        dataGridViewInfo.Focus();
                        return next;
                    }
                }
                if ( option.result ) {
                    if ( compare( chunk.Result.Original, option ) ) {
                        // �t�H�[�J�X�Z�b�g
                        treeViewLine.SelectedNode = next;
                        dataGridViewInfo.Rows[ 2 ].Cells[ 1 ].Selected = true;
                        dataGridViewInfo.Focus();
                        return next;
                    }
                }
                if ( option.resid ) {
                    if ( compare( chunk.ResponseID.Original, option ) ) {
                        // �t�H�[�J�X�Z�b�g
                        treeViewLine.SelectedNode = next;
                        dataGridViewInfo.Rows[ 3 ].Cells[ 1 ].Selected = true;
                        dataGridViewInfo.Focus();
                        return next;
                    }
                }
#endif
                if ( option.id ) {
                    if ( compare( chunk.ID.Original, option ) ) {
                        treeViewLine.SelectedNode = next;
                        treeViewLine.Focus();
                        return next;
                    }
                }
                next = getfunc( next );
            }
            return null;
        }

        bool compare( string text, Search.Option option ) {
            string a = text;
            if ( a == null ) {
                return false;
            }
            if ( a.Length <= 0 ) {
                return false;
            }
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

        public override void treeReset() {
            if ( treeViewLine.Nodes.Count > 0 ) {
                treeViewLine.SelectedNode = treeViewLine.Nodes[ 0 ];
            } else {
                treeViewLine.SelectedNode = null;
            }
        }



        int getNodeImageIndex( string id ) {
            int index = 2;
            if ( id != null ) {
                File.Chunk ori = Original.Chunks[ id ] as File.Chunk;
                File.Chunk mod = Mod.Chunks[ id ] as File.Chunk;
                if ( ori != null && mod != null && ori.MaleLine.Original != null && ori.FemaleLine.Original != null && mod.MaleLine.Original != null && mod.FemaleLine.Original != null ) {
                    int m = 0;
                    int f = 0;

                    bool ma = File.checkAvailable( ori.MaleLine.Original );
                    bool fa = File.checkAvailable( ori.FemaleLine.Original );

                    if ( ma ) {
                        //if ( ori.MaleLine.Original.Length > 0 && ori.MaleLine.Original.CompareTo( mod.MaleLine.Modify ) != 0 ) {
                        if ( ori.MaleLine.Original.CompareTo( mod.MaleLine.Modify ) != 0 ) {
                            ++m;
                        }
                    } else {
                        ++m;
                    }
                    if ( fa ) {
                        ///if ( ori.FemaleLine.Original.Length > 0 && ori.FemaleLine.Original.CompareTo( mod.FemaleLine.Modify ) != 0 ) {
                        if ( ori.FemaleLine.Original.CompareTo( mod.FemaleLine.Modify ) != 0 ) {
                            ++f;
                        }
                    } else {
                        ++f;
                    }
                    // �Е������L���łȂ��ꍇ�͒��Ԓl�͖�����
                    // �����Â��Ēʂ��Ă�0*=2�Ȃ̂ő������v
                    //if ( ori.MaleLine.Original.Length > 0 && ori.FemaleLine.Original.Length == 0 ) {
                    if ( ma && !fa ) {
                        m *= 2;
                    }
                    //if ( ori.MaleLine.Original.Length == 0 && ori.FemaleLine.Original.Length > 0 ) {
                    if ( !ma && fa ) {
                        f *= 2;
                    }
                    index = m + f;
                    index = System.Math.Min( index, 2 );
                }
            }
            return index;
        }

        public override void Eijirou_Click( object sender, EventArgs e ) {
            TabPage tab = tabControlLine.SelectedTab;
            if( tab != null ) {
                string text = null;
                TextBox box = tab.Tag as TextBox;
                if( box != null && box.Focused && box.SelectionLength > 0 ) {

                    text = box.SelectedText;

                } else if( textBoxOriginalM.Focused && textBoxOriginalM.SelectionLength > 0 ) {

                    text = textBoxOriginalM.SelectedText;

                } else if( textBoxOriginalF.Focused && textBoxOriginalF.SelectionLength > 0 ) {
                
                    text = textBoxOriginalF.SelectedText;

                }

                if( text != null ) {
                    TranslateBrowser browser = TranslateBrowser.Create( new WebTranslate.EijirouTranslate( text ), this );
                }

            }
        }

        public override void EijirouWeb_Click( object sender, EventArgs e ) {
            TabPage tab = tabControlLine.SelectedTab;
            if( tab != null ) {
                string text = null;
                TextBox box = tab.Tag as TextBox;
                if( box != null && box.Focused && box.SelectionLength > 0 ) {

                    text = box.SelectedText;

                } else if( textBoxOriginalM.Focused && textBoxOriginalM.SelectionLength > 0 ) {

                    text = textBoxOriginalM.SelectedText;

                } else if( textBoxOriginalF.Focused && textBoxOriginalF.SelectionLength > 0 ) {

                    text = textBoxOriginalF.SelectedText;

                }

                if( text != null ) {
                    string url = WebTranslate.EijirouTranslate.GetUrl( text );
                    if( url != null ) {
                        System.Diagnostics.Process.Start( url );
                    }
                }

            }
        }

    }
}
