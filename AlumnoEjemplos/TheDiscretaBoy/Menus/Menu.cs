﻿using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Input;
using Microsoft.DirectX.DirectInput;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.Collision.ElipsoidCollision;
using TgcViewer.Utils.Shaders;
using System.Windows.Forms;
using TgcViewer.Utils._2D;
using TgcViewer.Utils;
using AlumnoEjemplos.TheDiscretaBoy;

namespace AlumnoEjemplos.TheDiscretaBoy.Menus
{
    public class Menu
    {
        public List<MenuButton> buttons = new List<MenuButton>();
        public MenuButton header;
        public TgcSprite defaultBackgroud = new TgcSprite();

        public Menu(string headerPath)
        {
            this.defaultBackgroud.Texture = TgcTexture.createTexture(
                GuiController.Instance.AlumnoEjemplosMediaDir + "\\Texturas\\background.jpg");
            this.defaultBackgroud.Position = new Vector2(0, 0);
            this.defaultBackgroud.Scaling = new Vector2(
                (float)ScreenHelper.size().Width / this.defaultBackgroud.Texture.Width,
                (float)ScreenHelper.size().Height / this.defaultBackgroud.Texture.Height);
            this.setHeader(headerPath);

            addDifficultyButton(
                1,
                "\\Texturas\\Menu\\very_easy.jpg",
                1);
            addDifficultyButton(
                2,
                "\\Texturas\\Menu\\easy.jpg",
                2);
            addDifficultyButton(
                3,
                "\\Texturas\\Menu\\medium.jpg",
                3);
            addDifficultyButton(
                4,
                "\\Texturas\\Menu\\difficult.jpg",
                4);
            addDifficultyButton(
                5,
                "\\Texturas\\Menu\\very_difficult.jpg",
                5);
            addDifficultyButton(
                6,
                "\\Texturas\\Menu\\mission_impossible.jpg",
                6);
        }

        public void setHeader(string headerPath)
        {
            TgcSprite sprite = new TgcSprite();
            sprite.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + headerPath);
            this.header = new MenuButton(
                            -(0.5f),
                            sprite,
                            (game) => {});
        }

        private void addDifficultyButton(int position, string texturePath, int enemiesQuantity)
        {
            TgcSprite sprite = new TgcSprite();
            sprite.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + texturePath);
            buttons.Add(
                new MenuButton(
                    position,
                    sprite,
                    (game) => {
                        game.enemiesQuantity = enemiesQuantity;
                        game.play();
                    }));
        }
 
        public void render(EjemploAlumno game)
        {
            GuiController.Instance.Drawer2D.beginDrawSprite();
            if (game.status == GameStatus.UnStarted)
            {
                this.defaultBackgroud.render();
            }
            this.handleClicks(game);
            this.header.render();
            this.buttons.ForEach(button => button.render());
            GuiController.Instance.Drawer2D.endDrawSprite();
        }

        public void handleClicks(EjemploAlumno game)
        {
            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;
            float mouseX = d3dInput.Xpos;
            float mouseY = d3dInput.Ypos;

            if (d3dInput.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                foreach(MenuButton button in buttons)
                {
                    button.handleClick(game, mouseX, mouseY);
                }
            }
        }
    }
}
