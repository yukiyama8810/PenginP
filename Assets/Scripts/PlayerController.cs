using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�ړ����x")]
    public float moveSpeed;

    [Header("�������x")]
    public float fallSpeed;

    [Header("��������")]
    public bool inWater;

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

        //Debug.Log(rb.velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Water" && inWater == false)
        {
            inWater = true;

            //ToDo �����Ԃ��G�t�F�N�g�̎���
            Debug.Log("���� : " + inWater);
        }
    }

}
