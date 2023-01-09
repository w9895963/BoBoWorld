using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorEvent : MonoBehaviour
{
    public UnityEvent OnAnimationEnd = new UnityEvent();
    public void 动画结束点()
    {
        OnAnimationEnd?.Invoke();
    }
}
