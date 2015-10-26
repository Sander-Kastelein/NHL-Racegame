using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace NHLRacegame
{
    class Player : ISprite
    {

        public Game game;

        // Properties
        public int width, height;
        
        public double accelerationSpeedConstant;
        public double breakSpeedConstant;
        public double decelerationSpeedConstant;

        public double maxSpeed;
        public double maxSpeedWhenFuelIsEmpty;
        public double minSpeed;
        public double minSpeedWhenFuelIsEmpty;

        public double fullSpeedFuelUsage = 0.05;


        // Position
        public double posX, posY;
        public double speed = 0;
        public double rotation = 0;

        // Keybindings
        public Key upKey = Key.Up;
        public Key leftKey = Key.Left;
        public Key rightKey = Key.Right;
        public Key downKey = Key.Down;

        public bool isKeyReversed = false;


        // Stats
        public double fuel = 100;


        public Image bitmap;

        public Player(Game game)
        {
            this.game = game;
            // Init
            string path = Path.Combine(Environment.CurrentDirectory, "car.jpg");

            bitmap = Bitmap.FromFile(path);

            posX = 0;
            posY = 0;
            speed = 0;
            maxSpeed = 5;
            maxSpeedWhenFuelIsEmpty = 2;
            minSpeed = -2;
            minSpeedWhenFuelIsEmpty = -2;
            accelerationSpeedConstant = 0.01;
            decelerationSpeedConstant = 0.005;
            width = bitmap.Width;
            height = bitmap.Height;
            breakSpeedConstant = 0.01;

        }

        public void Loop()
        {
            // Loop
            Navigate();
            CalculateCoordinates();
            BurnFuel();
            
        }


        public void Navigate()
        {
            if(isKeyReversed)
            {
                Key tempKey = rightKey;
                rightKey = leftKey;
                leftKey = tempKey;
            }

            // UpKey
            if (Keyboard.IsKeyDown(upKey))
            {
                if (fuel > 0)
                {
                    if (speed < maxSpeed)
                    {
                        speed += AccelerationSpeed();
                        if (speed > maxSpeed)
                        {
                            speed = maxSpeed;
                        }
                    }
                }
                else
                {
                    if (speed < maxSpeedWhenFuelIsEmpty)
                    {
                        speed += AccelerationSpeed();
                        if (speed > maxSpeedWhenFuelIsEmpty)
                        {
                            speed = maxSpeedWhenFuelIsEmpty;
                        }
                    }
                    else
                    {
                        speed -= AccelerationSpeed();
                        if (speed < maxSpeedWhenFuelIsEmpty)
                        {
                            speed = maxSpeedWhenFuelIsEmpty;
                        }
                    }
                }


            }
            else
            {
                if (speed > 0)
                {
                    speed -= DecelerationSpeed();
                    if (speed < 0)
                    {
                        speed = 0;
                    }
                }
            }


            // DownKey

            if (Keyboard.IsKeyDown(downKey))
            {
                if (speed > 0)
                {
                    // Breaking
                    speed -= BreakSpeed();
                    if (speed < 0)
                    {
                        speed = 0;
                    }
                }
                else
                {
                    // Descelerating
                    if (fuel > 0)
                    {
                        speed -= DecelerationSpeed();
                        if (speed < minSpeed)
                        {
                            speed = minSpeed;
                        }
                    }
                    else
                    {
                        speed -= DecelerationSpeed();
                        if (speed < minSpeedWhenFuelIsEmpty)
                        {
                            speed = minSpeedWhenFuelIsEmpty;
                        }
                    }
                }

            }
            else
            {
                if (speed < 0)
                {
                    speed += DecelerationSpeed();
                    if (speed > 0)
                    {
                        speed = 0;
                    }
                }
            }

            // Corners

            if (Keyboard.IsKeyDown(leftKey))
            {
                rotation -= RotationSpeed();
            }
            if (Keyboard.IsKeyDown(rightKey))
            {
                rotation += RotationSpeed();
            }
            if (rotation < 0)
            {
                rotation += 360;
            }
            if (rotation > 360)
            {
                rotation -= 360;
            }

            if(isKeyReversed)
            {
                Key tempKey = rightKey;
                rightKey = leftKey;
                leftKey = tempKey;
            }
        }

        public void Draw(Graphics g)
        {

            g.TranslateTransform((float)posX, (float)posY);
            //g.TranslateTransform(-(int)Math.Floor((double)width / 2), -(int)Math.Floor((double)width / 2)); 
            g.RotateTransform((float)rotation);
            g.DrawImage(bitmap, -width / 2, -height / 2);

            g.RotateTransform(-(float)rotation);

            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Orange);
            PointF drawPoint = new PointF(0,0);
            g.DrawString("Fuel: "+fuel.ToString(), drawFont, drawBrush, drawPoint);


            g.ResetTransform();
            g.RotateTransform(0);
            
        }

        private void CalculateCoordinates()
        {
            double deltaX = Math.Cos(rotation / 180 * Math.PI);
            double deltaY = Math.Sin(rotation / 180 * Math.PI);

            double prevPosX = posX;
            double prevPosY = posY;

            posX += deltaX * speed;
            posY += deltaY * speed;

            // Collission detect map

            



        }



        private double RotationSpeed()
        {

            
            return Math.Abs(3.5d * (speed / maxSpeed)); // Graden per 1/60ste seconden
        }


        private void BurnFuel()
        {
            // Burn baby burn

            fuel -= fullSpeedFuelUsage * (Math.Abs(speed) / maxSpeed);
            if (fuel < 0)
            {
                fuel = 0;
            }
        }

        private double AccelerationSpeed()
        {
            return accelerationSpeedConstant;
        }

        private double DecelerationSpeed()
        {
            return decelerationSpeedConstant;
        }

        private double BreakSpeed()
        {
            return breakSpeedConstant;
        }

        //abc
    }
}