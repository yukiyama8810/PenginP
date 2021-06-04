using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class ResultPopUp : MonoBehaviour
{
    [SerializeField] Button btnRetry;
    [SerializeField] CanvasGroup canvasGroupTxt;
    [SerializeField] CanvasGroup canvasGroupPopUp;
    [SerializeField] Image imgTitle;
    bool isClickable;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroupPopUp.alpha = 0;
        canvasGroupTxt.alpha = 0;

        btnRetry.onClick.AddListener(OnClickRetry);
        btnRetry.interactable = false;
        isClickable = false;
    }

    public void DisplayResult()
    {
        canvasGroupPopUp.DOFade(1.0f, 1.0f).OnComplete(() =>
         {
             btnRetry.interactable = true;
             canvasGroupTxt.DOFade(1.0f, 1.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
         });
    }

    void OnClickRetry()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
