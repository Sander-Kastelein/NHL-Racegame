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
            maxSpeed = 3;
            maxSpeedWhenFuelIsEmpty = 1.5;
            minSpeed = -2;
            minSpeedWhenFuelIsEmpty = -1;
            accelerationSpeedConstant = 0.02;
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
                    }
                }
                else
                {
                    if (speed < maxSpeedWhenFuelIsEmpty)
                    {
                        speed += AccelerationSpeed();
                    }
                    else
                    {
                        speed -= AccelerationSpeed();
                    }
                }


            }
            else
            {
                if (speed > 0)
                {
                    speed -= DecelerationSpeed();
                }
            }


            // DownKey

            if (Keyboard.IsKeyDown(downKey))
            {
                if (speed > 0)
                {
                    // Breaking
                    speed -= BreakSpeed();
                }
                else
                {
                    // Descelerating
                    if (fuel > 0)
                    {
                        speed -= DecelerationSpeed();
                    }
                    else
                    {
                        speed -= DecelerationSpeed();
                    }
                }

            }
            else
            {
                if (speed < 0)
                {
                    speed += DecelerationSpeed();
                }
            }

            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
            if (speed < minSpeed)
            {
                speed = minSpeed;
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
            if (rotation >= 360)
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

            HandleCollisions(posX, posY, prevPosX, prevPosY);

        }


        private void HandleCollisions(double x, double y, double prevX, double prevY)
        {
            double rot = (rotation / 180) * Math.PI; // Deg 2 rad
            while (rot >= 2 * Math.PI)
            {
                rot -= 2 * Math.PI;
            }
            while (rot < 0)
            {
                rot += 2 * Math.PI;
            }

            double frontMiddleX = x + (height * Math.Cos(rot));
            double frontMiddleY = y + (height * Math.Sin(rot));

            double backMiddleX = x + (height * -Math.Cos(rot));
            double backMiddleY = y + (height * -Math.Sin(rot));


            double frontInverseWidth;
            double backInverseWidth;

            /*
            if (speed < 0)
            {
                frontInverseWidth = 5;
                backInverseWidth = 4;
            }
            else if (speed > 0)
            {

                frontInverseWidth = 4;
                backInverseWidth = 5;
            }
            else
            {
                frontInverseWidth = 4;
                backInverseWidth = 4;
            }
            */

            double frontRightX = frontMiddleX + (width / 4 * Math.Cos(rot + Math.PI * .5));
            double frontRightY = frontMiddleY + (width / 4 * Math.Sin(rot + Math.PI * .5));

            double frontLeftX = frontMiddleX + (width / 4 * -Math.Cos(rot + Math.PI * .5));
            double frontLeftY = frontMiddleY + (width / 4 * -Math.Sin(rot + Math.PI * .5));

            double backRightX = backMiddleX + (width / 4 * Math.Cos(rot + Math.PI * .5));
            double backRightY = backMiddleY + (width / 4 * Math.Sin(rot + Math.PI * .5));

            double backLeftX = backMiddleX + (width / 4 * -Math.Cos(rot + Math.PI * .5));
            double backLeftY = backMiddleY + (width / 4 * -Math.Sin(rot + Math.PI * .5));


            if (!game.isPositionOnRoad(frontLeftX, frontLeftY) || !game.isPositionOnRoad(frontRightX, frontRightY) || !game.isPositionOnRoad(backLeftX, backLeftY) || !game.isPositionOnRoad(backRightX, backRightY))
            {
                posX = prevX;
                posY = prevY;
            }

            if(!game.isPositionOnRoad(frontLeftX, frontLeftY) && !game.isPositionOnRoad(frontRightX, frontRightY)){
                speed = 0;
            }
            else if (!game.isPositionOnRoad(frontLeftX, frontLeftY) && !game.isPositionOnRoad(backLeftX, backLeftY))
            {
                posX += (width / 12 * Math.Cos(rot + Math.PI * .5));
                posY += (width / 12 * Math.Sin(rot + Math.PI * .5));
                BumpPenaltySpeed();
            }
            else if (!game.isPositionOnRoad(frontRightX, frontRightY) && !game.isPositionOnRoad(backRightX, backRightY))
            {
                posX += (width / 12 * -Math.Cos(rot + Math.PI * .5));
                posY += (width / 12 * -Math.Sin(rot + Math.PI * .5));
                BumpPenaltySpeed();
            }
            else if (!game.isPositionOnRoad(frontLeftX, frontLeftY) || !game.isPositionOnRoad(backRightX, backRightY))
            {
                //posX = prevX;
                //posY = prevY;
                BumpPenaltySpeed();
                rotation += BumpPenaltyRotation();
            }
            else if (!game.isPositionOnRoad(frontRightX, frontRightY) || !game.isPositionOnRoad(backLeftX, backLeftY))
            {
                //posX = prevX;
                //posY = prevY;
                BumpPenaltySpeed();
                rotation -= BumpPenaltyRotation();
            }

            // niet aankomen
            if(posX < 0) posX = 0;
            if (posX > game.Width) posX = game.Width;

            if (posY < 0) posY = 0;
            if (posY > game.Height) posY = game.Height;

        }

        private double RotationSpeed()
        {

            
            return Math.Abs(5d * (speed / maxSpeed)); // Graden per 1/60ste seconden
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

        private void BumpPenaltySpeed() // Amount of pixels/tick to slowdown when bumping into something. 
        { // Deze is moeilijk en moet goed!
            
            if (speed > 0)
            {
                speed = (Math.Sqrt(speed)) * 1.1;
            }
            else
            {
                speed -= (Math.Sqrt(speed) * 0.99 * Math.Sqrt(speed));
            }

        }

        private double BumpPenaltyRotation() // Amount of degrees to turn when hitting something sideways.
        {
            return RotationSpeed() * 1.5;
        }

        //abc
    }
}