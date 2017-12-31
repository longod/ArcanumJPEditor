// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArcanumJPEditor {
    public partial class Setting : Form {
        Setting() {
            InitializeComponent();
        }
        public Setting( Config config ) {
            InitializeComponent();

            textBoxArcanum.Text = config.ArcanumDirectory;
            textBoxEditor.Text = config.TextEditorPath;
            trackBarSpeechVolume.Value = config.SpeechVolume;
            updateNumVolume();

            buttonArcanumSelector.Click += new EventHandler( ArcanumSelector_Click );
            buttonEditorSelector.Click += new EventHandler( EditorSelector_Click );
            trackBarSpeechVolume.ValueChanged += new EventHandler( SpeechVolume_ValueChanged );
        }


        void ArcanumSelector_Click( object sender, EventArgs e ) {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = @"Arcanum インストールフォルダを選択";
            if ( System.IO.Directory.Exists( textBoxArcanum.Text ) ) {
                dialog.SelectedPath = textBoxArcanum.Text;
            }
            DialogResult result = dialog.ShowDialog( this );
            if ( result == DialogResult.OK ) {
                textBoxArcanum.Text = dialog.SelectedPath;
            }
        }

        void EditorSelector_Click( object sender, EventArgs e ) {
            OpenFileDialog dialog = new OpenFileDialog();
            if ( System.IO.File.Exists( textBoxEditor.Text ) ) {
                System.IO.FileInfo info = new System.IO.FileInfo( textBoxEditor.Text );
                dialog.InitialDirectory = info.DirectoryName;
                dialog.FileName = info.FullName;
            }

            dialog.Filter = "実行ファイル(*.exe)|*.exe|すべてのファイル(*.*)|*.*";
            //タイトルを設定する
            dialog.Title = "テキストエディタを選択";
            dialog.RestoreDirectory = true;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            DialogResult result = dialog.ShowDialog(this);
            if ( result == DialogResult.OK ) {
                textBoxEditor.Text = dialog.FileName;
            }
        }

        void SpeechVolume_ValueChanged( object sender, EventArgs e ) {
            updateNumVolume();
        }

        public void restore( Config config ) {
            config.ArcanumDirectory = textBoxArcanum.Text;
            config.TextEditorPath = textBoxEditor.Text;
            config.SpeechVolume = trackBarSpeechVolume.Value;
        }

        void updateNumVolume() {
            groupBoxSpeechVolume.Text = @"ボリューム: " + trackBarSpeechVolume.Value + "%";
            SpeechPlayer.Volume = trackBarSpeechVolume.Value;
        }

    }
}