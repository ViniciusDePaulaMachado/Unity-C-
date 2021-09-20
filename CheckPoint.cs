using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CheckPoint : MonoBehaviour
{
    public List<Volta> voltasCorrida;
    

    private void Start()
    {

        voltasCorrida = new List<Volta>();

        for(int i = 0; i < 5; i++)
        {
            voltasCorrida.Add(new Volta());        
        }

    }

    
    private void OnTriggerEnter(Collider other)
    {
        ContadorDeVoltas a = other.transform.root.GetComponent<ContadorDeVoltas>();
        RegistroDeVoltas(a, a.contadorDeVoltas);

        HUDCarro cHud = other.transform.root.GetComponent<HUDCarro>();
        
        if (cHud != null)
        {
            cHud.ReceberPosicao(RetornaPosicao(a, a.contadorDeVoltas));
            
        }
    }
    
    public bool PassouCheckPont(ContadorDeVoltas c ,int volta)
    {

        for (int i = 0; i < voltasCorrida[volta].carros.Count; i++)
        {
            if(voltasCorrida[volta].carros[i] == c)
            {
                return true;
            }
        }

            return false;
    } 

    public bool RegistroDeVoltas(ContadorDeVoltas carro, int volta)
    {
        voltasCorrida[volta].carros.Add(carro);
        return true;
    }

    public int RetornaPosicao(ContadorDeVoltas carro, int volta)
    {
        
        for (int i = 0; i < voltasCorrida[volta].carros.Count; i++ )
        {

            if (voltasCorrida[volta].carros[i] == carro)
            {   
                return i;
            }
        }
        return -1;
    }

    public class Volta
    {

        public List<ContadorDeVoltas> carros;

        public Volta()
        {
            carros = new List<ContadorDeVoltas>();
        }
    }

}
