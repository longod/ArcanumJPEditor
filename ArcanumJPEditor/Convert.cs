// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcanumJPEditor {
    public class Convert {

        static public void convert( string name ) {
            if ( System.IO.Directory.Exists( name ) ) {
                { // dir part
                    string current = System.IO.Directory.GetCurrentDirectory();
                    string[] dirs = System.IO.Directory.GetDirectories( name );
                    foreach ( string dir in dirs ) {
                        if ( IOController.validDirectory( dir ) ) {  
                            convert( dir );
                        }
                    }
                }

                { // file part
                              
                    string dname = System.IO.Path.GetFileName( name );
                    dname = dname.ToLower();
                    if( dname == Path.Data.DialogDirectoryName || dname == Path.Data.MessageDirectoryName ) {
                        string[] files = System.IO.Directory.GetFiles( name );
                        string mod = Path.Data.ModifiedDirectory;
                        foreach( string file in files ) {
                            if( IOController.validDlgMesFile( file ) ) {
                                int pos = file.IndexOf( mod );
                                string outfile = file.Substring( pos + mod.Length );

                                runConvertProcess( file, Path.Data.ConvertedDirectory + outfile, false );
                            }
                            MainForm.incrementProgressBar();

                        }
                    }
                }
            }
        }
        static public void convertPrefix( string name ) {
            if ( System.IO.Directory.Exists( name ) ) {
                { // dir part
                    string current = System.IO.Directory.GetCurrentDirectory();
                    string[] dirs = System.IO.Directory.GetDirectories( name );
                    foreach ( string dir in dirs ) {
                        if ( IOController.validDirectory( dir ) ) {
                            convertPrefix( dir );
                        }
                    }

                }
                { // file part
                    string dname = System.IO.Path.GetFileName( name );
                    dname = dname.ToLower();
                    if( dname == Path.Data.DialogDirectoryName || dname == Path.Data.MessageDirectoryName ) {
                        string[] files = System.IO.Directory.GetFiles( name );
                        string mod = Path.Data.ModifiedDirectory;
                        foreach( string file in files ) {
                            if( IOController.validDlgMesFile( file ) ) {
                                int pos = file.IndexOf( mod );
                                string outfile = file.Substring( pos + mod.Length );

                                File prefix = new File();
                                prefix.open( file, false );
                                prefix.addPrefixFileNumber();
                                //prefix.save( tempname );
                                prefix.save( Path.Data.TemporaryDirectory + outfile );
                                // proc�͑ҋ@���Ȃ��ƃf�b�h���b�N�Ƃ�����苤�L�A�N�Z�X�ᔽ���N�����\����
                                // �܂Ƃ��ɓ����Ă��ǂݏ����̃^�C�~���O����ł͏����I���Ȃ������ɑO�̃t�@�C������ǂ���͂�
                                // �}���`�œ����Ă���̂��m�F
                                // temp�f�B���N�g���Ɉ�U�S���o���o�������o�����ق��������̂��Ȃ�
                                //runConvertProcess( tempname, outfile, true );
                                runConvertProcess( Path.Data.TemporaryDirectory + outfile, Path.Data.ConvertedDirectory + outfile, false );
                            }

                            MainForm.incrementProgressBar();
                        }
                    }
                }
            }
        }

        static public void convertPrefixOriginal( string name ) {
            if ( System.IO.Directory.Exists( name ) ) {
                { // dir part
                    string current = System.IO.Directory.GetCurrentDirectory();
                    string[] dirs = System.IO.Directory.GetDirectories( name );
                    foreach ( string dir in dirs ) {
                        if ( IOController.validDirectory( dir ) ) {
                            convertPrefixOriginal( dir );
                        }
                    }

                }
                { // file part
                    string dname = System.IO.Path.GetFileName( name );
                    dname = dname.ToLower();
                    if( dname == Path.Data.DialogDirectoryName || dname == Path.Data.MessageDirectoryName ) {
                        string[] files = System.IO.Directory.GetFiles( name );
                        foreach( string file in files ) {
                            if( IOController.validDlgMesFile( file ) ) {
                                string outfile = file;

                                File prefix = new File();
                                prefix.open( file, true );
                                prefix.addPrefixFileNumber();
                                //prefix.save( tempname );
                                prefix.save( Path.Data.TemporaryDirectory + outfile );
                                // proc�͑ҋ@���Ȃ��ƃf�b�h���b�N�Ƃ�����苤�L�A�N�Z�X�ᔽ���N�����\����
                                // �܂Ƃ��ɓ����Ă��ǂݏ����̃^�C�~���O����ł͏����I���Ȃ������ɑO�̃t�@�C������ǂ���͂�
                                // �}���`�œ����Ă���̂��m�F
                                //runConvertProcess( tempname, outfile, true );
                                runConvertProcess( Path.Data.TemporaryDirectory + outfile, Path.Data.ConvertedDirectory + outfile, false );
                            }
                            MainForm.incrementProgressBar();
                        }
                    }
                }
            }
        }
        
        static void runConvertProcess( string input, string output, bool waitExit ) {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.WorkingDirectory = Path.ExecutableDirectory;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler( ErrorDataReceived );
            proc.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler( OutputDataReceived );
            proc.StartInfo.FileName = Path.Tools.ReplacerFile;
            proc.StartInfo.Arguments = "\"" + input + "\" \"" + output + "\"";

            proc.Start();

            // wait���Ȃ��Ƒ����I���R�[�h�͎���Ă�]�T���������낤��wait����ƒx���񂾂��
            if ( waitExit ) {
                proc.WaitForExit();
#if false
                if ( proc.HasExited ) {
                    System.Console.WriteLine( "[ExitCode] " + proc.ExitCode );
                }
#endif
            }
#if false
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine(); // std::cerr or std::clog �g������o�͂����
#endif
        }
        static void OutputDataReceived( object sender, System.Diagnostics.DataReceivedEventArgs e ) {
            if ( e.Data != null && e.Data.Length > 0 ) {
                System.Console.WriteLine( "[Output] " + e.Data );
            }
        }

        static void ErrorDataReceived( object sender, System.Diagnostics.DataReceivedEventArgs e ) {
            if ( e.Data != null && e.Data.Length > 0 ) {
                System.Console.WriteLine( "[Error] " + e.Data );
            }
        }
    }
}
