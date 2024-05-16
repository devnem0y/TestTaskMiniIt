using System;

public interface IPlayerUI
{
    public int Score { get; }
    public event Action<int> ChangeScore;
    
    public int Balls { get; }
    public event Action<int> ChangeBalls;
}