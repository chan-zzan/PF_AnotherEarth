using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentAnimType
{
    Idle = 0,
    Walk,
    Eat,
    Sleep,
    Jump
}

public class ParentsBaseAnim : MonoBehaviour
{
    public Animator anim;

    public void SetParentState(CurrentAnimType myType)
    {
        ResetState();

        switch (myType)
        {
            case CurrentAnimType.Idle:
                {
                    anim.SetBool("IsIdle", true);
                    break;
                }
                case CurrentAnimType.Walk:
                {
                    anim.SetBool("IsWalk", true);
                    break;
                }
                case CurrentAnimType.Eat:
                {
                    anim.SetBool("IsEat", true);
                    break;
                }
                case CurrentAnimType.Sleep:
                {
                    anim.SetBool("IsSleep", true);
                    break;
                }
            case CurrentAnimType.Jump:
                {
                    anim.SetBool("IsJump", true);
                    break;
                }

            default: break;
        }
    }

    void ResetState()
    {
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsWalk", false);
        anim.SetBool("IsEat", false);
        anim.SetBool("IsSleep", false);
        anim.SetBool("IsJump", false);
    }
}
