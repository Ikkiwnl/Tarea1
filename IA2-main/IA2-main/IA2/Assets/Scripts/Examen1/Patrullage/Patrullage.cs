//3.Patrullage(Opcional, 35 puntos): Un solo agente.

//El agente se moverá entre N puntos en la escena (Waypoints), de manera cíclica.
//Los waypoints en la escena deben poderse poner y quitar durante el juego, y mostrar el orden en que el agente los visitará.
//Poder poner y quitar waypoints con el mouse.
//Al llegar cerca del waypoint actual, el agente debe cambiar al siguiente en el orden.
//Al visitar el "último" waypoint, debe dirigirse hacia el primero, y repetir todo el ciclo.
//Si ya no hay waypoints, el agente debe frenar hasta quedar quieto.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrullage : MonoBehaviour
{
    //Declaramos nuestras variales que usaremos
    public GameObject go_Objective; //Objetivo a perseguir o evadir
    public Rigidbody rbPlayer;
    public GameObject go_Waypoint;

    public Rigidbody myRigidbody;

    public float f_MaxSpeed = 4f;
    public float f_ArriveRadius = 2f;
    public float f_MaxForce = 6f;
    private int i_ActualWaypoint;
    public int i_TargetWaypoint=0;
    public int i_Length;
    public Vector3 TargetPosition;

    public List<Vector3> Waypoints = new List<Vector3>();


    enum SteeringTarget { Waypoint }
    [SerializeField] SteeringTarget currentTarget = SteeringTarget.Waypoint;

    // Start is called before the first frame update
    //Creamos nuestro waypoint inicial al cual se movera nuestro agente
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();

        for(int i=0;i<2;i++)
            Instantiate(go_Waypoint, new Vector3(Random.Range(-6,6), Random.Range(-3, 3),0), Quaternion.identity);

    }

    // Update is called once per frame
    //Creamos los wayppoints en la escena a traves de clics en la pantalla
    void Update()
    {
        //Vemos cuantos objetos hay en lista
        i_Length = Waypoints.Count;
        //Con click derecho se agrega un waypoint
        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(go_Waypoint, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        }
        //Si no hay waypoints el agente se detiene
        if (i_Length == 0)
        {
            f_MaxSpeed = 0;
        }
        else
            f_MaxSpeed = 6;

        switch (currentTarget)
        {
            //Gestion de waypoints
            case SteeringTarget.Waypoint:
                        Patrullagef();

                    break;
        }


    }



    private void FixedUpdate()
    {
        TargetPosition.z = 0.0f;    //Le pone la z de la camara

        Vector3 v3SteeringForce = Vector3.zero;


        v3SteeringForce = Arrive(TargetPosition);
        //DrawGizmos

        myRigidbody.AddForce(v3SteeringForce, ForceMode.Acceleration);  //Aceleración ignora la masa

        //Clamp es para que no exceda la velocidad máxima
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, f_MaxSpeed);

    }

    Vector3 Arrive(Vector3 in_v3TargetPosition)
    {
        
        Vector3 v3Diff = in_v3TargetPosition - transform.position;
        float fDistance = v3Diff.magnitude;
        float fDesiredMagnitude = f_MaxSpeed;

        if (fDistance < f_ArriveRadius)
        {

            fDesiredMagnitude = Mathf.InverseLerp(0.0f, f_ArriveRadius, fDistance);
        }


        Vector3 v3DesiredVelocity = v3Diff.normalized * fDesiredMagnitude;

        Vector3 v3SteeringForce = v3DesiredVelocity - myRigidbody.velocity;



        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, f_MaxForce);
        return v3SteeringForce;
    }
    //Reinicia agente si llegamos al ultimo punto
    private void Patrullagef()
    {
        if(i_TargetWaypoint == i_Length)
            i_TargetWaypoint = 0;

        TargetPosition = Waypoints[i_TargetWaypoint];
    }
}
