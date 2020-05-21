using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Idle");
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
        animator.Play("Walk");
    }
    public void Run()
    {
        animator.Play("Run");
    }
    public void Jump()
    {
        animator.Play("f_melee_combat_run");
    }
}
