﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    /// <summary>
    /// Obstacles are Objects in the Game World, placed on the Platforms and block the Players movement.
    /// The basic Obstacle cannot be moved or destroyed by the Player.
    /// </summary>
    class Obstacle : DynamicGameObjects
    {
        public Obstacle()
        {
        }

        public Obstacle(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            initialize(id, pos, updateInfo);
        }

        #region initialization

        protected virtual void initialize(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            base.init(id, pos, updateInfo);
            this.m_position.Y += 1f;
            this.m_original_position = this.m_position;
            this.original_isActive = false;
        }

        public override void load(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("lakritztextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Toon");
            this.m_model = content.Load<Model>("lakritzblock");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
        }

        #endregion

        public override void update()
        {
        }

        #region collision related

        public override void isNotCollidingWith(GameObject obj)
        {
        }

        public override void hasCollidedWith(GameObject obj)
        {
        }

        #endregion

        public override void draw()
        {
            base.draw();
        }

        

    }
}