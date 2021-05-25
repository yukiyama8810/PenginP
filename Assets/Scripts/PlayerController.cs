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

    //キャラの状態の種類
    public enum AttitudeType
    {
        /*直滑降(デフォルト)*/Straight, /*伏せ*/Prone,
    }
    [Header("現在のキャラの姿勢")] public AttitudeType attitudeType;

    private Rigidbody rb;

    private float x;
    private float z;

    private float Altitude;
    

    private Vector3 straightRotation = new Vector3(180, 0, 0);  //頭を下方向に向ける際の回転角度
    private Vector3 proneRotation = new Vector3(-90, 0, 0);     //伏せの姿勢の回転角度

    private int score;

    private float attitudeTimer;        //姿勢変更可能になるまでのタイマー
    private float chargeTime = 2.0f;    //可能になるまでのチャージ時間

    [SerializeField, Header("水しぶきエフェクト")]
    private GameObject splashEffectPrefab = null;

    [SerializeField, Header("水しぶきSE")]
    private AudioClip splashSE = null;

    [SerializeField] private Text txtScore;
    [SerializeField] private Text txtAltitude;

    [SerializeField] private Button btnChangeAttitude;

    [SerializeField] private Image imgGauge;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.eulerAngles = straightRotation;
        Altitude = (transform.position.y - 4.0f) / 4;
        attitudeType = AttitudeType.Straight;

        btnChangeAttitude.onClick.AddListener(ChangeAttitude);
        
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
            //Debug.Log(txtAltitude.text);
        }

        if (Input.GetButtonDown("Jump"))
        {
            ChangeAttitude();
        }

        if(attitudeType == AttitudeType.Straight)
        {
            attitudeTimer += Time.deltaTime;

            imgGauge.DOFillAmount(attitudeTimer / chargeTime, 0.1f);

            if(attitudeTimer >= chargeTime)
            {
                attitudeTimer = chargeTime;
            }
        }
        if(attitudeType == AttitudeType.Prone)
        {
            attitudeTimer -= Time.deltaTime;

            imgGauge.DOFillAmount(attitudeTimer / chargeTime, 0.1f);

            if(attitudeTimer <= 0)
            {
                attitudeTimer = 0;
            }
        }
        
    }

    /// <summary>
    /// 姿勢の管理変更
    /// </summary>
    /// ToDo マウスでボタンをクリックしたのちスペースキーを押すと高速ダブルタップ判定になりバグる減少の解決（しなくてもいいかも）
    private void ChangeAttitude()
    {
        if (!inWater)
        {

            switch (attitudeType)
            {
                case AttitudeType.Straight:

                    attitudeType = AttitudeType.Prone;

                    transform.DORotate(proneRotation, 0.25f, RotateMode.WorldAxisAdd);

                    rb.drag = 25.0f;

                    btnChangeAttitude.transform.GetChild(0).DORotate(new Vector3(0, 0, 90), 0.25f);

                    break;

                case AttitudeType.Prone:

                    attitudeType = AttitudeType.Straight;

                    transform.DORotate(straightRotation, 0.25f);

                    rb.drag = 0f;

                    btnChangeAttitude.transform.GetChild(0).DORotate(new Vector3(0, 0, 180), 0.25f);

                    break;

            }
            
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
