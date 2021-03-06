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

        public string name;
        public int interfaceOffset = 0;

        public double accelerationSpeedConstant;
        public double breakSpeedConstant;
        public double decelerationSpeedConstant;
        public double accelerateBelowZeroConstant;

        public double maxSpeed;
        public double maxSpeedWhenFuelIsEmpty;
        public double minSpeed;
        public double minSpeedWhenFuelIsEmpty;

        public double fullSpeedFuelUsage = 0.07;
        public int mass = 30;

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
        public bool isInPitstop = false;

        // Stats
        public double fuel = 100;
        public int pitsstopsDone = 0;

        public int lapsDone = 0;
        public int nextCheckpoint = 255;
        public int lastCheckpoint = 253;

        public Image bitmap;
        



        public Player(Game game, string character)
        {
            this.game = game;
            // Init
            string path = Path.Combine(Environment.CurrentDirectory, "car_"+character+".png");
            bitmap = Bitmap.FromFile(path);

            posX = 0;
            posY = 0;
            speed = 0;
            maxSpeed = 3;
            maxSpeedWhenFuelIsEmpty = 1.5;
            minSpeed = -2;
            minSpeedWhenFuelIsEmpty = -1;
            accelerationSpeedConstant = 0.2;
            accelerateBelowZeroConstant = 0.02;
            decelerationSpeedConstant = 0.005;

            width = bitmap.Width;
            height = bitmap.Height;
            
            breakSpeedConstant = 0.04;
        }

        public void Loop()
        {
            // Loop
            Navigate();
            DetectPitstop();
            DetectCheckpoints();
            DetectFinish();
            CalculateCoordinates();
            BurnFuel();
            
        }

        public void DetectPitstop()
        {
            System.Drawing.Color currentPosOnMap = game.roadBitmap.GetPixel((int)posX, (int)posY);
            if (currentPosOnMap.G == 255 && currentPosOnMap.B == 0 && currentPosOnMap.R == 0)
            {
                if (!isInPitstop)
                {
                    pitsstopsDone++;
                }
                isInPitstop = true;
            }
            else
            {
                isInPitstop = false;
            }

            if (isInPitstop)
            {
                fuel += 0.2;
                if (fuel > 100)
                {
                    fuel = 100;
                }
            }

        }

        public void DetectCheckpoints()
        {
            System.Drawing.Color currentPosOnMap = game.roadBitmap.GetPixel((int)posX, (int)posY);
            if (currentPosOnMap.R == nextCheckpoint && currentPosOnMap.B == 0 && currentPosOnMap.G == 0)
            {
                nextCheckpoint--;
            }
        }

        public void DetectFinish()
        {

            System.Drawing.Color currentPosOnMap = game.roadBitmap.GetPixel((int)posX, (int)posY);
            if (currentPosOnMap.B == 255 && currentPosOnMap.R == 0 && currentPosOnMap.G == 0)
            {
                if(nextCheckpoint == lastCheckpoint-1)
                {
                    nextCheckpoint = 255;
                    lapsDone++;
                    
                    if (lapsDone == 3)
                    {
                        game.stopGame();
                        Form winScreen = new WinScreen();
                        winScreen.BackgroundImage = Image.FromFile(Path.Combine(Environment.CurrentDirectory, this.name.Replace(" ","") + "wins.bmp"));
                        winScreen.Show();
                        game.Hide();
                    }
                }
            }
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
                    // Decelerating
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

            g.ResetTransform();
            g.RotateTransform(0);

            Font drawFontFuel = new Font("Arial", 12);
            SolidBrush drawBrushFuel = new SolidBrush(System.Drawing.Color.Black);
            int drawFuel = (int)Math.Ceiling(fuel);
            PointF drawPointFuel = new PointF(10 + interfaceOffset, 768 - 35);
            g.DrawString((drawFuel < 100 ? "0" : "") + (drawFuel < 10 ? "0" : "") + drawFuel.ToString(), drawFontFuel, drawBrushFuel, drawPointFuel);

            Font drawFont = new Font("Arial", 20);
            SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Black);
            PointF drawPoint = new PointF(157 + interfaceOffset, 768-65);
            int displaySpeed = (int)Math.Abs(Math.Round(speed * 60));
            g.DrawString((displaySpeed < 100 ? "0" : "") + (displaySpeed < 10 ? "0" : "") +  displaySpeed.ToString(), drawFont, drawBrush, drawPoint);

            Font drawFontLaps = new Font("Arial", 12);
            SolidBrush drawBrushLaps = new SolidBrush(System.Drawing.Color.Black);
            PointF drawPointLaps = new PointF(116 + interfaceOffset, 768 - 37);
            g.DrawString(lapsDone.ToString(), drawFontLaps, drawBrushLaps, drawPointLaps);

            Font drawFontPit = new Font("Arial", 12);
            SolidBrush drawBrushPit = new SolidBrush(System.Drawing.Color.Black);
            PointF drawPointPit = new PointF(100 + interfaceOffset, 768 - 57);
            g.DrawString(pitsstopsDone.ToString(), drawFontPit, drawBrushPit, drawPointPit);
        
        }

        private void CalculateCoordinates()
        {

            double deltaX = Math.Cos(rotation / 180 * Math.PI);
            double deltaY = Math.Sin(rotation / 180 * Math.PI);

            double prevPosX = posX;
            double prevPosY = posY;

            if (!isInPitstop)
            {
                posX += deltaX * speed;
                posY += deltaY * speed;
            }
            else
            {
                posX += deltaX * speed * 0.6;
                posY += deltaY * speed * 0.6;
            }
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
            

            double frontRightX = frontMiddleX + (width / frontInverseWidth * Math.Cos(rot + Math.PI * .5));
            double frontRightY = frontMiddleY + (width / frontInverseWidth * Math.Sin(rot + Math.PI * .5));

            double frontLeftX = frontMiddleX + (width / frontInverseWidth * -Math.Cos(rot + Math.PI * .5));
            double frontLeftY = frontMiddleY + (width / frontInverseWidth * -Math.Sin(rot + Math.PI * .5));

            double backRightX = backMiddleX + (width / backInverseWidth * Math.Cos(rot + Math.PI * .5));
            double backRightY = backMiddleY + (width / backInverseWidth * Math.Sin(rot + Math.PI * .5));

            double backLeftX = backMiddleX + (width / backInverseWidth * -Math.Cos(rot + Math.PI * .5));
            double backLeftY = backMiddleY + (width / backInverseWidth * -Math.Sin(rot + Math.PI * .5));


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

            // do not touch!
            if(posX < 0) posX = 0;
            if (posX > game.Width) posX = game.Width;

            if (posY < 0) posY = 0;
            if (posY > game.Height) posY = game.Height;

        }

        private double RotationSpeed()
        {

            
            return Math.Abs(7d * (speed / (Math.Pow(speed, 2) + 1))); // Degrees for every 1/60th of a second.
        }


        private void BurnFuel()
        {
            // Burn baby burn

            fuel -= fullSpeedFuelUsage * (Math.Abs(speed) / ( 2 * maxSpeed));
            if (fuel < 0)
            {
                fuel = 0;
            }
        }

        private double AccelerationSpeed()
        {
            //Works only when speed is larger than 0.(???)
            if (speed < 0)
            {
                double accelerateBelowZero = accelerateBelowZeroConstant;
                return accelerateBelowZero;
            }
            else
            {
                double oppositeForce = speed * mass;
                double accelerationSpeed = accelerationSpeedConstant / (oppositeForce + 1);
                return accelerationSpeed;
            }
            //return accelerationSpeedConstant;
        }

        private double DecelerationSpeed()
        {
            //decelerationSpeedConstant could be looked at.
                return decelerationSpeedConstant;
        }
            //breakSpeedConstant is fine.
        private double BreakSpeed()
        {
            return breakSpeedConstant;
        }

        private void BumpPenaltySpeed() // Amount of pixels/tick to slowdown when bumping into something. 
        { // Difficult and needs to be done right!
            
            if (speed > 0)
            {
                speed = (Math.Sqrt(speed)) * 1.1;
            }
            else
            {
                speed = 0.9 * speed;
            }

        }

        private double BumpPenaltyRotation() // Amount of degrees to turn when hitting something sideways.
        {
            return RotationSpeed() * 1.3;
        }

        

        //abc
    }
}