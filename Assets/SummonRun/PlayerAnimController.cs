using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    Animator animator;
    Vector3 originalPlayerPosition;
    float speedyTimeToSlowDown = 0;
    public float animSpeedWhileSpeedRunning = 2;
    void Start()
    {
        GetAnimator().Play("Idle");
        originalPlayerPosition = transform.position;
    }

    public void ResetPlayerState()
    {
        transform.position = originalPlayerPosition;
        Run();
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
        if(speedyTimeToSlowDown != 0)
        {
            if(speedyTimeToSlowDown<Time.time)
            {
                GetAnimator().speed = 1;
                speedyTimeToSlowDown = 0;
            }
        }
    }
    public void Idle()
    {
        GetAnimator().Play("Idle");
    }

    public void Walk()
    {
        GetAnimator().Play("Walk");
    }
    public void Run()
    {
        GetAnimator().Play("Run");
        //GetAnimator().speed = 2;
    }

    public void SpeedUp(float runTime)
    {
        GetAnimator().speed = animSpeedWhileSpeedRunning;
        speedyTimeToSlowDown = Time.time + runTime;
    }

    public void Jump()
    {
        GetAnimator().Play("f_melee_combat_run");
    }
}
