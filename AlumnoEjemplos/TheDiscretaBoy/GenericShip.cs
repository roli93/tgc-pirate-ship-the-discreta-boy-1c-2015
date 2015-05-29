﻿using System;
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


        internal void crash()
        {
            life -= 25;
            bounce(Status.Resurrecting);
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
            
            if (status == Status.Bouncing)
            {
                if (Math.Abs(linearSpeed) > 1)
                {
                    if (!TgcCollisionUtils.testAABBAABB(EjemploAlumno.Instance.enemyShip.BoundingBox, BoundingBox))
                        if (linearSpeed > 0)
                            desaccelerate(elapsedTime);
                        else
                            accelerate(elapsedTime);
                }
                else
                    status = postBounceStatus;
                moveForward(elapsedTime);
                ship.render();
                cannon.render(elapsedTime);
            }


            if (status == Status.Resurrecting)
            {
                Position = initialPosition;
                cannon.Position = Position + cannonOffset;
                if(resurrectingElapsedTime < .7)
                {
                    resurrectingElapsedTime+= elapsedTime;
                    twinkler.doWhenItsTimeTo(() =>
                                                    {
                                                        ship.render();
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
            life -= quantity;
            barraDeVida.escalar(porcentajeDeVida());
            log("Vida del " + this.name() + ": " + (porcentajeDeVida() * 100) + "%");
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
