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
using TgcViewer.Utils;
using TgcViewer.Utils._2D;

namespace AlumnoEjemplos.TheDiscretaBoy

{
    public enum Status 
    {
        Alive, Sinking, Sunk, Bouncing, Resurrecting
    }
    public delegate void ShipCommand(GenericShip ship);
    public abstract class GenericShip :AimingCapable
    {
        public static int maximumLife = 300;

        internal TgcMesh ship;
        internal Status status = Status.Alive;
        internal float maxLinearSpeed = 500F;
        internal float minLinearSpeed = -500F;
        internal Cannon cannon;
        internal float linearSpeed = 0F;
        internal float rotationalSpeed = (float)Math.PI * 3 / 4;
        internal Vector3 cannonOffset;
        internal Vector3 initialPosition;
        internal float resurrectingElapsedTime;
        internal Status postBounceStatus;
        internal float acceleration = 1000F;
        internal Timer twinkler = new Timer(1000F);
        internal int life = maximumLife;
        public Barra barraDeVida;
        public Explocion explocion;
        public Hundimiento hundimiento;
        public Vector3 normal;

        public GenericShip(TgcMesh shipMesh, Vector3 initialPosition, Cannon cannon, Vector3 cannonOffset) : base()
        {
            ship = shipMesh;
            this.initialPosition = Position = initialPosition;
            this.cannon = cannon;
            cannon.Position = Position;
            cannon.Rotation = ship.Rotation;
            this.cannonOffset = cannonOffset;
            iniciarBarra();
            explocion = new Explocion();
            hundimiento = new Hundimiento();
            this.normal = EjemploAlumno.Instance.normalEnPunto(this.Position.X, this.Position.Z);
        }

        public bool isDead()
        {
            return (this.status == Status.Sinking) || (this.status == Status.Sunk);
        }

        public void iniciarBarra()
        {
            barraDeVida = new Barra(new Vector2(0, 0), name());
        }

        public void moveForward(float elapsedTime)
        {

            ship.moveOrientedY((linearSpeed > 500F ? 500F : linearSpeed) * elapsedTime);
            cannon.Position = ship.Position + cannonOffset;
            cannon.LinearSpeed = linearSpeed;
        }

        internal override TgcMesh getMesh()
        {
            return ship;
        }

        public override void turnRight(float elapsedTime)
        {
            ship.rotateY(rotationalSpeed * elapsedTime);
            cannon.rotateRight(elapsedTime);
            updateDirection(elapsedTime, rotationalSpeed);
        }

        public override void turnLeft(float elapsedTime)
        {
            ship.rotateY(-rotationalSpeed * elapsedTime);
            cannon.rotateLeft(elapsedTime);
            updateDirection(elapsedTime, -rotationalSpeed);
        }

        internal void accelerate(float elapsedTime)
        {
            linearSpeed += (linearSpeed > maxLinearSpeed ? 0F : acceleration*elapsedTime);
        }

        internal void desaccelerate(float elapsedTime)
        {
            linearSpeed -= (linearSpeed < minLinearSpeed ? 0F : acceleration*elapsedTime);
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

        public virtual void sink()
        {
            status= Status.Sinking;
            hundimiento.show();
        }

        internal void bounce(Status postBounceStatus)
        {
            linearSpeed *= -1;
            this.postBounceStatus = postBounceStatus;
            status = Status.Bouncing;
        }


        public abstract void crash();

        public void handleShipCollision(GenericShip ship)
        {
            if (ship != this)
                if (TgcCollisionUtils.testAABBAABB(ship.BoundingBox, this.BoundingBox))
                    this.crash();
        }

        public virtual void renderAlive(float elapsedTime) 
        {
            updatePosition();
            EjemploAlumno.Instance.forEachShip((ShipCommand)handleShipCollision);
            this.ship.render();
            this.cannon.render(elapsedTime);
        }

        public void updatePosition()
        {
            float Y = EjemploAlumno.Instance.alturaEnPunto(this.Position.X, this.Position.Z);
            this.Position = new Vector3(
                this.Position.X,
                Y,
                this.Position.Z);

            Vector3 previousNormal = this.normal;
            this.normal = EjemploAlumno.Instance.normalEnPunto(this.Position.X, this.Position.Z);
            float rotationX = (FastMath.Atan2(normal.Z, normal.Y) - FastMath.Atan2(previousNormal.Z, previousNormal.Y));
            float rotationZ = -(FastMath.Atan2(normal.X, normal.Y) - FastMath.Atan2(previousNormal.X, previousNormal.Y));
            this.ship.rotateX(rotationX);
            this.ship.rotateZ(rotationZ);
            this.cannon.getMesh().rotateX(rotationX);
            this.cannon.getMesh().rotateZ(rotationZ);
        }

        public bool isAlive() {
            return this.status == Status.Alive;
        }

        public bool crashingWith(GenericShip ship)
        {
            return (this != ship) && TgcCollisionUtils.testAABBAABB(ship.BoundingBox, this.BoundingBox);
        }

        public void drawNormal()
        {
            TgcArrow normalDibujable = new TgcArrow();
            normalDibujable.PStart = this.Position;
            normalDibujable.PEnd = this.Position + Vector3.Multiply(this.normal, 200);
            normalDibujable.updateValues();
            normalDibujable.render();
        }

        public virtual void render(float elapsedTime)
        {
            if (status == Status.Sinking)
            {
                updatePosition();
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
                ship.move(0, -100F * elapsedTime, 0);
                cannon.Position = ship.Position + cannonOffset;
                ship.render();
            }
            
            if (status == Status.Bouncing)
            {
                updatePosition();
                if (Math.Abs(linearSpeed) > 1)
                {
                    if (!EjemploAlumno.Instance.crashingWithAnyOther(this))
                        if (linearSpeed > 0)
                            desaccelerate(elapsedTime);
                        else
                            accelerate(elapsedTime);
                }
                else
                    status = postBounceStatus;
                moveForward(elapsedTime);
                this.ship.render();
                cannon.render(elapsedTime);
            }

            if (status == Status.Resurrecting)
            {
                Position = initialPosition;
                updatePosition();
                cannon.Position = Position + cannonOffset;
                if(resurrectingElapsedTime < .7)
                {
                    resurrectingElapsedTime+= elapsedTime;
                    twinkler.doWhenItsTimeTo(() =>
                                                    {
                                                        this.ship.render();
                                                        cannon.render(elapsedTime);
                                                    },
                                                    elapsedTime
                                             );
                }
                else
                {
                    status = Status.Alive;
                    resurrectingElapsedTime = 0;
                }
            }
        }

        public void renderBar()
        {
            barraDeVida.render();
        }

        public void beShot()
        {
            this.reduceLife(25);
            this.showExplotion();
        }

        public void showExplotion()
        {
            explocion.show();
        }

        public virtual void dispose()
        {
            ship.dispose();
            cannon.dispose();
            barraDeVida.dispose();
        }

        public virtual void reduceLife(int quantity)
        {
            if(life > 0)
            {
                life = Math.Max(life - quantity, 0);
                barraDeVida.escalar(porcentajeDeVida());
                log("Vida de " + this.name() + ": " + (porcentajeDeVida() * 100) + "%");
            }
        }

        public virtual string name() 
        {
            return "Barco generico";
        }

        public float porcentajeDeVida()
        {
            return (float)life / (float)maximumLife;
        }

        public void log(string comment)
        {
            GuiController.Instance.Logger.log(comment);
        }
    }

}
