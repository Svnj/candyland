using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace Candyland
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        AssetManager assetManager;

        KeyboardState oldState;
        KeyboardState newState;

        // Class to hold all data for game settings
        SaveSettingsData settingsData;
        public bool mute;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            settingsData = new SaveSettingsData();
            LoadSettings();
            graphics.IsFullScreen = settingsData.isFullscreen;
            if (graphics.IsFullScreen)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                graphics.PreferredBackBufferWidth = 800;
                graphics.PreferredBackBufferHeight = 480;
            }
            // Create the screen manager component.
            assetManager = new AssetManager();
            screenManager = new ScreenManager(this, graphics.IsFullScreen, 800, 480, assetManager, settingsData);
            Components.Add(screenManager);

           // Content.RootDirectory = "CandylandContent"; 
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
           // IsMouseVisible = true;

            mute = false;
            // Set start screen
            screenManager.AddScreen(new TitleScreen());

            BalanceBoard.initialize(this.Window.Handle);

            base.Initialize();
        }

        private void LoadSettings()
        {
            string filename = "settings.sav";

            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader reader;
            
            try
            {
                reader = XmlReader.Create(filename, settings);
                using (reader)
                {
                    settingsData = IntermediateSerializer.
                        Deserialize<SaveSettingsData>
                        (reader, null);
                }                
            }
            catch
            {
                // if no user saved settings, set to default
                settingsData.isFullscreen = false;
                settingsData.musicVolume = 5;
                settingsData.shadowQuality = 2;
                settingsData.showTutorial = true;
                settingsData.soundVolume = 5;
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            newState = Keyboard.GetState();

            // Allows the game to exit, only for debugging
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            //    || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    this.Exit();

            // Controls to Mute background music
            if (newState.IsKeyDown(Keys.L) && newState != oldState)
            {
                if (!mute)
                {
                    MediaPlayer.Volume = 0;
                    mute = true;
                }
                else
                {
                    MediaPlayer.Volume = ((float)screenManager.Settings.musicVolume) / 10;
                    mute = false;
                }
            }

            // Update saved state.
            oldState = newState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        //protected override void Draw(GameTime gameTime)
        //{

        //}
    }
}
