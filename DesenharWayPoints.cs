using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesenharWayPoints : MonoBehaviour
{

    public GameObject wayPoint;
    ControleCarro carro;
    bool criar = false;

    private void Start()
    {
        carro = GetComponent<ControleCarro>(); 
    }

    IEnumerator colocar()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject way = Instantiate(wayPoint, transform.position, Quaternion.identity);
        way.GetComponent<CriadorDeWayPoint>().velocidadeIA = carro.velocidadeKM;
        if (criar)
        {
            StartCoroutine(colocar());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            criar = !criar;

            if (criar)
            {
                StartCoroutine(colocar());
            }
        }
    }
}
