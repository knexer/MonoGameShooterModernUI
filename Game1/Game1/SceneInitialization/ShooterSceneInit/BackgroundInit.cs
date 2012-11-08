using Microsoft.Xna.Framework;
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
    public class BackgroundInit : ISceneInitializer
    {
        Random rand = new Random();

        public void InitializeScene(IEntityManager entityStore, Game game)
        {
            initializeBackground(entityStore, game);
        }

        private void initializeBackground(IEntityManager entityStore, Game game)
        {
            //Add purpleness to the background
            initializePurpleBackground(entityStore, game);

            //Add stars at varying 'depths'
            initializeBackgroundStars(entityStore, game);
        }

        private void initializePurpleBackground(IEntityManager entityStore, Game game)
        {
            Entity bg = new Entity();

            //Component: Has a position
            PositionComponent pos = new PositionComponent();
            pos.Position = new Vector2(0, 0);
            bg.AddComponent(pos);

            //Component: Has an AABB
            AABBComponent aabb = new AABBComponent();
            aabb.Width = game.GraphicsDevice.Viewport.Width;
            aabb.Height = game.GraphicsDevice.Viewport.Height;
            bg.AddComponent(aabb);

            //Component: Has a texture
            TextureComponent tex = new TextureComponent();
            tex.Texture = game.Content.Load<Texture2D>("spaceArt/png/Background/backgroundColor");
            tex.SourceRect = tex.Texture.Bounds;
            bg.AddComponent(tex);

            //Component: Is rendered at a specific layer (the backmost one!)
            RenderLayerComponent layer = new RenderLayerComponent();
            layer.LayerID = 0;
            bg.AddComponent(layer);

            entityStore.Add(bg);
        }

        private void initializeBackgroundStars(IEntityManager entityStore, Game game)
        {
            Texture2D bigStarTex = game.Content.Load<Texture2D>("spaceArt/png/Background/starBig");
            Texture2D littleStarTex = game.Content.Load<Texture2D>("spaceArt/png/Background/starSmall");

            //shoot for 1 star per 100000 square pixels
            double targetDensity = 0.00001;
            int numStars = 0;
            double area = game.GraphicsDevice.Viewport.Width * game.GraphicsDevice.Viewport.Height;

            while (numStars / area < targetDensity)
            {
                generateStar(entityStore, game, bigStarTex, littleStarTex);
                numStars++;
            }
        }

        private void generateStar(IEntityManager entityStore, Game game, Texture2D bigStarTex, Texture2D littleStarTex)
        {
            //determine the depth/speed of the star
            double speed = rand.NextDouble() * -3 - 1;

            Entity star = new Entity();

            //Component: Has a (random) position
            PositionComponent pos = new PositionComponent();
            pos.Position = new Vector2(rand.Next(0, game.GraphicsDevice.Viewport.Width),
                rand.Next(0, game.GraphicsDevice.Viewport.Height));
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

            entityStore.Add(star);
        }
    }
}
