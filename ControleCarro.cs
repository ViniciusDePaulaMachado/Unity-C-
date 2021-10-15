using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCarro : MonoBehaviour
{

    Rigidbody carrorigidbody;
    CarroInputs inputCarro;

    [SerializeField]
    private Vector3 centroDeMassa;

    [SerializeField]
    private WheelCollider[] rodascollider;

    [SerializeField]
    private Transform[] rodasMesh;

    [SerializeField]
    private AudioClip somMotor;


    //public AudioSource somDerrapagem;
    [SerializeField]
    private AudioSource veiculoCena;

    [SerializeField]
    private float somPith = 5500;

    //Configuraçao motor
    [SerializeField]
    private AnimationCurve torque = new AnimationCurve();

    [SerializeField]
    private float porenciaHP = 30;

    [SerializeField]
    private float maxRPM = 6500;

    [SerializeField]
    private float minRPM = 1000;


    //Configuracao direcao
    [SerializeField]
    private int anguloDirecao = 30;

    [SerializeField]
    private float velocidadeDoVolante = 10;

    [SerializeField]
    private AnimationCurve suavidadeDirecao;


    //Configuracao freios
    [SerializeField]
    private float freio = 3000;

    [SerializeField]
    private float velocidadeDeFrenagemMaxima = 3;

    //Configuracao transmisao
    [SerializeField]
    private float rpmVoltarMarcha = 4000;

    [SerializeField]
    private float rpmDubirMarcha = 8000;

    [SerializeField]
    private float[] quantidadeMarchas = { 3.36f, 2.09f, 1.47f, 1.15f, 0.98f };

    [SerializeField]
    private float relacaoDiferencial = 3.9f;

    [SerializeField]
    private int velocidadeDaTransmicao = 5;

    //Configuracao propiedades do veiculo
    [System.NonSerialized]
    public float velocidadeKM;
    [System.NonSerialized]
    public int marchaAtual = 0;
    [System.NonSerialized]
    public float rpm;

    private float aceleracao;
    private float forca;
    private float forcaFreio;
    private float somaInputs;

    void Start()
    {
        suavidadeDirecao  = new AnimationCurve(new Keyframe(0,anguloDirecao), new Keyframe(10,anguloDirecao), new Keyframe(50,15), new Keyframe(150,3), new Keyframe(600,1));

        carrorigidbody = GetComponent<Rigidbody>();
        carrorigidbody.centerOfMass = centroDeMassa;
        inputCarro = GetComponent<CarroInputs>();
        veiculoCena.clip = somMotor;

        if(gameObject.CompareTag("Player") == true)
        {
            GameObject.Find("HUB_CARRO");
        }
    }

    void Update()
    {
        Direcao();
        AcelerarFreiar();
    }

    void FixedUpdate()
    {
        
        Motor();
        Transmissao();
        SomCarro();
    }

    public void Direcao()
    {

        for (int i = 0; i < rodascollider.Length; i++)
        {
            somaInputs = inputCarro.getInputHorizontal() + inputCarro.getInputHorizontalTeclado();
        
            if (i < 2)
            {
                rodascollider[i].steerAngle = Mathf.Lerp(rodascollider[i].steerAngle,
                    somaInputs * suavidadeDirecao.Evaluate(velocidadeKM), Time.deltaTime * velocidadeDoVolante);
            }

            rodascollider[i].GetWorldPose(out Vector3 pos, out Quaternion rot);
            rodasMesh[i].position = pos;
            rodasMesh[i].rotation = rot;            
        }
    }

    void AcelerarFreiar()
    {

        // Acelerar        
        if (inputCarro.getInputVertical() > 0 && rpm < maxRPM || inputCarro.getInputVerticalTeclado() > 0 && rpm < maxRPM)
        {
             rodascollider[0].motorTorque = forca;
             rodascollider[1].motorTorque = forca;
        }
        else
        {
            rodascollider[0].motorTorque = 0;
            rodascollider[1].motorTorque = 0;
        }
        
        //Frear
        forcaFreio = Mathf.Lerp(forcaFreio,Mathf.Abs(inputCarro.getInputVertical() + inputCarro.getInputVerticalTeclado()) * freio, Time.deltaTime *
            velocidadeDeFrenagemMaxima);

        if (inputCarro.getInputVertical() < 0 || inputCarro.getInputVerticalTeclado() < 0)
        {
            rodascollider[0].brakeTorque = forcaFreio;
            rodascollider[1].brakeTorque = forcaFreio;
            rodascollider[2].brakeTorque = forcaFreio / 2;
            rodascollider[3].brakeTorque = forcaFreio / 2;                
        }
        else
        {
            forcaFreio = 0;
            rodascollider[0].brakeTorque = 0;
            rodascollider[1].brakeTorque = 0;
            rodascollider[2].brakeTorque = 0;
            rodascollider[3].brakeTorque = 0;
        }

        // dar Ré
        if(inputCarro.getDarRe() < 0)
        {
            if(marchaAtual < 1)
            {
                Debug.Log("Ré");
                rodascollider[0].motorTorque = -forca;
                rodascollider[1].motorTorque = -forca;
            }
        }
    }

    void Motor()
    {
        forca = torque.Evaluate(rpm) * (relacaoDiferencial + quantidadeMarchas[marchaAtual]);

        velocidadeKM = carrorigidbody.velocity.magnitude * 3.6f;

        rpm = Mathf.Lerp(rpm,velocidadeKM * (quantidadeMarchas[marchaAtual] * (relacaoDiferencial * 8)), Time.deltaTime * velocidadeDaTransmicao);

        if (rpm > 4000 && rpm < 6000 && forca < 1000 )
        {
            forca += porenciaHP * 1f;
        }
        
        if (rpm < minRPM)
        {
            rpm = 1000;
        }
    }

    void Transmissao()
    {
        
        if (rpm >= rpmDubirMarcha)
        {
            marchaAtual++;

            if (marchaAtual == quantidadeMarchas.Length)
            {
                marchaAtual--;
            }
        }

        if (rpm < rpmVoltarMarcha)
        {
            marchaAtual--;

            if (marchaAtual < 0f)
            {
                marchaAtual = 0;
            }
        } 
    }

    public void SubirMarcha()
    {
        if (marchaAtual < quantidadeMarchas.Length)
        {
            marchaAtual++;

            if (marchaAtual == quantidadeMarchas.Length)
            {
                marchaAtual--;
            }
        }
    }

    public void DescerMarcha()
    {
        if (marchaAtual > 0)
            marchaAtual--;
    }

    void SomCarro()
    {
        veiculoCena.pitch = rpm / somPith;

        /* "Esse fun��o est� funcionando, s� falta eu achar um audio bom de derrapagem para usar ela" 
        if(velocidadeKM > 40f)
        {
            float valorAngulo = Vector3.Angle(transform.forward, carrorigidbody.velocity);
            float angulo = (valorAngulo / 10f);
            somDerrapagem.volume = Mathf.Clamp(angulo, 0f, 1f);
        }*/
    }

    public float getMaxRpm()
    {
        return this.maxRPM;
    }
}


