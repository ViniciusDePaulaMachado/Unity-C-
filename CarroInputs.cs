using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarroInputs : MonoBehaviour
{

    ControleCarro carro;
   
    private GameObject _buttonAcelera, _buttonFreia, _buttonEsquerda, _buttonDireita, _buttonRe;
    ControlemoMobile direitaBotao, esquerdaBotao, freiarBotao, acelerarBotao, reBotao;

    float acelerou;
    float freiou;
    
    private float apertouEsquerda;
    private float apertouDireita;

    private float inputVertical;
    private float inputHorizontal;
    private float InputHorizontalTeclado;
    private float InputVerticalTeclado;
    private float deuRe;

    private bool contolePlayer;

    void Start()
    {
        
        ChecarPlayer();
        PegarBotoesDaCena();
        carro = GetComponent<ControleCarro>();
        Teste();
    }

    void FixedUpdate()
    {
        Acelerar();
        Direcao();
    }

    //Botão de Re "Mobile"
    public float getDarRe()
    {
        return this.deuRe;
    }
    
    //Converter para Inputs Horizontal os Botões da tela "Mobile", pegar input Horizontal, teclado, controle, etc.
    public void Direcao()
    {
        if (contolePlayer == true)
        {
            InputHorizontalTeclado = Input.GetAxis("Horizontal");


            apertouEsquerda = esquerdaBotao.input;
            apertouDireita = direitaBotao.input;
            float deuReInput = reBotao.input;
            deuRe = reBotao.input;

            

            if (apertouEsquerda > 0)
            {
                inputHorizontal = -apertouEsquerda;
            }
            else if (apertouDireita > 0)
            {
                inputHorizontal = apertouDireita;
            }
            else
            {
                inputHorizontal = 0;
            }

            if(deuReInput > 0)
            {
                deuRe = -deuReInput;
            }
            else
            {
                deuRe = 0;
            }
        }
    }

    //Converter para Inputs Vertical os Botões da tela "Mobile", teclado, controle, etc
    public void Acelerar()
    {

        if (contolePlayer == true)
        {


            acelerou = acelerarBotao.input;
            freiou = freiarBotao.input;
            
            

            if (acelerou > 0)
            {
                inputVertical = acelerou;
            }
            else if (freiou > 0)
            {
                inputVertical = -freiou;
            }
            else
            {
                inputVertical = 0;
            }

            InputVerticalTeclado = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                carro.SubirMarcha();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                carro.DescerMarcha();
            }
        }  
    }

    //checar se o script está no carro do player ou da IA, para não misturar os inputs dos botões da tela "Mobile"
    private void ChecarPlayer()
    {
        if (gameObject.CompareTag("Player") == true)
        {
            contolePlayer = true;
        }
        else
        {
            contolePlayer = false;
        }
    }

    //Pegar os botões da tela "Mobile", de forma automatica
    private void PegarBotoesDaCena()
    {
        if(contolePlayer == true)
        {
            _buttonAcelera = GameObject.Find("Acelerar");
            _buttonFreia = GameObject.Find("Freiar");
            _buttonEsquerda = GameObject.Find("Esquerda");
            _buttonDireita = GameObject.Find("Direita");
            _buttonRe = GameObject.Find("Re");
        }    
    }

    //iniciar referencia do script dos botões da tela "Mobile"
    private void Teste()
    {
        if(contolePlayer == true)
        {
            acelerarBotao = _buttonAcelera.GetComponent<ControlemoMobile>();
            freiarBotao = _buttonFreia.GetComponent<ControlemoMobile>();
            direitaBotao = _buttonDireita.GetComponent<ControlemoMobile>();
            esquerdaBotao = _buttonEsquerda.GetComponent<ControlemoMobile>();
            reBotao = _buttonRe.GetComponent<ControlemoMobile>();
        }
    }

    //Pegar Inputs vertical e horizontal
    public float getInputVertical()
    {
        return this.inputVertical;
    }

    public float getInputHorizontal()
    {
        return this.inputHorizontal;
    }

    public float getInputVerticalTeclado()
    {
        return this.InputVerticalTeclado;
    }

    public float getInputHorizontalTeclado()
    {
        return this.InputHorizontalTeclado;
    }

    //Inputs Controlados pela IA
    public void setImputVertical(int I)
    {
        this.inputVertical = I;
    }

    public void setInputHorizontal(float i)
    {
        this.inputHorizontal = i;
    }
}
