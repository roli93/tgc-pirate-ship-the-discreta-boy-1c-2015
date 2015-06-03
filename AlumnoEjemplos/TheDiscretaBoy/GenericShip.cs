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
using AlumnoEjemplos.TheDiscretaBoy.ShipStates;

namespace AlumnoEjemplos.TheDiscretaBoy

{
    public abstract class GenericShip :AimingCapable
    {
        public static int maximumLife = 300;

        internal TgcMesh ship;
        internal ShipState status = new Alive();
        internal float maxLinearSpeed = 500F;
        internal float minLinearSpeed = -500F;
        internal Cannon cannon;
        internal float linearSpeed = 0F;
        internal float rotationalSpeed = (float)Math.PI * 3 / 4;
        internal Vector3 cannonOffset;
        internal Vector3 initialPosition;
        internal float resurrectingElapsedTime;
        internal float acceleration = 1000F;
        internal Timer twinkler = new Timer(1000F);
        internal int life = maximumLife;
        public Barra barraDeVida;
        public Explocion explocion;
        public Hundimiento hundimiento;

        public abstract void renderPlaying(float elapsedTime);
        public void renderPaused(float elapsedTime)
        {
            renderOnlyVisible(elapsedTime);
        }

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

        public bool isDead()
        {
            return this.status.isDead(this);
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
            status.sink(this);
        }

        internal void bounce(ShipState postBounceStatus)
        {
            this.status.bounce(this, postBounceStatus);
        }

        internal void crash()
        {
            this.status.crash(this);
        }

        public virtual void renderAlive(float elapsedTime) 
        {
            updatePosition();
        }

        public void updatePosition()
        {
            float Y = EjemploAlumno.Instance.alturaEnPunto(this.Position.X, this.Position.Z);
            this.Position = new Vector3(
                this.Position.X,
                Y,
                this.Position.Z);
        }

        public void renderOnlyVisible(float elapsedTime)
        {
            this.status.renderOnlyVisible(this, elapsedTime);
        }

        public void updateCannonPosition()
        {
            this.cannon.Position = ship.Position + cannonOffset;
            this.cannon.Rotation = Rotation;
        }

        public void renderMesh()
        {
            this.ship.render();
        }

        public virtual void render(float elapsedTime)
        {
            if (status == Status.Sinking)
            {
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
                updatePosition();
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
