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
        private Bullet currentBullet;
        private TgcMesh cannon;
        private CircularBuffer<Bullet> bullets = new CircularBuffer<Bullet>();
        private bool spaceDown=false;
        public float LinearSpeed{get;set;}
        private float rotationalSpeed = (float)Math.PI * 3 / 4;
        
        public Cannon(TgcMesh cannonMesh, Vector3 shootingPosition)
        {

            LinearSpeed = 0F;
            cannon = cannonMesh;
            this.ShootingOffset = shootingPosition;
            for (int i = 0; i <50; i++)
            { 
                bullets.Add(new Bullet());
            }
            currentBullet = bullets.GetNext();
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
            if(!currentBullet.Visible)
            {
                currentBullet.beShot(this);
                currentBullet = bullets.GetNext();
            }
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
               if(!spaceDown) //Para q no se apriete 20 millones de veces y espere sa que la suelten
                {
                    shoot();
                    spaceDown = true;
                }
            }
            else
            {
                spaceDown = false;
            }

            foreach(Bullet bullet in bullets)
                bullet.render(elapsedTime);
            cannon.render();
        }
        public void dispose()
        {
            cannon.dispose();
            currentBullet.dispose();
        }

    }

    class CircularBuffer<T> : List<T>
    {
        private int currentIndex=0;

        public T GetNext(){
            T element = this.ElementAt<T>(currentIndex);
            currentIndex = (currentIndex+1) % this.Count;
            return element;
        }

    }
}
