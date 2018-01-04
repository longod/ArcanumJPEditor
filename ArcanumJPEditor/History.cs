// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcanumJPEditor {
    public class HistoryManager {

        internal class Controller {
            internal class History {
                public int start;
                public int length;
                public string text;
            }
            private Controller() {
            }
            internal Controller( string key ) {
                this.name = key;
            }

            internal void TextChange( System.Windows.Forms.TextBox box ) {
                if ( flag ) {
                    flag = false;
                } else {
                    //System.Console.WriteLine( "TextChange: " + name + " undo: " + undo_history.Count + " redo: " + redo_history.Count );
                    History history = new History();
                    history.start = box.SelectionStart;
                    history.length = box.SelectionLength;
                    history.text = box.Text; // 差分でやりたいんだけど？

                    undo_history.Add( history );
                    redo_history.Clear(); // やりなおせない
                }
            }
            internal void Undo( System.Windows.Forms.TextBox box ) {
                if ( undo_history.Count > 1 ) {
                    //System.Console.WriteLine( "Undo: " + name + " undo: " + undo_history.Count + " redo: " + redo_history.Count );
                    flag = true;
                    History history = undo_history[ undo_history.Count - 1 ];
                    redo_history.Add( history );
                    undo_history.RemoveAt( undo_history.Count - 1 );
                    history = undo_history[ undo_history.Count - 1 ];
                    box.Text = history.text;
                    box.SelectionStart = history.start;
                    box.SelectionLength = history.length;
                    box.ScrollToCaret();
                }
            }
            internal void Redo( System.Windows.Forms.TextBox box ) {
                if ( redo_history.Count > 0 ) {
                    //System.Console.WriteLine( "Redo: " + name + " undo: " + undo_history.Count + " redo: " + redo_history.Count );
                    flag = true;
                    History history = redo_history[ redo_history.Count - 1 ];
                    redo_history.RemoveAt( redo_history.Count - 1 );
                    undo_history.Add( history );
                    box.Text = history.text;
                    box.SelectionStart = history.start;
                    box.SelectionLength = history.length;
                    box.ScrollToCaret();
                }
            }

            // ノード切り替えると何個も履歴が作られちゃうな
            // これで回避できるかな？
            internal void NodeChange() {
                //System.Console.WriteLine( "NodeChange: " + name + " undo: " + undo_history.Count + " redo: " + redo_history.Count );
                flag = true;
            }
            
            internal void Clear() {
                undo_history.Clear();
                redo_history.Clear();
            }
            const int cap = 1024;
            List<History> undo_history = new List<History>( cap );
            List<History> redo_history = new List<History>( cap );
            bool flag = false;
            string name;

        }

        System.Collections.Hashtable controller = new System.Collections.Hashtable();

        public void NodeChange( string key ) {
            Controller con = controller[ key ] as Controller;
            if ( con != null ) {
                con.NodeChange();
            }
        }
        public void TextChange( string key, System.Windows.Forms.TextBox box ) {
            Controller con = controller[ key ] as Controller;
            if ( con == null ) {
                con = new Controller( key );
                controller.Add( key, con );
                //System.Console.WriteLine( "Create: " + key );
            }

            con.TextChange( box );
        }
        public void Undo( string key, System.Windows.Forms.TextBox box ) {
            Controller con = controller[ key ] as Controller;
            if ( con != null ) {
                con.Undo( box );
            }
        }
        public void Redo( string key, System.Windows.Forms.TextBox box ) {
            Controller con = controller[ key ] as Controller;
            if ( con != null ) {
                con.Redo( box );
            }
        }
    }
}
