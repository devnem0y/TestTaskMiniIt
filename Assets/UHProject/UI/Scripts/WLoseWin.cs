using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UralHedgehog;
using UralHedgehog.UI;
using YG;

public class WLoseWin : Widget
{
    [SerializeField] private TMP_Text _lblTitle;
    [SerializeField] private Button _btnRestart;
    [SerializeField] private Button _btnPlay;
    [SerializeField] private Button _btnBackMenu;
    
    [SerializeField] private AudioComponent _audio;
    [SerializeField] private Animator _animator;

    private ILevel _level;

    protected override void Awake()
    {
        base.Awake();
        YandexGame.CloseFullAdEvent += CloseFullAd;
    }

    private void OnDestroy()
    {
        YandexGame.CloseFullAdEvent -= CloseFullAd;
    }
    
    public override void Init(params object[] param)
    {
        _level = (ILevel)param[0];
        
        _audio.Init();
        
        _lblTitle.text = Game.Instance.GameState == GameState.VICTORY ? "Victory" : "Lose";

        _btnBackMenu.onClick.AddListener(() =>
        {
            Game.Instance.ChangeState(GameState.MAIN);
            _level.Clear();
            Hide();
        });
        
        _btnPlay.onClick.AddListener(OnRestart);
        
        _btnRestart.onClick.AddListener(() =>
        {
            //TODO: Show Ad
            
            if (YandexGame.allowedFullAd)
            {
                YandexGame.FullscreenShow();
                _btnRestart.gameObject.SetActive(false);
                _btnPlay.gameObject.SetActive(true);
            }
            else OnRestart();
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

    private void OnRestart()
    {
        Game.Instance.ChangeState(GameState.PLAY);
        _level.Reload();
        Hide();
    }
    
    private void CloseFullAd()
    {
        _btnPlay.gameObject.SetActive(true);
        _btnRestart.gameObject.SetActive(false);
    }
}
