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

    public abstract class AimingCapable
    {

        internal TgcSphere point;
        internal int pointOffset;

        public AimingCapable()
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
