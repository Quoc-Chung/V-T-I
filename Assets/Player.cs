using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    public float MoveSpeed;
    public float JumpForce;

    private float X_Input; 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        X_Input = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(X_Input * MoveSpeed, rb.velocity.y);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }
}
