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
using TgcViewer.Utils._2D;

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
        public TgcSphere cielo;
        public Vector3 lastYposition = new Vector3(0, 0, 0);
        public GenericShip ship, enemy;
        public TgcSprite barraDeVida;

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

            //Crear Sprite de barra de vida
            barraDeVida = new TgcSprite();
            barraDeVida.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\Texturas\\barra_de_vida.png");
            //La pongo arriba
            barraDeVida.Position = new Vector2(0,0);
            //La achico
            barraDeVida.Scaling = new Vector2(0.4f, 0.4f);

            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            string texturesPath = GuiController.Instance.ExamplesMediaDir + "Texturas\\SkyboxSet1\\ThickCloudsWater\\";
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene sceneShip = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vehiculos\\Canoa\\Canoa-TgcScene.xml");
            meshShip = sceneShip.Meshes[0];
            TgcScene sceneCanon = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Armas\\Canon\\Canon.max-TgcScene.xml");
            

            ship = new PlayerShip(meshShip, new Vector3(0, 2, 0),new Cannon(sceneCanon.Meshes[0], new Vector3(27,21,0)), new Vector3(0,1,0));

            water = TgcBox.fromSize(new Vector3(0, 0, 0), new Vector3(10000, 1, 10000), Color.Aqua);
            water.setTexture(TgcTexture.createTexture(d3dDevice, texturesPath + "ThickCloudsWaterDown2048.png"));

            sceneShip = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vehiculos\\Canoa\\Canoa-TgcScene.xml");
            meshEnemy = sceneShip.Meshes[0];
            sceneCanon = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Armas\\Canon\\Canon.max-TgcScene.xml");

            enemy = new EnemyShip(meshEnemy, new Vector3(100, 2, 0), new Cannon(sceneCanon.Meshes[0], new Vector3(27, 21, 0)), new Vector3(0, 1, 0));

            cielo = new TgcSphere();
            cielo.Radius = 5000;
            cielo.setTexture(TgcTexture.createTexture(d3dDevice, texturesPath + "sky-dome-panorma2.jpg"));
            cielo.LevelOfDetail = 1;
            cielo.Position = ship.Position;
            cielo.rotateY(-(float)Math.PI * 1 / 4);
            cielo.updateValues();
                /*new TgcSkyBox();
            cielo.Center = new Vector3(0, 512*5 , 0);
            cielo.Size = new Vector3(2048*5F, 1024*5F, 2048*5F);
            cielo.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "ThickCloudsWaterUp2048.png");
            cielo.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "ThickCloudsWaterLeft2048.png");
            cielo.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "ThickCloudsWaterDown2048.png");
            cielo.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "ThickCloudsWaterRight2048.png");
            cielo.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "ThickCloudsWaterBack2048.png");
            cielo.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "ThickCloudsWaterFront2048.png");*/


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
            enemy.render(elapsedTime);

            GuiController.Instance.Drawer2D.beginDrawSprite();
            barraDeVida.render();
            GuiController.Instance.Drawer2D.endDrawSprite();
        }

        public override void close()
        {

            water.dispose();
            ship.dispose();
            cielo.dispose();
            enemy.dispose();
            barraDeVida.dispose();
        }



    }
}