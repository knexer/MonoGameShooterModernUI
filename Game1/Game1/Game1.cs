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
            initializeBackground();
            initializeMinefield();
            initializeMusic();
        }

        private void initializeBackground()
        {
            //Add purpleness to the background
            initializePurpleBackground();

            //Add stars at varying 'depths'
            initializeBackgroundStars();
        }

        private void initializePurpleBackground()
        {
            Entity bg = new Entity();

            //Component: Has a position
            PositionComponent pos = new PositionComponent();
            pos.Position = new Vector2(0, 0);
            bg.AddComponent(pos);

            //Component: Has an AABB
            AABBComponent aabb = new AABBComponent();
            aabb.Width = GraphicsDevice.Viewport.Width;
            aabb.Height = GraphicsDevice.Viewport.Height;
            bg.AddComponent(aabb);

            //Component: Has a texture
            TextureComponent tex = new TextureComponent();
            tex.Texture = Content.Load<Texture2D>("spaceArt/png/Background/backgroundColor");
            tex.SourceRect = tex.Texture.Bounds;
            bg.AddComponent(tex);

            //Component: Is rendered at a specific layer (the backmost one!)
            RenderLayerComponent layer = new RenderLayerComponent();
            layer.LayerID = 0;
            bg.AddComponent(layer);

            entityStorage.Add(bg);
        }

        private void initializeBackgroundStars()
        {
            Texture2D bigStarTex = Content.Load<Texture2D>("spaceArt/png/Background/starBig");
            Texture2D littleStarTex = Content.Load<Texture2D>("spaceArt/png/Background/starSmall");

            //shoot for 1 star per 100000 square pixels
            double targetDensity = 0.00001;
            int numStars = 0;
            double area = GraphicsDevice.Viewport.Width * GraphicsDevice.Viewport.Height;

            while (numStars / area < targetDensity)
            {
                generateStar(bigStarTex, littleStarTex);
                numStars++;
            }
        }

        private void generateStar(Texture2D bigStarTex, Texture2D littleStarTex)
        {
            //determine the depth/speed of the star
            double speed = rand.NextDouble() * -3 - 1;

            Entity star = new Entity();

            //Component: Has a (random) position
            PositionComponent pos = new PositionComponent();
            pos.Position = new Vector2(rand.Next(0, GraphicsDevice.Viewport.Width),
                rand.Next(0, GraphicsDevice.Viewport.Height));
            star.AddComponent(pos);

            //Component: Moves at a constant speed
            LinearMovementComponent movementStrat = new LinearMovementComponent();
            star.AddComponent(movementStrat);

            //Component: Has a move speed
            MoveSpeedComponent speedComponent = new MoveSpeedComponent();
            speedComponent.MoveSpeed = (float)speed;
            star.AddComponent(speedComponent);

            //Component: Wraps around the screen
            ScreenWrappingComponent wrapper = new ScreenWrappingComponent();
            star.AddComponent(wrapper);

            //Component: Has a texture.  This should be little star for far away/slow stars, big otherwise
            TextureComponent tex = new TextureComponent();
            tex.Texture = (speed > -2.5) ? littleStarTex : bigStarTex;
            tex.SourceRect = tex.Texture.Bounds;
            star.AddComponent(tex);

            //Component: Has a bounding box
            AABBComponent aabb = new AABBComponent();
            aabb.Width = tex.Texture.Width;
            aabb.Height = tex.Texture.Height;
            star.AddComponent(aabb);

            //Component: Is rendered at a specific layer (just above the background)
            RenderLayerComponent layer = new RenderLayerComponent();
            layer.LayerID = 1;
            star.AddComponent(layer);

            entityStorage.Add(star);
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
            timer.Period = 500;
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
            tex.Texture = Content.Load<Texture2D>("spaceArt/png/meteorBig");
            tex.SourceRect = tex.Texture.Bounds;
            mineTemplate.AddComponent(tex);

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
            speed.MoveSpeed = -10;
            mineTemplate.AddComponent(speed);

            //Component: Has a bounding box
            AABBComponent aabb = new AABBComponent();
            aabb.Height = tex.SourceRect.Height;
            aabb.Width = tex.SourceRect.Width;
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

            //Component: Is destroyed when it runs out of health
            DestroyedWhenNoHealthComponent destroyer2 = new DestroyedWhenNoHealthComponent();
            mineTemplate.AddComponent(destroyer2);

            //Component: Leaves behind an explosion entity when it is destroyed
            AddComponentOnDestructionComponent explosionSpawnTriggerer = new AddComponentOnDestructionComponent();
            SpawnEntityAtPositionComponent explosionSpawner = new SpawnEntityAtPositionComponent();
            explosionSpawner.toSpawn = createExplosionEntity(aabb);
            explosionSpawnTriggerer.ToAdd = explosionSpawner;
            mineTemplate.AddComponent(explosionSpawnTriggerer);

            return mineTemplate;
        }

        private Entity createExplosionEntity(AABBComponent parentAABB)
        {
            Entity expl = new Entity();

            //Component: Has a texture
            TextureComponent tex = new TextureComponent();
            tex.Texture = Content.Load<Texture2D>("explosion");
            tex.SourceRect = tex.Texture.Bounds;
            expl.AddComponent(tex);

            //Component: Has an animation
            AnimationComponent anim = new AnimationComponent();
            anim.CurrentFrameIndex = 0;
            anim.FrameDuration = 45;
            anim.Looping = false;
            anim.NumFrames = 12;
            anim.TimeSinceFrameChange = 0;
            expl.AddComponent(anim);

            //Component: Is rendered at a specific layer (just behind the player)
            RenderLayerComponent layer = new RenderLayerComponent();
            layer.LayerID = 9;
            expl.AddComponent(layer);

            //Component: Has a bounding box
            AABBComponent aabb = new AABBComponent();
            aabb.Height = 134;
            aabb.Width = 134;
            expl.AddComponent(aabb);

            //Component: Destroys itself once its animation completes
            DestroyedWhenAnimationCompleteComponent destructor = new DestroyedWhenAnimationCompleteComponent();
            expl.AddComponent(destructor);

            //Component: Is offset such that it is concentric with its parent
            PositionDeltaComponent delta = new PositionDeltaComponent();
            float deltaX = parentAABB.Width / 2.0f - aabb.Width / 2.0f;
            float deltaY = parentAABB.Height / 2.0f - aabb.Height / 2.0f;
            delta.Delta = new Vector2(deltaX, deltaY);
            expl.AddComponent(delta);

            //Component: Plays an explosion sound
            SoundEffectComponent soundEffect = new SoundEffectComponent();
            soundEffect.effect = Content.Load<SoundEffect>("freesoundsorg/87529__robinhood76__01448-distant-big-explosion-2");
            expl.AddComponent(soundEffect);

            return expl;
        }

        private void initializeMusic()
        {
            Entity musicEntity = new Entity();

            MusicComponent music = new MusicComponent();
            music.music = Content.Load<Song>("sound/gameMusic");
            music.repeat = true;
            musicEntity.AddComponent(music);

            entityStorage.Add(musicEntity);
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
