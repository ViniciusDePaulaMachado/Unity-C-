using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleIA : MonoBehaviour
{
    CriadorDeWayPoint[] listaWayPoints;

    ControleCarro carro;
    CarroInputs carroInput;

    [SerializeField]
    private GameObject player;

    private int atual = 0;
    private float virar = 0;
    private float agrecividade;
    private float playerPosX;
    private float playerPosZ;
    private bool ativarCerebro = false;

    private void Start()
    {
        carro = GetComponent<ControleCarro>();

        carroInput = GetComponent<CarroInputs>();

        player = GameObject.FindWithTag("Player");

        GameObject parentWayPoints = GameObject.Find("WayPointes");

        listaWayPoints = parentWayPoints.GetComponentsInChildren<CriadorDeWayPoint>();

        agrecividade = Random.Range(-25f, 1f);
    }

    private void FixedUpdate()
    {
        Direcao();
        Cerebro();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") == true)
        {
            Debug.Log("Bateu no Player");
            ativarCerebro = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("desbateu no Player");
        ativarCerebro = false;
    }

    private void Cerebro()
    {
        playerPosX = Mathf.Abs(carro.transform.position.x - player.transform.position.x);

        playerPosZ = Mathf.Abs(carro.transform.position.z - player.transform.position.z);

        if(ativarCerebro == true)
        {
            carroInput.setImputVertical(-1);

            if (playerPosX < 2.5f)
            {
                //Debug.Log("X: " + playerPosX + " z: " + playerPosZ);
                Debug.Log("cerebro ativado");
                
            }
        }           
    }
    private void Direcao()
    {
        //Acelerar/Frear

        if (listaWayPoints[atual].velocidadeIA + agrecividade > carro.velocidadeKM)
        {

            carroInput.setImputVertical(+1);
        }
        else
        {
            carroInput.setImputVertical(-1);
        }
        //Fazer curva

        if (Vector3.Distance(transform.position, listaWayPoints[atual].transform.position) < 10f ||
           Vector3.Distance(transform.position, listaWayPoints[atual].transform.position) < -20f)
        {
            atual++;

            if (atual == listaWayPoints.Length)
            {
                atual = 0;
            }
        }

        Vector3 dir = transform.InverseTransformPoint(new Vector3(listaWayPoints[atual].transform.position.x,
        transform.position.y, listaWayPoints[atual].transform.position.z));

        virar = Mathf.Clamp((dir.x / dir.magnitude) * 10f, -1f, 1f);

        carroInput.setInputHorizontal(virar);
    }
}
