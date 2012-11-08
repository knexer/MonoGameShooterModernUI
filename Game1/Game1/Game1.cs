using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using Shooter.Components;
using Shooter.Systems;
using Shooter.EntityManagers;
using Shooter.Systems.SystemTopologicalSort;
using Game1.Systems.CollisionResolutionSystems;
using Shooter.Systems.MarkForDestructionSystems;
using Shooter.Systems.OnDestructionSystems;
using Shooter.SceneInitialization.ShooterSceneInit;

namespace Shooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //The systems that must be executed on the game objects.
        List<ASystem> updateTimeSystems;
        List<ASystem> renderTimeSystems;

        // Represents the player
        EntityManagerManager entityStorage;

        Random rand;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            rand = new Random();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Initialize the entity list
            entityStorage = new EntityManagerManager();

            //Initialize the systems lists
            updateTimeSystems = new List<ASystem>();
            renderTimeSystems = new List<ASystem>();

            //Enable the FreeDrag gesture.
            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            renderTimeSystems.Add(new AnimationResolutionSystem());

            // Initialize the renderer with the new spritebatch
            renderTimeSystems.Add(new RenderSystem(spriteBatch));

            updateTimeSystems.Add(new PeriodicAddComponentUpdater());

            updateTimeSystems.Add(new SpawnEntitySystem());

            updateTimeSystems.Add(new PlayerMovementSystem());

            updateTimeSystems.Add(new LinearMovementSystem());

            updateTimeSystems.Add(new RandomPositionOffsetSystem());

            updateTimeSystems.Add(new EntityTranslationSystem());

            //Needs the screen size to clamp to screen
            updateTimeSystems.Add(new ClampingSystem(GraphicsDevice.Viewport));

            updateTimeSystems.Add(new AnimationUpdateSystem());

            //Needs the screen size to wrap to screen
            updateTimeSystems.Add(new TiledScreenWrappingSystem(GraphicsDevice.Viewport));

            updateTimeSystems.Add(new PositionSlavingSystem());

            updateTimeSystems.Add(new PreMovementStrategyDummySystem());

            updateTimeSystems.Add(new PostMovementStrategyDummySystem());

            updateTimeSystems.Add(new PreCollisionResolutionDummySystem());

            updateTimeSystems.Add(new PostCollisionResolutionDummySystem());

            updateTimeSystems.Add(new BroadPhaseCollisionDetection());

            updateTimeSystems.Add(new DamageOnContactSystem());

            updateTimeSystems.Add(new SpawnEntityAtPositionSystem());

            //Entity Destruction

            updateTimeSystems.Add(new PreMarkForDestructionSystem());

            updateTimeSystems.Add(new PostMarkForDestructionSystem());

            updateTimeSystems.Add(new DestroyNoHealthEntitiesSystem());

            updateTimeSystems.Add(new DestroyOffScreenEntitiesSystem(GraphicsDevice.Viewport));

            updateTimeSystems.Add(new RemoveEntitiesMarkedForDestructionSystem());

            updateTimeSystems.Add(new PreOnDestructionSystem());

            updateTimeSystems.Add(new PostOnDestructionSystem());

            updateTimeSystems.Add(new AddComponentOnDestructionSystem());

            updateTimeSystems.Add(new DestroyAnimationCompleteEntitiesSystem());

            updateTimeSystems.Add(new DestroyChildEntitiesSystem());

            // Sound systems

            updateTimeSystems.Add(new PlaySoundEffectSystem());

            updateTimeSystems.Add(new PlayMusicSystem());

            initializeSystems();
            computeSystemOrderings();
            new PlayerInit().InitializeScene(entityStorage, this);
            new BackgroundInit().InitializeScene(entityStorage, this);
            new MinefieldInit().InitializeScene(entityStorage, this);
            new MusicInit().InitializeScene(entityStorage, this);
        }

        private void computeSystemOrderings()
        {
            SystemTopologicalSorter updateSystemSorter = new SystemTopologicalSorter(updateTimeSystems);
            updateTimeSystems = updateSystemSorter.getSortedOrdering();

            SystemTopologicalSorter renderSystemSorter = new SystemTopologicalSorter(renderTimeSystems);
            renderTimeSystems = renderSystemSorter.getSortedOrdering();
        }

        private void initializeSystems()
        {
            foreach (ASystem toInit in updateTimeSystems)
            {
                toInit.Initialize(entityStorage);
            }

            foreach (ASystem toInit in renderTimeSystems)
            {
                toInit.Initialize(entityStorage);
            }
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
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            foreach (ASystem cur in updateTimeSystems)
            {
                cur.Process(entityStorage, gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Start drawing
            spriteBatch.Begin();

            foreach (ASystem cur in renderTimeSystems)
            {
                cur.Process(entityStorage, gameTime);
            }

            //Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
