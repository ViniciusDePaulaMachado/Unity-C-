using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CronometroVoltas : MonoBehaviour
{
    private bool iniciarCronometro = false;

    private Text cronometroText;
    private GameObject cronometroTextObject;

    private Text TempoDeVoltaAnteriorText;
    private GameObject TempoDeVoltaAnteriorTextObject;

    private float segundos;
    private int minuto;

    private float segundosDeVolta;
    private int minutoDeVolta;

    //==============================================================

    private void Start()
    {
        cronometroTextObject = GameObject.Find("Cronometro");
        cronometroText = cronometroTextObject.GetComponent<Text>();

        TempoDeVoltaAnteriorTextObject = GameObject.Find("TempoDeVoltaAnteriorText");
        TempoDeVoltaAnteriorText = TempoDeVoltaAnteriorTextObject.GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        CronometrarVolta();
    }

    private void OnTriggerEnter(Collider c)
    {
                
        if (c.transform.root.gameObject.CompareTag("Player"))
        {
            Debug.Log("Ativado");

            iniciarCronometro = true;

            minutoDeVolta = this.minuto;

            segundosDeVolta = this.segundos;

            if(minutoDeVolta > 0 && segundosDeVolta > 0){

                TempoDeVoltaAnteriorText.text = minutoDeVolta.ToString() + ":" + segundosDeVolta.ToString("00.00");
            }
            else
            {
                TempoDeVoltaAnteriorText.text = "";
            }

            this.segundos = 0;
            this.minuto = 0;
        }        
    }

    private void CronometrarVolta()
    {
        if(iniciarCronometro == true)
        {
            segundos += Time.deltaTime;

            if (segundos > 59.59f)
            {
                minuto++;
                segundos = 0;
            }

            cronometroText.text = minuto.ToString() + ":" + segundos.ToString("00.00");
        }
    }
}
