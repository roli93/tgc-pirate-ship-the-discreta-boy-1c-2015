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
    public class Bullet
    {
        private TgcSphere bullet;
        private Vector3 renderLimit = new Vector3(10000,10000,10000);
        public Vector3 linearSpeed;
        public Vector2 initialSpeed = new Vector2(500, 500);

        public bool Visible { get;set;}

        public Bullet()
        {
            bullet = new TgcSphere();
            bullet.Radius = 3;
            bullet.setColor(Color.Black);
            bullet.LevelOfDetail = 1;
            bullet.updateValues();
        }

        public void beShot(Cannon carrier) //Esto se puede mejorar, pero no es priorotario
        {
                bullet.Position = carrier.Position;
                bullet.Position+= new Vector3(0, carrier.ShootingOffset.Y, 0);
                bullet.Rotation = carrier.Rotation;
                bullet.rotateY((float)Math.PI);
                bullet.moveOrientedY(carrier.ShootingOffset.X);
                bullet.updateValues();
                float parallelSpeedIncrement = carrier.LinearSpeed*-(float)Math.Cos((float)carrier.RelativeRotation.Y);
                float orthogonalSpeedIncrement = carrier.LinearSpeed * (float)Math.Sin((float)carrier.RelativeRotation.Y);
                linearSpeed = new Vector3(initialSpeed.X + parallelSpeedIncrement, initialSpeed.Y, orthogonalSpeedIncrement);
                Visible = true;
        }

        public void render(float elapsedTime)
        {
            if (Visible)
            {
                moveObliquely(elapsedTime);
                bullet.render();
                if (bullet.Position.X > renderLimit.X || bullet.Position.Y > renderLimit.Y || bullet.Position.Z > renderLimit.Z ||
                    bullet.Position.X < -renderLimit.X || bullet.Position.Y < -renderLimit.Y || bullet.Position.Z < -renderLimit.Z)
                    Visible = false;
            }
        }

        private void moveObliquely(float elapsedTime)
        {
            //MRU
            bullet.moveOrientedY(linearSpeed.X * elapsedTime);//V paralela al cañon
            bullet.rotateY((float)Math.PI * 0.5F);
            bullet.moveOrientedY(linearSpeed.Z * elapsedTime);//V perpendicular al cañon
            bullet.rotateY(-(float)Math.PI * 0.5F);
            //MRUV- Tirto vertical
            bullet.move(0,linearSpeed.Y * elapsedTime,0);
            linearSpeed.Y -= 1F;
        }

        public void dispose()
        {
            bullet.dispose();
        }
    }
}
