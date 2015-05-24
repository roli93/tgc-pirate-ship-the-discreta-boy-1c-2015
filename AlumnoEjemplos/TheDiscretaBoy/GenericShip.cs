using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.Input;
using Microsoft.DirectX.DirectInput;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.TheDiscretaBoy

{
    public enum Status 
    {
        Alive, Sinking, Sunk
    }

    public abstract class GenericShip
    {
        internal TgcMesh ship;
        internal Status status = Status.Alive;
        internal float maxLinearSpeed = 500F;
        internal float minLinearSpeed = -500F;
        internal Cannon cannon;
        internal float linearSpeed = 0F;
        internal float rotationalSpeed = (float)Math.PI * 3 / 4;
        internal Vector3 cannonOffset;
        internal int life = 1000;

        public GenericShip(TgcMesh shipMesh, Vector3 initialPosition, Cannon cannon, Vector3 cannonOffset)
        {
            ship = shipMesh;
            Position = initialPosition;
            this.cannon = cannon;
            cannon.Position = Position;
            cannon.Rotation = ship.Rotation;
            this.cannonOffset = cannonOffset;
        }

        public void moveForward(float elapsedTime)
        {

            ship.moveOrientedY((linearSpeed > 500F ? 500F : linearSpeed) * elapsedTime);
            cannon.Position = ship.Position + cannonOffset;
            cannon.LinearSpeed = linearSpeed;
        }

        public void turnRight(float elapsedTime)
        {
            ship.rotateY(rotationalSpeed * elapsedTime);
            cannon.rotateRight(elapsedTime);
        }

        public void turnLeft(float elapsedTime)
        {
            ship.rotateY(-rotationalSpeed * elapsedTime);
            cannon.rotateLeft(elapsedTime);
        }

        internal void accelerate()
        {
            linearSpeed += (linearSpeed > maxLinearSpeed ? 0F : 1.5F);
        }

        internal void desaccelerate()
        {
            linearSpeed -= (linearSpeed < minLinearSpeed ? 0F : 1.5F);
        }

        public Vector3 Position
        {
            get
            {
                return ship.Position;
            }
            set
            {
                ship.Position = value;
            }
        }

        public TgcBoundingBox BoundingBox
        {
            get
            {
                return ship.BoundingBox;
            }
            set
            {
                ship.BoundingBox = value;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return ship.Rotation;
            }
            set
            {
                ship.Rotation = value;
            }
        }

        public void sink()
        {
            status= Status.Sinking;
        }

        public abstract void renderAlive(float elapsedTime);

        public virtual void render(float elapsedTime)
        {
            if (status == Status.Sinking)
            {

                ship.rotateZ((float)Math.PI * elapsedTime);
                cannon.Position = ship.Position + cannonOffset;
                cannon.Rotation = Rotation;
                if (Rotation.Z >= Math.Abs(Math.PI))
                    status = Status.Sunk;
                ship.render();
                cannon.render(elapsedTime);
            }

            if (status == Status.Alive)
                if (life <= 0)
                    sink();
                else
                    renderAlive(elapsedTime);

            if (status == Status.Sunk)
            {
                GuiController.Instance.ThirdPersonCamera.Enable = false;
                ship.move(0, -100F * elapsedTime, 0);
                cannon.Position = ship.Position + cannonOffset;
                ship.render();
            }
        }

        public void beShot()
        {
            life -= 25;
        }

        public void dispose()
        {
            ship.dispose();
            cannon.dispose();
        }
    }

        public class PlayerShip : GenericShip
    {

            public PlayerShip(TgcMesh shipMesh, Vector3 initialPosition, Cannon cannon, Vector3 cannonOffset) : base(shipMesh, initialPosition, cannon, cannonOffset){}

            public override void renderAlive(float elapsedTime)
            {
                            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;

                            if (d3dInput.keyDown(Key.W))
                                accelerate();
                            else if (linearSpeed > 0F)
                                desaccelerate();

                            if (d3dInput.keyDown(Key.Q))
                                sink();

                            if (d3dInput.keyDown(Key.S))
                                desaccelerate();
                            else if (linearSpeed < 0F)
                                accelerate();

                            if (d3dInput.keyDown(Key.A))
                            {

                                turnLeft(elapsedTime);

                            }

                            if (d3dInput.keyDown(Key.D))
                            {

                                turnRight(elapsedTime);
                            }

                            if (d3dInput.keyDown(Key.LeftArrow))
                            {
                                cannon.turnLeft(elapsedTime);

                            }

                            if (d3dInput.keyDown(Key.RightArrow))
                            {
                                cannon.turnRight(elapsedTime);

                            }

                            if (d3dInput.keyPressed(Key.Space))
                            {
                                    cannon.shoot();
                            }


                            moveForward(elapsedTime);
                            ship.render();
                            cannon.render(elapsedTime);
              }
    }

        public class EnemyShip : GenericShip
    {
            private float time = 0F;
            private GenericShip victim = EjemploAlumno.Instance.ship;
            private Timer timer = new Timer(2F);
            private Oscilator speedAdjuster = new Oscilator(50F, 50F);

            public EnemyShip(TgcMesh shipMesh, Vector3 initialPosition, Cannon cannon, Vector3 cannonOffset) : base(shipMesh, initialPosition, cannon, cannonOffset) {}

            private Vector2 shootingSpeedForDistance(float distance, float elapsedTime)// a 45º
            {
                double speedModule =  + (14.0 +(distance/130)) * Math.Sqrt((speedAdjuster.oscilation(elapsedTime) +distance)/Math.Sin(Math.PI /2));
                return new Vector2((float)speedModule, (float)speedModule);
            }

            public override void renderAlive(float elapsedTime)
            {
                TgcD3dInput d3dInput = GuiController.Instance.D3dInput;
                Vector3 vectorToPlayerShip = victim.Position - Position;

                if (vectorToPlayerShip.Length() < 300)
                {
                    if (!cannon.aimingAt(victim))
                        cannon.aimAt(victim, elapsedTime);
                    else
                        timer.doWhenItsTimeTo(() => cannon.shootWithSpeed(shootingSpeedForDistance(vectorToPlayerShip.Length(), elapsedTime)), elapsedTime);
                }
                else
                    apporach(victim);

                if (d3dInput.keyDown(Key.K))
                {
                    cannon.turnLeft(elapsedTime);

                }

                if (d3dInput.keyDown(Key.L))
                {
                    cannon.aimingAt(EjemploAlumno.Instance.ship);
                }

                ship.render();
                cannon.render(elapsedTime);
            }

            private void apporach(GenericShip victim)
            {

            }
    }

    public class Timer
    {
        private float time;
        private float frequency;
        private bool itsTime=false;

        public Timer(float frequency)
        {
            this.frequency = frequency;
        }
        
        public void doWhenItsTimeTo(System.Action whatToDo, float elapsedTime){

            if(itsTime)
            {
                whatToDo();
                itsTime = false;
            }
            spendTime(elapsedTime);
        }

        private void spendTime(float elapsedTime)
        {
            if (time < Math.PI * 2)
                time += elapsedTime * (float)Math.PI * frequency;
            else
            {
                itsTime = true;
                this.time = 0;
            }
        }

    }

    public class Oscilator
    {
        private float frequency;
        private float rotation;
        private float amplitude;

        public Oscilator(float frequency, float amplitude)
        {
            this.frequency = frequency;
            this.amplitude = amplitude;
        }

        public float oscilation(float elapsedTime)
        {
            rotation += elapsedTime * 2*(float)Math.PI * frequency;
            float result = (float)Math.Sin(rotation)*amplitude;
            return result;
        }
    }

}
