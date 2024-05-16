using UnityEngine;

[CreateAssetMenu(fileName = "ComplexityData", menuName = "Data/Complexity", order = 1)]
public class ComplexityData : ScriptableObject
{
    [SerializeField] private Platform _platform;
    public Platform Platform => _platform;
    [SerializeField] private float _platformWidth;
    public float PlatformWidth => _platformWidth;
    
    [SerializeField] private Ball _ball;
    public Ball Ball => _ball;
    [SerializeField] private float _ballForce;
    public float BallForce => _ballForce;
    
    [SerializeField] private Brick _brick;
    public Brick Brick => _brick;
    [SerializeField] private int _bricksSize;
    public int BricksSize => _bricksSize;
}
