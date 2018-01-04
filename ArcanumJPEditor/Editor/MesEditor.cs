// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ArcanumJPEditor {
    public partial class MesEditor : Editor {
        private MesEditor() {
            InitializeComponent();
        }
        public MesEditor( string path ) {
            InitializeComponent();
            treeViewLine.HideSelection = false;
            this.Name = path;

            treeViewLine.ImageList = imageList1;

            // groupboxを下にしてツリーをうえにするとサイズの自動計算がおかしい

            open( path );

            // mes tree
            //System.IO.FileInfo info = new System.IO.FileInfo( Mod.FilePath );
            TreeNode root = new TreeNode( FileName );

            root.ImageIndex = root.SelectedImageIndex = 2;

            // nodeのlistだとセット関係が分からないからnodeのchunkのlistがいるのかなあ
            if ( Original.Type == File.FileType.MES ) {
                //int size = Mod.Chunks.Count;
                int size = Mod.IDs.Count;
                for ( int i = 0; i < size; ++i ) {
                    //File.Chunk chunk = Mod.Chunks[ i ] as File.Chunk;
                    File.Chunk chunk = Mod.Chunks[ Mod.IDs[ i ] ] as File.Chunk;
                    File.Node id = chunk.ID;
                    File.Node message = chunk.Message;
                    // 空は編集できないようにする
                    if ( message.Original != null && message.Original.Length > 0 ) {
                        TreeNode node = new TreeNode( id.Original );
                        node.Text = chunk.NodeText;
                        node.Tag = chunk;
                        root.Nodes.Add( node );

                        node.ImageIndex = node.SelectedImageIndex = getNodeImageIndex( id.Original );

                    }
                }
                root.Expand();
                treeViewLine.Nodes.Add( root );
            }

            treeViewLine.AfterSelect += new TreeViewEventHandler( treeViewLine_AfterSelect );
            treeViewLine.MouseDown += new MouseEventHandler( treeViewLine_MouseDown );

            textBoxModify.ModifiedChanged += new EventHandler( textBoxModify_ModifiedChanged );
            textBoxModify.TextChanged += new EventHandler( textBoxModify_TextChanged );

            toolStripButtonNext.Click += new EventHandler( Next_Click );
            toolStripButtonRestore.Click += new EventHandler( toolStripButtonRestore_Click );

            textBoxOriginal.ContextMenu = createMenuOriginal();
            textBoxModify.ContextMenu = createMenuModify();
            // 右押された時点でフォーカス移さないと右クリックメニュー処理がめんどい


            //textBoxOriginal.ContextMenu.Popup += new EventHandler( ContextMenu_Popup );

        }

        // translate test
#if false
        void ContextMenu_Popup( object sender, EventArgs e ) {
            ContextMenu menu = sender as ContextMenu;
            if ( menu != null ) {
                TextBox box = menu.SourceControl as TextBox;
                if ( box != null ) {
                    string text = box.SelectedText;
                    if ( text != null && text.Length > 0 ) {
                        WebTranslate.TranslateBase trans = new WebTranslate.GoogleTranslate( text );
                        string result = trans.GetResult();
                        MainForm.setStatus( result );
                    }
                }
            }
        }
#endif

        public override void Next_Click( object sender, EventArgs e ) {
            TreeNode node = treeViewLine.SelectedNode;
            if ( node != null ) {
                if ( node.NextNode != null ) {
                    treeViewLine.SelectedNode = node.NextNode;
                    //treeViewLine.Focus();
                }
            }
        }

        void toolStripButtonRestore_Click( object sender, EventArgs e ) {
            TreeNode node = textBoxModify.Tag as TreeNode;
            if ( node != null ) {
                File.Chunk chunk = node.Tag as File.Chunk;
                File.Node text = chunk.Message;
                text.Modify = text.Original.Clone() as string;
                textBoxModify.Text = text.Modify;

                MainForm.setStatus( @"編集前に戻す" );
            }
        }

        // コンフリクトしないようにするには1ファイル1つで順序を同一にしないとだめか
        // あとノードアイコンはディレクト利用に無しとか出来ないので、まずは背景カラーでやるのがいいのかなあ
        public enum Progress {
            Yet,
            Starting,
            Finish,
        }
        string[] ProgressText = {
            "未翻訳","翻訳中","翻訳済"
        };

        void textBoxModify_TextChanged( object sender, EventArgs e ) {
            TextBox box = sender as TextBox;
            if ( box != null ) {
                TreeNode node = box.Tag as TreeNode;
                if ( node != null ) {
                    File.Chunk chunk = node.Tag as File.Chunk;
                    File.Node id = chunk.ID;
                    File.Node text = chunk.Message;
                    
                    history.TextChange( chunk.ID.Original, textBoxModify );

                    text.Modify = box.Text; // ここで必要以上に書き戻されるのが少々気になるが・・・
                    // 書き戻すのは最後のセーブ時にするか？
                    // 重いし
                    node.Text = chunk.NodeText;

                    // image key
                    node.ImageIndex = node.SelectedImageIndex = getNodeImageIndex( chunk.ID.Original );

                }
            }
        }

        void textBoxModify_ModifiedChanged( object sender, EventArgs e ) {
            //System.Console.WriteLine("modified");
            TextBox box = sender as TextBox;
            TabPage tab = this.Parent as TabPage;
            if ( tab != null ) {
                if ( box.Modified ) {
                    // ファイル名がいい感じにとれないなあ
                } else {
                }
            }
        }

        void treeViewLine_AfterSelect( object sender, TreeViewEventArgs e ) {
            if ( e.Node != null ) {
                File.Chunk chunk = e.Node.Tag as File.Chunk;
                if ( chunk != null ) {
                    textBoxModify.Tag = e.Node;
                    File.Node id = chunk.ID;
                    File.Chunk origin = Original.Chunks[ id.Original ] as File.Chunk;
                    File.Node otext = origin.Message;
                    File.Node text = chunk.Message;

                    history.NodeChange( chunk.ID.Original );
                    groupBoxID.Text = id.Original;

                    MainForm.setTitle( " - " +  FileName + " {" + id.Original + "}");

                    textBoxOriginal.Text = otext.Original;
                    textBoxModify.Text = text.Modify; // textchangeが呼ばれる
                    //textBoxModify.Modified = false;
                }
            }
        }

        // 他のコントロールのイベントにまとめるとなんか問題ある？
        void treeViewLine_MouseDown( object sender, MouseEventArgs e ) {
            // 左右どちらでも挙動は同じにしよう 中クリックは無効
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
            if ( textBoxModify.Focused ) {
                textBoxModify.SelectAll();
                MainForm.setStatus( @"編集文をすべて選択" );
            } else if ( textBoxOriginal.Focused ) {
                textBoxOriginal.SelectAll();
                MainForm.setStatus( @"原文をすべて選択" );
            }


        }
        public override void Cut_Click( object sender, EventArgs e ) {
            if ( textBoxModify.Focused && textBoxModify.SelectionLength > 0 ) {
                textBoxModify.Cut();
                MainForm.setStatus( @"編集文を切り取り" + round( Clipboard.GetText(), 128 ) );
            } else if ( textBoxOriginal.Focused && textBoxOriginal.SelectionLength > 0 ) {
                textBoxOriginal.Copy(); // readonly
                MainForm.setStatus( @"原文をコピー" + round( Clipboard.GetText(), 128 ) );
            }

        }
        public override void Copy_Click( object sender, EventArgs e ) {
            if ( textBoxModify.Focused && textBoxModify.SelectionLength > 0 ) {
                textBoxModify.Copy();
                MainForm.setStatus( @"編集文をコピー: " + round( Clipboard.GetText(), 128 ) );
            } else if ( textBoxOriginal.Focused && textBoxOriginal.SelectionLength > 0 ) {
                textBoxOriginal.Copy();
                MainForm.setStatus( @"原文をコピー: " + round( Clipboard.GetText(), 128 ) );
            }

        }
        public override void Paste_Click( object sender, EventArgs e ) {
            if ( textBoxModify.Focused ) {
                textBoxModify.Paste();
                MainForm.setStatus( @"ペースト" );
            }
        }

        public override void Undo_Click( object sender, EventArgs e ) {
            //textBoxModify.Undo();
            TreeNode node = textBoxModify.Tag as TreeNode;
            if ( node != null ) {
                File.Chunk chunk = node.Tag as File.Chunk;
                history.Undo( chunk.ID.Original, textBoxModify);
                MainForm.setStatus( @"元に戻す" );
            }
        }
        public override void Redo_Click( object sender, EventArgs e ) {
            TreeNode node = textBoxModify.Tag as TreeNode;
            if ( node != null ) {
                File.Chunk chunk = node.Tag as File.Chunk;
                history.Redo( chunk.ID.Original, textBoxModify );
                MainForm.setStatus( @"やり直し" );
            }
        }


        public override bool search( Search.Option option, TreeNodeUtill.GetNode getfunc ) {
            // Fileつかって探るのとtree使うのはどっちが早いんだろうね
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
                    if ( compare( chunk.Message.Modify, option ) ) {
                        treeViewLine.SelectedNode = next;
                        if ( option.origin ) {
                            textBoxOriginal.Focus();
                        } else {
                            textBoxModify.Focus();
                        }
                        return next;
                    }
                }
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
            int index = 0;
            if ( id != null ) {
                File.Chunk ori = Original.Chunks[ id ] as File.Chunk;
                File.Chunk mod = Mod.Chunks[ id ] as File.Chunk;
                if ( ori != null && mod != null && ori.Message.Original != null && mod.Message.Modify != null ) {
                    //if ( ori.Message.Original.Length > 0 ) {
                    if ( File.checkAvailable( ori.Message.Original ) ) {
                        if ( ori.Message.Original.CompareTo( mod.Message.Modify ) != 0 ) {
                            index = 1;
                        }
                    } else {
                        index = 1;
                    }
                }
            }
            return index;
        }

        public override void Eijirou_Click( object sender, EventArgs e ) {
            string text = null;
            if( textBoxModify.Focused && textBoxModify.SelectionLength > 0 ) {
                text = textBoxModify.SelectedText;
            } else if( textBoxOriginal.Focused && textBoxOriginal.SelectionLength > 0 ) {
                text = textBoxOriginal.SelectedText;
            }

            if( text != null ) {
                TranslateBrowser browser = TranslateBrowser.Create( new WebTranslate.EijirouTranslate( text ), this );
            }

        }

        public override void EijirouWeb_Click( object sender, EventArgs e ) {
            string text = null;
            if( textBoxModify.Focused && textBoxModify.SelectionLength > 0 ) {
                text = textBoxModify.SelectedText;
            } else if( textBoxOriginal.Focused && textBoxOriginal.SelectionLength > 0 ) {
                text = textBoxOriginal.SelectedText;
            }

            if( text != null ) {
                TranslateBrowser browser = TranslateBrowser.Create( new WebTranslate.EijirouTranslate( text ), this );
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
