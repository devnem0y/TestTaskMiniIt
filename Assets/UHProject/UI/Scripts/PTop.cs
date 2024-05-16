using TMPro;
using UnityEngine;
using UralHedgehog.UI;

public class PTop : Widget
{
    [SerializeField] private TMP_Text _lblBalls;
    [SerializeField] private TMP_Text _lblScore;
    
    private IPlayerUI _player;
    
    public override void Init(params object[] param)
    {
        _player = (IPlayerUI)param[0];

        _player.ChangeScore += OnChangeScore;
        _player.ChangeBalls += OnChangeBalls;
        
        _lblScore.text = $"Score: {_player.Score}";
        _lblBalls.text = $"Ball: {_player.Balls}";
    }

    private void OnChangeScore(int value)
    {
        _lblScore.text = $"Score: {value}";
    }
    
    private void OnChangeBalls(int value)
    {
        _lblBalls.text = $"Ball: {value}";
    }

    private void OnDestroy()
    {
        _player.ChangeScore -= OnChangeScore;
        _player.ChangeBalls -= OnChangeBalls;
    }
}
