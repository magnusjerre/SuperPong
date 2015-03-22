using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.MJFrameWork
{
    public class MJSceneManager : MJUpdate, MJDraw
    {
        private static MJSceneManager singleton;

        ContentManager Content;
        Stack<MJScene> scenes;
        MJScene sceneToPush = null;
        Boolean popScene = false;        
        
        static string DEFAULT_SCENE = "DEFAUL_SCENE";

        public static void Init(ContentManager content) {
            if (singleton == null)
                singleton = new MJSceneManager(content);
        }

        public static MJSceneManager GetInstance()
        {
            return singleton;
        }

        private MJSceneManager(ContentManager content)
        {
            Content = content;
            scenes = new Stack<MJScene>();
            scenes.Push(new MJScene(DEFAULT_SCENE));
        }

        public void PushScene(MJScene scene)
        {
            sceneToPush = scene;
        }

        public void PopScene()
        {
            popScene = true;
        }


        public void Update(GameTime gameTime)
        {
            scenes.Peek().Update(gameTime);
            if (sceneToPush != null)
            {
                sceneToPush.Content = new ContentManager(Content.ServiceProvider, Content.RootDirectory);
                sceneToPush.Initialize();
                scenes.Push(sceneToPush);
                sceneToPush = null;
            }

            if (popScene)
            {
                popScene = false;
                if (scenes.Count > 1)
                {
                    scenes.Peek().UnloadContent();
                    scenes.Pop();
                    scenes.Peek().Initialize();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            scenes.Peek().Draw(spriteBatch);
        }
    }
}
