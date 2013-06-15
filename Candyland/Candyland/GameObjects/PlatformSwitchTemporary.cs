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
    /// Switch is only activated, when a Player (or an Obstacle) is standing on the Platform.
    /// </summary>
    class PlatformSwitchTemporary : PlatformSwitch
    {
        public PlatformSwitchTemporary(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            this.ID = id;
            this.m_position = pos;
            this.isActive = false;
            this.isActivated = false;
            this.m_updateInfo = updateInfo;
        }

        public override void load(ContentManager content)
        {
            this.m_model = content.Load<Model>("plattformschalter");
            this.calculateBoundingBox();
        }


        /// <summary>
        /// Updates the Switch's states.
        /// </summary>
        public override void update()
        {
            if (this.isTouched)
            {
                // Activate when touch occurs and was deactivated before
                if(!this.isActivated)
                    this.setActivated(true);
            }
            // Deactivate when not touched
            else
                this.setActivated(false);
        }

    }
}
