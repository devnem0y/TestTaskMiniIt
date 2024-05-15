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
        
        _sound.value = _settings.VolumeSound;
        _sound.onValueChanged.AddListener(delegate { _settings.ChangeVolumeSound(_sound.value); });
        
        SetComplexity();
    }
    
    private void OnComplexityLeft()
    {
        Game.Instance.Complexity--;
        SetComplexity();
    }
    
    private void OnComplexityRight()
    {
        Game.Instance.Complexity++;
        SetComplexity();
    }

    private void SetComplexity()
    {
        _lblComplexity.text = _complexityLabel[Game.Instance.Complexity];
        _btnComplexityLeft.interactable = Game.Instance.Complexity != 0;
        _btnComplexityRight.interactable = Game.Instance.Complexity != _complexityLabel.Length - 1;
    }
}