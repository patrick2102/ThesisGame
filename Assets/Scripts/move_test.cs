using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_test : MonoBehaviour
{

    public int moveMultiplier;

    private Rigidbody2D playerRB2D;

    private void Awake()
    {
        playerRB2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)) {
            playerRB2D.AddForce(transform.right * moveMultiplier * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerRB2D.AddForce(-transform.right * moveMultiplier * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            playerRB2D.AddForce(transform.up * moveMultiplier * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            playerRB2D.AddForce(-transform.up * moveMultiplier * Time.deltaTime);
        }
    }
}
