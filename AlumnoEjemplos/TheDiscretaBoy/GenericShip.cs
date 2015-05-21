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
        internal int life = 100;

        //private List<TgcSphere> bullets = new List<TgcSphere>();

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
                renderAlive(elapsedTime);

            if (status == Status.Sunk)
            {
                GuiController.Instance.ThirdPersonCamera.Enable = false;
                ship.move(0, -100F * elapsedTime, 0);
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
            private bool spaceDown = false;

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

                            if (d3dInput.keyDown(Key.Space))
                            {
                                if (!spaceDown) //Para q no se apriete 20 millones de veces y espere sa que la suelten
                                {
                                    cannon.shoot();
                                    spaceDown = true;
                                }
                            }
                            else
                            {
                                spaceDown = false;
                            }


                            moveForward(elapsedTime);
                            ship.render();
                            cannon.render(elapsedTime);
              }
    }

        public class EnemyShip : GenericShip
    {
            public EnemyShip(TgcMesh shipMesh, Vector3 initialPosition, Cannon cannon, Vector3 cannonOffset) : base(shipMesh, initialPosition, cannon, cannonOffset) {}

            public override void renderAlive(float elapsedTime)
            {
                if (life<=0)
                    sink();

                ship.render();
                cannon.render(elapsedTime);
            }
    }
}
