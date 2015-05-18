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
        private Vector3 renderLimit = new Vector3(1000,1000,1000);
        public Vector2 linearSpeed;

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
                linearSpeed = new Vector2(500F, 500F);
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
            bullet.moveOrientedY(linearSpeed.X* elapsedTime);
            //MRUV- Tirto vertical
            bullet.Position += new Vector3(0, linearSpeed.Y * elapsedTime, 0);
            linearSpeed.Y -= 1F;
        }

        public void dispose()
        {
            bullet.dispose();
        }
    }
}
