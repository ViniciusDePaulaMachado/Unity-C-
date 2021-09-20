using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CronometroVoltas : MonoBehaviour
{
    private Text cronometroText;
    private GameObject cronometroTextObject;

    private Text TempoDeVoltaAnteriorText;
    private GameObject TempoDeVoltaAnteriorTextObject;

    private float segundos;
    private int minuto;

    private float segundoAnterios;
    private int minutoAnterior;

    private bool iniciarCronometro = false;


    private void Start()
    {
        cronometroTextObject = GameObject.Find("Cronometro");
        cronometroText = cronometroTextObject.GetComponent<Text>();

        TempoDeVoltaAnteriorTextObject = GameObject.Find("TempoDeVoltaAnteriorText");
        TempoDeVoltaAnteriorText = TempoDeVoltaAnteriorTextObject.GetComponent<Text>();
    }

    void FixedUpdate()
    {
        CronometrarVolta();
    }

    private void OnTriggerEnter(Collider c)
    {
        
        if (c.transform.root.gameObject.CompareTag("Player"))
        {
            Debug.Log("Cronometro Iniciado");

            iniciarCronometro = true;

            minutoAnterior = this.minuto;

            segundoAnterios = this.segundos;

            TempoDeVoltaAnteriorText.text = minutoAnterior.ToString() + ":" + segundoAnterios.ToString("00.00");

            this.segundos = 0;

            this.minuto = 0;
        }        
    }
    
    private void CronometrarVolta()
    {
        if(iniciarCronometro == true)
        {
            segundos += Time.deltaTime;

            cronometroText.text = minuto.ToString() + ":" + segundos.ToString("00.00");

            if (segundos > 59.59f)
            {
                minuto++;

                segundos = 0;
            }
        }
    }
}
