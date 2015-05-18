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
        private Cannon cannon;
        private float linearSpeedFactor = 100F;
        private float rotationalSpeedFactor = (float)Math.PI * 3 / 4;

        //private List<TgcSphere> bullets = new List<TgcSphere>();

        public GenericShip(TgcMesh shipMesh, Vector3 initialPosition, Cannon cannon)
        {
           
            ship = shipMesh;
            Position = initialPosition;
            this.cannon = cannon;
            cannon.Position = Position;
            cannon.Rotation = ship.Rotation;
        }

       public void acelerate(float elapsedTime){
            
            ship.moveOrientedY(-linearSpeedFactor* elapsedTime);
            cannon.Position = ship.Position + new Vector3(0, 1, 0);
        }

        public void turnRight(float elapsedTime)
        {
            ship.rotateY(rotationalSpeedFactor * elapsedTime);
            cannon.turnRight(elapsedTime);
        }

        public void turnLeft(float elapsedTime)
        {
            ship.rotateY(-rotationalSpeedFactor * elapsedTime);
            cannon.turnLeft(elapsedTime);
        }

        public void desacelerate(float elapsedTime)
        {
            ship.moveOrientedY(linearSpeedFactor*elapsedTime);
            cannon.Position = ship.Position + new Vector3(0, 1, 0);
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
            {
                acelerate(elapsedTime);
            }


            if (d3dInput.keyDown(Key.S))
            {
                desacelerate(elapsedTime);


            }

            if (d3dInput.keyDown(Key.A))
            {

                turnLeft(elapsedTime);

            }

            if (d3dInput.keyDown(Key.D))
            {

                turnRight(elapsedTime);
            }

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
