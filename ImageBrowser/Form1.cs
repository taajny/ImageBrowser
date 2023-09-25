using ImageBrowser.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImageBrowser
{
    public partial class Form1 : Form
    {
        private List<string> _listOfFiles = new List<string>();
        private int _positionOnList = 0;

        public Form1()
        {
            InitializeComponent();
            ofdOpenPicture.Title = "Wybierz proszę plik graficzny do wyświetlenia:";
            ofdOpenPicture.Filter = "JPG (*.jpg,*.jpeg)|*.jpg;*.jpeg";

            if (!string.IsNullOrWhiteSpace(Settings.Default.LastOpenedFile))
            {
                ofdOpenPicture.InitialDirectory = Path.GetDirectoryName(Settings.Default.LastOpenedFile);
                DisplayPicture(Settings.Default.LastOpenedFile);

            }
            else
            {
                btnClose.Visible = false;
                ofdOpenPicture.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
        }

        private void btnOpenPicture_Click(object sender, EventArgs e)
        {
            if (ofdOpenPicture.ShowDialog() == DialogResult.OK)
            {
                DisplayPicture(ofdOpenPicture.FileName);
                ofdOpenPicture.InitialDirectory = Path.GetDirectoryName(ofdOpenPicture.FileName);
            }
        }

        private void DisplayPicture(string filePatch)
        {
            pbxMainPicture.Show();
            pbxMainPicture.ImageLocation = filePatch;
            pbxMainPicture.Load();
            btnClose.Visible = true;

            _listOfFiles = Directory.GetFiles(Path.GetDirectoryName(filePatch), "*.jp*g").ToList();
            _positionOnList = _listOfFiles.FindIndex(f => f.Equals(filePatch));
            
            if (_positionOnList > 0)
                btnPrevious.Visible = true;
            else
                btnPrevious.Visible = false;

            if (_positionOnList < _listOfFiles.Count() - 1)
                btnNextImage.Visible = true;
            else
                btnNextImage.Visible = false;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ( string.IsNullOrWhiteSpace(pbxMainPicture.ImageLocation))
                Settings.Default.LastOpenedFile = string.Empty;
            else
                Settings.Default.LastOpenedFile = pbxMainPicture.ImageLocation;
            Settings.Default.Save();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            pbxMainPicture.ImageLocation = string.Empty;
            pbxMainPicture.Hide();
            btnClose.Visible = false;
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            DisplayPicture(_listOfFiles[_positionOnList - 1].ToString());
        }

        private void btnNextImage_Click(object sender, EventArgs e)
        {
            DisplayPicture(_listOfFiles[_positionOnList + 1].ToString());
        }
    }
}
