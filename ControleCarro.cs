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

    

    public float torque = 1700;
    public float maxRPM = 6500;
    public float minRPM = 1000;
    public float rpmVoltarMarcha = 4000;
    public float[] quantidadeMarchas = {3.36f,2.09f ,1.47f ,1.15f ,0.98f};
    public float relacaoDiferencial = 3.9f;
    public float anguloDasRodas;
    public float freio;
    public float friccaoMotor;

    float velocidadeKM;
    int marchaAtual = 0;
    float rpm;
    float aceleracao;
    
    void direcao()
    {
        Vector3 pos;
        Quaternion rot;

        for (int i = 0; i < rodascollider.Length; i++)
        {

            if (i < 2)
            {
                rodascollider[i].steerAngle = Mathf.Lerp(rodascollider[i].steerAngle, anguloDasRodas * Input.GetAxis("Horizontal"), Time.deltaTime * 10);
            }

            rodascollider[i].GetWorldPose(out pos, out rot);
            rodasMesh[i].position = pos;
            rodasMesh[i].rotation = rot;
        }
        
    }
    void motor()
    {
        rodascollider[0].motorTorque = Input.GetAxis("Vertical"); * torque;
        rodascollider[1].motorTorque = Input.GetAxis("Vertical"); * torque;
    }
    void transmissao()
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

        if (rpm < minRPM)
        {
            rpm = 1000;
        }
        
    }

    void Irinel()
    {
        velocidadeKM = carrorigidbody.velocity.magnitude * 3.6f;
        rpm = velocidadeKM * quantidadeMarchas[marchaAtual] * (relacaoDiferencial * 8);
        veiculoCena.pitch = rpm / somPith;
    }

    void Update()
    {
        direcao();
        motor();
    }

    void FixedUpdate()
    {
        transmissao();
        Irinel();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 500, 32), Mathf.Round(velocidadeKM) + " KM/h");
        GUI.Label(new Rect(20, 40, 500, 32), (marchaAtual + 1) + "° Marcha");
        GUI.Label(new Rect(20, 60, 500, 32), Mathf.Round(rpm) + "° RPM");
    }
}
