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
using AlumnoEjemplos.TheDiscretaBoy.WeatherElements;
using AlumnoEjemplos.TheDiscretaBoy.Menus;

namespace AlumnoEjemplos.TheDiscretaBoy
{
    /// <summary>
    /// Ejemplo del alumno
    /// </summary>

    public enum GameStatus
    {
        Playing, Stopped, UnStarted
    }

    public enum Environment
    {
        Development, Production
    }

    public class EjemploAlumno : TgcExample
    {
        public static EjemploAlumno Instance { get; set; }
        public TgcMesh meshShip, meshEnemy;
        public SmartTerrain water;
        public TgcSphere sky;
        public GenericShip playerShip;
        public List<EnemyShip> enemies = new List<EnemyShip>();
        public TgcBoundingSphere skyBoundaries;
        public Vector3 lastPlayerPosition;
        Microsoft.DirectX.Direct3D.Effect efectoOlas;
        Microsoft.DirectX.Direct3D.Effect efectoCascada;
        string currentHeightmap;
        string currentTexture;
        float time;
        public Weather weather;
        public GameStatus status;
        public int enemiesQuantity = 0;
        public Menu menu = new Menu("\\Texturas\\logo.png");
        public Environment environment = Environment.Production;
        public Microsoft.DirectX.Direct3D.Effect effect;
        TgcBox lightBox;

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
            
           /* water = TgcBox.fromSize(new Vector3(0, 0, 0), new Vector3(10000, 1, 10000), Color.Aqua);
            water.setTexture(TgcTexture.createTexture(d3dDevice, texturesPath + "ThickCloudsWaterDown2048.png"))*/;

            currentHeightmap = GuiController.Instance.AlumnoEjemplosMediaDir + "texturas\\PerlinNoise.jpg";
            currentTexture =  texturesPath + "ThickCloudsWaterDown2048.png";
            water = new SmartTerrain();
            water.loadHeightmap(currentHeightmap, 200, /* (float)GuiController.Instance.Modifiers["WorldSize"], (float)GuiController.Instance.Modifiers["AlturaMarea"]*/1, new Vector3(0, 0, 0));
            water.loadTexture(currentTexture);
            efectoOlas = TgcShaders.loadEffect(GuiController.Instance.AlumnoEjemplosMediaDir + "Shaders\\shaderOlas.fx");
            water.Effect = efectoOlas;
            water.Technique = "RenderScene";
            
            sky = new TgcSphere();
            sky.Radius = 5000;
            this.useDefaultSkyTexture();
            sky.LevelOfDetail = 1;
            sky.Position = water.Center;
            sky.rotateY(-(float)Math.PI * 1 / 4);
            sky.updateValues();

            skyBoundaries = new TgcBoundingSphere(sky.Position + new Vector3(0,0,-1800), 2100);
            sky.updateValues();
            GuiController.Instance.Modifiers.addVertex3f("LightPosition", new Vector3(-5000, -5000, -5000), new Vector3(2000, 2000, 2000), skyBoundaries.Position + new Vector3(0, 0, 0));
            GuiController.Instance.Modifiers.addFloat("Ambient", 0, 1, 0.5f);
            GuiController.Instance.Modifiers.addFloat("Diffuse", 0, 1, 0.6f);
            GuiController.Instance.Modifiers.addFloat("Specular", 0, 1, 0.5f);
            GuiController.Instance.Modifiers.addFloat("SpecularPower", 1, 10, 1);

            lightBox = TgcBox.fromSize(new Vector3(5, 5, 5), Color.Yellow);

            effect = TgcShaders.loadEffect(GuiController.Instance.AlumnoEjemplosMediaDir +"Shaders\\PhongShading.fx"); 

            this.weather = new Weather();

            createUserVars();
            this.status = GameStatus.UnStarted;
        }

        public void stop()
        {
            this.status = GameStatus.Stopped;
        }

        public void play()
        {
            initializeGame();
            this.status = GameStatus.Playing;
        }

        public bool crashingWithAnyOther(GenericShip ship)
        {
            foreach (GenericShip anotherShip in this.allShips())
            {
                if (ship.crashingWith(anotherShip))
                {
                    return true;
                }
            }
            return false;
        }

        public void useDefaultSkyTexture()
        {
            this.useSkyTexture(
                TgcTexture.createTexture(
                GuiController.Instance.AlumnoEjemplosMediaDir + "Texturas\\SkyboxSet1\\ThickCloudsWater\\sky-dome-panorma2.jpg"));
        }

        public void useSkyTexture(TgcTexture texture)
        {
            this.sky.setTexture(texture);
        }

        private void createUserVars()
        {
            GuiController.Instance.UserVars.addVar("terreno", water);
            GuiController.Instance.Modifiers.addFloat("Altura del oleaje", 0, 100, 40f);
            GuiController.Instance.Modifiers.addBoolean("Desarrollo", "Desarrollo", false);
        }

        public float alturaEnPunto(float X, float Z)
        {
            SmartTerrain terrain = (SmartTerrain)GuiController.Instance.UserVars.getValue("terreno");
            float heighM = meshShip.Scale.Y;
            Vector2 texCoords;
            terrain.xzToHeightmapCoords(X, Z, out texCoords);
            float frecuencia = 10;
            float ola = FastMath.Sin(2 * (texCoords.Y / 2 - time)) + (float)GuiController.Instance.Modifiers.getValue("Altura del oleaje") * FastMath.Cos(2 * (texCoords.X / 5 - this.time));
            
            return (ola + heighM) * 0.1f * frecuencia;
        }

