using System;
using UnityEngine;
using UralHedgehog;

public class Brick : MonoBehaviour
{
    [SerializeField] private AudioComponent _audio;
    
    private Action _callback;
    
    public void Init(Action callback)
    {
        _callback = callback;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Ball")) return;
        
        _audio.Play(Sound.UI_BUTTON_CLICK_0); //TODO: fx_0
        _callback?.Invoke();
        Destroy(gameObject);
    }
}
