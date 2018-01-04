// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcanumJPEditor {
    public class IOController {

        // 削除系につき取り扱い注意
        public static void deleteDirectory( string path ) {
            // ディレクトリが読み取り専用かどううかとれんのだが
            // やっぱファイルを見ないとだめ？trueついてると問答無用かな
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo( path );
            if ( info.Exists && ( info.Attributes & System.IO.FileAttributes.ReadOnly ) != System.IO.FileAttributes.ReadOnly ) {
                info.Delete( true );
            }
        }

        public static bool validDirectory( string path ) {
            if ( System.IO.Directory.Exists( path ) ) {
                string name = System.IO.Path.GetFileName( path );
                if ( name.Length > 0 && name[ 0 ] != '.' ) {
                    return true;
                }
            }
            return false;
        }

        public static bool validDlgMesFile( string path ) {
            if ( System.IO.File.Exists( path ) ) {
                string name = System.IO.Path.GetExtension( path );
                if ( name.Length > 0 ) {
                    switch ( name.ToLower() ) {
                    case Path.Ext.DialogExtention:
                    case Path.Ext.MessageExtention:
                        return true;
                    }
                }
            }
            return false;
        }

        // ファイルの総数 再帰的
        // dlg,mesのみにした方がよいが、それならそもそも対象外の場所にあるこれらを除外しないと
        // これをつかっているのはプログレスバーのみなのでみかけが変になる程度か
        public static int getNumFiles( string name ) {
            int num = 0;
            getNumFiles( name, ref num );
            return num;
        }
        static void getNumFiles( string name, ref int num ) {
            { // dir part
                string current = System.IO.Directory.GetCurrentDirectory();
                if ( System.IO.Directory.Exists( name ) ) {
                    string dname = System.IO.Path.GetFileName( name );
                    dname = dname.ToLower();
                    if( dname == Path.Data.DialogDirectoryName || dname == Path.Data.MessageDirectoryName ) {
                        num += System.IO.Directory.GetFiles( name ).Length;
                    }
                    string[] dirs = System.IO.Directory.GetDirectories( name );
                    foreach ( string dir in dirs ) {
                        if ( validDirectory( dir ) ) {
                            getNumFiles( dir, ref num );
                        }
                    }
                }

            }
        }

    }
}
