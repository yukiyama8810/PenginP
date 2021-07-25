using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class FlowerCircle : MonoBehaviour
{
    [Header("持ち得点")]
    public int point;

    

    GameObject[] Flower;

    // Start is called before the first frame update
    void Start()
    {
        Flower = new GameObject[transform.childCount];

        for(int i = 0; i < transform.childCount; i++)
        {
            Flower[i] = transform.GetChild(i).gameObject;
        }
        //アタッチした花輪の回転
        transform.DORotate(new Vector3(0, 360, 0), 5.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }
    /// <summary>
    /// 花取得時のアニメーション及びエフェクト管理のメソッド
    /// </summary>
    public void GetFlowerAnim(Transform p)
    {
        
        foreach(GameObject a in Flower)
        {
            if (a.name.Contains("flower"))
            {
                a.transform.parent = p;
                a.transform.DOLocalMove(Vector3.zero, 1.5f);
                a.transform.DOScale(Vector3.zero, 1.5f);
                Destroy(a, 1.1f);
            }
        }
    }
}
