using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperPong.MJFrameWork.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperPong.MJFrameWork
{
    public class MJSceneManager : MJTransitionListener
    {
        private MJScene mainScene;
        public MJScene MainScene { get { return mainScene; } }
        
        private MJTransition transition;

        private List<MJScene> scenes;

        public MJSceneManager(MJScene firstScene)
        {
            scenes = new List<MJScene>();
            mainScene = firstScene;
            scenes.Add(mainScene);
        }

        public void AddScene(MJScene scene)
        {
            scenes.Add(scene);
        }

        public void RemoveScene(MJScene scene)
        {
            scenes.Remove(scene);
        }

        public MJScene GetSceneNamed(string sceneName)
        {
            foreach (MJScene scene in scenes)
            {
                if (scene.Name.Equals(sceneName))
                {
                    return scene;
                }
            }

            return null;
        }

        public void NotifyTransitionEnded(MJTransition transition)
        {
            mainScene.UnloadContent();
            mainScene = transition.NewScene;
            transition = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mainScene.Draw(spriteBatch);

            if (transition != null)
            {
                transition.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            mainScene.Update(gameTime);

            if (transition != null)
            {
                transition.Update(gameTime);
            }

        }

        public void TransitionTo(MJScene scene)
        {
            transition = new MJTransition(this, mainScene, scene, MJTransisitionType.None);
            transition.StartTransition();
        }

        public void LoadContent()
        {
            mainScene.LoadContent();
        }

        public void Initialize()
        {
            mainScene.Initialize();
        }

        public void UnloadContent()
        {
            mainScene.UnloadContent();
        }
    }
}
