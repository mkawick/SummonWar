using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    [Range(3,10)]
    public float jumpVelocity = 10;
    [Range(1.1f, 8)]
    public float fallMultiplier = 2.5f;
    [Range(1.1f, 8)]
    public float lowJumpMultiplier = 2.0f;
    [Range(0.3f, 3)]
    public float doubleJumpMultiplier = 1.0f;

    [Range(0.8f, 3)]
    public float distanceToGround = 1.0f;

    PlayerAnimController pac;


    int jumpCount = 0;
    float colliderCenterHeight;
    Rigidbody rigidBody;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        pac = GetComponent<PlayerAnimController>();
        var bc = GetComponent<BoxCollider>();
        colliderCenterHeight = bc.center.y;
        //bc.center.y;
    }
    void Start()
    {
        pac.Run();
    }
    void Update()
    {
        if (DidJump() == true)
        {
            
            if (jumpCount != 2)
            {
                jumpCount++;

                if (IsFalling())
                    rigidBody.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
                else
                    rigidBody.AddForce(Vector3.up * jumpVelocity * doubleJumpMultiplier, ForceMode.VelocityChange);
            }

            //Debug.Log("gravity: " + rigidBody.useGravity);
        }
        ModifyFalling();

        if (IsStandingOnSomething())
        {
            jumpCount = 0;
            pac.Run();
        }
        else
        {
            pac.Jump();
        }
    }

    bool DidJump()
    {
        return Input.GetMouseButtonDown(0) | Input.GetKeyDown(KeyCode.Space);
    }

    //https://www.youtube.com/watch?time_continue=708&v=7KiK0Aqtmzc&feature=emb_logo
    void ModifyFalling()
    {
        if (IsFalling())
        {
            rigidBody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidBody.velocity.y > 0 && Input.GetMouseButton(0) == false)
        {
            rigidBody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        }
    }

    bool IsFalling()
    {
        if (rigidBody.velocity.y < 0)
            return true;
        return false;
    }
    bool IsStandingOnSomething()
    {
        // return true;
        //colliderCenterHeight+
        if (//rigidBody.velocity.y < 0 && 
            transform.position.y < distanceToGround)//1f)
            return true;
      /*  {
            //int layerMask = 1 << 8;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                if ((hit.point - transform.position).magnitude < distanceToGround)
                {
                    return true;
                }
            }
        }*/
       
        return false;
    }

}
