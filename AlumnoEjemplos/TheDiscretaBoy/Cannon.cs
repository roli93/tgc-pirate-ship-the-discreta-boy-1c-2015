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
    public class Cannon : Aimer
    {
        private Bullet currentBullet;
        private TgcMesh cannon;
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
            for (int i = 0; i <50; i++)
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

        public void render(float elapsedTime)
        {
            foreach(Bullet bullet in bullets)
                bullet.render(elapsedTime);
            cannon.render();
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

    class CircularBuffer<T> : List<T>
    {
        private int currentIndex=0;

        public T GetNext(){
            T element = this.ElementAt<T>(currentIndex);
            currentIndex = (currentIndex+1) % this.Count;
            return element;
        }

    }

    public abstract class Aimer
    {

        internal TgcSphere point;
        internal int pointOffset;

        public Aimer()
        {
            point = new TgcSphere();
            point.Radius = 5;
            point.setColor(Color.Red);
            point.LevelOfDetail = 1;
            point.updateValues();
            pointOffset = -100;
        }

        internal abstract TgcMesh getMesh();
        public abstract void turnRight(float elapsedTime);
        public abstract void turnLeft(float elapsedTime);

        public Vector2 Direction// vector x,z
        {
            get
            {
                Vector3 directon3D = point.Position - Position;
                return Vector2.Normalize(new Vector2(directon3D.X, directon3D.Z));
            }
        }

        public Vector3 Position
        {
            get
            {
                return getMesh().Position;
            }
            set
            {
                getMesh().Position = value;
                point.Position = value;
                point.moveOrientedY(pointOffset);
            }
        }

        internal void updateDirection(float elapsedTime, float speed)
        {
            point.Position = Position;
            point.rotateY(speed * elapsedTime);
            point.moveOrientedY(pointOffset);
        }

        private double angle(Vector2 a, Vector2 b)
        {
            if (a.Length() == 0 || b.Length() == 0)
                return 0;

            float absAtimesAbsb = a.Length() * b.Length();
            float dotAB = Vector2.Dot(a, b);
            float division = dotAB / absAtimesAbsb;
            double bareAngle = Math.Acos(division);

            double angulo = bareAngle * Math.Sign(Vector2.Ccw(a, b));
            return angulo;
        }

        public void aimAt(GenericShip objective, float elapsedTime)
        {
            if (onLeftSideOf(objective))
                turnRight(elapsedTime);
            else if (onRightSideOf(objective))
                turnLeft(elapsedTime);
        }

        public bool aimingAt(GenericShip ship)
        {
            Vector3 aimVector = ship.Position - Position;
            double theAngle = angle(Direction, new Vector2(aimVector.X, aimVector.Z));
            bool aiming = Math.Abs(theAngle) < 0.1F;
            return aiming;
        }

        private bool onRightSideOf(GenericShip ship)
        {
            Vector3 aimVector = ship.Position - Position;
            double theAngle = angle(Direction, new Vector2(aimVector.X, aimVector.Z));
            return theAngle > 0F;
        }


        private bool onLeftSideOf(GenericShip ship)
        {
            Vector3 aimVector = ship.Position - Position;
            double theAngle = angle(Direction, new Vector2(aimVector.X, aimVector.Z));
            return theAngle < 0F;
        }

    }
}
