// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcanumJPEditor {
    public class Config {
        public Config() {
        }
        public string ArcanumDirectory;
        public string TextEditorPath;
        public int SpeechVolume = 100;
    }
    public class History {
        public History() {
        }
        public List<string> Path = new List<string>();
    }
}
