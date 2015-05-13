using System;
using System.Collections.Generic;
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

namespace AlumnoEjemplos.MiGrupo
{
    /// <summary>
    /// Ejemplo del alumno
    /// </summary>
    public class EjemploAlumno : TgcExample
    {

        TgcBox canon;
        TgcBox water;
        TgcSkeletalMesh mesh;
        Vector3 lastYposition = new Vector3(0, 0, 0);
        GenericShip ship;
        /// <summary>
        /// Categoría a la que pertenece el ejemplo.
        /// Influye en donde se va a haber en el árbol de la derecha de la pantalla.
        /// </summary>
        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        /// <summary>
        /// Completar nombre del grupo en formato Grupo NN
        /// </summary>
        public override string getName()
        {
            return "Discreta Ship";
        }

        /// <summary>
        /// Completar con la descripción del TP
        /// </summary>
        public override string getDescription()
        {
            return "Battle Ship";
        }

        /// <summary>
        /// Método que se llama una sola vez,  al principio cuando se ejecuta el ejemplo.
        /// Escribir aquí todo el código de inicialización: cargar modelos, texturas, modifiers, uservars, etc.
        /// Borrar todo lo que no haga falta
        /// </summary>
        public override void init()
        {




            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;


            Vector3 center = new Vector3(0, 0, 0);
            Vector3 size = new Vector3(500, 1, 500);
            Color color = Color.Aqua;


            water = TgcBox.fromSize(center, size, color);
            ship = new GenericShip();
            canon = TgcBox.fromSize(new Vector3(0, 3, 0), new Vector3(1, 1, 1), Color.Black);

            GuiController.Instance.ThirdPersonCamera.Enable = true;
            Vector3 CameraPosition = ship.Position();
            GuiController.Instance.ThirdPersonCamera.setCamera(CameraPosition, 25, -50);







        }
        public override void render(float elapsedTime)
        {

            //Device de DirectX para renderizar
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;

            GuiController.Instance.ThirdPersonCamera.updateCamera();


            //Calcular proxima posicion de personaje segun Input
            Vector3 move = new Vector3(0, 0, 0);


            //Multiplicar la velocidad por el tiempo transcurrido, para no acoplarse al CPU
            float speed = 10f * elapsedTime;

            if (d3dInput.keyDown(Key.W))
            {
                ship.acelerate(speed);
                canon.Position = ship.Position() + new Vector3(0, 1, 0);


            }

            //Atras
            if (d3dInput.keyDown(Key.S))
            {
                ship.desacelerate(speed);
                canon.moveOrientedY(-speed);

            }

            //Izquierda
            if (d3dInput.keyDown(Key.A))
            {

                ship.turnLeft(elapsedTime);
                canon.rotateY(-(float)Math.PI * 3 / 4 * elapsedTime);


            }

            //Derecha
            if (d3dInput.keyDown(Key.D))
            {

                ship.turnRigth(elapsedTime);
                canon.rotateY((float)Math.PI * 3 / 4 * elapsedTime);


            }


            //Izquierda cañon
            if (d3dInput.keyDown(Key.LeftArrow))
            {

                canon.rotateY((float)Math.PI * 3 / 4 * elapsedTime);
                lastYposition = canon.Rotation;
            }

            //Derecha cañon
            if (d3dInput.keyDown(Key.RightArrow))
            {

                canon.rotateY(-(float)Math.PI * 3 / 4 * elapsedTime);
                lastYposition = canon.Rotation;

            }

            //Disparo
            if (d3dInput.keyPressed(Key.Space))
            {
                 canon.moveOrientedY(-speed);

            }



            water.setTexture(TgcTexture.createTexture(d3dDevice, "C:\\Git\\TgcViewer\\Examples\\Optimizacion\\Isla\\Textures\\agua 10.jpg"));


            canon.move(move);
            GuiController.Instance.ThirdPersonCamera.Target = ship.Position();
            ship.render();
            water.render();
            canon.render();


        }

        public override void close()
        {

            water.dispose();
            ship.dispose();
            canon.dispose();
        }

    }
}