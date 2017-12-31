// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcanumJPEditor {
    public class Path {
        public static string ExecutablePath;
        public static string ExecutableFile;
        public static string ExecutableDirectory;

        public const string Backslash = @"\";

        public class Data {
            public const string BaseDirectoryName = @"Arcanum";
            public const string BaseDirectory = BaseDirectoryName + Backslash;
            public const string ModuleDirectory = BaseDirectory + @"modules\Arcanum" + Backslash;
            public const string ModifiedDirectory = @"mod\";
            public const string TemporaryDirectory = @"temp\";
            public const string ConvertedDirectory = @"conv\";

            public const string DialogDirectoryName = @"dlg";
            public const string DialogDirectory = DialogDirectoryName + Backslash;
            public const string MessageDirectoryName = @"mes";
            public const string MessageDirectory = MessageDirectoryName + Backslash;

            public const string MainFile = @"Arcanum9.dat";
            public const string ModuleFile = @"Arcanum.PATCH7";
        } 
        public class Ext {
            public const string DialogExtention = @".dlg";
            public const string MessageExtention = @".mes";
            public const string SpeechExtention = @".mp3";
        }
        public class Tools {
            public const string NotepadFile = @"notepad";
            public const string ReplacerFile = @"adr.exe";
            public const string MakerFile = @"dbmaker.exe";
            public const string UnpackerFile = @"unpack.bat";
        }
        public class Xml {
            public const string ConfigFile = @"config.xml";
            public const string FormatFile = @"format.xml";
            public const string HistoryFile = @"history.xml";
            public const string ProgressFile = @"progress.xml";
        }
#if false // current‚ðexecutable‚É‚µ‚½‚Ì‚Å•s—v
        static string convineFullPath( string path ) {
            return ExecutableDirectory + @"\" + path;
        }
#endif
    }
}
