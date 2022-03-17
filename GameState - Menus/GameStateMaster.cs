﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CS5410
{
    public class GameStateMaster : Game
    {
        private GraphicsDeviceManager m_graphics;
        private IGameState m_currentState;
        private GameStateEnum m_nextStateEnum = GameStateEnum.MainMenu;
        private Dictionary<GameStateEnum, IGameState> m_states;

        public GameStateMaster()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Dear Grader, 
            // If you want to test my game's ability to change screen sizes, here ↓,
            // please, also change the game screen width
            // and the dimensions/ quanity of cells you expect to see
            // found in GamePlayView.cs
            // Thanks, Trent. 
            m_graphics.PreferredBackBufferWidth = 1366;
            m_graphics.PreferredBackBufferHeight = 768;

            m_graphics.ApplyChanges();

            // Create all the game states here
            m_states = new Dictionary<GameStateEnum, IGameState>();
            m_states.Add(GameStateEnum.MainMenu, new MainMenuView());
            m_states.Add(GameStateEnum.GamePlay, new GamePlayView());
            m_states.Add(GameStateEnum.HighScores, new HighScoresView());
            m_states.Add(GameStateEnum.Settings, new SettingsView());
            m_states.Add(GameStateEnum.About, new AboutView());

            // We are starting with the main menu
            m_currentState = m_states[GameStateEnum.MainMenu];

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Give all game states a chance to load their content
            foreach (var item in m_states)
            {
                item.Value.initialize(this.GraphicsDevice, m_graphics);
                item.Value.loadContent(this.Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            m_nextStateEnum = m_currentState.processInput(gameTime);
            // Special case for exiting the game
            if (m_nextStateEnum == GameStateEnum.Exit)
            {
                Exit();
            }

            m_currentState.update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            m_currentState.render(gameTime);

            m_currentState = m_states[m_nextStateEnum];

            base.Draw(gameTime);
        }
    }
}
