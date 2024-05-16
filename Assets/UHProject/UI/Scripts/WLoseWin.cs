using System.Collections;
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
    
    [SerializeField] private AudioComponent _audio;
    [SerializeField] private Animator _animator;

    public override void Init(params object[] param)
    {
        var level = (ILevel)param[0];
        
        _audio.Init();
        
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
    
    public override void Show()
    {
        _animator.Play("Show");
        base.Show();
    }
    
    public override void Hide()
    {
        _audio.Play(Sound.UI_BUTTON_CLICK_0);
        StartCoroutine(AnimHide());
    }

    private IEnumerator AnimHide()
    {
        _animator.Play("Hide");
        var totalTime = _animator.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(totalTime);
        base.Hide();
    }
}
