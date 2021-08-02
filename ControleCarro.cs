using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCarro : MonoBehaviour
{

    Rigidbody carrorigidbody;
    public Vector3 centroDeMassa;
    public WheelCollider[] rodascollider;
    public Transform[] rodasMesh;

    public AudioClip somMotor;
    //public AudioSource somDerrapagem;
    public AudioSource veiculoCena;
    public float somPith = 5500;

    // motor
    public float torque;
    public float torqueAdicionalAltaVelocidade = 30;
    public float maxRPM = 6500;
    public float minRPM = 1000;
    
    // direção
    public float anguloDasRodas = 30;
    public float velocidadeDoVolante = 10;
    public AnimationCurve suavidadeDireção;

    // freios
    public float freio = 3000;
    public float velocidadeDeFrenagemMaxima = 3;

    // transmisão
    public float rpmVoltarMarcha = 4000;
    public float[] quantidadeMarchas = { 3.36f, 2.09f, 1.47f, 1.15f, 0.98f };
    public float relacaoDiferencial = 3.9f;

    // propiedades do veiculo
    public float velocidadeKM;
    public int marchaAtual = 0;
    public float rpm;
    float aceleracao;
    float forca;
    float forcaFreio;
    float rpmTeste = 0f;

    //inputBotoes;
    public float inputVertical;
    public float InputVerticallTeclado;
    public float inputHorizontal;
    public float InputHorizontalTeclado;
    float somaInputs;
    
    float acelerou;
    float freiou;
    public float deuRe;

    public void Direcao()
    {

        for (int i = 0; i < rodascollider.Length; i++)
        {
            somaInputs = inputHorizontal + InputHorizontalTeclado;
        
            if (i < 2)
            {
                rodascollider[i].steerAngle = Mathf.Lerp(rodascollider[i].steerAngle,
                    somaInputs * suavidadeDireção.Evaluate(velocidadeKM), Time.deltaTime * velocidadeDoVolante);
            }

            rodascollider[i].GetWorldPose(out Vector3 pos, out Quaternion rot);
            rodasMesh[i].position = pos;
            rodasMesh[i].rotation = rot;            
        }
    }

    void AcelerarFreiar()
    {
                
        if (inputVertical > 0 || InputVerticallTeclado > 0)
        {
            rodascollider[0].motorTorque = forca;
            rodascollider[1].motorTorque = forca;
        }
        else
        {
            rodascollider[0].motorTorque = 0;
            rodascollider[1].motorTorque = 0;
        }
        
        forcaFreio = Mathf.Lerp(forcaFreio,Mathf.Abs(inputVertical + InputVerticallTeclado) * freio, Time.deltaTime * velocidadeDeFrenagemMaxima);

        if (inputVertical < 0 || InputVerticallTeclado < 0)
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

        if(deuRe > 0)
        {
            rodascollider[0].motorTorque = -500;
            rodascollider[1].motorTorque = -500;
        }

        if (velocidadeKM == 0)
        {
            forcaFreio = 0;
        }
    }

    void Motor()
    {
        forca = torque * (relacaoDiferencial + quantidadeMarchas[marchaAtual]);

        velocidadeKM = carrorigidbody.velocity.magnitude * 3.6f;
        rpm = velocidadeKM * quantidadeMarchas[marchaAtual] * (relacaoDiferencial * 8);

        if(forca < 1000)
        {
            forca += torqueAdicionalAltaVelocidade * 10f;

        }
        
        if (rpm < minRPM)
        {
            rpm = 1000;
        }
    }

    void Transmissao()
    {
        
        if (rpm > maxRPM)
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

    void SomCarro()
    {
        veiculoCena.pitch = rpm / somPith;

        /* "Esse função está funcionando, só falta eu achar um audio bom de derrapagem para usar ela" 
        if(velocidadeKM > 40f)
        {
            float valorAngulo = Vector3.Angle(transform.forward, carrorigidbody.velocity);
            float angulo = (valorAngulo / 10f);
            somDerrapagem.volume = Mathf.Clamp(angulo, 0f, 1f);
        }*/
    }

    void Start()
    {
        carrorigidbody = GetComponent<Rigidbody>();
        carrorigidbody.centerOfMass = centroDeMassa;

        veiculoCena.clip = somMotor;
    }

    public void SubirMarcha()
    {
        if (marchaAtual < quantidadeMarchas.Length)
            marchaAtual++;
    }

    public void DescerMarcha()
    {
        if (marchaAtual > 0)
            marchaAtual--;
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

   private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 500, 32), rpmTeste + " Rpm");
        /*GUI.Label(new Rect(20, 40, 500, 32), (marchaAtual + 1) + "° Marcha");
        GUI.Label(new Rect(20, 60, 500, 32), Mathf.Round(rpm) + " RPM");
        GUI.Label(new Rect(20, 80, 500, 32), forca + " Torque");
        GUI.Label(new Rect(20, 100, 500, 32), forcaFreio + " freio"); */
    }
    
}
