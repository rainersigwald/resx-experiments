using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp12.Properties;

namespace WindowsFormsApp12
{
    class MediaPlayer
    {
        System.Media.SoundPlayer soundPlayer;
        public MediaPlayer()
        {
            soundPlayer = new System.Media.SoundPlayer(Resources.Ring08);
        }
        public void Play() { soundPlayer.Play(); }
        public void Play(byte[] buffer)
        {
            soundPlayer.Stream.Seek(0, SeekOrigin.Begin);
            soundPlayer.Stream.Write(buffer, 0, buffer.Length);
            soundPlayer.Play();
        }
    }
}
