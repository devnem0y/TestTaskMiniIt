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

        private bool _isFirstLaunch;

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
                    _isFirstLaunch = true;
                    break;
                case GameState.MAIN:
                    Debug.Log("<color=yellow>Main</color>");
                    Cursor.visible = true;
                    UIManager.OpenViewMainMenu(_level);
                    if (_isFirstLaunch)
                    {
                        ScreenTransition.Show();
                        _isFirstLaunch = false;
                    }
                    break;
                case GameState.PLAY:
                    Debug.Log("<color=yellow>Play</color>");
                    Cursor.visible = false;
                    _level.Init(_complexity, _player);
                    UIManager.ShowViewTop(_player);
                    break;
                case GameState.VICTORY:
                    Debug.Log("<color=yellow>Victory</color>");
                    Cursor.visible = true;
                    UIManager.HideViewTop();
                    UIManager.OpenViewLoseWin(_level);
                    break;
                case GameState.DEFEAT:
                    Debug.Log("<color=yellow>Defeat</color>");
                    Cursor.visible = true;
                    UIManager.HideViewTop();
                    UIManager.OpenViewLoseWin(_level);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}