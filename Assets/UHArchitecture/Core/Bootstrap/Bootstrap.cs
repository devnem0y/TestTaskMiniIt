﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UralHedgehog.UI;

namespace UralHedgehog
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Loader _loader;
        [SerializeField] private Saver _saver;
        
        [SerializeField] private LocalizationConfig _localizationConfig;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioResources _audioResources;
        
        [SerializeField] private ScreenTransition _screenTransition;
        
        public LocalizationManager LocalizationManager { get; private set; }
        public AudioManager AudioManager { get; private set; }
        public UIManager UIManager { get; protected set; }
        public GameState GameState { get; private set; }
        
        public bool Pause { get; private set; }
        public bool SoundOn { get; set; }

        public ScreenTransition ScreenTransition => _screenTransition;

        public event Action Loading;
        public event Action Launch;
        public event Action Begin;

        private bool _init;
        
        protected Settings _settings;
        protected Player _player;

        protected int _soundIndex;

        protected void Run()
        {
            ChangeState(GameState.LOADING);
            StartCoroutine(AlternateСall());
        }
        
        private IEnumerator AlternateСall()
        {
            _loader.Load();
            yield return new WaitUntil(() => _loader.IsLoaded);
            Loading?.Invoke();
            yield return new WaitForSeconds(0.01f);
            Initialization();
            yield return new WaitUntil(() => _init);
            Launch?.Invoke();
            yield return new WaitForSeconds(0.01f);
            Begin?.Invoke();
            ChangeState(GameState.MAIN);
        }

        /// <summary>
        /// Base не удалять! Все что нужно писать после.
        /// </summary>
        protected virtual void Initialization()
        {
            _settings = new Settings(_loader.SettingsInfo.SettingsData, _audioMixer);
            LocalizationManager = new LocalizationManager(_localizationConfig) { Language = _settings.Language };
            _settings.OnChangeLanguage += OnLocalize;
            AudioManager = new AudioManager(_audioMixer, _audioResources);
            _player = new Player(_loader.UserInfo.PlayerData);
            
            _init = true;
        }

        public void SaveSettings()
        {
            _saver.Write(_settings);
        }
        
        public void SaveUser(bool isCloud = false)
        {
            _saver.Write(_player, isCloud);
        }

        public virtual void ChangeState(GameState state)
        {
            GameState = state;
        }
        
        public void SetPause(bool pause)
        {
            Pause = pause;
            Time.timeScale = Pause ? 0 : 1;
        }

        /// <summary>
        /// Base не удалять! Все что нужно писать после.
        /// </summary>
        protected virtual void OnDestroy()
        {
            _settings.OnChangeLanguage -= OnLocalize;
        }

        private void OnLocalize()
        {
            LocalizationManager.OnLocalize();
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                if (Pause)
                {
                    if (SoundOn) _settings.ChangeVolumeMaster(1f);
                    if (GameState == GameState.PLAY) SetPause(false);
                }
                else
                {
                    if (SoundOn) _settings.ChangeVolumeMaster(1f);
                    SetPause(false);
                }
            }
            else
            {
                if (SoundOn) _settings.ChangeVolumeMaster(0f);
                if (GameState == GameState.PLAY) SetPause(true);
            }
        }
        
        private void OnApplicationQuit()
        {
            SaveSettings();
            SaveUser(true);
        }
    }
}