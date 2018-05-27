using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //movement variables
    public float maxSpeed;

    //jumping variables
    private bool grounded = false;
    private bool jumping = false;
    private bool canDoubleJump = true;

    public LayerMask groundLayer;
    public float jumpSpeed;
    public bool facingRight = true;

    Rigidbody2D myRB;
    Animator myAnim;

    // Use this for initialization
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        if(!facingRight)
        {
            Flip();
            facingRight = !facingRight;
        }
    }

    void FixedUpdate()
    {
        if (GetComponent<CircleCollider2D>().IsTouchingLayers(groundLayer))
        {
            grounded = true;
            canDoubleJump = false;
            jumping = false;
        }

        myAnim.SetBool("isGrounded", grounded);
        myAnim.SetFloat("verticalSpeed", myRB.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && (grounded || canDoubleJump))
        {
            Jump();
        }
        myAnim.SetBool("isGrounded", grounded);
        myAnim.SetFloat("verticalSpeed", myRB.velocity.y);

        float move = 0;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            move = 1;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            move = -1;
        }

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

        myAnim.SetFloat("speed", Mathf.Abs(move));
        myRB.velocity = new Vector2(move * maxSpeed, myRB.velocity.y);
    }

    void Jump()
    {
        if(grounded)
        {
            jumping = true;
            grounded = false;
            myRB.AddForce(new Vector2(myRB.velocity.x, jumpSpeed));
            canDoubleJump = true;
        }
        else if(canDoubleJump)
        {
            canDoubleJump = false;
            myRB.AddForce(new Vector2(myRB.velocity.x, jumpSpeed));
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
