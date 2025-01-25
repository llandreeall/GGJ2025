using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float drag;
    [SerializeField]
    private float height;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private float airMultiplier;
    [SerializeField]
    private LayerMask ground;

    private bool onGround;
    private float hInput;
    private Vector2 moveDir;
    private Rigidbody2D rb;
    private bool readyToJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.Raycast(transform.position, Vector3.down, height, ground) ? true : false;
        //If UI window open, return
        //Process input
        GetInput();
        SpeedControl();

        if (onGround)
        {
            rb.drag = drag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        hInput = Input.GetAxisRaw("Horizontal");

        //Animate player

        //Jump
        if (Input.GetMouseButtonDown(0) && readyToJump && onGround)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), cooldown);
        }
    }

    private void Move()
    {
        moveDir = transform.right * hInput;
        if (onGround)
        {
            rb.AddForce(moveDir.normalized * speed, ForceMode2D.Force);
        }
        else
        {
            rb.AddForce(moveDir.normalized * speed * airMultiplier, ForceMode2D.Force);
        }
    }

    private void SpeedControl()
    {
        Vector2 flatVelocity = new Vector2(rb.velocity.x, 0f);
        if (flatVelocity.magnitude > speed)
        {
            Vector2 velocityLimited = flatVelocity.normalized * speed;
            rb.velocity = new Vector2(velocityLimited.x, rb.velocity.y);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
