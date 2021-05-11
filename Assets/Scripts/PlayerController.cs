using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("ˆÚ“®‘¬“x")]
    public float moveSpeed;

    [Header("—Ž‰º‘¬“x")]
    public float fallSpeed;

    private Rigidbody rb;

    private float x;
    private float z;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(x * moveSpeed, -fallSpeed, z * moveSpeed);

        Debug.Log(rb.velocity);
    }

    
}
