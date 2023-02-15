using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_test : MonoBehaviour
{

    public int moveMultiplier;
    Vector3 tempPos;

    private Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)) {
            rb2D.AddForce(transform.right * moveMultiplier);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb2D.AddForce(-transform.right * moveMultiplier);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb2D.AddForce(transform.up * moveMultiplier);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb2D.AddForce(-transform.up * moveMultiplier);
        }
    }
}
