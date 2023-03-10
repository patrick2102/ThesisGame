using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D playerRB;
    private Vector2 movement;
    
    private void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        playerRB.velocity = new Vector2(movement.x, movement.y).normalized * moveSpeed;
    }
}
