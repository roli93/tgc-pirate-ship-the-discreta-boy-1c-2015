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
using TgcViewer.Utils.Terrain;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    /// <summary>
    /// Ejemplo del alumno
    /// </summary>
    public class EjemploAlumno : TgcExample
    {

        TgcMesh mesh;
        TgcBox water;
        TgcSkyBox cielo;
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
            string texturesPath = GuiController.Instance.ExamplesMediaDir + "Texturas\\Quake\\SkyBox LostAtSeaDay\\";
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene sceneShip = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vehiculos\\Canoa\\Canoa-TgcScene.xml");
            mesh = sceneShip.Meshes[0];
            TgcScene sceneCanon = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Armas\\Canon\\Canon.max-TgcScene.xml");

            water = TgcBox.fromSize(new Vector3(0, 0, 0), new Vector3(5000, 1, 5000), Color.Aqua);
            water.setTexture(TgcTexture.createTexture(d3dDevice, texturesPath + "lostatseaday_dn.jpg"));
            ship = new GenericShip(mesh, new Vector3(0, 2, 0),new Cannon(sceneCanon.Meshes[0], new Vector3(27,21,0)), new Vector3(0,1,0));

            cielo = new TgcSkyBox();
            cielo.Center = new Vector3(0, 500, 0);
            cielo.Size = new Vector3(5000, 1000, 5000);
            cielo.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "lostatseaday_up.jpg");
            cielo.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "lostatseaday_dn.jpg");
            cielo.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "lostatseaday_lf.jpg");
            cielo.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "lostatseaday_rt.jpg");
            cielo.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "lostatseaday_ft.jpg");
            cielo.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "lostatseaday_bk.jpg");


            GuiController.Instance.ThirdPersonCamera.Enable = true;
            Vector3 CameraPosition = ship.Position;
            GuiController.Instance.ThirdPersonCamera.setCamera(CameraPosition, 100, -750);
            //GuiController.Instance.RotCamera.Enable = true;

            cielo.updateValues();


        }

        public override void render(float elapsedTime)
        {

            GuiController.Instance.ThirdPersonCamera.updateCamera();
            GuiController.Instance.ThirdPersonCamera.Target = ship.Position;
            ship.render(elapsedTime);
            water.render();
            cielo.render();
        }

        public override void close()
        {

            water.dispose();
            ship.dispose();
            cielo.dispose();

        }



    }
}