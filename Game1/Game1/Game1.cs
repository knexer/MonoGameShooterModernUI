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
using Game1.Components;
using Game1.Systems;

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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            updateTimeSystems.Add(new DestroyOffScreenEntitiesSystem(GraphicsDevice.Viewport));

            updateTimeSystems.Add(new DamageOnContactSystem());

            updateTimeSystems.Add(new DestroyNoHealthEntitiesSystem());

            initializeSystems();
            computeSystemOrderings();
            initializePlayer();
            initializeBackgrounds();
            initializeMinefield();
        }

        private void initializePlayer()
        {
            Entity player = new Entity();

            //give the player health
            HealthComponent hp = new HealthComponent();
            hp.Health = 100;
            player.AddComponent(hp);

            //Give the player a position
            PositionComponent pos = new PositionComponent();
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            pos.Position = playerPosition;
            player.AddComponent(pos);

            //And a texture
            TextureComponent tex = new TextureComponent();
            tex.Texture = Content.Load<Texture2D>("shipAnimation");
            tex.SourceRect = tex.Texture.Bounds;
            player.AddComponent(tex);

            //And the ability to react to inputs
            PlayerMovementComponent inputManager = new PlayerMovementComponent();
            player.AddComponent(inputManager);

            //And a constant movement speed
            MoveSpeedComponent speed = new MoveSpeedComponent();
            speed.MoveSpeed = 8.0f;
            player.AddComponent(speed);

            //And a bounding box for clamping (and collisions in the future!)
            AABBComponent aabb = new AABBComponent();
            aabb.Width = tex.Texture.Width / 8;
            aabb.Height = tex.Texture.Height;
            player.AddComponent(aabb);

            //And animation information
            AnimationComponent anim = new AnimationComponent();
            anim.CurrentFrameIndex = 0;
            anim.FrameDuration = 30;
            anim.Looping = true;
            anim.NumFrames = 8;
            anim.TimeSinceFrameChange = 0;
            player.AddComponent(anim);

            //And render layer information.  We'll have the player render at level 10 for now.
            RenderLayerComponent layer = new RenderLayerComponent();
            layer.LayerID = 10;
            player.AddComponent(layer);

            //And a component to indicate the the player needs to be clamped to the screen
            ScreenClampedComponent clamper = new ScreenClampedComponent();
            player.AddComponent(clamper);

            //And a component that indicates that the player can be destroyed if it runs out of health
            DestroyedWhenNoHealthComponent destruct = new DestroyedWhenNoHealthComponent();
            player.AddComponent(destruct);

            entityStorage.Add(player);
        }

        private void initializeBackgrounds()
        {
            initializeBackground(Content.Load<Texture2D>("mainBackground"), 0, 0);
            initializeBackground(Content.Load<Texture2D>("bgLayer1"), -1, 1);
            initializeBackground(Content.Load<Texture2D>("bgLayer2"), -2, 2);
        }

        private void initializeBackground(Texture2D texture, int moveSpeed, int renderLayer)
        {
            //Create the primary background
            Entity bg1 = createBackground(texture, moveSpeed, renderLayer);

            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;

            //Create and add the slaves
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    Entity slave = createPositionSlavedEntity(bg1, new Vector2(i * width, j * height));

                    //Add references to some of the master's components
                    slave.AddComponent(bg1.components[typeof(TextureComponent)]);
                    slave.AddComponent(bg1.components[typeof(AABBComponent)]);
                    slave.AddComponent(bg1.components[typeof(RenderLayerComponent)]);

                    entityStorage.Add(slave);
                }
            }

            entityStorage.Add(bg1);

        }

        private Entity createPositionSlavedEntity(Entity master, Vector2 offset)
        {
            Entity slave = new Entity();

            slave.AddComponent(new PositionComponent());

            SlavedPositionComponent comp = new SlavedPositionComponent();
            comp.master = master;
            comp.offset = offset;
            slave.AddComponent(comp);

            return slave;
        }

        private Entity createBackground(Texture2D tex, int pixelsPerFrame, int renderLayerID)
        {
            Entity bg = new Entity();

            //The background has a texture
            TextureComponent texComponent = new TextureComponent();
            texComponent.Texture = tex;
            texComponent.SourceRect = tex.Bounds;
            bg.AddComponent(texComponent);

            //The background has an AABB
            AABBComponent aabb = new AABBComponent();
            aabb.Height = tex.Height;
            aabb.Width = tex.Width;
            bg.AddComponent(aabb);

            //The background has linear movement
            LinearMovementComponent move = new LinearMovementComponent();
            bg.AddComponent(move);

            //The background moves at a specific speed
            MoveSpeedComponent speed = new MoveSpeedComponent();
            speed.MoveSpeed = pixelsPerFrame;
            bg.AddComponent(speed);

            //The background has a position
            PositionComponent pos = new PositionComponent();
            pos.Position = new Vector2(0, 0);
            bg.AddComponent(pos);

            //The background has a screen wrapping indicator
            ScreenWrappingComponent wrapIndicator = new ScreenWrappingComponent();
            bg.AddComponent(wrapIndicator);

            //The background is rendered at the indicated layer
            RenderLayerComponent layer = new RenderLayerComponent();
            layer.LayerID = renderLayerID;
            bg.AddComponent(layer);

            return bg;
        }

        private void initializeMinefield()
        {
            //Create a mine-spawning entity that spawns mines in random positions every once in a while
            Entity minespawner = new Entity();

            //Component: Spawn an entity
            SpawnEntityComponent spawnComponent = new SpawnEntityComponent();
            spawnComponent.EntityToSpawn = createMineTemplateEntity();
            
            //Component: Periodically add a spawn entity component
            PeriodicAddComponentComponent timer = new PeriodicAddComponentComponent();
            timer.ComponentToAdd = spawnComponent;
            timer.Period = 1000;
            timer.TimeSinceLastFiring = 0.0f;
            minespawner.AddComponent(timer);

            //Add the minefield to the entity store
            entityStorage.Add(minespawner);
        }

        private Entity createMineTemplateEntity()
        {
            Entity mineTemplate = new Entity();

            //Component: Damage the player.
            DamageOnContactComponent damage = new DamageOnContactComponent();
            damage.Damage = 10;
            mineTemplate.AddComponent(damage);

            //Component: Has health
            HealthComponent health = new HealthComponent();
            health.Health = 10;
            mineTemplate.AddComponent(health);

            //Component: Has a texture
            TextureComponent tex = new TextureComponent();
            tex.Texture = Content.Load<Texture2D>("mineAnimation");
            tex.SourceRect = tex.Texture.Bounds;
            mineTemplate.AddComponent(tex);

            //Component: Has an animation
            AnimationComponent anim = new AnimationComponent();
            anim.CurrentFrameIndex = 0;
            anim.FrameDuration = 30;
            anim.Looping = true;
            anim.NumFrames = 8;
            anim.TimeSinceFrameChange = 0;
            mineTemplate.AddComponent(anim);

            //Component: Has a position.
            PositionComponent pos = new PositionComponent();
            pos.Position.X = GraphicsDevice.Viewport.Width - 1;
            pos.Position.Y = GraphicsDevice.Viewport.Height / 2 - tex.SourceRect.Height / 2;
            mineTemplate.AddComponent(pos);

            //Component: Given a random starting vertical position
            RandomPositionOffsetComponent randomOffset = new RandomPositionOffsetComponent();
            randomOffset.Minimum = new Vector2(0.0f, -0.4f * GraphicsDevice.Viewport.Height);
            randomOffset.Maximum = new Vector2(0.0f, 0.4f * GraphicsDevice.Viewport.Height);
            mineTemplate.AddComponent(randomOffset);

            //Component: Moves linearly
            LinearMovementComponent movement = new LinearMovementComponent();
            mineTemplate.AddComponent(movement);

            //Component: Moves at constant speed
            MoveSpeedComponent speed = new MoveSpeedComponent();
            speed.MoveSpeed = -6;
            mineTemplate.AddComponent(speed);

            //Component: Has a bounding box
            AABBComponent aabb = new AABBComponent();
            aabb.Height = tex.SourceRect.Height;
            aabb.Width = tex.SourceRect.Width / anim.NumFrames;
            mineTemplate.AddComponent(aabb);

            //Component: Is rendered at a specific layer - just above the player
            RenderLayerComponent layer = new RenderLayerComponent();
            layer.LayerID = 11;
            mineTemplate.AddComponent(layer);

            //Component: Is destroyed when it ventures off the (left side of the) screen
            DestroyedWhenOffScreenComponent destroyer = new DestroyedWhenOffScreenComponent();
            mineTemplate.AddComponent(destroyer);

            //Component: Has a score value
            // TODO this is more complex than the other ones, involves the changing of global state!

            return mineTemplate;
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

            /*

            // Update the component timers
            componentTimer.Process(entityStorage, gameTime);

            // Spawn entities
            entitySpawner.Process(entityStorage, gameTime);

            // Handle player input
            playerMover.Process(entityStorage, gameTime);

            // Move linearly-moving entities
            linearMover.Process(entityStorage, gameTime);

            // Position randomly-located entities
            randomPositioner.Process(entityStorage, gameTime);

            // Move entities
            entityMover.Process(entityStorage, gameTime);

            // Clamp entities to the screen
            clamper.Process(entityStorage, gameTime);

            // Wrap entitites onto the screen
            screenWrapper.Process(entityStorage, gameTime);

            // Update the animations
            animationUpdater.Process(entityStorage, gameTime);

            // update the positions of the slaves
            slaver.Process(entityStorage, gameTime);*/

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

            /*
            //Resolve the animation states to texture sources
            animationResolver.Process(entityStorage, gameTime);
            
            // Draw the entities
            renderer.Process(entityStorage, gameTime);
            */

            //Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
