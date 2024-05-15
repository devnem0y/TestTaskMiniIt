namespace UralHedgehog
{
    public interface IPlayer : IPlayerUI
    {
        public void AddScore();
        public void ResetScore();
        public void RemoveBall();
        public void SetBall();
    }
}