// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArcanumJPEditor {
    public partial class Search : Form {
        public class Option {
            public enum Range {
                // 上三つは効率よくできるかどうかわからん。いったん開いたらキャッシュしないときついだろうなあ。
                //ByAll,
                //ByMes,
                //ByDlg,
                BySelected,
                ByOpened,
                ByFileName,
            }
            public Option() {
            }
            public Option( Option option ) {
                this.search_word = option.search_word;
                this.range = option.range;

                this.id = option.id;
                this.line = option.line;
                this.integer = option.integer;
                this.test  = option.test;
                this.result = option.result;
                this.resid = option.resid;

                this.origin = option.origin;
                this.complete = option.complete;
                this.charactor = option.charactor;
                this.repeat = option.repeat;
                this.regex = option.regex;
            }
            public string search_word = "";
            public Range range = ( Range )0;

            public bool id = false;
            public bool line = true;
            public bool integer = false;
            public bool test = false;
            public bool result = false;
            public bool resid = false;

            public bool origin = false;
            public bool complete = false;
            public bool charactor = false;
            public bool repeat = false;
            public bool regex = false;
        };
        string[] RangeName = {
            //"全てのファイル",
            //"MESファイル",
            //"DLGファイル",
            "選択されているタブ",
            "開いているタブ",
            "ファイル名",
        };

        Search() {
            InitializeComponent();
        }
        public Search( Option option ) {
            InitializeComponent();
            comboBoxRange.Items.AddRange( RangeName );

            textBoxSearch.Text = option.search_word;
            comboBoxRange.SelectedIndex = ( int )option.range;

            checkBoxID.Checked = option.id;
            checkBoxLine.Checked = option.line;
            checkBoxInt.Checked = option.integer;
            checkBoxTest.Checked = option.test;
            checkBoxResult.Checked = option.result;
            checkBoxResID.Checked = option.resid;

            checkBoxOrigin.Checked = option.origin;
            checkBoxComplete.Checked = option.complete;
            checkBoxChar.Checked = option.charactor;
            checkBoxRepeat.Checked = option.repeat;
            checkBoxRegex.Checked = option.regex;
            
            buttonClose.Click += new EventHandler( Close_Click );
        }

        void Close_Click( object sender, EventArgs e ) {
            this.Close();
        }
        public void restore( Option option ) {
            option.search_word = textBoxSearch.Text;
            option.range = ( Search.Option.Range )comboBoxRange.SelectedIndex;
    
            option.id = checkBoxID.Checked;
            option.line = checkBoxLine.Checked;
            option.integer = checkBoxInt.Checked;
            option.test = checkBoxTest.Checked;
            option.result = checkBoxResult.Checked;
            option.resid = checkBoxResID.Checked;

            option.origin = checkBoxOrigin.Checked;
            option.complete = checkBoxComplete.Checked;
            option.charactor = checkBoxChar.Checked;
            option.repeat = checkBoxRepeat.Checked;
            option.regex = checkBoxRegex.Checked;
        }
        public Button ButtonSearch {
            get { return buttonSearch; }
        }
        public Button ButtonSearchPrev {
            get { return buttonSearchPrev; }
        }
        public TextBox TextBoxSearch {
            get { return textBoxSearch; }
        }
    }
}