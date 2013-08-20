using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Candyland
{
    /// <summary>
    /// This contains all the important data regarding updates.
    /// </summary>
    public class UpdateInfo
    {
        public string currentAreaID { get; set; }
        public string currentLevelID { get; set; }

        // if the player is on the last platform before a level change
        // the bool is true and the int tells which level exit it is
        // (this is used to start updating the next level early enough)
        public bool playerIsOnAreaExit { get; set; }
        public string areaAfterExitID { get; set; }

        // if the player is on the last platform before a level change
        // the bool is true and the int tells which level exit it is
        // (this is used to start updating the next area early enough)
        public bool playerIsOnLevelExit { get; set; }
        public string levelAfterExitID { get; set; }

        // if this is true we are currently processing a reset
        // which moves the player to the level start position
        // and resets the dynamic elements in the level
        public bool reset { get; set; }      
  
        // if this is true we are watching an action be performed
        // in "movie mode" and can't move (can click through a conversation though)
        // movie mode is a TODO!
        public bool locked { get; set; }

        /// <summary>
        /// all salesman IDs, which the player has already talked to
        /// </summary>
        public List<String> activeTeleports { get; set; }

        public int chocoChipsSpent { get; set; }

        public Matrix viewMatrix { get; set; }
        public Matrix projectionMatrix { get ; set; }

        public bool candyselected { get; set; }
        public void switchPlayer() { if(helperavailable) candyselected = !candyselected; }
        public bool helperavailable { get; set; }
        public void togglehelper() { helperavailable = !helperavailable; }

        public List<Keys> currentpushedKeys { get; set; }

        public GameTime gameTime { get; set; }

        public ScreenManager m_screenManager { get; set; }

        // we do not use this after all, probably; remove later!
        //public Dictionary<String, GameObject> currentObjectsToBeCollided { get; set; }

        /********************************************************************************
        Debugging Area
         * GraphicsDevice needed to render the Bounding Boxes
         * Parameter in Constructor can be removed, when rendering is no longer needed*/

        public GraphicsDevice graphics;

        /********************************************************************************/


        public UpdateInfo(GraphicsDevice graphicsDevice, ScreenManager screenManager)
        {
            currentAreaID = GameConstants.startAreaID;
            currentLevelID = GameConstants.startLevelID;

            playerIsOnAreaExit = false;
            areaAfterExitID = "";

            playerIsOnLevelExit = false;
            levelAfterExitID = "";

            reset = false;
            currentpushedKeys = new List<Keys>();

            // currently not in use, remove later!
            //currentObjectsToBeCollided = new Dictionary<String, GameObject>();

            candyselected = true;
            helperavailable = true;

            m_screenManager = screenManager;

            activeTeleports = new List<string>(10);

            /**********************************************************************/
            graphics = graphicsDevice;
            /**********************************************************************/
        }

    }
}
