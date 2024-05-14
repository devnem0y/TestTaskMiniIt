namespace UralHedgehog
{
    namespace UI
    {
        public class UIManager
        {
            private Data _wSettingsData;

            private readonly ISettings _settings;
            
            public UIManager(ISettings settings)
            {
                _settings = settings;
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

            #endregion
        }
    }
}