using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UralHedgehog;
using UralHedgehog.UI;

public class WSettings : Widget
{
    [SerializeField] private TMP_Text _lblComplexity;
    [SerializeField] private Button _btnComplexityLeft;
    [SerializeField] private Button _btnComplexityRight;
    [SerializeField] private Button _btnClose;
    [SerializeField] private Slider _sound;
    
    [SerializeField] private AudioComponent _audio;
    [SerializeField] private Animator _animator;
    
    private readonly string[] _complexityLabel = {"EASY", "NORMAL", "HARD"};

    private ISettings _settings;

    protected override void Awake()
    {
        base.Awake();
        _btnClose.onClick.AddListener(Hide);
        _btnComplexityLeft.onClick.AddListener(OnComplexityLeft);
        _btnComplexityRight.onClick.AddListener(OnComplexityRight);
    }

    public override void Init(params object[] param)
    {
        _settings = (ISettings) param[0];
        
        _audio.Init();
        
        _sound.value = _settings.VolumeSound;
        _sound.onValueChanged.AddListener(delegate { _settings.ChangeVolumeSound(_sound.value); });
        
        SetComplexity();
    }
    
    private void OnComplexityLeft()
    {
        _audio.Play(Sound.UI_BUTTON_CLICK_1);
        Game.Instance.Complexity--;
        SetComplexity();
    }
    
    private void OnComplexityRight()
    {
        _audio.Play(Sound.UI_BUTTON_CLICK_1);
        Game.Instance.Complexity++;
        SetComplexity();
    }

    private void SetComplexity()
    {
        _lblComplexity.text = _complexityLabel[Game.Instance.Complexity];
        _btnComplexityLeft.interactable = Game.Instance.Complexity != 0;
        _btnComplexityRight.interactable = Game.Instance.Complexity != _complexityLabel.Length - 1;
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