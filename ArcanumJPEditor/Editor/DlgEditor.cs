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
        // enum toStringですむかも
        string[] infoname = { "Int", "Test", "ResponseID", "Result", "Reference" };
        class NodeInfo {
            // TreeNodeはかならずあるとは限らない
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
            // nodeのlistだとセット関係が分からないからnodeのchunkのlistがいるのかなあ
            if ( Mod.Type == File.FileType.DLG ) {
                int index = 0;
                while ( index < size ) {
                    //File.Chunk chunk = Mod.Chunks[ index ] as File.Chunk;
                    File.Chunk chunk = Mod.Chunks[ Mod.IDs[ index ] ] as File.Chunk;
                    File.Node id = chunk.ID;
                    File.Node male = chunk.MaleLine;
                    File.Node female = chunk.FemaleLine;

                    // 空は編集できないようにする
                    // のだが様子見中
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

                        // PC会話判定条件
                        // int checkの有無かね？
                        File.Node res = child.IntCheck;
                        if ( res.Original.Length == 0 ) {
                            //rollback
                            //--index;
                            break;
                        }

                        File.Node pcid = child.ID;
                        File.Node pcmale = child.MaleLine;
                        File.Node pcfemale = child.FemaleLine;

                        // 空は編集できないようにする
                        // のだが様子見中
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

                // 逆引き
                nodeinfo = new System.Collections.Hashtable( size );
                foreach ( TreeNode node in root.Nodes ) {
                    // PC会話は逆引き無いんじゃないか？
                    // 将来的にそれ以外も拡張することを見越しておくか…
                    File.Chunk chunk = node.Tag as File.Chunk;
                    if ( chunk != null ) {
                        NodeInfo ni = new NodeInfo();

                        // 先にパース
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
                        if ( id != null ) { // そもそもidないと成り立たない
                            nodeinfo.Add( id.Original, ni );
                        }
                    }
                    foreach ( TreeNode child in node.Nodes ) {
                        chunk = child.Tag as File.Chunk;
                        if ( chunk != null ) {
                            NodeInfo ni = new NodeInfo();

                            // 先にパース
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
                            if ( id != null ) { // そもそもidないと成り立たない
                                nodeinfo.Add( id.Original, ni );
                            }
                        }
                    }
                }
                foreach ( TreeNode node in root.Nodes ) {
                    // PC会話は逆引き無いんじゃないか？
                    // 将来的にそれ以外も拡張することを見越しておくか…
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

            // 編集メニュー系はこっちのツールバーの方が楽なんだよなあ。移すかも

            // indexからの判断でごちゃごちゃになりがちなのでこれつかいましょう
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
            dataGridViewInfo.CurrentCell = null; // 描画開始後
            dataGridViewInfo.CellDoubleClick += new DataGridViewCellEventHandler( dataGridViewInfo_CellDoubleClick );
            // datasourceで読ませてるかvirtualmode true 7でないと使えない
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
            items[ 0 ] = new MenuItem( "展開(&E)", Expand_Click );
            items[ 1 ] = new MenuItem( "全展開(&X)", ExpandAll_Click );
            items[ 2 ] = new MenuItem( "折り畳み(&C)", Collapse_Click );
            items[ 3 ] = new MenuItem( "全折り畳み(&O)", CollapseAll_Click );
            treeViewLine.ContextMenu = new ContextMenu( items );

        }

        void treeViewInfo_DoubleClick( object sender, EventArgs e ) {
            // ジャンプ可能なら実行する
            TreeView tree = sender as TreeView;
            if ( tree != null && tree.SelectedNode != null && tree.SelectedNode.Tag != null ) {
                TreeNode node = tree.SelectedNode.Tag as TreeNode;
                if ( node != null ) {
                    treeViewLine.SelectedNode = node;
                    // フォーカスどっかに移す
                    treeViewLine.Focus();
                }
            }
        }

        bool doubleclick = false;
        void treeViewInfo_BeforeCollapse( object sender, TreeViewCancelEventArgs e ) {
            TreeView tree = sender as TreeView;
            if ( tree != null && e.Node != null ) {
                // root nodeの場合は通常動作、それ以外は展開無効でダブルクリック優先
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
                // root nodeの場合は通常動作、それ以外は展開無効でダブルクリック優先
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
            // 左右どちらでも挙動は同じにしよう 中クリックは無効
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
                    // 原文が同一出ない場合は確認ダイアログだすか
                    if ( textBoxOriginalM.Text != textBoxOriginalF.Text ) {
                    //if ( textBoxOriginalM.Text.CompareTo( textBoxOriginalF.Text) == 0 ) {
                        DialogResult result = MessageBox.Show( this, @"原文が異なりますが、コピーしますか？", @"Female LineをMale Lineにコピー", MessageBoxButtons.YesNo );
                        if ( result == DialogResult.No ) {
                            return;
                        }
                    }
                    textBoxModifyM.Text = textBoxModifyF.Text.Clone() as string;
                    MainForm.setStatus( @"Female LineをMale Lineにコピー" );
                }
            }
        }
        public override void ToFemale_Click( object sender, EventArgs e ) {
            if ( textBoxModifyM.ReadOnly == false && textBoxModifyF.ReadOnly == false ) {
                if ( textBoxModifyM.Text != null && textBoxModifyM.Text.Length > 0 ) {
                    if ( textBoxOriginalM.Text != textBoxOriginalF.Text ) {
                    //if ( textBoxOriginalM.Text.CompareTo( textBoxOriginalF.Text ) == 0 ) {
                        DialogResult result = MessageBox.Show( this, @"原文が異なりますが、コピーしますか？", @"Male LineをFemale Lineにコピー", MessageBoxButtons.YesNo );
                        if ( result == DialogResult.No ) {
                            return;
                        }
                    }
                    textBoxModifyF.Text = textBoxModifyM.Text.Clone() as string;
                    MainForm.setStatus( @"Male LineをFemale Lineにコピー" );
                }
            }
        }

        public override void SpeechPlay_Click( object sender, EventArgs e ) {
            // 音声再生
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
                                        // アイコン変えられないかな？
                                        // ただし変えた場合は、コールバックで終了とって色々せんといかんのでめんどい
                                    } else {
                                        // enable=falseなりでなかったらなんかする
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

                            MainForm.setStatus( @"編集前に戻す" );

                        }
                    }
                }
            }

        }

        // M Fを1つの関数でやりたいが…
        void textBoxModifyM_TextChanged( object sender, EventArgs e ) {
            TextBox box = sender as TextBox;
            if ( box != null ) {
                TreeNode node = box.Tag as TreeNode;
                if ( node != null ) {
                    File.Chunk chunk = node.Tag as File.Chunk;
                    File.Node id = chunk.ID;
                    File.Node male = chunk.MaleLine;

                    history.TextChange( chunk.ID.Original + "M", textBoxModifyM );

                    male.Modify = box.Text; // ここで必要以上に書き戻されるのが少々気になるが・・・
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

                    female.Modify = box.Text; // ここで必要以上に書き戻されるのが少々気になるが・・・
                    node.Text = chunk.NodeText;

                    node.ImageIndex = node.SelectedImageIndex = getNodeImageIndex( id.Original );

                }
            }
        }
        public override void Next_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewLine.SelectedNode; // box.tagからとるかなあ? textとちがってどっちでもいいが
            if ( node != null ) {
                File.Chunk chunk = node.Tag as File.Chunk;
                if ( chunk != null ) {
                    string next = chunk.ResponseID.Original;
                    next = Parser.removeSign( next ); // 絶対値
                    // かつ0などの特殊コードではない
                    if( next != null && next.Length > 0 ) {
                        // search next
                        // ModからNode取ってきてTreeNodeを逆引きできれば楽なんだが あんまり相互参照するのもねえ
                        TreeNode much = searchID( next );
                        if( much != null ) {
                            much.Expand(); // いらぬおせっかい
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
                            much.Expand(); // いらぬおせっかい
                            treeViewLine.SelectedNode = much;
                            //treeViewLine.Focus();
                        }
                    }
                }
            }
        }
        // intに変換するのとどちらが早いのやら 多くて5文字、大抵2文字程度だろうからstringの方がいいかな？
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
                // ありえないが一応
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


        // 逆引きID
        void searchReferenceID( string id, TreeNode node, ref System.Collections.Generic.List<TreeNode> list ) {

            File.Chunk chunk = node.Tag as File.Chunk;
            if ( chunk != null ) {
                // responce id
                File.Node res = chunk.ResponseID;
                string resid = res.Original;
                // ありえないが一応
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

                //タブのテキストと背景を描画するためのブラシを決定する
                Brush foreBrush = System.Drawing.SystemBrushes.ControlText;
                switch ( e.Index ) {
                    case 0:
                        // afterselectよりも後らしいのでreadonlyフラグでチェックする
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

                //StringFormatを作成
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                //背景の描画
                //e.Graphics.FillRectangle( backBrush, e.Bounds );
                //Textの描画
                e.Graphics.DrawString( txt, e.Font, foreBrush, e.Bounds, sf );
            }
        }
        void treeViewLine_AfterSelect( object sender, TreeViewEventArgs e ) {
            if ( e.Node != null ) {
                File.Chunk chunk = e.Node.Tag as File.Chunk;
                if ( chunk != null ) {
                    textBoxModifyM.Tag = e.Node; // これつかいますかね
                    textBoxModifyF.Tag = e.Node; // これつかいますかね

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

                    // 実際の値はパーサにかけて読み込まないと
                    // 展開具合の保持、毎回全展開でもいいのかなあ
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
                                    nodes = new TreeNode[ 1 ]; // 複数ある可能性が
                                    nodes[ 0 ] = new TreeNode( text );
                                }
#endif
                                if ( info != null && info.Test != null ) {
                                    //1でkey空がくる
                                    nodes = new TreeNode[ info.Test.Count ]; // 無効なのがなければよいが
                                    int index = 0;
                                    foreach ( KeyArguments ka in info.Test ) {
                                        string refid = null;
                                        string text = Parser.getValue( test, ka, out refid );
                                        if ( text != null && text.Length > 0 ) {
                                            nodes[ index ] = new TreeNode( text );
                                            if ( refid != null ) {
                                                // 引用元をぶらさげる
                                                TreeNode much = searchID( refid );
                                                if ( much != null ) {
                                                    nodes[ index ].Tag = much;
                                                    TreeNode child = new TreeNode( much.Text );
                                                    child.Tag = much;
                                                    nodes[ index ].Nodes.Add( child );
                                                }
                                            }
                                        } else {
                                            // 解析できなかった… 数字だけとかよく分からんのが偶にある
                                            // 空でここにくる可能性もあるか？
                                            // FIXME 先に有効数をカウントして確保したい
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
                                    nodes = new TreeNode[ 1 ]; // 複数ある可能性が
                                    nodes[ 0 ] = new TreeNode( text );
                                }
#endif
                                if ( info != null && info.Result != null ) {
                                    //1でkey空がくる
                                    nodes = new TreeNode[ info.Result.Count ]; // 無効なのがなければよいが
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
                                                    // 引用元をぶらさげる
                                                    TreeNode child = new TreeNode( much.Text );
                                                    child.Tag = much;
                                                    nodes[ index ].Nodes.Add( child );
                                                    nodes[ index ].Expand();
                                                }
                                            }
                                        } else {
                                            // 解析できなかった… 数字だけとかよく分からんのが偶にある
                                            // 空でここにくる可能性もあるか？
                                            // FIXME 先に有効数をカウントして確保したい
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
                        // 1個のgridだと多いとスクロールしきれんな

                    } else {
                        dataGridViewInfo.Rows[ 4 ].Cells[ 1 ].Value = null;
                    }
#endif
                    //dataGridViewInfo.Rows[ 4 ].Cells[ 1 ].Value = 
                    // 逆引き結果の追加tagあたりに配列で入れるとジャンプ時に楽かも
                    // datagridviewだとスクロールしにくいから無いかも
                    // コンテキストメニューの追加

                    // original見て空ならtab封
                    // editableに記録しておいてもいいかもね
                    // 気をつけないとまたエンバグするが
                    // original のoriginalがいいのだが、取れないのでひとまずmodから
                    // originalからとるようにしたよ
                    // これで元を消して空で保存した場合に編集できなくなるのは解消したか
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
                        // 1:malelineがPC男性台詞 0:malelineがPC女性台詞
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

                    // 翻訳に限れば毎回やる必要はないな。
                    // そうそうありえないが、変数の部分を訳されたら即座に反映されないと駄目か
                    // 今の他の評価はgetValueで文字列生成してるからできている
                    // なので事前計算が必要そうならばという程度で
                    // 中ツリーに1個増やしてそこにぶら下げるのを検討しているが…
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

                    // 音停止が手動なら、常に有効化かも→それじゃあ音声あるのかどうかわからんからないな
                    bool enable_speech = false;
                    // testが1つでkeyなし、arg数字1つの場合、音声を示す
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

        // 他のコントロールのイベントにまとめるとなんか問題ある？
        void treeViewLine_MouseDown( object sender, MouseEventArgs e ) {
            // 左右どちらでも挙動は同じにしよう 中栗は無効
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
            // originalの方も選択できないとカーソルがどこあるかみんと
            TabPage tab = tabControlLine.SelectedTab;
            if ( tab != null ) {
                TextBox box = tab.Tag as TextBox;
                if ( box != null && box.Focused ) {
                    box.SelectAll();
                    MainForm.setStatus( @"編集文をすべて選択" );
                } else if ( textBoxOriginalM.Focused ) {
                    textBoxOriginalM.SelectAll();
                    MainForm.setStatus( @"原文をすべて選択" );
                } else if ( textBoxOriginalF.Focused ) {
                    textBoxOriginalF.SelectAll();
                    MainForm.setStatus( @"原文をすべて選択" );
                }
            }
        }
        public override void Cut_Click( object sender, EventArgs e ) {
            TabPage tab = tabControlLine.SelectedTab;
            if ( tab != null ) {
                TextBox box = tab.Tag as TextBox;
                if ( box != null && box.Focused && box.SelectionLength > 0 ) {
                    box.Cut();
                    MainForm.setStatus( @"編集文を切り取り" + round( Clipboard.GetText(), 128 ) );
                } else if ( textBoxOriginalM.Focused && textBoxOriginalM.SelectionLength > 0 ) {
                    textBoxOriginalM.Copy(); // readonlyですよ
                    MainForm.setStatus( @"原文をコピー" + round( Clipboard.GetText(), 128 ) );
                } else if ( textBoxOriginalF.Focused && textBoxOriginalF.SelectionLength > 0 ) {
                    textBoxOriginalF.Copy(); // readonlyですよ
                    MainForm.setStatus( @"原文をコピー" + round( Clipboard.GetText(), 128 ) );
                }
            }
        }
        public override void Copy_Click( object sender, EventArgs e ) {
            TabPage tab = tabControlLine.SelectedTab;
            if ( tab != null ) {
                TextBox box = tab.Tag as TextBox;
                if ( box != null && box.Focused && box.SelectionLength > 0 ) {
                    box.Copy();
                    MainForm.setStatus( @"編集文をコピー: " + round( Clipboard.GetText(), 128 ) );
                } else if ( textBoxOriginalM.Focused && textBoxOriginalM.SelectionLength > 0 ) {
                    textBoxOriginalM.Copy();
                    MainForm.setStatus( @"原文をコピー: " + round( Clipboard.GetText(), 128 ) );
                } else if ( textBoxOriginalF.Focused && textBoxOriginalF.SelectionLength > 0 ) {
                    textBoxOriginalF.Copy();
                    MainForm.setStatus( @"原文をコピー: " + round( Clipboard.GetText(), 128 ) );
                }
            }
        }
        public override void Paste_Click( object sender, EventArgs e ) {
            TabPage tab = tabControlLine.SelectedTab;
            if ( tab != null ) {
                TextBox box = tab.Tag as TextBox;
                if ( box != null && box.Focused ) {
                    box.Paste();
                    MainForm.setStatus( @"ペースト" );
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
                        MainForm.setStatus( @"元に戻す" );
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
                        MainForm.setStatus( @"やり直し" );
                    }
                }
            }
        }

        public override bool search( Search.Option option, TreeNodeUtill.GetNode getfunc ) {
            TreeNode select = treeViewLine.SelectedNode;
            if ( select == null ) {
                if ( treeViewLine.Nodes.Count > 0 ) {
                    // これをselectにするとこいつは調べてくれない
                    // まあ今のところは調べる必要は薄いのだが
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
                    // tabselectは自動？主導？
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
                // この辺はパーサ前のデータでいいのかなあ
                if ( option.integer ) {
                    if ( compare( chunk.IntCheck.Original, option ) ) {
                        // フォーカスセット
                        treeViewLine.SelectedNode = next;
                        dataGridViewInfo.Rows[ 0 ].Cells[ 1 ].Selected = true;
                        dataGridViewInfo.Focus();
                        return next;
                    }
                }
                if ( option.test ) {
                    if ( compare( chunk.TestCodes.Original, option ) ) {
                        // フォーカスセット
                        treeViewLine.SelectedNode = next;
                        dataGridViewInfo.Rows[ 1 ].Cells[ 1 ].Selected = true;
                        dataGridViewInfo.Focus();
                        return next;
                    }
                }
                if ( option.result ) {
                    if ( compare( chunk.Result.Original, option ) ) {
                        // フォーカスセット
                        treeViewLine.SelectedNode = next;
                        dataGridViewInfo.Rows[ 2 ].Cells[ 1 ].Selected = true;
                        dataGridViewInfo.Focus();
                        return next;
                    }
                }
                if ( option.resid ) {
                    if ( compare( chunk.ResponseID.Original, option ) ) {
                        // フォーカスセット
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
            string b = option.search_word; // こっちは事前に作っておくと軽そうなんだが

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
                    // 片方しか有効でない場合は中間値は無しよ
                    // 条件甘くて通っても0*=2なので多分大丈夫
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
