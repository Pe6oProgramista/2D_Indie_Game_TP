using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 5;
    public float jumpSpeed = 6;
    public bool facingRight = true;

    private bool grounded = false;
    private float moveDir = 0;
    private int canJump = 2;

    private Rigidbody2D myRB;
    private Animator myAnim;

    void Start()
    {
        SceneManager.UnloadSceneAsync(6);
        jumpSpeed = 6;
        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        if(!facingRight)
        {
            Flip();
            facingRight = !facingRight;
        }
    }

    private void Update()
    {
        myAnim.SetBool("isGrounded", grounded);
        myAnim.SetFloat("verticalSpeed", myRB.velocity.y);
        myAnim.SetFloat("speed", Mathf.Abs(moveDir));
        myRB.velocity = new Vector2(moveDir * maxSpeed, myRB.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.W))
        {
            canJump--;
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDir = 1;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 0;
        }

        if ((moveDir > 0 && !facingRight) || (moveDir < 0 && facingRight))
        {
            Flip();
        }
    }

    void OnBecameInvisible()
    {
        SceneManager.LoadScene("Level");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            SceneManager.LoadScene("Level");
        }
        if(collision.collider.gameObject.tag.Equals("Ground"))
        {
            if (GetComponent<EdgeCollider2D>().IsTouching(collision.collider))
            {
                grounded = true;
                canJump = 2;
            }
        }
    }

    void Jump()
    {
        if (canJump >= 0)
        {
            grounded = false;
            myRB.velocity = new Vector2(myRB.velocity.x, (canJump > 1) ? jumpSpeed : jumpSpeed + 1);
        }
    }

    private void Dash()
    {
        myRB.velocity = new Vector2(moveDir * 50, myRB.velocity.y);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
