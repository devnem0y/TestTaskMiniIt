using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UralHedgehog;
using UralHedgehog.UI;

public class WLoseWin : Widget
{
    [SerializeField] private TMP_Text _lblTitle;
    [SerializeField] private Button _btnRestart;
    [SerializeField] private Button _btnBackMenu;

    public override void Init(params object[] param)
    {
        var level = (ILevel)param[0];
        
        _lblTitle.text = Game.Instance.GameState == GameState.VICTORY ? "Victory" : "Lose";

        _btnBackMenu.onClick.AddListener(() =>
        {
            Game.Instance.ChangeState(GameState.MAIN);
            level.Clear();
            Hide();
        });
        
        _btnRestart.onClick.AddListener(() =>
        {
            Game.Instance.ChangeState(GameState.PLAY);
            level.Reload();
            Hide();
        });
    }
}
