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
    public class PlayerShip : GenericShip
    {

        public PlayerShip(TgcMesh shipMesh, Vector3 initialPosition, Cannon cannon, Vector3 cannonOffset) : base(shipMesh, initialPosition, cannon, cannonOffset) 
        {
        }

        public override void renderBar()
        {
            base.renderBar();
            barraDeVida.render();
        }

        public override void handleInput(float elapsedTime)
        {
            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;

            if (d3dInput.keyDown(Key.W)){
                accelerate(elapsedTime);
            }
            else if (linearSpeed > 1F)
                desaccelerate(elapsedTime);
            else if (d3dInput.keyDown(Key.S))
                desaccelerate(elapsedTime);
            else if (linearSpeed < -1F)
                accelerate(elapsedTime);
            else
                linearSpeed = 0F;

            if (d3dInput.keyDown(Key.Q))
                sink();


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

            if (!TgcCollisionUtils.testSphereAABB(EjemploAlumno.Instance.skyBoundaries, this.BoundingBox))
                this.status.crashWithSky(this);

            foreach (EnemyShip enemyShip in EjemploAlumno.Instance.enemies)
                if (TgcCollisionUtils.testAABBAABB(enemyShip.BoundingBox, BoundingBox))
                    crash();

            cannon.renderAction(elapsedTime);
        }

        public override string name()
        {
            return "You";
        }

        public override void sink()
        {
            base.sink();
            (new Failure()).show();
        }

        public override void renderPlaying(float elapsedTime)
        {
            this.status.renderPlaying(this, elapsedTime);
        }

        public override void renderOnlyVisible(float elapsedTime)
        {
            base.renderOnlyVisible(elapsedTime);
            this.barraDeVida.render();
        }
    }
}
