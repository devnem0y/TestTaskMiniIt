using System;
using UnityEngine;
using UralHedgehog.UI;

namespace UralHedgehog
{
    public class Game : Bootstrap
    {
        public static Game Instance { get; private set; }

        [SerializeField] private Level _level;
        [SerializeField] private ComplexityData _complexity; //TODO: Пока заглушка

        private void Awake()
        {
            Instance = this;
        }

        protected void Start()
        {
            Run();
        }

        protected override void Initialization()
        {
            base.Initialization();
            UIManager = new UIManager(_settings, _player);
        }

        public override void ChangeState(GameState state)
        {
            base.ChangeState(state);
            
            switch (GameState)
            {
                case GameState.LOADING:
                    Debug.Log("<color=yellow>Loading</color>");
                    ScreenTransition.Perform(null, TransitionMode.STATIC);
                    break;
                case GameState.MAIN:
                    Debug.Log("<color=yellow>Main</color>");
                    Cursor.visible = true;
                    UIManager.OpenViewMainMenu(_level);
                    ScreenTransition.Show();
                    break;
                case GameState.PLAY:
                    Debug.Log("<color=yellow>Play</color>");
                    Cursor.visible = false;
                    _level.Init(_complexity, _player);
                    break;
                case GameState.VICTORY:
                    Debug.Log("<color=yellow>Victory</color>");
                    Cursor.visible = true;
                    break;
                case GameState.DEFEAT:
                    Debug.Log("<color=yellow>Defeat</color>");
                    Cursor.visible = true;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}