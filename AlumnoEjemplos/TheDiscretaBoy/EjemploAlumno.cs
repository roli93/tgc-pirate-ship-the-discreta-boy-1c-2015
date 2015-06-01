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
using TgcViewer.Utils.Shaders;
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
        public TgcSphere sky;
        public GenericShip playerShip, enemyShip;
        public TgcBoundingSphere skyBoundaries;
        public Vector3 lastPlayerPosition;
        private TgcText2d playerMessage;

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
            string texturesPath = GuiController.Instance.AlumnoEjemplosMediaDir + "Texturas\\SkyboxSet1\\ThickCloudsWater\\";
            
            water = TgcBox.fromSize(new Vector3(0, 0, 0), new Vector3(10000, 1, 10000), Color.Aqua);
            water.setTexture(TgcTexture.createTexture(d3dDevice, texturesPath + "ThickCloudsWaterDown2048.png"));

            sky = new TgcSphere();
            sky.Radius = 5000;
            sky.setTexture(TgcTexture.createTexture(d3dDevice, texturesPath + "sky-dome-panorma2.jpg"));
            sky.LevelOfDetail = 1;
            sky.Position = water.Position;
            sky.rotateY(-(float)Math.PI * 1 / 4);
            sky.updateValues();

            skyBoundaries = new TgcBoundingSphere(sky.Position + new Vector3(0,0,-1800), 2100);
            sky.updateValues();
           /* GuiController.Instance.Modifiers.addFloat("Ambient", 0, 1, 0.5f);
            GuiController.Instance.Modifiers.addFloat("Diffuse", 0, 1, 0.6f);
            GuiController.Instance.Modifiers.addFloat("Specular", 0, 1, 0.5f);
            GuiController.Instance.Modifiers.addFloat("SpecularPower", 1, 100, 16); */

            initializeGame();
        }

        public void initializePlayerMessage(string message)
        {
            playerMessage = new TgcText2d();
            playerMessage.Text = message;
            playerMessage.changeFont(new System.Drawing.Font("Tahoma", 30));
            playerMessage.Color = Color.WhiteSmoke;
            playerMessage.Align = TgcText2d.TextAlign.CENTER;
            playerMessage.Size = new Size(1000, 150);

            Size screenSize = GuiController.Instance.Panel3d.Size;
            Size textSize = playerMessage.Size;
            playerMessage.Position = new Point(FastMath.Max(screenSize.Width / 2 - textSize.Width / 2, 0), screenSize.Height * 2 / 3);
            
        }

        public void initializeCamera()
        {
            GuiController.Instance.ThirdPersonCamera.Enable = true;
            Vector3 CameraPosition = playerShip.Position;
            GuiController.Instance.ThirdPersonCamera.setCamera(CameraPosition, 100, -750);
        }

        public void initializeShips()
        {
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene sceneShip = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vehiculos\\Canoa\\Canoa-TgcScene.xml");
            meshShip = sceneShip.Meshes[0];
            TgcScene sceneCanon = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Armas\\Canon\\Canon.max-TgcScene.xml");

            playerShip = new PlayerShip(meshShip, new Vector3(-100, 2, -1800), new Cannon(sceneCanon.Meshes[0], new Vector3(27, 21, 0)), new Vector3(0, 1, 0));

            sceneShip = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vehiculos\\Canoa\\Canoa-TgcScene.xml");
            meshEnemy = sceneShip.Meshes[0];
            sceneCanon = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Armas\\Canon\\Canon.max-TgcScene.xml");

            enemyShip = new EnemyShip(meshEnemy, new Vector3(1000, 2, 1000), new Cannon(sceneCanon.Meshes[0], new Vector3(27, 21, 0)), new Vector3(0, 1, 0));
        }

        public override void render(float elapsedTime)
        {

            GuiController.Instance.ThirdPersonCamera.updateCamera();
            GuiController.Instance.ThirdPersonCamera.Target = playerShip.Position;

            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;

            /*Microsoft.DirectX.Direct3D.Effect effect;

            effect = TgcShaders.loadEffect(GuiController.Instance.ExamplesDir + "Shaders\\WorkshopShaders\\Shaders\\PhongShading.fx"); ;

            effect.SetValue("fvLightPosition", TgcParserUtils.vector3ToFloat3Array(GuiController.Instance.ThirdPersonCamera.getPosition() + new Vector3(0, 2000, 0)));
            effect.SetValue("fvEyePosition", TgcParserUtils.vector3ToFloat3Array(GuiController.Instance.ThirdPersonCamera.getPosition()));
            effect.SetValue("k_la", (float)GuiController.Instance.Modifiers["Ambient"]);
            effect.SetValue("k_ld", (float)GuiController.Instance.Modifiers["Diffuse"]);
            effect.SetValue("k_ls", (float)GuiController.Instance.Modifiers["Specular"]);
            effect.SetValue("fSpecularPower", (float)GuiController.Instance.Modifiers["SpecularPower"]);

            playerShip.ship.Effect = effect;
            playerShip.cannon.cannon.Effect = effect;
            water.Effect = effect;
            

            playerShip.ship.Technique = "DefaultTechnique";
            playerShip.cannon.cannon.Technique = "DefaultTechnique";
            water.Technique = "DefaultTechnique";*/

            if (d3dInput.keyDown(Key.R))
            {
                initializeGame();
            }

            playerShip.render(elapsedTime);
            water.render();
            sky.render();
            enemyShip.render(elapsedTime);
            Notification.instance.render();
            
            GuiController.Instance.Drawer2D.beginDrawSprite();
            playerMessage.render();
            GuiController.Instance.Drawer2D.endDrawSprite();
        }

        public void initializeGame() 
        {
            initializePlayerMessage("");
            Notification.instance.sprite = null;
            initializeShips();
            initializeCamera();
        }

        public override void close()
        {

            water.dispose();
            playerShip.dispose();
            sky.dispose();
            enemyShip.dispose();
            Notification.instance.dispose();
            playerMessage.dispose();
        }



    }
}