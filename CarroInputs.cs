using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarroInputs : MonoBehaviour
{

    ControleCarro carro;
   
    public GameObject _buttonAcelera, _buttonFreia, _buttonEsquerda, _buttonDireita, _buttonRe;
    ControlemoMobile direitaBotao, esquerdaBotao, freiarBotao, acelerarBotao, reBotao;

    float acelerou;
    float freiou;
    float deuRe;
    float apertouEsquerda;
    float apertouDireita;
    float inputVertical;
    float inputHorizontal;
    

    public void Direcao()
    {
        apertouEsquerda = esquerdaBotao.input;
        apertouDireita = direitaBotao.input;
        carro.deuRe = reBotao.input;
        carro.InputHorizontalTeclado = Input.GetAxis("Horizontal");


        if (apertouEsquerda > 0)
        {
            inputHorizontal = -apertouEsquerda;
        }
        else if(apertouDireita > 0)
        {
            inputHorizontal = apertouDireita;
        }else
        {
            inputHorizontal = 0;
        }
        carro.inputHorizontal = inputHorizontal;
    }

    public void Acelerar()
    {
        acelerou = acelerarBotao.input;
        freiou = freiarBotao.input;

        if (acelerou > 0)
        {
            inputVertical = acelerou;
        }
        else if(freiou > 0)
        {
            inputVertical = -freiou;
        }
        else
        {
            inputVertical = 0;
        }

        carro.inputVertical = inputVertical;
        carro.InputVerticallTeclado = Input.GetAxis("Vertical");
    }

    void Start()
    {
        carro = GetComponent<ControleCarro>();

        acelerarBotao = _buttonAcelera.GetComponent<ControlemoMobile>();
        freiarBotao = _buttonFreia.GetComponent<ControlemoMobile>();
        direitaBotao = _buttonDireita.GetComponent<ControlemoMobile>();
        esquerdaBotao = _buttonEsquerda.GetComponent<ControlemoMobile>();
        reBotao = _buttonRe.GetComponent<ControlemoMobile>();
    }


    void Update()
    {
        Acelerar();
        Direcao();
    }
}
