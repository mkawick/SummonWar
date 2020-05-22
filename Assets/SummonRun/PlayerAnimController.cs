using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        GetAnimator().Play("Idle");
    }

    Animator GetAnimator()
    {
        if(animator == null)
            animator = GetComponent<Animator>();
        return animator;
    }
    // Update is called once per frame
    void Update()
    {
       /* if(Input.GetKeyUp(KeyCode.Space))
        {
            animator.Play("Walk");
        }*/
    }

    public void Walk()
    {
        GetAnimator().Play("Walk");
    }
    public void Run()
    {
        GetAnimator().Play("Run");
    }
    public void Jump()
    {
        GetAnimator().Play("f_melee_combat_run");
    }
}
