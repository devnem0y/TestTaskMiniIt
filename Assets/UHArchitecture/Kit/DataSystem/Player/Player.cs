using System;
using UralHedgehog;

public class Player : PlayerBase, IPlayer
{
    public int Score => GetCounter<Score>().Value;
    public event Action<int> ChangeScore;

    public Player(PlayerData data)
    {
        Data = data;
        
        var score = new Score(0);
        CountersAdd(score);
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
}