        public Vector3 normalEnPunto(float X, float Z)
        {
            float delta = 0.3f;
            float alturaN = this.alturaEnPunto(X, Z + delta);
            float alturaS = this.alturaEnPunto(X, Z - delta);
            float alturaE = this.alturaEnPunto(X + delta, Z);
            float alturaO = this.alturaEnPunto(X - delta, Z);

            Vector3 vectorEO = new Vector3(delta * 2, alturaE - alturaO, 0);
            Vector3 vectorNS = new Vector3(0, alturaN - alturaS, delta * 2);

            return Vector3.Cross(vectorNS, vectorEO);
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

            enemies.Clear();
            for (int i = 0; i < enemiesQuantity; i++)
            {
                sceneShip = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vehiculos\\Canoa\\Canoa-TgcScene.xml");
                meshEnemy = sceneShip.Meshes[0];
                sceneCanon = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Armas\\Canon\\Canon.max-TgcScene.xml");
                TgcMesh meshCannon = sceneCanon.Meshes[0];

                meshEnemy.Effect = effect;
                meshCannon.Effect = effect;           
                
                meshEnemy.Technique = "DefaultTechnique";
                meshCannon.Technique = "DefaultTechnique";

                enemies.Add(new EnemyShip(meshEnemy, new Vector3(800 + (300 * i), 2, 800 + (300 * i)), new Cannon(meshCannon, new Vector3(27, 21, 0)), new Vector3(0, 1, 0), new Timer(1.5f + (i / 2))));
            }
        }

        public void handleEnemySunk()
        {
            if (allEnemiesSunk())
            {
                (new Triumph()).show();
            }
        }

        public bool allEnemiesSunk()
        {
            foreach (EnemyShip enemyShip in enemies)
                if (!enemyShip.isDead()) return false;
            return true;
         }

        public void updateEnvironment()
        {
            if ((bool)GuiController.Instance.Modifiers.getValue("Desarrollo"))
                this.environment = Environment.Development;
            else
                this.environment = Environment.Production;
        }

        public override void render(float elapsedTime)
        {
            updateEnvironment();

            if (this.status == GameStatus.UnStarted)
            {
                menu.render(this);
            }
            else
            {
                GuiController.Instance.ThirdPersonCamera.updateCamera();
                GuiController.Instance.ThirdPersonCamera.Target = playerShip.Position;

                TgcD3dInput d3dInput = GuiController.Instance.D3dInput;

                Vector3 lightPosition = new Vector3(0, 400, -5000);

                lightBox.Position = lightPosition;
            
                //Cargar variables de shader
                effect.SetValue("fvLightPosition", TgcParserUtils.vector3ToFloat3Array(lightPosition));
                effect.SetValue("fvEyePosition", TgcParserUtils.vector3ToFloat3Array(GuiController.Instance.ThirdPersonCamera.getPosition()));
                effect.SetValue("k_la", 0.55F);
                effect.SetValue("k_ld", 1);
                effect.SetValue("k_ls", 0.35F);
                effect.SetValue("fSpecularPower", 10);

                water.Effect.SetValue("fvLightPosition", TgcParserUtils.vector3ToFloat3Array(GuiController.Instance.ThirdPersonCamera.getPosition() + new Vector3(0, 2000, 0)));
                water.Effect.SetValue("fvEyePosition", TgcParserUtils.vector3ToFloat3Array(GuiController.Instance.RotCamera.getPosition()));
                water.Effect.SetValue("k_la", 1);
                water.Effect.SetValue("k_ld", 0);
                water.Effect.SetValue("k_ls", 1);
                water.Effect.SetValue("fSpecularPower", 1.9F);

                playerShip.ship.Effect = effect;
                playerShip.cannon.cannon.Effect = effect;
            

                playerShip.ship.Technique = "DefaultTechnique";
                playerShip.cannon.cannon.Technique = "DefaultTechnique";

                if (d3dInput.keyDown(Key.Escape))
                {
                    close();
                }
                time += elapsedTime;
                water.Effect.SetValue("time", time);
                water.Effect.SetValue("altura", (float)GuiController.Instance.Modifiers.getValue("Altura del oleaje"));
                water.render();

                sky.render();
                lightBox.render();

                forEachShip((Action<GenericShip>)((ship) => ship.render(elapsedTime)));

                weather.render();

                renderBars();
                
                if (this.status == GameStatus.Stopped)
                {
                    menu.render(this);
                }
                
            }

        }

        public void forEachShip(Action<GenericShip> todo)
        {
            allShips().ForEach(todo);
        }

        public void forEachShip(ShipCommand toDo) 
        {
            foreach(GenericShip ship in allShips())
            {
                toDo(ship);
            }
        }

        public List<GenericShip> allShips()
        {
            List<GenericShip> allShips = new List<GenericShip>();
            allShips.AddRange(enemies);
            allShips.Add(playerShip);
            return allShips;
        }

        public void renderBars()
        {
            //forEachShip((Action<GenericShip>)((ship)=> ship.renderBar()));
            playerShip.renderBar();
        }

        public void initializeGame() 
        {
            initializeShips();
            initializeCamera();
        }

        public override void close()
        {

            water.dispose();
            sky.dispose();
            forEachShip((Action<GenericShip>)((ship) => ship.dispose()));
        }

    }
}