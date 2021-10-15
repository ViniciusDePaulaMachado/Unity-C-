using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDCarro : MonoBehaviour
{
    ControleCarro carro;

    ContadorDeVoltas ContadorDeVolta;

    CheckPointInicial check;

    LogicaGanhaDinheiro logicaDinheiro;

    ConfiguracaoPista configPista;

    private RectTransform ponteiroRPM;
    private GameObject ponteiroRPMObject;
    
    private Text textoVelocidade;
    private GameObject textoVelocidadeObject;
    
    private Text textMarchaAtual;
    private GameObject textMarchaAtualObject;
    
    private Text voltaAtualText;
    private GameObject voltaAtualTextObject;
    
    private Text posicaoAtualText;
    private GameObject posicaoAtualTextObject;

    private Text textoFimDaCorrida;
    private GameObject textoFimDaCorridaObject;


    [SerializeField]
    private float minAng = 150;

    [SerializeField]
    private float maxAng = 220;

    public int pos;
    private int quantidadeDeDineiroPerdido;
    private int quantidadeDeDineiroGanho;
    private int quantidadeDeVoltas;
    private int voltasSalvas;
    //========================================================================================================
    private float rpmPonteiro;
    private int limite;
    private float tempo;

    //========================================================================================================
    void Start()
    {
        carro = GetComponent<ControleCarro>();
        ContadorDeVolta = GetComponent<ContadorDeVoltas>();
        
        CarregarHud();
        check = GetComponent<CheckPointInicial>();
        logicaDinheiro = GetComponent<LogicaGanhaDinheiro>();
        voltasSalvas = PlayerPrefs.GetInt("Voltas");
    }
    //========================================================================================================
    private void FixedUpdate()
    {
        InformacoesDoCarro();
        ContaVoltasEPos();
        FimDaCorrida();
    }
    //========================================================================================================
    public void InformacoesDoCarro()
    {
        textoVelocidade.text = Mathf.Round(carro.velocidadeKM).ToString();

        textMarchaAtual.text = (carro.marchaAtual+1).ToString() + "°";

        Vector3 RotacaoPonteiro = ponteiroRPM.rotation.eulerAngles;

        rpmPonteiro = Mathf.Lerp(rpmPonteiro, carro.rpm, 0.3f);

        RotacaoPonteiro.z = ((rpmPonteiro * maxAng) / carro.getMaxRpm()) * -1f + minAng;

        ponteiroRPM.eulerAngles = RotacaoPonteiro;
    }
    //========================================================================================================
    public void ContaVoltasEPos()
    {
        voltaAtualText.text = (ContadorDeVolta.contadorDeVoltas + 1).ToString() + "/"+ quantidadeDeVoltas.ToString();

        posicaoAtualText.text = 1 + pos + "°";       
    }
    //========================================================================================================
    public int ReceberPosicao(int volta)
    {
        return pos = volta;

    }
    //========================================================================================================
    public void FimDaCorrida()
    {
        if (ContadorDeVolta.contadorDeVoltas == quantidadeDeVoltas)
        {
            //---------------------------------------------------------------------------------------------------------

            if (ContadorDeVolta.GetComponent<ControleIA>() == null)
            {
                limite++;

                if(limite == 1)
                {
                    if (pos == 0)
                    {
                        logicaDinheiro.GanharDinheiro(quantidadeDeDineiroGanho * voltasSalvas);

                        textoFimDaCorrida.text = "Vencedor " + (pos + 1) + "° Lugar " + " +R$ " + (quantidadeDeDineiroGanho * voltasSalvas)+",00";

                    }
                }

                if (limite == 1)
                {
                    if (pos == 1 || pos == 2)
                    {
                        logicaDinheiro.GanharDinheiro((quantidadeDeDineiroGanho * voltasSalvas) / (pos + 1));
                        textoFimDaCorrida.text = "Fim Da Corrida " + (pos + 1) + "° Lugar " + " +R$ " + ((quantidadeDeDineiroGanho * voltasSalvas) / (pos + 1))+ ",00";
                    }
                }

                if (limite == 1)
                {
                    if (pos > 2)
                    {
                        logicaDinheiro.PerderDinheiro(quantidadeDeDineiroPerdido);
                        textoFimDaCorrida.text = " Fim Da Corrida " + (pos + 1) + "° Lugar " + " R$-" + quantidadeDeDineiroPerdido+ ",00";

                    }
                }
                
                limite++;
                tempo += Time.deltaTime;

                if (tempo > 10.5f)
                {
                    textoFimDaCorrida.text = "Vá ao Pit para voltar ao Menu";

                }
                if(tempo > 20.5f)
                {
                    textoFimDaCorrida.text = "";
                }
            }
        }
    }
    //========================================================================================================
    private void CarregarHud()
    {
        ponteiroRPMObject = GameObject.Find("PonteiroImg");
        ponteiroRPM = ponteiroRPMObject.GetComponent<RectTransform>();

        textoVelocidadeObject = GameObject.Find("VelocidadeTexto");
        textoVelocidade = textoVelocidadeObject.GetComponent<Text>();

        textMarchaAtualObject = GameObject.Find("TextMarchaAtual");
        textMarchaAtual = textMarchaAtualObject.GetComponent<Text>();
       
        voltaAtualTextObject = GameObject.Find("VoltasText");
        voltaAtualText = voltaAtualTextObject.GetComponent<Text>();

        posicaoAtualTextObject = GameObject.Find("PosicaoText");
        posicaoAtualText = posicaoAtualTextObject.GetComponent<Text>();

        textoFimDaCorridaObject = GameObject.Find("TextFimDaCorrida");
        textoFimDaCorrida = textoFimDaCorridaObject.GetComponent<Text>();
    }

    public int setQuantidadeGanho(int g)
    {
        return this.quantidadeDeDineiroGanho = g;
    }

    public int setQuatidadePerdido(int p)
    {
        return this.quantidadeDeDineiroPerdido = p;
    }

    public int setQuantidadeDeVoltas(int v)
    {
        return this.quantidadeDeVoltas = v;
    }
}
