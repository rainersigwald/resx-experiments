using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string file1 = @"c:\Ring08.wav";
            List<byte> soundBytes = new List<byte>(File.ReadAllBytes(file1));
            //create media player loading the first half of the sound file
            MediaPlayer mPlayer = new MediaPlayer(soundBytes.ToArray());
            //begin playing the file
            mPlayer.Play();
        }
    }
}
