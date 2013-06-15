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
        protected bool isOnSlipperyGround;

        public ObstacleMoveable(String id, Vector3 pos, UpdateInfo updateInfo)
        {
            this.ID = id;
            this.m_position = pos;
            this.m_position.Y += 0.2f;
            this.m_original_position = this.m_position;
            this.isActive = false;
            this.original_isActive = false;
            this.m_updateInfo = updateInfo;
            this.isOnSlipperyGround = false;
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
            this.currentspeed = 0;
            this.upvelocity = 0;
        }


        public override void load(ContentManager content)
        {
            this.m_texture = content.Load<Texture2D>("wunderkugeltextur");
            this.m_original_texture = this.m_texture;
            this.effect = content.Load<Effect>("ToonObjects");
            this.m_model = content.Load<Model>("wunderkugelmovable");
            this.m_original_model = this.m_model;

            this.calculateBoundingBox();
            minOld = m_boundingBox.Min;
            maxOld = m_boundingBox.Max;
        }


        public override void update()
        {
            fall();

            // Obstacle is sliding
            if (currentspeed != 0 && isOnSlipperyGround)
            {
                move();
            }
        }

        public override void collide(GameObject obj)
        {
            // TODO Test for Collison

            if (obj.GetType() == typeof(Platform))
            {
                // Obstacle sits on a Platform
                if (obj.getBoundingBox().Intersects(m_boundingBox))
                {
                    preventIntersection(obj);
                    Platform platform = (Platform) obj;
                    isOnSlipperyGround = platform.getSlippery();
                }
                else
                {
                    isonground = isonground || false;
                } 
            }
        }

        public override void hasCollidedWith(GameObject obj)
        {
            // getting pushed by the player
            if (obj.GetType() == typeof(CandyGuy))
            {
                this.isActive = true;
                this.currentspeed = obj.getCurrentSpeed();

                // Find out on which boundingbox side the collision occurs

                    // Obstacle should only be moved, if collided from the side
                    // Test if no collision in Y direction
                    if(obj.getBoundingBox().Min.Y > m_boundingBox.Max.Y
                        || obj.getBoundingBox().Max.Y < m_boundingBox.Min.Y)
                    {
                        //Test if collison in X direction
                        if (obj.getBoundingBox().Min.X < m_boundingBox.Max.X
                            || obj.getBoundingBox().Max.X > m_boundingBox.Min.X)
                        {
                            this.direction = new Vector3(obj.getDirection().X,0,0);
                        }
                        // Test if collision in Z direction
                        if (obj.getBoundingBox().Min.Z < m_boundingBox.Max.Z
                            || obj.getBoundingBox().Max.Z > m_boundingBox.Min.Z)
                        {
                            this.direction = new Vector3(0, 0, obj.getDirection().Z);
                        }
                    }
                move();
            }
        }


        /// <summary>
        /// Obstacle starts moving, when pushed by a Player
        /// </summary>
        /// <param name="direction">normalised Vector3 indicating the direction of the movement</param>
        /// <param name="slipperyGround">bool cointaining information about the platform, the object is currently on</param>
        /// <param name="speed">how fast the object ought to move</param>
        public void move()
        {
            Vector3 newPosition;
            Vector3 translate;

            // move Obstacle
                translate = currentspeed * direction;
                newPosition = this.getPosition() + translate;
                this.setPosition(newPosition);

            // move Bounding Box at the same time
            this.m_boundingBox.Min += translate;
            this.m_boundingBox.Max += translate;
        }


    }
}
