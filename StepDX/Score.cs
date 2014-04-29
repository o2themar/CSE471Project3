using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace StepDX
{
    class Score
    {

        // Total score
        private int scorenum = 0;

        private Device device = null;

        // Font to use for writing
        private Microsoft.DirectX.Direct3D.Font font;

        // List to hold flying scores
        private List<ScoreFlying> FlyingScores = new List<ScoreFlying>();

        public Score(Device dev)
        {
            device = dev;

            //create the font
            font = new Microsoft.DirectX.Direct3D.Font(device,  // Device we are drawing on
                20,         // Font height in pixels
                0,          // Font width in pixels or zero to match height
                FontWeight.Bold,    // Font weight (Normal, Bold, etc.)
                0,          // mip levels (0 for default)
                false,      // italics?
                CharacterSet.Default,   // Character set to use
                Precision.Default,      // The font precision, try some of them...
                FontQuality.Default,    // Quality?
                PitchAndFamily.FamilyDoNotCare,     // Pitch and family, we don't care
                "Arial");               // And the name of the font
        }

        public void AddToScore(int num)
        {
            scorenum += num;
        }

        public void AddFlyingScore(int num, int x, int y, double time)
        {
            ScoreFlying fs = new ScoreFlying();

            fs.Score = num;
            fs.Position = new Point(x, y);
            fs.StartTime = time;

            //add flying score to list of flying scores
            FlyingScores.Add(fs);

            //Add score to total score
            AddToScore(num);

        }

        public void DisplayScore()
        {
          

            // Show score in upper left corner
            font.DrawText(null,     // Because I say so
                "Score: " + scorenum,  // Text to draw
                new Point(10, 25),  // Location on the display (pixels with 0,0 as upper left)
                Color.LightCyan);   // Font color

            foreach (ScoreFlying s in FlyingScores)
            {
                s.DisplayScore(font);
            }

        }

        public void Advance(double time)
        {

            List<ScoreFlying> ToRemove = new List<ScoreFlying>();

            // Mark all flying scores that have been displayed too long for removal
            foreach (ScoreFlying s in FlyingScores)
            {
                if (time > s.StartTime + 1000)
                {
                    ToRemove.Add(s);
                }
            }

            // Remove all flying scores marked for removal
            foreach (ScoreFlying s in ToRemove)
            {
                FlyingScores.Remove(s);
            }
        }


         public void DisplayFinalScore()
        {
            //create the font
           

            if (scorenum <= 2048)
            {
                font.DrawText(null, "You Have Been Burninated!!!",
                    new Point(300, 190),
                    Color.Orange);
            }
            else
            {
                font.DrawText(null, "You fought Valiantly!!!",
                    new Point(180, 190),
                    Color.Yellow);
                font.DrawText(null, "...until you died!!!",
                    new Point(210, 230),
                    Color.Yellow);
            }
        }

    }
}
