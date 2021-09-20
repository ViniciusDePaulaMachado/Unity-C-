using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointInicial : MonoBehaviour
{

    CheckPoint[] checkpoints;
   

    private void Awake()
    {
        checkpoints = FindObjectsOfType<CheckPoint>();
    }

    private void Start()
    {
        HUDCarro cHud = GetComponent<HUDCarro>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        ContadorDeVoltas controladorDeVoltas = other.transform.root.GetComponent<ContadorDeVoltas>();

        foreach(CheckPoint ch in checkpoints)
        {
            if (!ch.PassouCheckPont(controladorDeVoltas, controladorDeVoltas.contadorDeVoltas))
            {
                Debug.Log("Volta Invalida");

                return;
            }
        }
        controladorDeVoltas.SomarVoltas();
    }

}
