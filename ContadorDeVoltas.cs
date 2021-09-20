using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContadorDeVoltas : MonoBehaviour
{
    public int contadorDeVoltas = 0;

    public void SomarVoltas()
    {
        contadorDeVoltas++;
        Debug.Log("Volta Completa" + contadorDeVoltas.ToString());
    }

}
