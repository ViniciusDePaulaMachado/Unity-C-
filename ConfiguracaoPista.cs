using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ConfiguracaoPista : MonoBehaviour
{
    HUDCarro hudCarro;
    GameObject t;

    [SerializeField]
    private int direinhoGanho = 30000;
    [SerializeField]
    private int dinheitoPerdido = 5000;
    [SerializeField]
    private int quantidadeDeVoltas = 4;


    private void Start()
    {
        t = GameObject.FindWithTag("Player");

         hudCarro = t.GetComponent<HUDCarro>();

        //hudCarro = GetComponent<HUDCarro>();

        hudCarro.setQuantidadeDeVoltas(this.quantidadeDeVoltas);
        hudCarro.setQuantidadeGanho(this.direinhoGanho);
        hudCarro.setQuatidadePerdido(this.dinheitoPerdido);

        /*
        hudCarro.setQuantidadeDeVoltas(quantidadeDeVoltas);
        hudCarro.setQuantidadeGanho(direinhoGanho);
        hudCarro.setQuatidadePerdido(dinheitoPerdido);
        */
    }
}
