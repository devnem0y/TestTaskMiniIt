using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UralHedgehog;
using UralHedgehog.UI;

public class WMainMenu : Widget
{
    [SerializeField] private Button _btnPlay;
    [SerializeField] private Button _btnSettings;
    [SerializeField] private Button _btnExit;

    [SerializeField] private AudioComponent _audio;
    [SerializeField] private Animator _animator;

    private ILevel _level;
    
    public override void Init(params object[] param)
    {
        _level = (ILevel) param[0];
        
        _audio.Init();
        
        _btnPlay.onClick.AddListener(Hide);
        
        _btnSettings.onClick.AddListener(() =>
        {
            _audio.Play(Sound.UI_BUTTON_CLICK_0);
            Game.Instance.UIManager.OpenViewSettings();
        });
        
        _btnExit.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
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
        Game.Instance.ChangeState(GameState.PLAY);
        _level.Load();
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