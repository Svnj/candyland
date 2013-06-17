﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Candyland
{
    class ObstacleForSwitch : Obstacle
    {
        public ObstacleForSwitch(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            base.initialize(id, pos, updateInfo);
        }

        public override void load(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("blocktextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("Toon");
            this.m_model = content.Load<Model>("blockmovable");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
        }


        public override void update()
        {
            // let the Object fall, if no collision with lower Objects
            fall();
            isonground = false;
        }

        #region collision

        // nothing to do here so far

        #endregion
    }
}