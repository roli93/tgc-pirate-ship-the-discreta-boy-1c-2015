using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Cannon
    {
        private Bullet bullet;
        private TgcMesh cannon;

        private float rotationalSpeed = (float)Math.PI * 3 / 4;
        
        public Cannon(TgcMesh cannonMesh, Vector3 shootingPosition)
        {
           
            cannon = cannonMesh;
            this.ShootingOffset = shootingPosition;
           /* for (int i = 0; i < 20; i++)
            { 
                TgcSphere bullet = new TgcSphere();
            */
            bullet = new Bullet();
             /*   bullets.Add(bullet);
            }*/

        }

        public Vector3 ShootingOffset{get;set;}

        public Vector3 Position
        {
            get
            {
                return cannon.Position;
            }
            set
            {
                cannon.Position = value;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return cannon.Rotation;
            }
            set
            {
                cannon.Rotation = value;
            }
        }

        public void turnRight(float elapsedTime)
        {
            cannon.rotateY(rotationalSpeed * elapsedTime);
        }

        public void turnLeft(float elapsedTime)
        {
            cannon.rotateY(-rotationalSpeed * elapsedTime);
        }

        public void shoot()
        {
            bullet.beShot(this);
        }

        public void render(float elapsedTime)
        {

            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;

            if (d3dInput.keyDown(Key.LeftArrow))
            {

                turnLeft(elapsedTime);

            }

            if (d3dInput.keyDown(Key.RightArrow))
            {

                turnRight(elapsedTime);


            }

            if (d3dInput.keyDown(Key.Space))
            {
                shoot();
            }

            bullet.render(elapsedTime);
            cannon.render();
        }
        public void dispose()
        {
            cannon.dispose();
            bullet.dispose();
        }

    }
}
