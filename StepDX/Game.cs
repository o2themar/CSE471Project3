﻿using System;
using System.IO;
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
    public partial class Game : Form
    {
        /// This is a test change of file Game.cs
        /// <summary>
        /// The DirectX device we will draw on
        /// </summary>
        private Device device = null;

        GameAudio audio; 
        /// <summary>
        /// Height of our playing area (meters)
        /// </summary>
        private float playingH = 4;

        /// <summary>
        /// Width of our playing area (meters)
        /// </summary>
        private float playingW = 32;

        /// <summary>
        /// Vertex buffer for our drawing
        /// </summary>
        private VertexBuffer vertices = null;

        /// <summary>
        /// The background image class
        /// </summary>
        private Background background = null;

        /// <summary>
        /// What the last time reading was
        /// </summary>
        private long lastTime;

        /// <summary>
        /// A stopwatch to use to keep track of time
        /// </summary>
        private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        float playerMinY = 1.0f;                    // Minimum y allowed


        public bool gamedelay = false;

        /// <summary>
        /// All of the polygons that make up our world
        /// </summary>
        List<Polygon> world = new List<Polygon>();

        /// <summary>
        /// Our player sprite
        /// </summary>
        GameSprite player = new GameSprite();

        /// <summary>
        /// The collision testing subsystem
        /// </summary>
        Collision collision = new Collision();

        // boolean to track standing condition. set in SpacebarDown and Advance (velocity->0)
        private bool stood;

        // object to keep track of score
        private Score score;

        // object to keep track of lives
        private Lives lives;

        // object to handle game overs
        private GameOver game_over;


        public Game()
        {
            InitializeComponent();

            if (!InitializeDirect3D())
                return;

            vertices = new VertexBuffer(typeof(CustomVertex.PositionColored), // Type of vertex
                                        4,      // How many
                                        device, // What device
                                        0,      // No special usage
                                        CustomVertex.PositionColored.Format,
                                        Pool.Managed);

            string fileName = "mars.bmp";
            string bg_fileName = "bg_image.bmp";
            background = new Background(device, playingW, playingH, bg_fileName);

            // Add files for polygons here
            string pentegonfile = "../../pentagon.txt";
            string trianglefile = "../../triangle.txt";
            string trapizoidfile = "../../trapizoid.txt";
            string arrowfile = "../../arrow.txt";
            string tboxfile = "../../tbox.txt";

            audio = new GameAudio(this);

            score = new Score(device);

            lives = new Lives(device);
            lives.lives_number = 3;         // Set life count to 3

            game_over = new GameOver(device);

            // Determine the last time
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;

            Polygon floor = new Polygon();
            floor.AddVertex(new Vector2(0, 1));
            floor.AddVertex(new Vector2(playingW, 1));
            floor.AddVertex(new Vector2(playingW, 0.9f));
            floor.AddVertex(new Vector2(0, 0.9f));
            floor.Color = Color.CornflowerBlue;
            world.Add(floor);


            Polygon ceil = new Polygon();
            ceil.AddVertex(new Vector2(0, 1));
            ceil.AddVertex(new Vector2(playingW, 1));
            ceil.AddVertex(new Vector2(playingW, 0.9f));
            ceil.AddVertex(new Vector2(0, 0.9f));
            ceil.Color = Color.CornflowerBlue;
            world.Add(ceil);

            //AddObstacle(2, 3, 1.7f, 1.9f, Color.Crimson);
            //AddObstacle(4, 4.2f, 1, 2.1f, Color.Coral);
            //AddObstacle(5, 6, 2.2f, 2.4f, Color.BurlyWood);
            //AddObstacle(5.5f, 6.5f, 3.2f, 3.4f, Color.PeachPuff);
            //AddObstacle(6.5f, 7.5f, 2.5f, 2.7f, Color.Chocolate);

            Platform platform = new Platform();
            platform.AddVertex(new Vector2(18.2f, 2));
            platform.AddVertex(new Vector2(18.7f, 2));
            platform.AddVertex(new Vector2(18.7f, 1.8f));
            platform.AddVertex(new Vector2(18.2f, 1.8f));
           platform.Color = Color.CornflowerBlue;
            world.Add(platform);

            //FOR Adding trogdor to the monster polygon
            Texture texture6 = TextureLoader.FromFile(device, "../../trogdor1.png");


            Monster monster = new Monster();
            monster.Tex = texture6;
            monster.AddVertex(new Vector2(3, 1f));
            monster.AddTex(new Vector2(1,1));
            monster.AddVertex(new Vector2(3, 1.8f));
            monster.AddTex(new Vector2(1,0));
            monster.AddVertex(new Vector2(4, 1.8f));
            monster.AddTex(new Vector2(0, 0));
            monster.AddVertex(new Vector2(4, 1f));
            monster.AddTex(new Vector2(0, 1));
            monster.Color = Color.Transparent;
            monster.Transparent =true;
            world.Add(monster);


            Monster monster2 = new Monster();
            monster2.Tex = texture6;
            monster2.AddVertex(new Vector2(6, 1f));
            monster2.AddTex(new Vector2(1, 1));
            monster2.AddVertex(new Vector2(6, 1.8f));
            monster2.AddTex(new Vector2(1, 0));
            monster2.AddVertex(new Vector2(7, 1.8f));
            monster2.AddTex(new Vector2(0, 0));
            monster2.AddVertex(new Vector2(7, 1f));
            monster2.AddTex(new Vector2(0, 1));
            monster2.Color = Color.Transparent;
            monster2.Transparent = true;
            world.Add(monster2);

            Monster monster3 = new Monster();
            monster3.Tex = texture6;
            monster3.AddVertex(new Vector2(13, 1f));
            monster3.AddTex(new Vector2(1, 1));
            monster3.AddVertex(new Vector2(13, 2f));
            monster3.AddTex(new Vector2(1, 0));
            monster3.AddVertex(new Vector2(14, 2f));
            monster3.AddTex(new Vector2(0, 0));
            monster3.AddVertex(new Vector2(14, 1f));
            monster3.AddTex(new Vector2(0, 1));
            monster3.Color = Color.Transparent;
            monster3.Transparent = true;
            world.Add(monster3);

            Monster monster4 = new Monster();
            monster4.isBig = true;
            monster4.Tex = texture6;
            monster4.AddVertex(new Vector2(15, 1f));
            monster4.AddTex(new Vector2(1, 1));
            monster4.AddVertex(new Vector2(15, 2.8f));
            monster4.AddTex(new Vector2(1, 0));
            monster4.AddVertex(new Vector2(16, 2.8f));
            monster4.AddTex(new Vector2(0, 0));
            monster4.AddVertex(new Vector2(16, 1f));
            monster4.AddTex(new Vector2(0, 1));
            monster4.Color = Color.Transparent;
            monster4.Transparent = true;
            world.Add(monster4);


            Monster monster5 = new Monster();
            monster5.Tex = texture6;
            monster5.isBig = true;
            monster5.AddVertex(new Vector2(19, 1f));
            monster5.AddTex(new Vector2(1, 1));
            monster5.AddVertex(new Vector2(19, 2.3f));
            monster5.AddTex(new Vector2(1, 0));
            monster5.AddVertex(new Vector2(21, 2.3f));
            monster5.AddTex(new Vector2(0, 0));
            monster5.AddVertex(new Vector2(21, 1f));
            monster5.AddTex(new Vector2(0, 1));
            monster5.Color = Color.Transparent;
            monster5.Transparent = true;
            world.Add(monster5);

            Monster monster6 = new Monster();
            monster6.Tex = texture6;
            monster6.is748 = true;
            monster6.AddVertex(new Vector2(21, 1f));
            monster6.AddTex(new Vector2(1, 1));
            monster6.AddVertex(new Vector2(21, 3f));
            monster6.AddTex(new Vector2(1, 0));
            monster6.AddVertex(new Vector2(23, 3f));
            monster6.AddTex(new Vector2(0, 0));
            monster6.AddVertex(new Vector2(23, 1f));
            monster6.AddTex(new Vector2(0, 1));
            monster6.Color = Color.Transparent;
            monster6.Transparent = true;
            world.Add(monster6);

            Monster monster7 = new Monster();
            monster7.Tex = texture6;
            monster7.is2 = true;
            monster7.AddVertex(new Vector2(28, 1f));
            monster7.AddTex(new Vector2(1, 1));
            monster7.AddVertex(new Vector2(28, 4f));
            monster7.AddTex(new Vector2(1, 0));
            monster7.AddVertex(new Vector2(31, 4f));
            monster7.AddTex(new Vector2(0, 0));
            monster7.AddVertex(new Vector2(31, 1f));
            monster7.AddTex(new Vector2(0, 1));
            monster7.Color = Color.Transparent;
            monster7.Transparent = true;
            world.Add(monster7);


            Texture texture = TextureLoader.FromFile(device, "../../metal.bmp");
            PolygonTextured pt = new PolygonTextured();
            pt.Tex = texture;
            /*
            pt.AddVertex(new Vector2(1.2f, 3.5f));
            pt.AddTex(new Vector2(0, 1));
            pt.AddVertex(new Vector2(1.9f, 3.5f));
            pt.AddTex(new Vector2(0, 0));
            pt.AddVertex(new Vector2(1.9f, 3.3f));
            pt.AddTex(new Vector2(1, 0));
            pt.AddVertex(new Vector2(1.2f, 3.3f));
            pt.AddTex(new Vector2(1, 1));
            pt.Color = Color.Transparent;
            world.Add(pt);*/

            ReadFileAndTexture(pentegonfile, pt, 1f, 0f);
            pt.Color = Color.Transparent;
            pt.ismapped = true;
            world.Add(pt);


            Texture texture2 = TextureLoader.FromFile(device, "../../black_dots.bmp");
            PolygonTextured pt2 = new PolygonTextured();
            pt2.Tex = texture2;
            pt2.ismapped = true;
            ReadFileAndTexture(trianglefile, pt2, 3, .5f);
            pt2.Color = Color.Transparent;
            world.Add(pt2);


            Texture texture3 = TextureLoader.FromFile(device, "../../metal.bmp");
            PolygonTextured pt3 = new PolygonTextured();
            pt3.Tex = texture3;
            pt3.ismapped = true;
            ReadFileAndTexture(trapizoidfile, pt3, 5, .8f);
            pt3.Color = Color.Transparent;
            world.Add(pt3);


            Texture texture4 = TextureLoader.FromFile(device, "../../flower.bmp");
            PolygonTextured pt4 = new PolygonTextured();
            pt4.Tex = texture4;
            pt4.ismapped = true;
            ReadFileAndTexture(arrowfile, pt4, 7f, 0);
            pt4.Color = Color.Transparent;
            world.Add(pt4);


            Texture texture5 = TextureLoader.FromFile(device, "../../lightning.bmp");
            PolygonTextured pt5 = new PolygonTextured();
            pt5.Tex = texture5;
            pt5.ismapped = true;
            ReadFileAndTexture(tboxfile, pt5, 9, 0);
            pt5.Color = Color.Transparent;
            world.Add(pt5);


            Texture spritetexture = TextureLoader.FromFile(device, "../../guy8.bmp");
            player.P = new Vector2(0.5f, 1);
            player.Tex = spritetexture;
            player.Transparent = true;
            player.AddVertex(new Vector2(-0.2f, 0));
            player.AddTex(new Vector2(0, 1));
            player.AddVertex(new Vector2(-0.2f, 1));
            player.AddTex(new Vector2(0, 0));
            player.AddVertex(new Vector2(0.2f, 1));
            player.AddTex(new Vector2(0.125f, 0));
            player.AddVertex(new Vector2(0.2f, 0));
            player.AddTex(new Vector2(0.125f, 1));
            player.Color = Color.Transparent;

            stood = true;

        }


        /// <summary>
        /// Initialize the Direct3D device for rendering
        /// </summary>
        /// <returns>true if successful</returns>
        private bool InitializeDirect3D()
        {
            try
            {
                // Now let's setup our D3D stuff
                PresentParameters presentParams = new PresentParameters();
                presentParams.Windowed = true;
                presentParams.SwapEffect = SwapEffect.Discard;

                device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, presentParams);
            }
            catch (DirectXException)
            {
                return false;
            }

            return true;
        }

        public void Render()
        {
            if (device == null)
                return;

            device.Clear(ClearFlags.Target, System.Drawing.Color.Blue, 1.0f, 0);

            int wid = Width;                            // Width of our display window
            int hit = Height;                           // Height of our display window.
            float aspect = (float)wid / (float)hit;     // What is the aspect ratio?

            device.RenderState.ZBufferEnable = false;   // We'll not use this feature
            device.RenderState.Lighting = false;        // Or this one...
            device.RenderState.CullMode = Cull.None;    // Or this one...

            float widP = playingH * aspect;         // Total width of window

            float winCenter = player.P.X;
            if (winCenter - widP / 2 < 0)
                winCenter = widP / 2;
            else if (winCenter + widP / 2 > playingW)
                winCenter = playingW - widP / 2;

            device.Transform.Projection = Matrix.OrthoOffCenterLH(winCenter - widP / 2,
                                                                  winCenter + widP / 2,
                                                                  0, playingH, 0, 1);

            //Begin the scene
            device.BeginScene();

            // Render the background
            background.Render();
            foreach (Polygon p in world)
            {
                p.Render(device);
            }

      
            player.Render(device);

            score.DisplayScore();
            lives.DisplayLives();

            if (game_over.gameOver)
            {
                game_over.DisplayGameOver();
                score.DisplayFinalScore();
            }

            //End the scene
            device.EndScene();
            device.Present();
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close(); // Esc was pressed
            else if (!game_over.gameOver)
            {
                if (e.KeyCode == Keys.Right)
                {
                
                     Vector2 v = player.V;
                     v.X = 1.5f;
                     player.V = v;
                   
                }
                else if (e.KeyCode == Keys.Left)
                {
                    Vector2 v = player.V;
                    v.X = -1.5f;
                    player.V = v;
                }
                else if (e.KeyCode == Keys.Space && stood == true)  // only allow jumps from standing condition
                {
                    stood = false;
                    audio.JMP();
                    Vector2 v = player.V;
                    v.Y = 5.5f;
                    player.V = v;
                    player.A = new Vector2(0, -9.8f);
                }
            }

        }

        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left)
            {
                Vector2 v = player.V;
                v.X = 0;
                player.V = v;
            }
        }

        /// <summary>
        /// Advance the game in time
        /// </summary>
        public void Advance()
        {
            // How much time change has there been?
            long time = stopwatch.ElapsedMilliseconds;
            float delta = (time - lastTime) * 0.001f;       // Delta time in milliseconds
            lastTime = time;
            

            while (delta > 0)
            {

                float step = delta;
                if (step > 0.05f)
                    step = 0.05f;

                float maxspeed = Math.Max(Math.Abs(player.V.X), Math.Abs(player.V.Y));
                if (maxspeed > 0)
                {
                    step = (float)Math.Min(step, 0.05 / maxspeed);
                }
                player.PostStood(stood);  // post stood condition variable to GameSprite object
                player.Advance(step);

                foreach (Polygon p in world)
                    p.Advance(step);

                foreach (Polygon p in world)
                {
                    if (collision.Test(player, p))
                    {
                        float depth = collision.P1inP2 ?
                                  collision.Depth : -collision.Depth;
                        player.P = player.P + collision.N * depth;
                        Vector2 v = player.V;
                        if (collision.N.X != 0)
                            v.X = 0;
                        if (collision.N.Y != 0)
                        {
                            v.Y = 0;
                            stood = true;
                        }



                        //
                        if (p.ismapped)
                        {
                            score.AddFlyingScore(-1, PlayerFeet().X, PlayerFeet().Y, lastTime);
                        }



                        // IF PLAYER is not on GROUND and touches a MONSTER!!!
                        if (p.isMonster && player.V.Y != 0)
                        {
                            v.Y = 5.5f;
                            stood = false;

                            v.X = -10.5f;
                           
                            player.A = new Vector2(0, -6.8f);

                            if (p.points_to_give > 0)
                            {

                                if (p.is2)
                                {
                                    v.X = -125.5f;
                                    score.AddFlyingScore(2, PlayerFeet().X, PlayerFeet().Y, lastTime);
                                    p.points_to_give +=1; // this will always give points
                                    lives.lives_number -= 1;
                                    if (lives.lives_number < 1)
                                    {
                                        lives.lives_number = 0;
                                        game_over.gameOver = true;

                                    }
                                }
                                if (p.is748)
                                {
                                    score.AddFlyingScore(748, PlayerFeet().X, PlayerFeet().Y, lastTime);
                                    v.X = -25.5f;
                                    lives.lives_number -= 1;
                                    if (lives.lives_number < 1)
                                    {
                                        lives.lives_number = 0;
                                        game_over.gameOver = true;

                                    }
                                }
                                else if (!p.isBig) score.AddFlyingScore(100, PlayerFeet().X, PlayerFeet().Y, lastTime);
                                else score.AddFlyingScore(500, PlayerFeet().X, PlayerFeet().Y, lastTime);
                                p.points_to_give -= 1;
                                audio.PNT();
                            }
                        }

                        else if (p.isMonster && player.V.Y != 0)
                        {
                            v.X = -25.5f;
                            lives.lives_number -= 1;
                            if (lives.lives_number < 1)
                            {
                                lives.lives_number = 0;
                                game_over.gameOver = true;

                            }
                            audio.JMP();
                        }

                        //if hit monster's right side... you lose/ game over
                        else if (p.isMonster && player.V.X <= 0 )
                        {
                            lives.lives_number -= 3;
                            if (lives.lives_number < 1)
                            {
                                lives.lives_number = 0;
                                game_over.gameOver = true;

                            }
                            
                        }
                       player.V = v;
                       player.Advance(0);

                    }
                }

                delta -= step;
            }

            score.Advance(time);

            if (!game_over.gameOver) { audio.BGD(); }
            else { audio.BGD(true);  audio.OVR(); }
        }

        public void AddObstacle(float left, float right, float bottom, float top, Color color)
        {
            Polygon newpoly = new Polygon();
            newpoly.AddVertex(new Vector2(left, bottom));
            newpoly.AddVertex(new Vector2(left, top));
            newpoly.AddVertex(new Vector2(right, top));
            newpoly.AddVertex(new Vector2(right, bottom));
            newpoly.Color = color;
            world.Add(newpoly);
        }

        public void ReadFileAndTexture(string fileName, PolygonTextured pt, float shiftX, float shiftY)
        {

            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    int counter = 0;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (counter % 2 == 0)
                        {
                            // Read the vertex line
                            String v_line = line;
                            string[] v_arry = v_line.Split(',');

                            pt.AddVertex(new Vector2(float.Parse(v_arry[0]) + shiftX, float.Parse(v_arry[1]) + shiftY));
                        }
                        else
                        {
                            // Read the texture line
                            String t_line = line;
                            string[] t_arry = t_line.Split(',');

                            pt.AddTex(new Vector2(float.Parse(t_arry[0]), float.Parse(t_arry[1])));
                        }
                        counter++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        //Gives a point that is approximately at the player's feet
        public Point PlayerFeet()
        {
            float aspect = (float)Width / (float)Height;     // What is the aspect ratio?

            float widP = playingH * aspect;         // Total width of window

            float winCenter = player.P.X;
            if (winCenter - widP / 2 < 0)
                winCenter = widP / 2;
            else if (winCenter + widP / 2 > playingW)
                winCenter = playingW - widP / 2;

            int x = Convert.ToInt32((player.P.X - (winCenter - widP / 2)) / widP * Width) - 25;
            int y = Height - Convert.ToInt32(player.P.Y / playingH * Height) - 40;

            return new Point(x, y);
        }
    }
}
