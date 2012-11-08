using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.SceneInitialization.ShooterSceneInit
{
    public class MusicInit : ISceneInitializer
    {
        public void InitializeScene(IEntityManager entityStore, Game game)
        {
            Entity musicEntity = new Entity();

            MusicComponent music = new MusicComponent();
            music.music = game.Content.Load<Song>("sound/gameMusic");
            music.repeat = true;
            musicEntity.AddComponent(music);

            entityStore.Add(musicEntity);
        }
    }
}
