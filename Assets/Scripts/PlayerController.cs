using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed;

    [Header("落下速度")]
    public float fallSpeed;

    [Header("着水判定")]
    public bool inWater;

    private Rigidbody rb;

    private float x;
    private float z;

    [SerializeField, Header("水しぶきエフェクト")]
    private GameObject splashEffectPrefab = null;

    [SerializeField, Header("水しぶきSE")]
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

            //エフェクト生成、生成されたエフェクトをeffectに代入
            GameObject effect = Instantiate(splashEffectPrefab, transform.position, Quaternion.identity);

            //effect変数を利用して、エフェクトの位置を調整
            effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y + 4.0f, effect.transform.position.z - 0.5f);

            //effect変数を利用してエフェクトを2秒後に破壊
            Destroy(effect, 2.0f);


            AudioSource.PlayClipAtPoint(splashSE, transform.position);

        }
    }

}
