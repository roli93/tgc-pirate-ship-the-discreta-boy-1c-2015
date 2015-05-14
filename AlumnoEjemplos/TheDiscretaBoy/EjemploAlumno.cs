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

        TgcMesh canon;
        TgcMesh mesh;
        TgcBox water;
        Vector3 lastYposition = new Vector3(0, 0, 0);
        GenericShip ship;

        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }


        public override string getName()
        {
            return "Discreta Ship";
        }


        public override string getDescription()
        {
            return "Battle Ship";
        }


        public override void init()
        {


            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene sceneShip = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vehiculos\\Canoa\\Canoa-TgcScene.xml");
            mesh = sceneShip.Meshes[0];
            TgcScene sceneCanon = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Armas\\Canon\\Canon.max-TgcScene.xml");
            canon = sceneCanon.Meshes[0];

            water = TgcBox.fromSize(new Vector3(0, 0, 0), new Vector3(5000, 1, 5000), Color.Aqua);
            water.setTexture(TgcTexture.createTexture(d3dDevice, GuiController.Instance.ExamplesDir + "Optimizacion\\Isla\\Textures\\agua 10.jpg"));
            ship = new GenericShip(mesh, new Vector3(0, 10, 0));



            GuiController.Instance.ThirdPersonCamera.Enable = true;
            Vector3 CameraPosition = ship.Position();
            GuiController.Instance.ThirdPersonCamera.setCamera(CameraPosition, 250, 500);


        }

        public override void render(float elapsedTime)
        {


            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;

            GuiController.Instance.ThirdPersonCamera.updateCamera();


            float speed = 100f * elapsedTime;

            if (d3dInput.keyDown(Key.W))
            {
                ship.acelerate(speed);
                canon.Position = ship.Position() + new Vector3(0, 1, 0);
            }


            if (d3dInput.keyDown(Key.S))
            {
                ship.desacelerate(speed);
                canon.moveOrientedY(-speed);

            }

            if (d3dInput.keyDown(Key.A))
            {

                ship.turnLeft(elapsedTime);
                canon.rotateY(-(float)Math.PI * 3 / 4 * elapsedTime);

            }

            if (d3dInput.keyDown(Key.D))
            {

                ship.turnRigth(elapsedTime);
                canon.rotateY((float)Math.PI * 3 / 4 * elapsedTime);
            }

            if (d3dInput.keyDown(Key.LeftArrow))
            {

                canon.rotateY(-(float)Math.PI * 3 / 4 * elapsedTime);

            }

            if (d3dInput.keyDown(Key.RightArrow))
            {

                canon.rotateY((float)Math.PI * 3 / 4 * elapsedTime);


            }








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