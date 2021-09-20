using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudarCameras : MonoBehaviour
{

    public GameObject camera1;
    public GameObject camera2;
    public int cameras = 1;
    public int numeroDeCameras = 2;


    public void AtivaCameraUm()
    {
        cameras++;

        if(cameras == 1)
        {
            camera1.SetActive(true);
            camera2.SetActive(false);
        }

        if (cameras == 2)
        {
            camera2.SetActive(true);
            camera1.SetActive(false);
        }

        if (cameras >= numeroDeCameras)
        {
            cameras = 0;
        }
    }

}
