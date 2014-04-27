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
    class ScoreFlying
    {
        // time that score item was initialized
        private double starttime = 0;

        public double StartTime { set { starttime = value; } get { return starttime; } }

        // score that is being displayed
        private int scorenum = 0;

        public int Score { set { scorenum = value; } get { return scorenum; } }

        // position to display score
        private Point pos = new Point( 0, 0 );

        public Point Position { set { pos = value; } get { return pos; } }

        //Display flying score
        public void DisplayScore(Microsoft.DirectX.Direct3D.Font font)
        {
            font.DrawText(null, "+" + scorenum, pos, Color.LightCyan);
        }

    }
}
