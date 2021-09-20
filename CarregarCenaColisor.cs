using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarregarCenaColisor : MonoBehaviour
{
    [SerializeField]
    private string nomeDaCena;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.CompareTag("Player"))
        {
            LoadSceneScript.LoadSceneName(nomeDaCena);
        }
    }
}
