// GameManager, Nos ayuda a generar los obstaculos de manera aleatoria dentro de la escena. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_Obstaculos : MonoBehaviour
{

    public GameObject Obstaculo;
    //Generamos 7 obstaculos en posciones aleatorias dentro de los rangos asignados
    void Start()
    {
        for (int i = 0; i < 7; i++)
            Instantiate(Obstaculo, new Vector3(Random.Range(-10, 10), Random.Range(-7, 7), 0), Quaternion.identity);
    }

}
