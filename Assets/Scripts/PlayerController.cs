using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Coffee.UIExtensions;


public class PlayerController : MonoBehaviour
{
    [Header("�ړ����x")]
    public float moveSpeed;

    [Header("�������x")]
    public float fallSpeed;

    [Header("��������")]
    public bool inWater;

    //�L�����̏�Ԃ̎��
    public enum AttitudeType
    {
        /*�����~(�f�t�H���g)*/Straight, /*����*/Prone,
    }
    [Header("���݂̃L�����̎p��")] public AttitudeType attitudeType;

    private Rigidbody rb;
    private Animator anim;

    private float x;
    private float z;

    private float Altitude;
    

    private Vector3 straightRotation = new Vector3(180, 0, 0);  //�����������Ɍ�����ۂ̉�]�p�x
    private Vector3 proneRotation = new Vector3(-90, 0, 0);     //�����̎p���̉�]�p�x

    private int score;

    private float attitudeTimer;        //�p���ύX�\�ɂȂ�܂ł̃^�C�}�[
    private float chargeTime = 2.0f;    //�\�ɂȂ�܂ł̃`���[�W����
    private bool attitudeChecker;      

    [SerializeField, Header("�����Ԃ��G�t�F�N�g")]
    private GameObject splashEffectPrefab = null;

    [SerializeField, Header("�����Ԃ�SE")]
    private AudioClip splashSE = null;

    [SerializeField] private Text txtScore;
    [SerializeField] private Text txtAltitude;

    [SerializeField] private Button btnChangeAttitude;

    [SerializeField] private Image imgGauge;
    [SerializeField] private Image imgStop;

    [SerializeField] GameObject effectPrefab;
    [SerializeField] ShinyEffectForUGUI shinyEffect;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        transform.eulerAngles = straightRotation;
        Altitude = (transform.position.y - 4.0f) / 4;
        attitudeType = AttitudeType.Straight;

        btnChangeAttitude.onClick.AddListener(ChangeAttitude);
        
    }

    private void Update()
    {
        if (Altitude > 0)
        {
            //Y���W�S�̒n�_��0m�Ƃ����W���S�������ƂɂP���Ɖ��肵�Čv�Z
            
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

        if(attitudeChecker == false && attitudeType == AttitudeType.Straight)   //�f�t�H�̎��`���[�W���߂�
        {
            attitudeTimer += Time.deltaTime;

            imgGauge.DOFillAmount(attitudeTimer / chargeTime, 0.1f);

            if(attitudeTimer >= chargeTime)
            {
                shinyEffect.Play(0.5f);
                attitudeTimer = chargeTime;
                attitudeChecker = true;
                imgStop.gameObject.SetActive(false);
            }
        }
        if(attitudeType == AttitudeType.Prone)      //���s�̎��Q�[�W���炷
        {
            attitudeTimer -= Time.deltaTime;

            imgGauge.DOFillAmount(attitudeTimer / chargeTime, 0.1f);

            if(attitudeTimer <= 0)
            {
                attitudeTimer = 0;
                ChangeAttitude();
            }
        }
        
    }

    /// <summary>
    /// �p���̊Ǘ��ύX
    /// </summary>
    /// ToDo �}�E�X�Ń{�^�����N���b�N�����̂��X�y�[�X�L�[�������ƍ����_�u���^�b�v����ɂȂ�o�O�錸���̉����i���Ȃ��Ă����������j
    private void ChangeAttitude()
    {
        if (!inWater)
        {

            switch (attitudeType)
            {
                case AttitudeType.Straight:

                    if (attitudeChecker)
                    {
                        attitudeType = AttitudeType.Prone;

                        transform.DORotate(proneRotation, 0.25f, RotateMode.WorldAxisAdd);

                        rb.drag = 25.0f;

                        btnChangeAttitude.transform.GetChild(0).DORotate(new Vector3(0, 0, 90), 0.25f);

                        attitudeChecker = false;

                        anim.SetBool("Prone", true);
                    }

                    

                    break;

                case AttitudeType.Prone:

                    attitudeType = AttitudeType.Straight;

                    transform.DORotate(straightRotation, 0.25f);

                    rb.drag = 0f;

                    btnChangeAttitude.transform.GetChild(0).DORotate(new Vector3(0, 0, 180), 0.25f);

                    imgStop.gameObject.SetActive(true);

                    anim.SetBool("Prone", false);

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

            //�G�t�F�N�g�����A�������ꂽ�G�t�F�N�g��effect�ɑ��
            GameObject effect = Instantiate(splashEffectPrefab, transform.position, Quaternion.identity);

            //effect�ϐ��𗘗p���āA�G�t�F�N�g�̈ʒu�𒲐�
            effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y + 4.0f, effect.transform.position.z - 0.5f);

            //effect�ϐ��𗘗p���ăG�t�F�N�g��2�b��ɔj��
            Destroy(effect, 2.0f);


            //AudioSource.PlayClipAtPoint(splashSE, transform.position);

            StartCoroutine(OutOfWater());

        }

        if(other.gameObject.tag == "FlowerCircle")
        {
            //Debug.Log("�Ԃ���");

            //�Փ˂����Ԃɂ�������point�̒l�����Z
            score += other.transform.parent.GetComponent<FlowerCircle>().point;

            //Debug.Log("���݂̓��_ :" + score);
            txtScore.text = score.ToString();

            StartCoroutine(GetFlowerEfc());

            other.transform.parent.GetComponent<FlowerCircle>().GetFlowerAnim(this.transform);
        }
    }
    private IEnumerator GetFlowerEfc()
    {
        yield return new WaitForSeconds(1.1f);

        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

        Destroy(effect, 1.0f);
    }

    /// <summary>
    /// ���ʂɊ���o��
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
