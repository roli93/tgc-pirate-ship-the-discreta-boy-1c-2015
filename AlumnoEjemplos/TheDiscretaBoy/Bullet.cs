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

        public bool Visible { get;set;}

        public Bullet()
        {
            bullet = new TgcSphere();
            bullet.Radius = 3;
            bullet.setColor(Color.Black);
            bullet.LevelOfDetail = 5;
            bullet.updateValues();
        }

        public void beShot(Cannon carrier)
        {
            bullet.Position = carrier.Position;
            bullet.Position+= new Vector3(0, carrier.ShootingOffset.Y, 0);
            bullet.moveOrientedY(carrier.ShootingOffset.X);
            bullet.Rotation = carrier.Rotation;
            bullet.rotateY((float)Math.PI);
            bullet.updateValues();
            Visible = true;
        }

        public void render(float speed)
        {
            if (Visible)
            {
                //MRU
                bullet.moveOrientedY(5 * speed);
                //MRUV- Tirto vertical
                moveVertically();
                bullet.render();
                if (bullet.Position.X > renderLimit.X || bullet.Position.Y > renderLimit.Y || bullet.Position.Z > renderLimit.Z)
                    Visible = false;
            }
        }

        private void moveVertically()
        {
            bullet.Position += new Vector3(0, 1, 0);
        }

        public void dispose()
        {
            bullet.dispose();
        }
    }
}
