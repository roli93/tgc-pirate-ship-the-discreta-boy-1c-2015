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
    public class GenericShip 
    {        
        private TgcMesh ship;
        private float maxLinearSpeed = 500F;
        private float minLinearSpeed = -500F;
        private Cannon cannon;
        private float linearSpeed = 0F;
        private float rotationalSpeed = (float)Math.PI * 3 / 4;
        private Vector3 cannonOffset; 

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

       public void moveForward(float elapsedTime){
            
            ship.moveOrientedY((linearSpeed>500F?500F:linearSpeed)* elapsedTime);
            cannon.Position = ship.Position + cannonOffset;
            cannon.LinearSpeed = linearSpeed;
        }

        public void turnRight(float elapsedTime)
        {
            ship.rotateY(rotationalSpeed * elapsedTime);
            cannon.turnRight(elapsedTime);
        }

        public void turnLeft(float elapsedTime)
        {
            ship.rotateY(-rotationalSpeed * elapsedTime);
            cannon.turnLeft(elapsedTime);
        }

        private void accelerate()
        {
            linearSpeed += (linearSpeed > maxLinearSpeed ? 0F : 1.5F);
        }

        public void desaccelerate()
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

        public void render(float elapsedTime)
        {
            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;

            if (d3dInput.keyDown(Key.W))
                accelerate();
            else if(linearSpeed>0F)
                desaccelerate();


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

            moveForward(elapsedTime);

            ship.render();
            cannon.render(elapsedTime);
        }
        public void dispose()
        {
            ship.dispose();
            cannon.dispose();
        }
    }
}
