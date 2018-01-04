// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcanumJPEditor {
    public class File {
        public bool open( string path, bool original ) {


            System.IO.FileInfo info = new System.IO.FileInfo( path );
            if ( !info.Exists ) {
                // not found
                return false;
            }

            switch ( info.Extension.ToLower() ) {
                case Path.Ext.DialogExtention:
                    this.Type = FileType.DLG;
                    break;
                case Path.Ext.MessageExtention:
                    this.Type = FileType.MES;
                    break;
                default:
                    // not found extension
                    break;
            }

            System.Text.Encoding encode = System.Text.Encoding.GetEncoding( "shift_jis" );
            if ( original ) {
                encode = System.Text.Encoding.GetEncoding( "Windows-1252" );
            }

            using ( System.IO.StreamReader reader = new System.IO.StreamReader( path, encode ) ) {
                string file = reader.ReadToEnd();

                bool isJoint = true; //dlg.Substring;

                int startIndex = 0;
                int nodecount = 0;
                int idcount = 0;
                int type = 0;

                Chunk chunk = new Chunk( this );

                for ( int index = 0; index < file.Length; ++index ) {

                    char p = file[ index ]; // マルチバイトもちゃんと入る模様

                    // だがsjisで取るとunicodeに変換されるのでアクサン認識できず
                    // asciiで取るとエンコードミスで?になってしまい認識できず
                    // バイナリで取るしかないのか？

                    //西ヨーロッパ言語で取れば取れるが、
                    //sjisで保存すると英文字になるなあ
                    //Windows-1252のほうがよさげ？どうせwindowsで作ったんだろうし
                    // iso-8859-1またはWindows-1252でぐぐるとwikipediaに符号表もあるでよ

                    switch ( p ) {
                        //case '/': // comment out1
                        case '{':
                            if ( isJoint ) {
                                string sub = file.Substring( startIndex, index - startIndex ); // 節
                                // 空なら飛ばす
                                if ( sub.Length > 0 ) {
                                    //sub = sub.Replace( "}", "" );// 駄目文字誤爆しねえ？
                                    File.Node node = new File.Node( this );
                                    node.setOriginal( sub, this );
                                    node.Modify = node.Original;
                                    node.Type = File.Node.NodeType.Joint;
                                    node.Editable = false;
                                    //this.Nodes.Add( nodecount, node );
                                    this.Nodes.Add( node );
                                    ++nodecount;
                                }
                                isJoint = false; // 腹
                                startIndex = index + 1;
                            } else {
                                // error
                            }
                            break;
                        case '}':
                            if ( !isJoint ) {
                                string sub = file.Substring( startIndex, index - startIndex ); // 腹
                                //sub = sub.Replace("{","");// 駄目文字誤爆しねえ？
                                File.Node node = new File.Node( this );
                                
                                switch ( this.Type ) {
                                    case FileType.DLG:
                                        node.Type = File.DlgState[ type ];
                                        ++type;
                                        type = type % File.DlgState.Length;
                                        break;
                                    case FileType.MES:
                                        node.Type = File.MesState[ type ];
                                        ++type;
                                        type = type % File.MesState.Length;
                                        break;
                                    default:
                                        // error
                                        break;
                                }
                                // 節時の改行なんぞで二重にチェックしたいな
                                // しないと03042Blue_Stone_EXA.dlgとか不完全なファイルがあるなあ


                                node.Editable =
                                    ( node.Type == Node.NodeType.MaleLine ||
                                    node.Type == Node.NodeType.FemaleLine ||
                                    node.Type == Node.NodeType.Message ); // temp

                                node.setOriginal( sub, this );
                                if ( node.Editable ) {
                                    node.Modify = node.Original.Clone() as string;
                                } else {
                                    node.Modify = node.Original;
                                }

                                //this.Nodes.Add( nodecount, node );
                                this.Nodes.Add( node );
                                //chunk.Key[ ( int )node.Type ] = nodecount;
                                chunk.Keys[ ( int )node.Type ] = node;
                                // dlg or mesでごそっとわけたほうがいいのかね
                                if ( node.Type == Node.NodeType.Result ||
                                     node.Type == Node.NodeType.Message ) {

#if false
                                    // debug
                                    // 8文字以上にしてみたよ
                                    string filename = System.IO.Path.GetFileNameWithoutExtension( path );
                                    if ( Type == FileType.MES ) {
                                        if ( chunk.Message.Modify != null && chunk.Message.Modify.Length > 2 ) {
                                            chunk.Message.Modify = filename + "-" + chunk.ID.Original + " " + chunk.Message.Modify;
                                        }
                                    } else {
                                        if ( chunk.MaleLine.Modify != null && chunk.MaleLine.Modify.Length > 2 ) {
                                            chunk.MaleLine.Modify = filename + "-" + chunk.ID.Original + " " + chunk.MaleLine.Modify;
                                        }
                                        if ( chunk.FemaleLine.Modify != null && chunk.FemaleLine.Modify.Length > 2 ) {
                                            chunk.FemaleLine.Modify = filename + "-" + chunk.ID.Original + " " + chunk.FemaleLine.Modify;
                                        }
                                    }
#endif
                                    // なんか重複IDがあるんだが、バグじゃねえのこれ
                                    if ( Chunks.ContainsKey( chunk.ID.Original ) ) {
                                        System.Console.WriteLine( "[WARINIG] found duplicating ID: " + chunk.ID.Original + " in " + path );
                                    } else {
                                        IDs.Add( idcount, chunk.ID.Original );
                                        ++idcount;
                                        Chunks.Add( chunk.ID.Original, chunk.clone() );
                                    }
                                    chunk.clear(); // deepcopyしないと消えちゃうな
                                }
                                ++nodecount;
                                // endof hara
                                isJoint = true;
                                startIndex = index + 1;
                            } else {
                                // error
                            }
                            break;
                        case '\n':
                            // 条件チェック中
                            // とりあえずフォーマットがおかしいファイルは限られているのでなんとかなりそう。
                            if ( isJoint ) {
                                if ( type != ( int )Node.NodeType.ID ) {
                                    System.Console.WriteLine( "[WARINIG] not fullfill elements: " + path );

#if true
                                    if ( chunk.ID != null ) {
                                        if ( Chunks.ContainsKey( chunk.ID.Original ) ) {
                                            System.Console.WriteLine( "[WARINIG] found duplicating ID: " + chunk.ID.Original + " in " + path );
                                        } else {
                                            IDs.Add( idcount, chunk.ID.Original );
                                            ++idcount;
                                            Chunks.Add( chunk.ID.Original, chunk.clone() );
                                        }
                                    }
                                    chunk.clear(); // deepcopyしないと消えちゃうな
                                    type = 0;
#endif
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

                // 腹のままならエラー
                if ( !isJoint ) {
                    MainForm.setStatus( "構文エラー: " + path );
                    //return false;
                }
                // chunkも中途半端だと足したいが、そもそもそれはデータがおかしいような

                // 最後の残り
                if ( startIndex < ( file.Length - 1 ) ) {
                    string sub = file.Substring( startIndex ); // 節
                    // 空なら飛ばす
                    if ( sub.Length > 0 ) {
                        //sub = sub.Replace( "}", "" );// 駄目文字誤爆しねえ？
                        File.Node node = new File.Node( this );
                        node.setOriginal( sub, this );
                        node.Modify = node.Original;
                        node.Type = File.Node.NodeType.Joint;
                        node.Editable = false;
                        //this.Nodes.Add( nodecount, node );
                        this.Nodes.Add( node );
                        //++count;
                    }
                    //startIndex = index;
                }

                // 変なデータだとエラーチェックめんどくさそうだなあ

            }
            if ( original ) {
                // read onlyにしたいが…アクセッサでなんとかしろと？
#if false
                foreach ( File.Node node in this.Nodes ) {

                }
#endif
            }
            this.FilePath = path;
            this.IsOriginal = original;
            return true;
        }

        public void save( string path ) {

            string data = "";
            int size = this.Nodes.Count;
            for ( int i = 0; i < size; ++i ) {
                Node node = this.Nodes[ i ] as Node;
                if ( node != null ) {
                    if ( node.Type == Node.NodeType.Joint ) {
                        data += node.Original;
                        //data += node.Modify;
                    } else {
                        string text = node.Original;
                        // mes,male.female
                        if( node.Editable ) {
                            // 不味い文字が入っていないか
                            text = node.Modify.Replace( '　', ' ' ); // 全角スペースを半角スペースに置換
                            //if( text.IndexOf( '{' ) > -1 ) {
                            text = text.Replace( "{", "" );
                            //}
                            //if( text.IndexOf( '}' ) > -1 ) {
                            text = text.Replace( "}", "" );
                            //}
                        }
                        data += "{" + text + "}";
                        if ( node.Editable ) {
                            // 変更の反映
                            // 全部コピーと比較してからコピーはどっちがよい？
                            //node.Original = node.Modify.Clone() as string;
                            node.setOriginal( text, this );
                            // modがまだなくoriginal=modの場合読み直さないとoriginalの表示がおかしくなる
                            // のだがこれをしないと変更チェックが出来ない。
                            // dirtyフラグにするのか？
                        }
                    }
                }
            }

            System.IO.FileInfo info = new System.IO.FileInfo( path );
            if ( !info.Exists ) {
            }
            // TODO:拡張子とfiletypeのチェック

            if ( System.IO.Directory.Exists( info.DirectoryName ) == false ) {
                System.IO.Directory.CreateDirectory( info.DirectoryName );
            }

            using ( System.IO.StreamWriter writer = new System.IO.StreamWriter( path, false, System.Text.Encoding.GetEncoding( "shift_jis" ) ) ) {
                //writer.AutoFlush = true; // 即時書き込み
                writer.Write( data );
            }
        }

        // 行頭にファイル名とIDを付与する
        public void addPrefixFileNumber() {
            // gamearea.mesなどのファイル全体でやるとまずいやつ
            if ( checkIgnoreByFileName() == false ) {
                return;
            }

            string name = System.IO.Path.GetFileName( this.FilePath );

            int num = IDs.Count;
            for ( int i = 0; i < num; ++i ) {
                string id = IDs[ i ] as string;
                if ( id != null ) {
                    Chunk chunk = Chunks[ id ] as Chunk;
                    if ( chunk != null ) {
                        // この比較回数を削りたい
                        string file = System.IO.Path.GetFileNameWithoutExtension( name );
                        if ( this.Type == FileType.MES ) {
                            string prefix = "(" + file + ".M-" + chunk.ID.Original;
                            Node mes = chunk.Message;
                            if ( checkIgnore( mes.Modify ) ) {
                                mes.Modify = prefix + ")" + mes.Modify;
                            }
                        } else if ( this.Type == FileType.DLG ) {
                            string prefix = "(" + file;
                            // 5桁数字のみ
                            if ( file.Length > 5 ) {
                                prefix = "(" + file.Substring( 0, 5 );
                            } 
                            prefix += ".D:" + chunk.ID.Original;
                            Node male = chunk.MaleLine;
                            if ( checkIgnore( male.Modify ) ) {
                                //male.Modify = prefix + "M)" + male.Modify;
                                male.Modify = prefix + ")" + male.Modify;
                            }
                            Node female = chunk.FemaleLine;
                            if ( checkIgnore( female.Modify ) ) {
                                //female.Modify = prefix + "F)" + female.Modify;
                                female.Modify = prefix + ")" + female.Modify;
                            }

                        }
                    }
                }
            }
        }

        // gamearea.mesなどのファイル全体でやるとまずいやつ
        bool checkIgnoreByFileName() {
            string name = System.IO.Path.GetFileName( this.FilePath );
            if ( name.ToLower() == "gamearea.mes" ) {
                return false;
            }
            return true;
        }
        public static bool checkIgnore( string line ) {
            string text = line.TrimStart(); // 先頭に空白が混入されている場合があるので
            // empty
            if ( text != null && text.Length > 0 ) {
                // limit
                // only number
                if ( text.Length > 1 && char.IsNumber( text[ 0 ] ) == false ) {
                    if ( text.Length > 2 && char.IsNumber( text[ 1 ] ) == false ) {
                        //if ( text.Length > 2 ) {

                        // var code
                        //if ( ( char.IsUpper( text[ 0 ] ) && text[ 1 ] == ':' ) == false ) {
                        if ( text[ 1 ] != ':' ) {
                            // 本などの書式指定で先頭の数字…というパターンには対応しきれない
                            //int output = 0;
                            //if ( int.TryParse( text, out output ) == false ) {
                            return true;
                            //}
                        }
                    }
                }

            }
            return false;
        }
        public static bool checkAvailable( string line ) {
            string text = line.TrimStart(); // 先頭に空白が混入されている場合があるので
            // empty
            if ( text != null && text.Length > 1 ) {
                // var code
                // テンプレート書式判定
                if ( text[ 1 ] != ':' ) {
                    // 本などの書式指定で先頭の数字…というパターンには対応しきれないか
                    //int output = 0;
                    //if ( int.TryParse( text, out output ) == false ) {
                    return true;
                    //}
                }

            } else {
                // 1文字しかない
                // femalelineが 1:malelineがPC男性台詞 0:malelineがPC女性台詞
                // 入れなくても良いかな
            }
#if false // output match text
            if ( text != null && text.Length > 0 ) {
                System.Console.WriteLine( line );
            }
#endif
            return false;
        }

        public bool checkModify() {
            for ( int i = 0; i < Nodes.Count; ++i ) {
                Node node = Nodes[ i ] as Node;
                if ( node.Editable ) {
                    if ( node.Modify != node.Original ) {
                        return true;
                    }
                } else {
                    // 例外投げた方がいいか？
#if false // 重い
                    string err = "[ERROR] Not Editable nodes are modified!";
                    System.Diagnostics.Debug.Assert( node.Modify == node.Original, err );
                    if ( node.Modify == node.Original ) {
                        System.Console.WriteLine( err );
                    }
#endif
                }
            }
            return false;
        }

        // 腹anti-node{}か}{節nodeかで構成される可変長のリストだなあ
        // 腹のみ編集可能
        public class Node {
            public Node( File parent ) {
                Parent = parent;
            }
            private Node() {
            }
            public bool Editable; // != Joint && (dialog || Message)
            public string Original {
                get { return original; }
            }
            public string Modify {
                get { return modify; }
                set { modify = value; }
            }
            // only friend File
            public void setOriginal( string text, File sender ) {
                if ( sender == Parent ) {
                    original = text;
                }
            }
            string original;
            string modify;
            //bool dirty; // これつけて保存の管理してると、元に戻すでの管理がめんどうなんだよな
            public enum NodeType {
                ID, // for dlg and mes
                MaleLine, // and  Player Response
                FemaleLine, // and Gender Text
                IntCheck,
                TestCodes, // if
                ResponseID,
                Result,
                Message, // for mes
                Joint, // not anti-node
            }
            public NodeType Type;
            public File Parent;
        }

        public class Chunk {
            //public int[] Key = new int[ ( int )Node.NodeType.Joint ];
            public Node[] Keys = new Node[ ( int )Node.NodeType.Joint ];
            File Parent;

            public Chunk( File parent ) {
                Parent = parent;
                clear();
            }
            private Chunk() {
            }
            public Chunk clone() {
                Chunk chunk = new Chunk( Parent );
                // clearはしっちゃうな
                for ( int i = 0; i < Keys.Length; ++i ) {
                    chunk.Keys[ i ] = this.Keys[ i ];
                }
                return chunk;
            }
            public void clear() {
                for ( int i = 0; i < Keys.Length; ++i ) {
                    //Key[ i ] = -1;
                    Keys[ i ] = null;
                }
            }

            public Node ID {
                get {
                    return getNode( Node.NodeType.ID );
                }
            }
            public Node MaleLine {
                get {
                    return getNode( Node.NodeType.MaleLine );
                }
            }
            public Node FemaleLine {
                get {
                    return getNode( Node.NodeType.FemaleLine );
                }
            }
            public Node IntCheck {
                get {
                    return getNode( Node.NodeType.IntCheck );
                }
            }
            public Node TestCodes {
                get {
                    return getNode( Node.NodeType.TestCodes );
                }
            }
            public Node ResponseID {
                get {
                    return getNode( Node.NodeType.ResponseID );
                }
            }
            public Node Result {
                get {
                    return getNode( Node.NodeType.Result );
                }
            }
            public Node Message {
                get {
                    return getNode( Node.NodeType.Message );
                }
            }
            Node getNode( Node.NodeType type ) {
                int index = ( int )type;
                if ( Keys.Length > index ) {
                    return Keys[ index ];
                }
                return null;
#if false
                Node node = null;
                if ( Parent != null ) {
                    node = Parent.Nodes[ getKey( type ) ] as Node;
                }
                return node;
#endif
            }
#if false
            int getKey( Node.NodeType type ) {
                return Key[ ( int )type ];
            }
#endif
            public string NodeText {
                get {
                    string text = null;
                    switch ( Parent.Type ) {
                        case FileType.MES:
                            text = "{" + ID.Original + "}";
                            if ( Message != null ) {
                                if ( Message.Modify != null && Message.Modify.Length > 0 ) {
                                    text += "  ";
                                    text += round( Message.Modify, 96 );
                                }
                            }
                            break;
                        case FileType.DLG:
                            text = "{" + ID.Original + "}";
                            if ( MaleLine != null && MaleLine.Modify != null && MaleLine.Modify.Length > 0 &&
                                 FemaleLine != null && FemaleLine.Modify != null && FemaleLine.Modify.Length > 0 ) {
                                text += "  ";
                                text += round( MaleLine.Modify, 48 );
                                text += " / ";
                                text += round( FemaleLine.Modify, 48 );
                            } else if ( MaleLine != null && MaleLine.Modify != null && MaleLine.Modify.Length > 0 ) {
                                text += "  ";
                                text += round( MaleLine.Modify, 96 );
                            } else if ( FemaleLine != null && FemaleLine.Modify != null && FemaleLine.Modify.Length > 0 ) {
                                text += "  ";
                                text += round( FemaleLine.Modify, 96 );
                            }
                            break;
                    }
                    return text;
                }
            }

            static string round( string text, int max ) {
                string ret = text;
                if ( text.Length > max ) {
                    string sub = text.Substring( 0, max );
                    ret = sub + "...";
                }
                return ret;
            }
        }


        static public Node.NodeType[] DlgState = {
            Node.NodeType.ID,
            Node.NodeType.MaleLine,
            Node.NodeType.FemaleLine,
            Node.NodeType.IntCheck,
            Node.NodeType.TestCodes,
            Node.NodeType.ResponseID,
            Node.NodeType.Result };

        static public Node.NodeType[] MesState = {
            Node.NodeType.ID,
            Node.NodeType.Message };

        public enum FileType {
            DLG,
            MES,
        }
        public FileType Type;
        public string FilePath;
        public bool IsOriginal;

        public List<Node> Nodes = new List<Node>();
        public System.Collections.Hashtable IDs = new System.Collections.Hashtable();
        public System.Collections.Hashtable Chunks = new System.Collections.Hashtable();

        // なんで上のはリストでないの？
        //public System.Collections.Generic.List<Chunk> Chunks = new System.Collections.Generic.List<Chunk>();

    }
}
