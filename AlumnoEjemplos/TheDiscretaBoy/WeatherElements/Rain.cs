using TgcViewer;
using TgcViewer.Utils._2D;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.DirectX.Direct3D;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.Sound;
using System.IO;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.TheDiscretaBoy.WeatherElements
{
    public class Rain : TgcAnimatedSprite
    {
        TgcMp3Player player = GuiController.Instance.Mp3Player;

        public Rain() : base(GuiController.Instance.AlumnoEjemplosMediaDir + "texturas\\LLUVIA2.png", new Size(128, 128),16,20)
        {
            Position = new Vector2(-10, 0);
            Scaling = new Vector2(8, 4);
            player.FileName = GuiController.Instance.AlumnoEjemplosMediaDir + "Sound\\rain-03.mp3";
            player.play(true);
            player.pause();
            GuiController.Instance.Modifiers.addBoolean("tormenta", "Iniciar tormenta", false);
        }

        new public void render()
        {
            if ((bool)GuiController.Instance.Modifiers.getValue("tormenta"))
            {
                if (player.getStatus() == TgcMp3Player.States.Paused)
                { 
                    player.resume();
                    EjemploAlumno.Instance.useSkyTexture(
                        TgcTexture.createTexture(
                            GuiController.Instance.D3dDevice,
                            GuiController.Instance.AlumnoEjemplosMediaDir + "Texturas\\storm_sky.jpg"));
                } 
                GuiController.Instance.Drawer2D.beginDrawSprite();
                updateAndRender();
                GuiController.Instance.Drawer2D.endDrawSprite();
            }
            else
            {
                if (player.getStatus() == TgcMp3Player.States.Playing)
                {
                    player.pause();
                }
                else
                {
                    EjemploAlumno.Instance.useDefaultSkyTexture();
                } 
           }
        }
    }
}
