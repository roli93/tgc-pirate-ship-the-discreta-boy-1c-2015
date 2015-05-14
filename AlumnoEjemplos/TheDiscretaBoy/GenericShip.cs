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


namespace AlumnoEjemplos.MiGrupo
{
    class GenericShip
    {

        
        TgcMesh ship;


        public GenericShip(TgcMesh shipMesh, Vector3 initialPosition)
        {
           
            ship = shipMesh;
            ship.Position = initialPosition;
     

        }

       public void acelerate(float speed){
            
            ship.moveOrientedY(-speed);
        }

        public void turnRigth(float elapsedTime)
        {
            ship.rotateY((float)Math.PI * 3 / 4 * elapsedTime);
        }

        public void turnLeft(float elapsedTime)
        {
            ship.rotateY(-(float)Math.PI * 3 / 4 * elapsedTime);
        }

        public void desacelerate(float speed)
        {

            ship.moveOrientedY(speed);
        }

        public Vector3 Position()
        {
            return ship.Position;
        }





        public void render()
        {
            ship.render();
        }
        public void dispose()
        {
            ship.dispose();
        }
    }
}
