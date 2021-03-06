﻿using System;
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
        private float speed = 1f;

        /// <summary>
        /// How high we go
        /// </summary>
        private float maxWidth = 1f;

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

        public override void Render(Device device)
        {
            // Get the vertices we will draw
            List<Vector2> vertices = Vertices;


            // Ensure the number of vertices and textures are the same
            System.Diagnostics.Debug.Assert(textureC.Count == vertices.Count);

            if (verticesV == null)
            {
                verticesV = new VertexBuffer(typeof(CustomVertex.PositionColoredTextured), // Type
                   vertices.Count,      // How many
                   device, // What device
                   0,      // No special usage
                   CustomVertex.PositionColoredTextured.Format,
                   Pool.Managed);
            }

            GraphicsStream gs = verticesV.Lock(0,0,0);     // Lock the background vertex list
            int clr = color.ToArgb();

            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 v = vertices[i];
                Vector2 t = textureC[i];
                gs.Write(new CustomVertex.PositionColoredTextured(v.X, v.Y, 0, clr, t.X, t.Y));
            }

            verticesV.Unlock();

            if (transparent)
            {
                device.RenderState.AlphaBlendEnable = true;
                device.RenderState.SourceBlend = Blend.SourceAlpha;
                device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
            }

            device.SetTexture(0, texture);
            device.SetStreamSource(0, verticesV, 0);
            device.VertexFormat = CustomVertex.PositionColoredTextured.Format;
            device.DrawPrimitives(PrimitiveType.TriangleFan, 0, vertices.Count - 2);
            device.SetTexture(0, null);

            if (transparent)
                device.RenderState.AlphaBlendEnable = false;


        }


        /// <summary>
        /// Advance the platform animation in time
        /// </summary>
        /// <param name="dt">The delta time in seconds</param>
        public override void Advance(float dt)
        {
            time += dt;

            // I'm going to base my width entirely on the current time.
            // From 0 to speed, we are going right, speed to 2*speed we are 
            // lefting.  So we need to know what step we are in.
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
