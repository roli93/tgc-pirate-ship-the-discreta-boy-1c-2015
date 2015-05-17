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
        private TgcSphere bullet;
        private TgcMesh cannon;
        private Vector3 shootingPosition;

        public Cannon(TgcMesh cannonMesh, Vector3 shootingPosition)
        {
           
            cannon = cannonMesh;
            this.shootingPosition = shootingPosition;
           /* for (int i = 0; i < 20; i++)
            { 
                TgcSphere bullet = new TgcSphere();
            */
                bullet = new TgcSphere();
                bullet.Radius = 3;
                bullet.setColor(Color.Black);
                bullet.LevelOfDetail = 5;
                bullet.updateValues();
             /*   bullets.Add(bullet);
            }*/

        }

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
            cannon.rotateY((float)Math.PI * 3 / 4 * elapsedTime);
        }

        public void turnLeft(float elapsedTime)
        {
            cannon.rotateY(-(float)Math.PI * 3 / 4 * elapsedTime);
        }

        public void shoot()
        {
            bullet.Position = cannon.Position;
            bullet.Position+= new Vector3(0, shootingPosition.Y, 0);
            bullet.moveOrientedY(shootingPosition.X);
            bullet.Rotation = cannon.Rotation;
            bullet.rotateY((float)Math.PI);
            bullet.updateValues();
        }

        public void render(float speed)
        {
            bullet.moveOrientedY(5 * speed);
            bullet.Position += new Vector3(0, 1, 0);
            bullet.render();
            cannon.render();
        }
        public void dispose()
        {
            cannon.dispose();
            bullet.dispose();
        }

    }
}
