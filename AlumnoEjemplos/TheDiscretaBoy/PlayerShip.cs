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

        public PlayerShip(TgcMesh shipMesh, Vector3 initialPosition, Cannon cannon, Vector3 cannonOffset) : base(shipMesh, initialPosition, cannon, cannonOffset) { }

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

            if (!TgcCollisionUtils.testSphereAABB(EjemploAlumno.Instance.skyBoundaries, this.BoundingBox))
                bounce(Status.Alive);
            if (TgcCollisionUtils.testAABBAABB(EjemploAlumno.Instance.enemyShip.BoundingBox, BoundingBox))
            {
                crash();
            }

            ship.render();
            cannon.render(elapsedTime);
        }
    }
}
