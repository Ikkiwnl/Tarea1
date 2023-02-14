// Agente que se movera hacia donde se de click, si en su camino encuentra algun obstaculo tiene cambiar su ruta para no chocar, pero mantener su objetivo.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    //Decalracion de variables
    public GameObject go_Objective; //Objetivo a perseguir o evadir
    public Rigidbody rb_Player;
    public GameObject go_Obstaculo;

    public Rigidbody myRigidbody;
    public float f_MaxSpeed = 4f;
    public float f_ArriveRadius = 2f;
    public float f_MaxForce = 6f;

    public Vector3 TargetPosition;
    public Vector3 v3_SteeringForce;

    public bool b_fleeing = false;

    enum SteeringTarget { mouse }
    [SerializeField] SteeringTarget currentTarget = SteeringTarget.mouse;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Hacemos que nuestra pelota se mueva a traves de clics en la pantalla
        switch (currentTarget)
        {
            case SteeringTarget.mouse:
                if (Input.GetMouseButtonDown(0))
                {
                    TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                break;
        }
    }

    //Si hace colision con un objeto activa la funcion flee
    private void OnTriggerEnter(Collider other)
    {
        b_fleeing = true;
        go_Obstaculo = other.gameObject;
    }
    //Si sale de nuestro alrededor desactiva flee
    private void OnTriggerExit(Collider other)
    {

        b_fleeing = false;
    }
    //Hacemos que se mueva nuestra bolita a traves de nuestro arrive y la funcion flee
    private void FixedUpdate()
    {
        
        TargetPosition.z = 0.0f;    //Le ponemos la z de la camara

        v3_SteeringForce = Vector3.zero;


        if (b_fleeing == false)
            v3_SteeringForce = Arrive(TargetPosition);

        if(b_fleeing == true)
        {
            v3_SteeringForce = Arrive(TargetPosition);
            v3_SteeringForce += Flee(go_Obstaculo.transform.position);
        }
            


        //DrawGizmos

        myRigidbody.AddForce(v3_SteeringForce, ForceMode.Acceleration);  //Nuestra aceleracion ignorará a la masa

        
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, f_MaxSpeed); //Clamp para no rebasar una velocidad maxima

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

        Vector3 v3_SteeringForce = v3DesiredVelocity - myRigidbody.velocity;

        
        v3_SteeringForce = Vector3.ClampMagnitude(v3_SteeringForce, f_MaxForce);
        return v3_SteeringForce;
    }

    public Vector3 Flee(Vector3 in_v3TargetPosition)
    {
        //Dirección deseada es punta - cola
        Vector3 v3DesiredDirection = transform.position - in_v3TargetPosition;

        //Normalized para que la magnitud de la fuerza nunca sea mayor que la maxSpeed
        Vector3 v3DesiredVelocity = v3DesiredDirection.normalized * f_MaxSpeed;

        Vector3 v3_SteeringForce = v3DesiredVelocity - myRigidbody.velocity;

        v3_SteeringForce = Vector3.ClampMagnitude(v3_SteeringForce, f_MaxForce);

        return v3_SteeringForce;
    }

    //Creamos la linea roja que nos dice la ruta de nuestra bolita
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, TargetPosition);
    }
}
