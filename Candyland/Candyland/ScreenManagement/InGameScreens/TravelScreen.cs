﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class TravelScreen : GameScreen
    {
        Texture2D background;
        Texture2D teleportSpot;
        Texture2D currentSpot;
        Texture2D selectedSpot;

        SpriteFont font;

        int screenWidth;
        int screenHeight;

        private string salesmanID;
        private UpdateInfo m_updateInfo;

        private int numOfTeleportOptions;
        private int activeIndex = 0;
        private int currentSpotIndex;
        private Vector2[] teleportPositions;

        // used to return to maingame istead of just going back into the dialog menu
        private GameScreen lastScreen;

        // Border
        protected Rectangle MenuBox;

        protected Rectangle MenuBoxTL;
        protected Rectangle MenuBoxTR;
        protected Rectangle MenuBoxBL;
        protected Rectangle MenuBoxBR;
        protected Rectangle MenuBoxL;
        protected Rectangle MenuBoxR;
        protected Rectangle MenuBoxT;
        protected Rectangle MenuBoxB;
        protected Rectangle MenuBoxM;

        protected Texture2D BorderTopLeft;
        protected Texture2D BorderTopRight;
        protected Texture2D BorderBottomLeft;
        protected Texture2D BorderBottomRight;
        protected Texture2D BorderLeft;
        protected Texture2D BorderRight;
        protected Texture2D BorderTop;
        protected Texture2D BorderBottom;
        protected Texture2D BorderMiddle;

        Button backButton;

        public TravelScreen(string saleID, UpdateInfo info, GameScreen dialogScreen)
        {
            salesmanID = saleID;
            m_updateInfo = info;
            lastScreen = dialogScreen;
        }

        public override void Open(Game game, AssetManager assets)
        {
            this.isFullscreen = true;

            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;

            font = assets.mainText;

            background = assets.map;
            teleportSpot = assets.pinAvailable;
            currentSpot = assets.pinCurrent;
            selectedSpot = assets.pinSelected;

            BorderTopLeft = assets.dialogTL;
            BorderTopRight = assets.dialogTR;
            BorderBottomLeft = assets.dialogBL;
            BorderBottomRight = assets.dialogBR;
            BorderLeft = assets.dialogL;
            BorderRight = assets.dialogR;
            BorderTop = assets.dialogT;
            BorderBottom = assets.dialogB;
            BorderMiddle = assets.dialogC;

            int offset = 5;

            int MenuBoxWidth = ScreenManager.PrefScreenWidth - 2 * offset;
            int MenuBoxHeight = ScreenManager.PrefScreenHeight - 2 * offset;

            MenuBox = new Rectangle((screenWidth - MenuBoxWidth) / 2, (screenHeight - MenuBoxHeight) / 2, MenuBoxWidth, MenuBoxHeight);

            MakeBorderBox(new Rectangle((screenWidth - MenuBoxWidth) / 2, (screenHeight - MenuBoxHeight) / 2, MenuBoxWidth, MenuBoxHeight),
                out MenuBoxTL, out MenuBoxT, out MenuBoxTR, out MenuBoxR,
                out MenuBoxBR, out MenuBoxB, out MenuBoxBL, out MenuBoxL, out MenuBoxM);

            numOfTeleportOptions = m_updateInfo.activeTeleports.Count;

            // fill list with active teleport spots
            int index = 0;
            teleportPositions = new Vector2[numOfTeleportOptions];
            foreach (string id in m_updateInfo.activeTeleports)
            {
                if (salesmanID == id)
                    currentSpotIndex = index;
                switch (id)
                {
                    case "0.Korridor": teleportPositions[index] = (new Vector2(160, 270)); break; //manually set position of map
                    case "schieb.k2": teleportPositions[index] = (new Vector2(180, 60)); break;
                    case "5.korridor": teleportPositions[index] = (new Vector2(450, 60)); break;
                    case "rutsch.korridor": teleportPositions[index] = (new Vector2(415, 240)); break;
                    case "255.2": teleportPositions[index] = (new Vector2(625, 160)); break;
                }
                index++;
            }

            backButton = new Button("Zurück", new Vector2(MenuBoxL.Right - 25, MenuBoxB.Top - 20), assets, this);
        }

        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.Input.Equals(InputState.Back))
            {
                ScreenManager.ResumeLast(this);
            }

            bool enterPressed = false;

            // look at input and update button selection
            switch (ScreenManager.Input)
            {
                case InputState.Continue: enterPressed = true; break;
                case InputState.Left: if (activeIndex == currentSpotIndex + 1) activeIndex -= 2; else activeIndex--; break;
                case InputState.Right: if (activeIndex == currentSpotIndex - 1) activeIndex += 2; else activeIndex++; break;
            }

            if (activeIndex == currentSpotIndex) activeIndex++;
            if (activeIndex > numOfTeleportOptions) activeIndex = 0;
            if (activeIndex < 0) activeIndex = numOfTeleportOptions;

            if (activeIndex == numOfTeleportOptions) backButton.selected = true;
            if (activeIndex == numOfTeleportOptions && enterPressed)
            {
                ScreenManager.ResumeLast(this);
                return;
            }

            // Open Question
            if (enterPressed && (activeIndex != currentSpotIndex))
            {
                ScreenManager.ActivateNewScreen(new TravelQuestion(m_updateInfo, lastScreen, this, activeIndex));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch m_sprite = ScreenManager.SpriteBatch;

            m_sprite.Begin();

            DrawBorder(m_sprite);

            Rectangle backgroundBox = new Rectangle(MenuBox.X, MenuBox.Y - 40, MenuBox.Width, MenuBox.Height);
            m_sprite.Draw(background, backgroundBox, Color.White);

            DrawTeleportPositions(m_sprite);

            m_sprite.Draw(currentSpot, new Rectangle(MenuBoxL.Left + 180, MenuBoxB.Top, 35, 35), Color.White);
            m_sprite.Draw(selectedSpot, new Rectangle(MenuBoxL.Left + 300, MenuBoxB.Top, 35, 35), Color.White);
            m_sprite.Draw(teleportSpot, new Rectangle(MenuBoxL.Left + 550, MenuBoxB.Top, 35, 35), Color.White);
            m_sprite.DrawString(font, "Hier", new Vector2(MenuBoxL.Left + 230, MenuBoxB.Top), Color.Black);
            m_sprite.DrawString(font, "ausgewähltes Ziel", new Vector2(MenuBoxL.Left + 350, MenuBoxB.Top), Color.Black);
            m_sprite.DrawString(font, "mögliches Ziel", new Vector2(MenuBoxL.Left + 600, MenuBoxB.Top), Color.Black);

            backButton.Draw(m_sprite);

            m_sprite.End();

            // we need the following as spriteBatch.Begin() sets them to None and AlphaBlend
            // which breaks our model rendering
            GraphicsDevice m_graphics = ScreenManager.Game.GraphicsDevice;
            m_graphics.DepthStencilState = DepthStencilState.Default;
            m_graphics.BlendState = BlendState.Opaque;
        }

        private void DrawTeleportPositions(SpriteBatch m_sprite)
        {
            int size = 35;
            int index = 0;
            foreach (Vector2 pos in teleportPositions)
            {
                if (index == currentSpotIndex)
                    m_sprite.Draw(currentSpot, new Rectangle(MenuBoxL.Left + (int)pos.X, MenuBoxT.Top + (int)pos.Y, size, size), Color.White);
                else if (index == activeIndex)
                    m_sprite.Draw(selectedSpot, new Rectangle(MenuBoxL.Left + (int)pos.X, MenuBoxT.Top + (int)pos.Y, size, size), Color.White);
                else
                    m_sprite.Draw(teleportSpot, new Rectangle(MenuBoxL.Left + (int)pos.X, MenuBoxT.Top + (int)pos.Y, size, size), Color.White);

                index++;
            }
        }

        private void DrawBorder(SpriteBatch m_sprite)
        {
            m_sprite.Draw(BorderTopLeft, MenuBoxTL, Color.White);
            m_sprite.Draw(BorderTopRight, MenuBoxTR, Color.White);
            m_sprite.Draw(BorderBottomLeft, MenuBoxBL, Color.White);
            m_sprite.Draw(BorderBottomRight, MenuBoxBR, Color.White);
            m_sprite.Draw(BorderLeft, MenuBoxL, Color.White);
            m_sprite.Draw(BorderRight, MenuBoxR, Color.White);
            m_sprite.Draw(BorderTop, MenuBoxT, Color.White);
            m_sprite.Draw(BorderBottom, MenuBoxB, Color.White);
            m_sprite.Draw(BorderMiddle, MenuBoxM, Color.White);
        }
    }
}
