using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private const float FALL_MULT = 1;
    private const float LOW_JUMP_MULT = 1.5f;

    public float gravity;
    public float Speed;
    public float jumpSpeed;

    public float moveX, moveZ;

    public bool isGrounded;

    public LayerMask GroundLayer;

    private Rigidbody rb;
    public int Health;

    public float VerticalVelocity;
    public bool canDoubleJump;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Health = 3;
    }
    void FixedUpdate()
    {
        isGrounded = (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z), Vector2.down, 0.6f, GroundLayer));

        CheckInput();
        Move();

        adjustGravity();
    }

    void Move()
    {
        rb.velocity = new Vector3 (moveX * Speed, rb.velocity.y, moveZ * Speed);
        // gameObject.transform.Translate (new Vector3(moveX * Speed, rb.velocity.y, moveZ * Speed));
    }

    void Jump()
    {
        // controller.AddForce(Vector3.up * jumpSpeed);
        rb.AddForce (Vector3.up * jumpSpeed);
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            canDoubleJump = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump)
        {
            Jump();
            canDoubleJump = false;
        }
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
    }

    void adjustGravity ()
    {
        if (VerticalVelocity < 0 && isGrounded == false)
        {
            VerticalVelocity += Physics.gravity.y * FALL_MULT * Time.deltaTime;
        }
        if (VerticalVelocity > 0 && (!Input.GetKeyDown(KeyCode.Space)) && isGrounded == false)
        {
            VerticalVelocity += Physics.gravity.y * FALL_MULT * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "HealthPickUp")
        {
            if (Health < 3)
            {
                Health ++;
                Destroy(col.gameObject);
            }
        }
    }
}