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
    private int quantidadeDeVoltas;

    private void Start()
    {
        t = GameObject.FindWithTag("Player");

        hudCarro = t.GetComponent<HUDCarro>();
        quantidadeDeVoltas = PlayerPrefs.GetInt("Voltas");
        hudCarro.setQuantidadeDeVoltas(this.quantidadeDeVoltas);
        hudCarro.setQuantidadeGanho(this.direinhoGanho);
        hudCarro.setQuatidadePerdido(this.dinheitoPerdido);

    }
}
