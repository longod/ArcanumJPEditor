// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcanumJPEditor {
    // これfileの中に持たせた方が管理良いのでないの？ originalでは不要になるが
#if false
    public class ProgressList {
        string[] ProgressText = {
            "未翻訳","翻訳中","翻訳済"
        };
        public ProgressList() {
        }
        public ProgressList( int size ) {
            Levels = new Progress[ size ];
        }
        public enum Progress {
            NotYetStarted, // 未着手
            Start,   // 着手
            Finish, // 完了
        }
        public string Description;
        public Progress[] Levels;
    }
#endif
    public struct ProgressCount {
        public ProgressCount( string name, int translated, int total, bool exist ) {
            Name = name;
            Translated = translated;
            Total = total;
            Exist = exist;
        }
        public string Name;
        public int Translated;
        public int Total;
        public bool Exist;
    }
    public class ProgressFile {
        public ProgressFile() {
        }
        //public System.Collections.Hashtable Ignore = new System.Collections.Hashtable();
        //public System.Collections.Hashtable Pending = new System.Collections.Hashtable();
        // setが無いのでこれ使うよ
        public List<string> Ignore = new List<string>();
        public List<string> Pending = new List<string>();
    }

}
