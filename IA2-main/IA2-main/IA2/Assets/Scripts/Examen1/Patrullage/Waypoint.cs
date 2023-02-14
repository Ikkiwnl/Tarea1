/*El agente se moverá entre N puntos en la escena (Waypoints), de manera cíclica.
Los waypoints en la escena deben poderse poner y quitar durante el juego, y mostrar el orden en que el agente los visitará.
Poder poner y quitar waypoints con el mouse.
Al llegar cerca del waypoint actual, el agente debe cambiar al siguiente en el orden.
Al visitar el "último" waypoint, debe dirigirse hacia el primero, y repetir todo el ciclo.
Si ya no hay waypoints, el agente debe frenar hasta quedar quieto.*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{



    //Referenciamos nuestro script
    public Patrullage sPatrullage;
    void Start()
    {
        //Se busca nuestro agente patrullaje en la escena
        sPatrullage = FindObjectOfType<Patrullage>();
        //Se establece posicion de aparicion
        transform.position = new Vector3(transform.position.x,transform.position.y,0);
        //Se agrega la posicion del objeto a la lista de waypoints
        sPatrullage.Waypoints.Add(transform.position);

    }
    //Detecta si damos click en el objeto
    private void OnMouseDown()
    {
        sPatrullage.Waypoints.Remove(transform.position);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // Si la posicion corresponde a la del waypoint en busqueda
            if (sPatrullage.Waypoints[sPatrullage.i_TargetWaypoint] == transform.position)
            {
                //Mandamos al siguiente punto
                sPatrullage.i_TargetWaypoint= sPatrullage.i_TargetWaypoint+1;
            }
                
        }
    }


}
