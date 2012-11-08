using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.SceneInitialization.ShooterSceneInit
{
    public class MinefieldInit : ISceneInitializer
    {
        public void InitializeScene(IEntityManager entityStore, Game game)
        {
            initializeMinefield(entityStore, game);
        }

        private void initializeMinefield(IEntityManager entityStore, Game game)
        {
            //Create a mine-spawning entity that spawns mines in random positions every once in a while
            Entity minespawner = new Entity();

            //Component: Spawn an entity
            SpawnEntityComponent spawnComponent = new SpawnEntityComponent();
            spawnComponent.EntityToSpawn = createMineTemplateEntity(game);

            //Component: Periodically add a spawn entity component
            PeriodicAddComponentComponent timer = new PeriodicAddComponentComponent();
            timer.ComponentToAdd = spawnComponent;
            timer.Period = 500;
            timer.TimeSinceLastFiring = 0.0f;
            minespawner.AddComponent(timer);

            //Add the minefield to the entity store
            entityStore.Add(minespawner);
        }

        private Entity createMineTemplateEntity(Game game)
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
            tex.Texture = game.Content.Load<Texture2D>("spaceArt/png/meteorBig");
            tex.SourceRect = tex.Texture.Bounds;
            mineTemplate.AddComponent(tex);

            //Component: Has a position.
            PositionComponent pos = new PositionComponent();
            pos.Position.X = game.GraphicsDevice.Viewport.Width - 1;
            pos.Position.Y = game.GraphicsDevice.Viewport.Height / 2 - tex.SourceRect.Height / 2;
            mineTemplate.AddComponent(pos);

            //Component: Given a random starting vertical position
            RandomPositionOffsetComponent randomOffset = new RandomPositionOffsetComponent();
            randomOffset.Minimum = new Vector2(0.0f, -0.4f * game.GraphicsDevice.Viewport.Height);
            randomOffset.Maximum = new Vector2(0.0f, 0.4f * game.GraphicsDevice.Viewport.Height);
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
            explosionSpawner.toSpawn = createExplosionEntity(game, aabb);
            explosionSpawnTriggerer.ToAdd = explosionSpawner;
            mineTemplate.AddComponent(explosionSpawnTriggerer);

            return mineTemplate;
        }

        private Entity createExplosionEntity(Game game, AABBComponent parentAABB)
        {
            Entity expl = new Entity();

            //Component: Has a texture
            TextureComponent tex = new TextureComponent();
            tex.Texture = game.Content.Load<Texture2D>("explosion");
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
            soundEffect.effect = game.Content.Load<SoundEffect>("freesoundsorg/87529__robinhood76__01448-distant-big-explosion-2");
            expl.AddComponent(soundEffect);

            return expl;
        }
    }
}
