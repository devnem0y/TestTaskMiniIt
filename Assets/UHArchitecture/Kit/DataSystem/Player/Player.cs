using System;
using UralHedgehog;

public class Player : PlayerBase, IPlayer
{
    public int Score => GetCounter<Score>().Value;
    public int Balls => GetCounter<BallCount>().Value;
    
    public event Action<int> ChangeScore;
    public event Action<int> ChangeBalls;

    private BallCount _ballCount;
    
    public Player(PlayerData data)
    {
        Data = data;
        
        var score = new Score(0);
        _ballCount = new BallCount(0);
        CountersAdd(score, _ballCount);
    }

    public override void Save()
    {
        //TODO: Сохраняем данные игрока, но в нашем случае их сейчас нет
        //Data = new PlayerData();
    }
    
    public void AddScore()
    {
        GetCounter<Score>().Add(1);
        ChangeScore?.Invoke(Score);
    }

    public void ResetScore()
    {
        GetCounter<Score>().Withdraw(Score);
        ChangeScore?.Invoke(Score);
    }

    public void RemoveBall()
    {
        GetCounter<BallCount>().Withdraw(1);
        ChangeBalls?.Invoke(Balls);
    }

    public void SetBall()
    {
        GetCounter<BallCount>().Withdraw(Balls);
        GetCounter<BallCount>().Add(3);
        ChangeBalls?.Invoke(Balls);
    }
}