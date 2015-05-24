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
        public static EjemploAlumno Instance { get; set; }
        public TgcMesh meshShip, meshEnemy;
        public TgcBox water;
        public TgcSphere sky;
        public GenericShip playerShip, enemyShip;
        public TgcBoundingSphere skyBoundaries;
        public Vector3 lastPlayerPosition;

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
            EjemploAlumno.Instance = this;

            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            string texturesPath = GuiController.Instance.ExamplesMediaDir + "Texturas\\SkyboxSet1\\ThickCloudsWater\\";
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene sceneShip = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vehiculos\\Canoa\\Canoa-TgcScene.xml");
            meshShip = sceneShip.Meshes[0];
            TgcScene sceneCanon = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Armas\\Canon\\Canon.max-TgcScene.xml");

            playerShip = new PlayerShip(meshShip, new Vector3(-100, 2, -1800), new Cannon(sceneCanon.Meshes[0], new Vector3(27, 21, 0)), new Vector3(0, 1, 0));

            water = TgcBox.fromSize(new Vector3(0, 0, 0), new Vector3(10000, 1, 10000), Color.Aqua);
            water.setTexture(TgcTexture.createTexture(d3dDevice, texturesPath + "ThickCloudsWaterDown2048.png"));

            sceneShip = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vehiculos\\Canoa\\Canoa-TgcScene.xml");
            meshEnemy = sceneShip.Meshes[0];
            sceneCanon = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Armas\\Canon\\Canon.max-TgcScene.xml");

            enemyShip = new EnemyShip(meshEnemy, new Vector3(1000, 2, 1000), new Cannon(sceneCanon.Meshes[0], new Vector3(27, 21, 0)), new Vector3(0, 1, 0));

            sky = new TgcSphere();
            sky.Radius = 5000;
            sky.setTexture(TgcTexture.createTexture(d3dDevice, texturesPath + "sky-dome-panorma2.jpg"));
            sky.LevelOfDetail = 1;
            sky.Position = water.Position;
            sky.rotateY(-(float)Math.PI * 1 / 4);
            sky.updateValues();

            skyBoundaries = new TgcBoundingSphere(sky.Position + new Vector3(0,0,-1800), 2100);

            GuiController.Instance.ThirdPersonCamera.Enable = true;
            Vector3 CameraPosition = playerShip.Position;
            GuiController.Instance.ThirdPersonCamera.setCamera(CameraPosition, 100, -750);

            sky.updateValues();


        }

        public override void render(float elapsedTime)
        {

            GuiController.Instance.ThirdPersonCamera.updateCamera();
            GuiController.Instance.ThirdPersonCamera.Target = playerShip.Position;

            playerShip.render(elapsedTime);
            water.render();
            sky.render();
            enemyShip.render(elapsedTime);
        }

        public override void close()
        {

            water.dispose();
            playerShip.dispose();
            sky.dispose();
            enemyShip.dispose();
        }



    }
}