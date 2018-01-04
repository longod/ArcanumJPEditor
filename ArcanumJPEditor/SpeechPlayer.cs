// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcanumJPEditor {
    public class SpeechPlayer {
        static SpeechPlayer instance = null;
        // ������dlg�^�u�����݂��邽�ߑ����P��C���X�^���X��
        // �Ȃ񂩐F�X�Ɩ����K����.net�Ƃ͈Ⴄ��
        WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
        object speaker = null; // �N���炵����
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
        // 3.0����System.Windows.Media.MediaPlayer������炵�� System.Media��wave�̂�

        SpeechPlayer() {
            //player.uiMode = "invisible";
            //player.
            //player.settings.volume = 10;// maybe 0-100 ���ꂢ�����wmp�̒l�������킯�ł͂Ȃ��炵���H
        }

        public static string exists( string path, string id, bool female ) {
            // �t�@�C�����A��5���̐���
            if ( IOController.validDlgMesFile( path ) ) {
                string file = System.IO.Path.GetFileNameWithoutExtension( path );
                if ( file.Length > 5 ) {
                    string dir = file.Substring( 0, 5 ); // �����ǂ������`�F�b�N���������ǂ���
                    // slide.mes�̉����Đ������邪�A����͍\�����قȂ�
                    string speech = Path.Data.ModuleDirectory + @"Sound\Speech\" + dir + Path.Backslash;
                    if ( System.IO.Directory.Exists( speech ) ) {
                        string prefix = @"v";
                        string m = @"_m";
                        string f = @"_f";
                        string ext = Path.Ext.SpeechExtention;

                        // �������肦�Ȃ����낤���A�Ăяo�����ŕ����Ƃ��Ă�������
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
                Volume = Volume; // settings����volume�ݒ�
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

        // pause�Ȃǂ͂���񂩂ȁH
        // �׋��c�[���Ȃ玞�ԑюw��ɂ�郊�s�[�g�Ƃ��~�������A�����܂Œ���̕��͋C���~������������

    }
}
