// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcanumJPEditor {
    public class SpeechPlayer {
        static SpeechPlayer instance = null;
        // 複数のdlgタブが存在するため多分単一インスタンスに
        // なんか色々と命名規則が.netとは違うな
        WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
        object speaker = null; // 誰が鳴らしたか
        static int volume = 100;
        public static int Volume {
            get {
                return volume; 
            }
            set {
                volume = value;
                if ( instance != null ) {
                    instance.player.settings.volume = volume;
                }
            }
        }
        // 3.0からSystem.Windows.Media.MediaPlayerがあるらしい System.Mediaはwaveのみ

        SpeechPlayer() {
            //player.uiMode = "invisible";
            //player.
            //player.settings.volume = 10;// maybe 0-100 これいじるとwmpの値がかわるわけではないらしい？
        }

        public static string exists( string path, string id, bool female ) {
            // ファイル名、頭5桁の数字
            if ( IOController.validDlgMesFile( path ) ) {
                string file = System.IO.Path.GetFileNameWithoutExtension( path );
                if ( file.Length > 5 ) {
                    string dir = file.Substring( 0, 5 ); // 数かどうかもチェックした方が良いが
                    // slide.mesの音声再生があるが、あれは構造が異なる
                    string speech = Path.Data.ModuleDirectory + @"Sound\Speech\" + dir + Path.Backslash;
                    if ( System.IO.Directory.Exists( speech ) ) {
                        string prefix = @"v";
                        string m = @"_m";
                        string f = @"_f";
                        string ext = Path.Ext.SpeechExtention;

                        // 多分ありえないだろうが、呼び出し元で符号とっておくこと
                        string name = prefix + id;
                        if ( female ) {
                            string fpath = speech + name + f + ext;
                            if ( System.IO.File.Exists( fpath ) ) {
                                return fpath;
                            }
                        }

                        string mpath = speech + name + m + ext;
                        if ( System.IO.File.Exists( mpath ) ) {
                            return mpath;
                        }
                    }
                }
            }
            return null;
        }

        public static bool play( object sender, string path ) {
            stop( sender );
            if( System.IO.File.Exists( path ) ) {
                createInstance();
                Volume = Volume; // settings側のvolume設定
                instance.player.URL = path;
                instance.player.controls.play();
                instance.speaker = sender;
                return true;
            }
            return false;
        }
        public static bool stop( object sender ) {
            if( instance != null ) {
                if( sender == null || ( sender != null && sender == instance.speaker ) ) {
                    instance.player.controls.stop();
                    instance.speaker = null;
                    return true;
                }
            }
            return false;
        }

        static void createInstance() {
            if ( instance == null ) {
                instance = new SpeechPlayer();
            }
        }

        // pauseなどはいらんかな？
        // 勉強ツールなら時間帯指定によるリピートとか欲しいが、あくまで喋りの雰囲気が欲しいだけだし

    }
}
