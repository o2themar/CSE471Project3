using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace StepDX
{
    public class Monster : Platform
    {

        /// <summary>
        /// The texture map we use for this polygon
        /// </summary>
        private Texture texture = null;

        /// <summary>
        /// The texture map we use for this polygon
        /// </summary>
        public Texture Tex { get { return texture; } set { texture = value; } }

        /// <summary>
        /// List of texture coordinates
        /// </summary>
        protected List<Vector2> textureC = new List<Vector2>();

        /// <summary>
        /// Indicates if the texture is transparent
        /// </summary>
        private bool transparent = false;

        /// <summary>
        /// Indicates if the texture is transparent
        /// </summary>
        public bool Transparent { set { transparent = value; } get { return transparent; } }

        /// <summary>
        /// Add a texture coordinate
        /// </summary>
        /// <param name="v">Texture coordinate</param>
        public void AddTex(Vector2 v)
        {
            textureC.Add(v);
        }

        /// <summary>
        /// Render the textured polygon
        /// </summary>
        /// <param name="device">Device to render onto</param>


        /// <summary>
        /// Vertices after we move them
        /// </summary>
        protected List<Vector2> verticesM = new List<Vector2>();

        /// <summary>
        /// Vertices after they have been moved
        /// </summary>
        public override List<Vector2> Vertices { get { return verticesM; } }

        /// <summary>
        /// Current time for the object
        /// </summary>
        private float time = 0;

        /// <summary>
        /// For saving the state
        /// </summary>
        private float saveTime = 0;

        /// <summary>
        /// Speed in meters per second
        /// </summary>
        private float speed = 1;

        /// <summary>
        /// How high we go
        /// </summary>
        private float maxWidth = 1;

        /// <summary>
        /// Save the current platform position state
        /// </summary>
        public void SaveState()
        {
            saveTime = time;
        }

        /// <summary>
        /// Restore the current platform position state
        /// </summary>
        public void RestoreState()
        {
            time = saveTime;
        }

        /// <summary>
        /// Advance the platform animation in time
        /// </summary>
        /// <param name="dt">The delta time in seconds</param>
        public override void Advance(float dt)
        {
            time += dt;

            // I'm going to base my width entirely on the current time.
            // From 0 to speed, we are rising, speed to 2*speed we are 
            // falling.  So we need to know what step we are in.
            float w;

            isMonster = true;  //we know it's a monster...

            int step = (int)(time / speed);
            if (step % 2 == 0)
            {
                // Even, Right
                w = maxWidth * (time - step * speed) / speed;
            }
            else
            {
                w = 1 - maxWidth * (time - step * speed) / speed;
            }

            // Move it
            verticesM.Clear();
            foreach (Vector2 v in verticesB)
            {
                verticesM.Add(v + new Vector2(w, 0));
            }

        }

    }
}
