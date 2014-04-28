using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;  // referenced library to handle sound in Windows

namespace StepDX
{
    class GameAudio
    {
        private Device SoundDevice = null;  // declare sound playing device

        // declare buffers to hold each sound file
        private SecondaryBuffer bgd = null;
        private SecondaryBuffer jmp = null;
        private SecondaryBuffer ovr = null;
        private SecondaryBuffer pnt = null;



        // initialize sound device and all loaded sound files
        public GameAudio(Form form)
        {
            SoundDevice = new Device();
            SoundDevice.SetCooperativeLevel(form, CooperativeLevel.Priority);

            // call Load() to load in each sound file to be played
            Load(ref bgd, "../../audio/Background1.wav");
            Load(ref ovr, "../../audio/Background2.wav");
            Load(ref jmp, "../../audio/Jump1.wav");
            Load(ref pnt, "../../audio/Point1.wav");


        }


        // function to load sound to be played into appropriate buffer
        private void Load(ref SecondaryBuffer buffer, string filename)
        {
            try
            {
                buffer = new SecondaryBuffer(filename, SoundDevice);
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to load " + filename,
                                "Sound Load Error", MessageBoxButtons.OK);
                buffer = null;
            }
        }


        // function to begin playing background music, called with each advance() in game
        public void BGD(bool gameover = false)
        {
            if (bgd == null)
                return;  // if sound not loaded properly, do nothing

            if (!bgd.Status.Playing)
                bgd.Play(0, BufferPlayFlags.Default);  // if sound not currently playing, play it

            if (gameover == true) { bgd.Stop(); }
        }


        // function to begin playing jump sound effect
        public void JMP()
        {
            if (jmp == null)
                return;  // if sound not loaded properly, do nothing

            if (!jmp.Status.Playing)
                jmp.Play(0, BufferPlayFlags.Default);  // if sound not currently playing, play it
        }

        // function to begin playing Game Over music, called in advance()
        public void OVR()
        {
            if (ovr == null)
                return;  // if sound not loaded properly, do nothing

            if (!ovr.Status.Playing)
                ovr.Play(0, BufferPlayFlags.Default);  // if sound not currently playing, play it
        }

        // function to play sound when a point is earned
        public void PNT()
        {
            if (pnt == null)
                return;  // if sound not loaded properly, do nothing

            if (!pnt.Status.Playing)
                pnt.Play(0, BufferPlayFlags.Default);  // if sound not currently playing, play it
        }

    }
}
