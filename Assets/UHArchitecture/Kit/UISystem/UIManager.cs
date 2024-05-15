namespace UralHedgehog
{
    namespace UI
    {
        public class UIManager
        {
            private Data _wSettingsData;
            private Data _wMainMenuData;
            private Data _wLoseWinData;
            private Data _pTopData;

            private readonly ISettings _settings;
            private readonly IPlayerUI _player;
            
            public UIManager(ISettings settings, IPlayerUI player)
            {
                _settings = settings;
                _player = player;
            }

            #region Open

            /// <summary>
            /// Поднимает виджет настроек
            /// </summary>
            public void OpenViewSettings() // Вариант 1 (если данные переданы при инициализации и больше неизменяются)
            {
                _wSettingsData = new Data(nameof(WSettings), _settings);
                UIDispatcher.Send(EventUI.SHOW_WIDGET, _wSettingsData);
            }
            
            public void OpenViewSettings(ISettings settings) // Вариант 2
            {
                _wSettingsData = new Data(nameof(WSettings), settings);
                UIDispatcher.Send(EventUI.SHOW_WIDGET, _wSettingsData);
            }
            
            public void OpenViewMainMenu(ILevel level) 
            {
                _wMainMenuData = new Data(nameof(WMainMenu), level);
                UIDispatcher.Send(EventUI.SHOW_WIDGET, _wMainMenuData);
            }
            
            public void OpenViewLoseWin(ILevel level) 
            {
                _wLoseWinData = new Data(nameof(WLoseWin), level);
                UIDispatcher.Send(EventUI.SHOW_WIDGET, _wLoseWinData);
            }
            
            public void ShowViewTop(IPlayerUI player) 
            {
                _pTopData = new Data(nameof(PTop), player);
                UIDispatcher.Send(EventUI.SHOW_WIDGET, _pTopData);
            }

            #endregion

            //TODO: Методы Close могут понадобиться для принудительного закрытия виджета, либо если виджет не имеет кнопки закрыть.
            //TODO: Его можно закрыть и уничтожить из любого места.
            #region Close

            /// <summary>
            /// Уничтожает виджет настроек
            /// </summary>
            public void CloseViewSettings()
            {
                UIDispatcher.Send(EventUI.KILL, _wSettingsData);
            }
            
            public void HideViewTop()
            {
                UIDispatcher.Send(EventUI.HIDE_WIDGET, _pTopData);
            }

            #endregion
        }
    }
}