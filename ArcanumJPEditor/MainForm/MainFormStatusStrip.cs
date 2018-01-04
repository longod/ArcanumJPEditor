// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArcanumJPEditor {
    public partial class MainForm {
        static public void initializeProgressBar( int max ) {
            Instance.toolStripProgressBar.Minimum = 0;
            Instance.toolStripProgressBar.Maximum = max;
            Instance.toolStripProgressBar.Value = 0;
        }
        static public void incrementProgressBar() {
            if ( Instance.toolStripProgressBar.Value < Instance.toolStripProgressBar.Maximum ) {
                ++Instance.toolStripProgressBar.Value;
            }
        }
        static public void completeProgressBar() {
            Instance.toolStripProgressBar.Value = Instance.toolStripProgressBar.Maximum;
        }

        static public void setStatus( string text ) {
#if DEBUG
            System.Console.Write( text + "\n" );
#endif
            if ( text.Length > 0 && Instance.console != null ) {
                if ( Instance.console.Log.Text.Length > 0 ) {
                    Instance.console.Log.Text += "\r\n";
                }
                Instance.console.Log.Text += text;
                Instance.console.Log.Update();
            }
            Instance.toolStripStatusLabel.Text = text;
            Instance.Update();
        }
        static public void addStatus( string text ) {
#if DEBUG
            System.Console.Write( text + "\n" );
#endif
            if ( Instance.console != null ) {
                Instance.console.Log.Text += text;
                Instance.console.Log.Update();
            }
            Instance.toolStripStatusLabel.Text += text;
            Instance.Update();
        }
        static public string getStatus() {
            return Instance.toolStripStatusLabel.Text;
            //return null;
        }

        static public void setTitle( string text ) {
            Instance.Text = Instance.title + " " + Instance.version + " " + text;
        }
    }
}
