using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    [SerializeField] Camera fpsCamera;
    [SerializeField] Camera selfishCamera;
    [SerializeField] Button btnChangeCamera;
    int CameraIndex;
    Camera mainCamera;

    [Header("ÉJÉÅÉâãóó£ÇÃé©ìÆí≤êÆ")]
    [SerializeField] bool AutoAdjustOffset;

    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        if (AutoAdjustOffset)
        {
            transform.position = playerController.transform.position + new Vector3(0, 12, -4);
            AutoAdjustOffset = false;
        }
        offset = transform.position - playerController.transform.position;

        mainCamera = Camera.main;
        btnChangeCamera.onClick.AddListener(ChangeCamera);
        SetDefaultCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.inWater == true)
        {
            if(CameraIndex != 0)
            {
                SetDefaultCamera();
            }
            return;
        }
        if(playerController != null)
        {
            transform.position = playerController.transform.position + offset;
        }
    }

    void ChangeCamera()
    {
        if (!playerController.inWater)
        {
            switch (CameraIndex)
            {
                case 0:
                    CameraIndex++;
                    mainCamera.enabled = false;
                    fpsCamera.enabled = true;
                    break;
                case 1:
                    CameraIndex++;
                    fpsCamera.enabled = false;
                    selfishCamera.enabled = true;
                    break;
                case 2:
                    CameraIndex = 0;
                    selfishCamera.enabled = false;
                    mainCamera.enabled = true;
                    break;
            }
        }        
    }

    void SetDefaultCamera()
    {
        CameraIndex = 0;

        mainCamera.enabled = true;
        fpsCamera.enabled = false;
        selfishCamera.enabled = false;
    }
}
