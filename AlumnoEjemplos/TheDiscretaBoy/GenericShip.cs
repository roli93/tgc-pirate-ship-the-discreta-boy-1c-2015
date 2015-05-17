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

        
        private TgcMesh ship;
        private TgcSphere bullet; 
        //private List<TgcSphere> bullets = new List<TgcSphere>();

        public GenericShip(TgcMesh shipMesh, Vector3 initialPosition)
        {
           
            ship = shipMesh;
            ship.Position = initialPosition;
           /* for (int i = 0; i < 20; i++)
            { 
                TgcSphere bullet = new TgcSphere();
            */
                bullet = new TgcSphere();
                bullet.Radius = 5;
                bullet.setColor(Color.Black);
                bullet.LevelOfDetail = 5;
                bullet.updateValues();
             /*   bullets.Add(bullet);
            }*/

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

        public void shoot()
        {
            bullet.Position = ship.Position;
            bullet.updateValues();
        }



        public void render()
        {
            ship.render();
            bullet.render();
        }
        public void dispose()
        {
            ship.dispose();
        }
    }
}
