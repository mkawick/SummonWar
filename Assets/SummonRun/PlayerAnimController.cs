using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    Animator animator;
    Vector3 originalPlayerPosition;
    public Transform maximumPlayerRunningPosition;
    //Vector3 maximumPlayerPosition;
    float speedyTimeToSlowDown = 0;
    public float animSpeedWhileSpeedRunning = 2;
    bool playerIsPaused = false;

    public float quickRunGroundSpeed = 1;
    void Start()
    {
        GetAnimator().Play("Idle");
        originalPlayerPosition = transform.position;
        //maximumPlayerPosition = maximumPlayerRunningPosition.position;
    }

    public void ResetPlayerState()
    {
        transform.position = originalPlayerPosition;
        playerIsPaused = false;
        //Run();
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
            else
            {
                if(transform.position.z != originalPlayerPosition.z)
                {
                    float currentZ = transform.position.z;
                    //Vector3 temp = originalPlayerPosition.z;
                    float dist = maximumPlayerRunningPosition.position.z - currentZ;
                    if(dist<0.01f)
                    {
                        transform.position = maximumPlayerRunningPosition.position;
                    }
                    else
                    {
                        Vector3 diff = maximumPlayerRunningPosition.position - transform.position;
                        diff *= (quickRunGroundSpeed * Time.deltaTime);
                        transform.position += diff;
                    }
                }
            }
        }
    }
    public void Idle()
    {
        if (playerIsPaused == true)
            return;

        GetAnimator().Play("Idle");
    }

    public void Walk()
    {
        if (playerIsPaused == true)
            return;

        GetAnimator().Play("Walk");
    }
    public void Run()
    {
        if (playerIsPaused == true)
            return;

        GetAnimator().Play("Run");
        //GetAnimator().speed = 2;
    }

    public void PlayerIsDying()
    {
        Idle();
        playerIsPaused = true;
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
