// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArcanumJPEditor {
    public partial class ConsoleLog : Form {
        public ConsoleLog() {
            InitializeComponent();
        }
        public TextBox Log {
            get { return textBox1; }
        }
    
    }
}