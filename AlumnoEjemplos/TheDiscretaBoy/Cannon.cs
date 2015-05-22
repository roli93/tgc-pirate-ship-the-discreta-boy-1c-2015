﻿using System;
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
        public float LinearSpeed{get;set;}
        public Vector3 RelativeRotation { get; set; }
        private float rotationalSpeed = (float)Math.PI * 3 / 4;
        
        public Cannon(TgcMesh cannonMesh, Vector3 shootingPosition)
        {
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

        public void rotateRight(float elapsedTime)
        {
            cannon.rotateY(rotationalSpeed * elapsedTime);
        }

        public void rotateLeft(float elapsedTime)
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
            foreach(Bullet bullet in bullets)
                bullet.render(elapsedTime);
            cannon.render();
        }

        public void turnRight(float elapsedTime)
        {
            Vector3 previousRotation = Rotation;
            rotateRight(elapsedTime);
            Vector3 rotationalIncrement = Rotation - previousRotation;
            RelativeRotation += rotationalIncrement;
        }

        public void turnLeft(float elapsedTime)
        {
            Vector3 previousRotation = Rotation;
            rotateLeft(elapsedTime);
            Vector3 rotationalIncrement = Rotation - previousRotation;
            RelativeRotation += rotationalIncrement;
        }
        public void dispose()
        {
            cannon.dispose();
            currentBullet.dispose();
        }

        private double angle(Vector2 a, Vector2 b)
        {
            float angulo = (float) (Vector2.Dot(a, b) == 0 ? Math.PI * .5F : Math.Acos((a.Length() * b.Length()) / Vector2.Dot(a, b)));
            return angulo;
        }

        public void aimAt(Vector3 objective)
        {
            Vector2 orientation = new Vector2((float)Math.Sin(cannon.Rotation.Y), -(float)Math.Cos(cannon.Rotation.Y));
            cannon.rotateY((float)angle(new Vector2(objective.X, objective.Z), orientation));
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
