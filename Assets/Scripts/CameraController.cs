using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.inWater == true)
        {
            return;
        }
        if(playerController != null)
        {
            transform.position = playerController.transform.position + offset;
        }
    }
}
