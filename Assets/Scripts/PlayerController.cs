using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


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

    private float Altitude;
    

    private Vector3 straightRotation = new Vector3(180, 0, 0); //頭を下方向に向ける際の回転角度

    private int score;

    [SerializeField, Header("水しぶきエフェクト")]
    private GameObject splashEffectPrefab = null;

    [SerializeField, Header("水しぶきSE")]
    private AudioClip splashSE = null;

    [SerializeField] private Text txtScore;
    [SerializeField] private Text txtAltitude;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.eulerAngles = straightRotation;
        Altitude = (transform.position.y - 4.0f) / 4;
        
    }

    private void Update()
    {
        if (Altitude > 0)
        {
            //Y座標４の地点を0mとし座標が４動くごとに１ｍと仮定して計算
            
            txtAltitude.text = Altitude.ToString("F2");
            Altitude = (transform.position.y - 4) / 4;
            //Debug.Log("test"+test);
            //Debug.Log("Altitude :" + Altitude);
            Debug.Log(txtAltitude.text);
        }
        
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


            //AudioSource.PlayClipAtPoint(splashSE, transform.position);

            StartCoroutine(OutOfWater());

        }

        if(other.gameObject.tag == "FlowerCircle")
        {
            //Debug.Log("花おｋ");

            //衝突した花にくっついたpointの値を加算
            score += other.transform.parent.GetComponent<FlowerCircle>().point;

            //Debug.Log("現在の得点 :" + score);
            txtScore.text = score.ToString();

        }
    }
    /// <summary>
    /// 水面に顔を出す
    /// </summary>
    /// <returns></returns>
    private IEnumerator OutOfWater()
    {
        yield return new WaitForSeconds(1.0f);

        rb.isKinematic = true;

        transform.eulerAngles = new Vector3(-30, 180, 0);

        transform.DOMoveY(3.9f, 1.0f);
    }

}
