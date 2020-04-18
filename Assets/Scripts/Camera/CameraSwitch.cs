using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject ThirdPersonCam;
    public GameObject FirstPersonCam;
    public int CamMode;


    private void Start()
    {
        CamMode = 0;
        StartCoroutine(CamChange());
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("CameraSwitch"))
        {
            if (CamMode == 1)
            {
                CamMode = 0;
            }
            else if (CamMode == 0)
            {
                CamMode = 1;
            }

            StartCoroutine(CamChange());
        }
    }

    IEnumerator CamChange()
    {
        yield return new WaitForSeconds(0.01f);
        if (CamMode == 0)
        {
            ThirdPersonCam.SetActive(true);
            FirstPersonCam.SetActive(false);
        }
        else if (CamMode == 1)
        {
            FirstPersonCam.SetActive(true);
            ThirdPersonCam.SetActive(false);
            
        }
    }
}
