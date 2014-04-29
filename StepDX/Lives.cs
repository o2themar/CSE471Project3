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
    class Lives
    {

        // Number of lives
        private int lives_num = 0;

        public int lives_number { set { lives_num = value; } get { return lives_num; } }

        // Font to use for writing
        private Microsoft.DirectX.Direct3D.Font font;

        public Lives(Device device)
        {

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

        // Adds num to the number of lives and returns false if the number of lives remaining is not positive
        public bool AddToLives(int num)
        {
            lives_num += num;

            return lives_num > 0;
        }

        public void DisplayLives()
        {
            // Show number of lives in upper left corner
            font.DrawText(null,     // Because I say so
                "Lives: " + lives_num,  // Text to draw
                new Point(10, 10),  // Location on the display (pixels with 0,0 as upper left)
                Color.LightCyan);   // Font color
        }

    }
}
