// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcanumJPEditor {
    // ����file�̒��Ɏ������������Ǘ��ǂ��̂łȂ��́H original�ł͕s�v�ɂȂ邪
#if false
    public class ProgressList {
        string[] ProgressText = {
            "���|��","�|��","�|���"
        };
        public ProgressList() {
        }
        public ProgressList( int size ) {
            Levels = new Progress[ size ];
        }
        public enum Progress {
            NotYetStarted, // ������
            Start,   // ����
            Finish, // ����
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
        // set�������̂ł���g����
        public List<string> Ignore = new List<string>();
        public List<string> Pending = new List<string>();
    }

}
