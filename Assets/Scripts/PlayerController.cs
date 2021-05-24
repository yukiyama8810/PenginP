using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


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

    private Vector3 straightRotation = new Vector3(180, 0, 0); //�����������Ɍ�����ۂ̉�]�p�x

    private int score;

    [SerializeField, Header("�����Ԃ��G�t�F�N�g")]
    private GameObject splashEffectPrefab = null;

    [SerializeField, Header("�����Ԃ�SE")]
    private AudioClip splashSE = null;

    [SerializeField] private Text txtScore;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.eulerAngles = straightRotation;
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

        }
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
