using System;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public event Action OnFinish;

    [SerializeField] private Animator _animator;

    private void AnimationFinishedTrigger()
    {
        _animator.SetBool("Attacking", false);
    }

}
