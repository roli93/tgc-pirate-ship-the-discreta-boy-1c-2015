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
    public class Cannon : AimingCapable
    {
        private Bullet currentBullet;
        public TgcMesh cannon;
        private CircularBuffer<Bullet> bullets = new CircularBuffer<Bullet>();
        public float LinearSpeed{get;set;}
        public Vector3 RelativeRotation { get; set; }
        private float rotationalSpeed = (float)Math.PI * 3 / 4;
        public Vector2 InitialSpeed { get; set; }
        
        public Cannon(TgcMesh cannonMesh, Vector3 shootingPosition) : base()
        {
            InitialSpeed = new Vector2(200, 200);
            RelativeRotation = new Vector3(0, 0, 0);
            LinearSpeed = 0F;
            cannon = cannonMesh;
            this.ShootingOffset = shootingPosition;
            for (int i = 0; i < 50; i++)
            {
                bullets.Add(new Bullet());
            }
            currentBullet = bullets.GetNext();
        }

        public Vector3 ShootingOffset{get;set;}


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

        public void rotateRight(float elapsedTime)
        {
            rotate(elapsedTime, rotationalSpeed);
        }

        public void rotateLeft(float elapsedTime)
        {
            rotate(elapsedTime, -rotationalSpeed);
        }

        private void rotate(float elapsedTime, float speed)
        {
            cannon.rotateY(speed * elapsedTime);
            updateDirection(elapsedTime, speed);
        }

        public void shootWithSpeed(Vector2 speed)
        {
            InitialSpeed = speed; 
            shoot();
        }

        public void shoot()
        {
            if(!currentBullet.Visible)
            {
                currentBullet.InitialSpeed = InitialSpeed;
                currentBullet.beShot(this);
                currentBullet = bullets.GetNext();
            }
        }

        public void renderMesh(float elapsedTime)
        {
            cannon.render();

            foreach (Bullet bullet in bullets)
                bullet.render(elapsedTime);
        }

        public override void turnRight(float elapsedTime)
        {
            Vector3 previousRotation = Rotation;
            rotateRight(elapsedTime);
            Vector3 rotationalIncrement = Rotation - previousRotation;
            RelativeRotation += rotationalIncrement;
        }

        public override void turnLeft(float elapsedTime)
        {
            Vector3 previousRotation = Rotation;
            rotateLeft(elapsedTime);
            Vector3 rotationalIncrement = Rotation - previousRotation;
            RelativeRotation += rotationalIncrement;
        }

        internal override TgcMesh getMesh()
        {
            return cannon;
        }

        public void dispose()
        {
            cannon.dispose();
            currentBullet.dispose();
        }

    }
    
}
