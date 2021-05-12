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

    [SerializeField, Header("�����Ԃ��G�t�F�N�g")]
    private GameObject splashEffectPrefab = null;

    [SerializeField, Header("�����Ԃ�SE")]
    private AudioClip splashSE = null;



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

            //�G�t�F�N�g�����A�������ꂽ�G�t�F�N�g��effect�ɑ��
            GameObject effect = Instantiate(splashEffectPrefab, transform.position, Quaternion.identity);

            //effect�ϐ��𗘗p���āA�G�t�F�N�g�̈ʒu�𒲐�
            effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y + 4.0f, effect.transform.position.z - 0.5f);

            //effect�ϐ��𗘗p���ăG�t�F�N�g��2�b��ɔj��
            Destroy(effect, 2.0f);


            AudioSource.PlayClipAtPoint(splashSE, transform.position);

        }
    }

}
