using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachineAnimator : MonoBehaviour
{
    

    public void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void SetAnim(string anim)
    {
        _animator.Play(anim);
    }
}
