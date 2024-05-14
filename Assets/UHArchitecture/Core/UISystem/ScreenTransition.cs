using System;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] private TransitionEffect _transitionEffect;

    public void Perform(Action callback, TransitionMode transitionMode = TransitionMode.HIDE_SHOW)
    {
        var effect = Instantiate(_transitionEffect, transform);
        effect.Init(callback, transitionMode);
    }

    public void Show()
    {
        transform.GetChild(0).GetComponent<TransitionEffect>().OnShow();
    }
    
    public void ShowLabelNext()
    {
        transform.GetChild(0).GetComponent<TransitionEffect>().ShowLabelNext();
    }
}

public enum TransitionMode
{
    NONE = 0, 
    SHOW = 1, // Появление
    HIDE = 2, // Затухание
    HIDE_SHOW = 3, // Затухание - появление
    STATIC = 4, // Темный экран на старте
}
