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

                    char p = file[ index ]; // �}���`�o�C�g�������Ɠ���͗l

                    // ����sjis�Ŏ���unicode�ɕϊ������̂ŃA�N�T���F���ł���
                    // ascii�Ŏ��ƃG���R�[�h�~�X��?�ɂȂ��Ă��܂��F���ł���
                    // �o�C�i���Ŏ�邵���Ȃ��̂��H

                    //�����[���b�p����Ŏ��Ύ��邪�A
                    //sjis�ŕۑ�����Ɖp�����ɂȂ�Ȃ�
                    //Windows-1252�̂ق����悳���H�ǂ���windows�ō�����񂾂낤��
                    // iso-8859-1�܂���Windows-1252�ł������wikipedia�ɕ����\������ł�

                    switch ( p ) {
                        //case '/': // comment out1
                        case '{':
                            if ( isJoint ) {
                                string sub = file.Substring( startIndex, index - startIndex ); // ��
                                // ��Ȃ��΂�
                                if ( sub.Length > 0 ) {
                                    //sub = sub.Replace( "}", "" );// �ʖڕ����딚���˂��H
                                    File.Node node = new File.Node( this );
                                    node.setOriginal( sub, this );
                                    node.Modify = node.Original;
                                    node.Type = File.Node.NodeType.Joint;
                                    node.Editable = false;
                                    //this.Nodes.Add( nodecount, node );
                                    this.Nodes.Add( node );
                                    ++nodecount;
                                }
                                isJoint = false; // ��
                                startIndex = index + 1;
                            } else {
                                // error
                            }
                            break;
                        case '}':
                            if ( !isJoint ) {
                                string sub = file.Substring( startIndex, index - startIndex ); // ��
                                //sub = sub.Replace("{","");// �ʖڕ����딚���˂��H
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
                                // �ߎ��̉��s�Ȃ񂼂œ�d�Ƀ`�F�b�N��������
                                // ���Ȃ���03042Blue_Stone_EXA.dlg�Ƃ��s���S�ȃt�@�C��������Ȃ�


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
                                // dlg or mes�ł������Ƃ킯���ق��������̂���
                                if ( node.Type == Node.NodeType.Result ||
                                     node.Type == Node.NodeType.Message ) {

#if false
                                    // debug
                                    // 8�����ȏ�ɂ��Ă݂���
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
                                    // �Ȃ񂩏d��ID������񂾂��A�o�O����˂��̂���
                                    if ( Chunks.ContainsKey( chunk.ID.Original ) ) {
                                        System.Console.WriteLine( "[WARINIG] found duplicating ID: " + chunk.ID.Original + " in " + path );
                                    } else {
                                        IDs.Add( idcount, chunk.ID.Original );
                                        ++idcount;
                                        Chunks.Add( chunk.ID.Original, chunk.clone() );
                                    }
                                    chunk.clear(); // deepcopy���Ȃ��Ə������Ⴄ��
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
                            // �����`�F�b�N��
                            // �Ƃ肠�����t�H�[�}�b�g�����������t�@�C���͌����Ă���̂łȂ�Ƃ��Ȃ肻���B
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
                                    chunk.clear(); // deepcopy���Ȃ��Ə������Ⴄ��
                                    type = 0;
#endif
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

                // ���̂܂܂Ȃ�G���[
                if ( !isJoint ) {
                    MainForm.setStatus( "�\���G���[: " + path );
                    //return false;
                }
                // chunk�����r���[���Ƒ����������A������������̓f�[�^�����������悤��

                // �Ō�̎c��
                if ( startIndex < ( file.Length - 1 ) ) {
                    string sub = file.Substring( startIndex ); // ��
                    // ��Ȃ��΂�
                    if ( sub.Length > 0 ) {
                        //sub = sub.Replace( "}", "" );// �ʖڕ����딚���˂��H
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

                // �ςȃf�[�^���ƃG���[�`�F�b�N�߂�ǂ����������Ȃ�

            }
            if ( original ) {
                // read only�ɂ��������c�A�N�Z�b�T�łȂ�Ƃ�����ƁH
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
                            // �s���������������Ă��Ȃ���
                            text = node.Modify.Replace( '�@', ' ' ); // �S�p�X�y�[�X�𔼊p�X�y�[�X�ɒu��
                            //if( text.IndexOf( '{' ) > -1 ) {
                            text = text.Replace( "{", "" );
                            //}
                            //if( text.IndexOf( '}' ) > -1 ) {
                            text = text.Replace( "}", "" );
                            //}
                        }
                        data += "{" + text + "}";
                        if ( node.Editable ) {
                            // �ύX�̔��f
                            // �S���R�s�[�Ɣ�r���Ă���R�s�[�͂ǂ������悢�H
                            //node.Original = node.Modify.Clone() as string;
                            node.setOriginal( text, this );
                            // mod���܂��Ȃ�original=mod�̏ꍇ�ǂݒ����Ȃ���original�̕\�������������Ȃ�
                            // �̂�����������Ȃ��ƕύX�`�F�b�N���o���Ȃ��B
                            // dirty�t���O�ɂ���̂��H
                        }
                    }
                }
            }

            System.IO.FileInfo info = new System.IO.FileInfo( path );
            if ( !info.Exists ) {
            }
            // TODO:�g���q��filetype�̃`�F�b�N

            if ( System.IO.Directory.Exists( info.DirectoryName ) == false ) {
                System.IO.Directory.CreateDirectory( info.DirectoryName );
            }

            using ( System.IO.StreamWriter writer = new System.IO.StreamWriter( path, false, System.Text.Encoding.GetEncoding( "shift_jis" ) ) ) {
                //writer.AutoFlush = true; // ������������
                writer.Write( data );
            }
        }

        // �s���Ƀt�@�C������ID��t�^����
        public void addPrefixFileNumber() {
            // gamearea.mes�Ȃǂ̃t�@�C���S�̂ł��Ƃ܂������
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
                        // ���̔�r�񐔂���肽��
                        string file = System.IO.Path.GetFileNameWithoutExtension( name );
                        if ( this.Type == FileType.MES ) {
                            string prefix = "(" + file + ".M-" + chunk.ID.Original;
                            Node mes = chunk.Message;
                            if ( checkIgnore( mes.Modify ) ) {
                                mes.Modify = prefix + ")" + mes.Modify;
                            }
                        } else if ( this.Type == FileType.DLG ) {
                            string prefix = "(" + file;
                            // 5�������̂�
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

        // gamearea.mes�Ȃǂ̃t�@�C���S�̂ł��Ƃ܂������
        bool checkIgnoreByFileName() {
            string name = System.IO.Path.GetFileName( this.FilePath );
            if ( name.ToLower() == "gamearea.mes" ) {
                return false;
            }
            return true;
        }
        public static bool checkIgnore( string line ) {
            string text = line.TrimStart(); // �擪�ɋ󔒂���������Ă���ꍇ������̂�
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
                            // �{�Ȃǂ̏����w��Ő擪�̐����c�Ƃ����p�^�[���ɂ͑Ή�������Ȃ�
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
            string text = line.TrimStart(); // �擪�ɋ󔒂���������Ă���ꍇ������̂�
            // empty
            if ( text != null && text.Length > 1 ) {
                // var code
                // �e���v���[�g��������
                if ( text[ 1 ] != ':' ) {
                    // �{�Ȃǂ̏����w��Ő擪�̐����c�Ƃ����p�^�[���ɂ͑Ή�������Ȃ���
                    //int output = 0;
                    //if ( int.TryParse( text, out output ) == false ) {
                    return true;
                    //}
                }

            } else {
                // 1���������Ȃ�
                // femaleline�� 1:maleline��PC�j���䎌 0:maleline��PC�����䎌
                // ����Ȃ��Ă��ǂ�����
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
                    // ��O�����������������H
#if false // �d��
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

        // ��anti-node{}��}{��node���ō\�������ϒ��̃��X�g���Ȃ�
        // ���̂ݕҏW�\
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
            //bool dirty; // ������ĕۑ��̊Ǘ����Ă�ƁA���ɖ߂��ł̊Ǘ����߂�ǂ��Ȃ񂾂��
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
                // clear�͂������Ⴄ��
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

        // �Ȃ�ŏ�̂̓��X�g�łȂ��́H
        //public System.Collections.Generic.List<Chunk> Chunks = new System.Collections.Generic.List<Chunk>();

    }
}
