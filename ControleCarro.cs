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
    public AudioSource veiculoCena;
    public float somPith = 5500;

    
    // motor
    public float torque;
    public float torqueAdicionalAltaVelocidade = 900;
    public float maxRPM = 6500;
    public float minRPM = 1000;
    //public float velocidadeDeAceleracaoMaxima = 5;
    //public float friccaoMotor;

    // direção
    public float anguloDasRodas;
    public float velocidadeDoVolante = 10;
    public AnimationCurve suavidadeDireção;

    // freios
    public float freio;
    public float velocidadeDeFrenagemMaxima = 3;
    // transmisão
    public float rpmVoltarMarcha = 4000;
    public float[] quantidadeMarchas = { 3.36f, 2.09f, 1.47f, 1.15f, 0.98f };
    public float relacaoDiferencial = 3.9f;

    // propiedades do veiculo
    float velocidadeKM;
    int marchaAtual = 0;
    float rpm;
    float aceleracao;
    float forca;
    float forcaFreio;
    
    void Direcao()
    {
        Vector3 pos;
        Quaternion rot;

        for (int i = 0; i < rodascollider.Length; i++)
        {

            if (i < 2)
            {
                //rodascollider[i].steerAngle = Mathf.Lerp(rodascollider[i].steerAngle, anguloDasRodas * Input.GetAxis("Horizontal"), Time.deltaTime * velocidadeDoVolante);
                rodascollider[i].steerAngle = Input.GetAxis("Horizontal") * suavidadeDireção.Evaluate(velocidadeKM);
            }

            rodascollider[i].GetWorldPose(out pos, out rot);
            rodasMesh[i].position = pos;
            rodasMesh[i].rotation = rot;
            
        }
        
    }
    void AcelerarEFrear()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            rodascollider[0].motorTorque = forca;
            rodascollider[1].motorTorque = forca;
        } else
        {
            rodascollider[0].motorTorque = 0;
            rodascollider[1].motorTorque = 0;

        }
        if(Input.GetAxis("Vertical") < 0)
        {
            forcaFreio = Mathf.Lerp(forcaFreio,freio , Time.deltaTime * velocidadeDeFrenagemMaxima);

            rodascollider[0].brakeTorque = forcaFreio;
            rodascollider[1].brakeTorque = forcaFreio;
            rodascollider[2].brakeTorque = forcaFreio;
            rodascollider[3].brakeTorque = forcaFreio;
        }
        else
        {
            forcaFreio = 0;
            rodascollider[0].brakeTorque = 0;
            rodascollider[1].brakeTorque = 0;
            rodascollider[2].brakeTorque = 0;
            rodascollider[3].brakeTorque = 0;

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
    }

    void Start()
    {
        carrorigidbody = GetComponent<Rigidbody>();
        carrorigidbody.centerOfMass = centroDeMassa;
        veiculoCena.clip = somMotor;
    }

    void Update()
    {
        Direcao();
        AcelerarEFrear();
        Motor();

    }

    void FixedUpdate()
    {
        Transmissao();
        SomCarro();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 500, 32), Mathf.Round(velocidadeKM) + " KM/h");
        GUI.Label(new Rect(20, 40, 500, 32), (marchaAtual + 1) + "° Marcha");
        GUI.Label(new Rect(20, 60, 500, 32), Mathf.Round(rpm) + " RPM");
        GUI.Label(new Rect(20, 80, 500, 32), forca + " Torque");
        GUI.Label(new Rect(20, 100, 500, 32), forcaFreio + " freio");
    }
}
