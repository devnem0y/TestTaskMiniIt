using System;

public interface ILevel
{
    public event Action Completed;
    public event Action Lose;
    
    void Load();
    void Reload();
    void Clear();
}