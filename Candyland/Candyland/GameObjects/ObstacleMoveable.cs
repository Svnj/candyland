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
    /// Obstacle, that can be moved by the Player.
    /// </summary>
    class ObstacleMoveable : Obstacle
    {
        public ObstacleMoveable(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            this.ID = id;
            this.m_position = pos;
            this.isActive = false;
            this.m_updateInfo = updateInfo;
        }


        public override void load(ContentManager content)
        {
            this.m_model = content.Load<Model>("wunderkugelmovable");

            this.calculateBoundingBox();
        }


        public override void update()
        {
            // TODO Decide when to call move and with what parameters or maybe make different methodes like push and slide
            // this.move(...);
            // this.setActive(true); // when obstacle is moving
        }

        public override void collide(GameObject obj)
        {
            // TODO Test for Collison
        }

        public override void hasCollidedWith(GameObject obj)
        {
            // getting pushed by the player
            if (obj.GetType() == typeof(CandyGuy))
            {
                CandyGuy player = (CandyGuy)obj; /*TODO this is really ugly and should be changed.
                                                  * Maybe make speed and direction GameObject attributes*/
                move(false, player.getCurrentSpeed(),player.getDirection());
            }
        }


        /// <summary>
        /// Obstacle starts moving, when pushed by a Player
        /// </summary>
        /// <param name="direction">normalised Vector3 indicating the direction of the movement</param>
        /// <param name="slipperyGround">bool cointaining information about the platform, the object is currently on</param>
        /// <param name="speed">how fast the object ought to move</param>
        public void move(bool slipperyGround, float playerSpeed, Vector3 direction)
        {
            //TODO This is only a first try and should be changed, when we are clear on how this might work

            Vector3 newPosition;
            Vector3 translate;

            // Obstacle moves with constant speed, while on slippery Platforms and not colliding
            if (slipperyGround)
            {
                translate = 0.3f * direction;
                newPosition = this.getPosition() + translate; // TODO add speed constant to Game Constants Class
                this.setPosition(newPosition);
            }
            // Obstacle moves with the same speed as the Player, who is pushing it
            else
            {
                translate = playerSpeed * direction;
                newPosition = this.getPosition() + translate;
                this.setPosition(newPosition);
            }

            // move Bounding Box at the same time
            this.m_boundingBox.Min += translate;
            this.m_boundingBox.Max += translate;
        }


    }
}
