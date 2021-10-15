using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudarCameras : MonoBehaviour
{

    private GameObject cameraPrincipal;
    private GameObject camera1;
    private int limitador;
    
    void Start()
    {
        camera1 = GameObject.Find("Camera");
        cameraPrincipal = GameObject.Find("MultipurposeCameraRig");
        camera1.SetActive(false);
    }

    public void AtivaCameraUm()
    {
        limitador++;

        if(limitador == 1)
        {
            camera1.SetActive(true);
            cameraPrincipal.SetActive(false);
        } 
        if (limitador == 2)
        {
            cameraPrincipal.SetActive(true);
            camera1.SetActive(false);
            limitador = 0;
        }
        
    }

}
