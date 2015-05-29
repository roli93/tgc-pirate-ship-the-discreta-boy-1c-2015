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

    public class EnemyShip : GenericShip
    {
        private GenericShip victim = EjemploAlumno.Instance.ship;
        private Timer timer = new Timer(2F);
        private Oscilator speedAdjuster = new Oscilator(50F, 50F);

        public EnemyShip(TgcMesh shipMesh, Vector3 initialPosition, Cannon cannon, Vector3 cannonOffset) : base(shipMesh, initialPosition, cannon, cannonOffset) 
        {
            barraDeVida.alinearDerecha();
        }

        private void shootPeriodically(float elapsedTime)
        {
            if (!cannon.aimingAt(victim))
                cannon.aimAt(victim, elapsedTime);
            else
                timer.doWhenItsTimeTo(() => cannon.shootWithSpeed(shootingSpeedForDistance(distanceToVictim(), elapsedTime)), elapsedTime);
        }

        private float distanceToVictim()
        {
            return VectorToVictim.Length();
        }

        private void apporach(GenericShip victim, float elapsedTime)
        {
            if (!aimingAt(victim))
                aimAt(victim, elapsedTime);
            else
                desaccelerate();
        }

        private Vector2 shootingSpeedForDistance(float distance, float elapsedTime)// a 45º
        {
            double speedModule = +(14.0 + (distance / 130)) * Math.Sqrt((speedAdjuster.oscilation(elapsedTime) + distance) / Math.Sin(Math.PI / 2));
            return new Vector2((float)speedModule, (float)speedModule);
        }

        private Vector3 VectorToVictim
        {
            get
            {
                return victim.Position - Position;
            }
        }

        public override void renderAlive(float elapsedTime)
        {
            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;

            float distancte = distanceToVictim();
            if (distancte < 300)
            {
                if (linearSpeed < 0F)
                    accelerate();
                shootPeriodically(elapsedTime);
            }
            else
                apporach(victim, elapsedTime);

            if (d3dInput.keyDown(Key.K))
            {
                cannon.turnLeft(elapsedTime);

            }

            if (d3dInput.keyDown(Key.L))
            {
                cannon.aimingAt(EjemploAlumno.Instance.ship);
            }

            moveForward(elapsedTime);
            ship.render();
            cannon.render(elapsedTime);
        }

        public override string name()
        {
            return "Barco enemigo";
        }
    }

}
