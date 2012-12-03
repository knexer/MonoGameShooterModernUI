using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Shooter.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shooter.EntityManagers;
using Microsoft.Xna.Framework.Audio;
using Shooter.EntityInitialization;

namespace Shooter.SceneInitialization.ShooterSceneInit
{
    public class PlayerInit : ISceneInitializer
    {
        public void InitializeScene(IEntityManager entityStore, Game game)
        {
            initializePlayer(entityStore, game);
        }

        //Create the player entity and its associated entities
        private void initializePlayer(IEntityManager entityStore, Game game)
        {
            Entity player = new Entity();

            //give the player health
            HealthComponent hp = new HealthComponent();
            hp.Health = 100;
            player.AddComponent(hp);

            //Give the player a position
            PositionComponent pos = new PositionComponent();
            Vector2 playerPosition = new Vector2(game.GraphicsDevice.Viewport.TitleSafeArea.X, game.GraphicsDevice.Viewport.TitleSafeArea.Y + game.GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            pos.Position = playerPosition;
            player.AddComponent(pos);

            //And a texture
            TextureComponent tex = new TextureComponent();
            tex.Texture = game.Content.Load<Texture2D>("spaceArt/png/player");
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
            aabb.Width = tex.Texture.Width;
            aabb.Height = tex.Texture.Height;
            player.AddComponent(aabb);

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

            //And the ability to damage entities on contact with them
            DamageOnContactComponent damager = new DamageOnContactComponent();
            damager.Damage = 10;
            player.AddComponent(damager);

            entityStore.Add(player);

            //also create the player's gun
            initializePlayerGuns(entityStore, game, player);
        }

        //create the player's guns
        private void initializePlayerGuns(IEntityManager entityStore, Game game, Entity player)
        {
            //Generate the shared bullet template
            Entity bullet = createBulletTemplate(game);

            createPlayerGun(entityStore, player, bullet, 0.333f, 0.0f);
            createPlayerGun(entityStore, player, bullet, 0.667f, 0.5f);
        }

        //Note: timerPhaseAngle is expected to be in the interval [0, 1]
        private void createPlayerGun(IEntityManager entityStore, Entity player, Entity bullet, float offsetProportion, float timerPhaseAngle)
        {
            //compute the gun's offset from the player, then create the gun
            AABBComponent bulletBox = (AABBComponent)bullet.components[typeof(AABBComponent)];
            AABBComponent playerBox = (AABBComponent)player.components[typeof(AABBComponent)];
            Entity gun = createPositionSlavedEntity(player, new Vector2(playerBox.Width + 0.1f, playerBox.Height * offsetProportion - bulletBox.Height / 2.0f));

            //The gun now has a position coupled to that of the player
            //So spawn bullets at the gun!
            SpawnEntityComponent spawner = new SpawnEntityComponent();
            spawner.Factory = new ComposedEntityFactory(new List<IEntityFactory>() { 
                new CloneEntityFactory(bullet), 
                new InheritParentComponentEntityFactory(typeof(PositionComponent)) });

            //Bullets should be spawned periodically
            PeriodicAddComponentComponent timer = new PeriodicAddComponentComponent();
            timer.Period = 200.0f;
            timer.TimeSinceLastFiring = timer.Period * timerPhaseAngle;
            timer.ComponentToAdd = spawner;
            gun.AddComponent(timer);

            //The gun should be removed from the world when the player dies
            DestroyedOnParentDestroyedComponent existentialDependency = new DestroyedOnParentDestroyedComponent();
            existentialDependency.parent = player;
            gun.AddComponent(existentialDependency);

            //finally, add the gun to the world
            entityStore.Add(gun);
        }

        private Entity createBulletTemplate(Game game)
        {
            Entity bullet = new Entity();

            //Component: Damage entities.
            DamageOnContactComponent damage = new DamageOnContactComponent();
            damage.Damage = 1;
            bullet.AddComponent(damage);

            //Component: Has health. Used to simulate destruction on contact with an entity
            HealthComponent health = new HealthComponent();
            health.Health = 1;
            bullet.AddComponent(health);

            //Component: Has a texture
            TextureComponent tex = new TextureComponent();
            tex.Texture = game.Content.Load<Texture2D>("spaceArt/png/laserGreen");
            tex.SourceRect = tex.Texture.Bounds;
            bullet.AddComponent(tex);

            //Component: Moves linearly
            LinearMovementComponent movement = new LinearMovementComponent();
            bullet.AddComponent(movement);

            //Component: Moves at constant speed
            MoveSpeedComponent speed = new MoveSpeedComponent();
            speed.MoveSpeed = 18;
            bullet.AddComponent(speed);

            //Component: Has a bounding box
            AABBComponent aabb = new AABBComponent();
            aabb.Height = tex.SourceRect.Height;
            aabb.Width = tex.SourceRect.Width;
            bullet.AddComponent(aabb);

            //Component: Is rendered at a specific layer - just above the enemies
            RenderLayerComponent layer = new RenderLayerComponent();
            layer.LayerID = 12;
            bullet.AddComponent(layer);

            //Component: Is destroyed when it ventures off the (right side of the) screen
            DestroyedWhenOffScreenComponent destroyer = new DestroyedWhenOffScreenComponent();
            bullet.AddComponent(destroyer);

            //Component: Is destroyed when it runs out of health
            DestroyedWhenNoHealthComponent destroyer2 = new DestroyedWhenNoHealthComponent();
            bullet.AddComponent(destroyer2);

            //Component: Plays a laser sound
            SoundEffectComponent soundEffect = new SoundEffectComponent();
            soundEffect.effect = game.Content.Load<SoundEffect>("sound/39459__the-bizniss__laser");
            bullet.AddComponent(soundEffect);

            return bullet;
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
    }
}
