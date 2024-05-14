using UnityEngine;
using UnityEngine.UI;
using UralHedgehog;
using UralHedgehog.UI;

public class WMainMenu : Widget
{
    [SerializeField] private Button _btnPlay;
    [SerializeField] private Button _btnSettings;
    
    public override void Init(params object[] param)
    {
        var level = (ILevel) param[0];
        
        _btnPlay.onClick.AddListener(() =>
        {
            Close(() =>
            {
                Game.Instance.ChangeState(GameState.PLAY);
                level.Load();
                Hide();
            });
        });
        
        _btnSettings.onClick.AddListener(Game.Instance.UIManager.OpenViewSettings);
    }
}