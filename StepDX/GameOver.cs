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
    class GameOver
    {

        // boolean to keep track of if a game over has occurred
        private bool game_over = false;

        public bool gameOver { set { game_over = value; } get { return game_over; } }

        // Font to use for writing
        private Microsoft.DirectX.Direct3D.Font font;

        public GameOver(Device device)
        {

            //create the font
            font = new Microsoft.DirectX.Direct3D.Font(device,  // Device we are drawing on
                50,         // Font height in pixels
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

        public void DisplayGameOver()
        {
            // Show a game over text
            font.DrawText(null,     // Because I say so
                "Game Over",  // Text to draw
                new Point(300, 150),  // Location on the display (pixels with 0,0 as upper left)
                Color.Red);   // Font color

        }
    }
}
