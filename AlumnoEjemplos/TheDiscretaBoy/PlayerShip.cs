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
using TgcViewer.Utils._2D;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    public class PlayerShip : GenericShip
    {
        public TgcSprite barraDeVida;

        public PlayerShip(TgcMesh shipMesh, Vector3 initialPosition, Cannon cannon, Vector3 cannonOffset, TgcSprite barraDeVida) : base(shipMesh, initialPosition, cannon, cannonOffset) 
        {
            this.barraDeVida = barraDeVida;
        }

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

        public override void reduceLife(int quantity)
        {
            base.reduceLife(quantity);
            
            barraDeVida.Scaling = new Vector2(0.4f * porcentajeDeVida(), 0.4f);
        }

        public override string name()
        {
            return "Barco del jugador";
        }
    }
}
