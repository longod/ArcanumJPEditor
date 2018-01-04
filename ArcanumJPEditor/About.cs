// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArcanumJPEditor {
    public partial class About : Form {
        string url = @"http://blackmasqueradegames.com/";

        public About() {
            InitializeComponent();
        }

        public About( string name, string version ) {
            InitializeComponent();
            this.Text = name;
            label1.Text = name;
            label2.Text += " " + version;
            linkLabel1.Text = url;
            this.Load += new EventHandler( About_Load );
        }

        void About_Load( object sender, EventArgs e ) {
            this.Location = new Point( this.Owner.Location.X + this.Owner.Size.Width / 2 - this.Size.Width / 2, this.Owner.Location.Y + this.Owner.Size.Height / 2 - this.Size.Height / 2 );
        }


        private void buttonOK_Click( object sender, EventArgs e ) {
            this.Close();
        }

        private void linkLabel1_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e ) {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start( url );
        }
    }
}