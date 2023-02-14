//El agente infiltrador:
//Cuando el usuario dé click en la pantalla, el infiltrador se dirigirá hacia la posición del click, con un Arrive (esto es indispensable para poder comprobar al guardia). Si se da click en otra posición mientras se desplaza, debe cambiar de objetivo hacia el click más reciente.
//Una vez que llegue a la posición del click, debe quedarse quieto hasta que se dé otro click
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infiltrador : MonoBehaviour
{

    public GameObject go_Objective; //Objetivo a perseguir o evadir
    public Rigidbody rbPlayer;

    public Rigidbody myRigidbody;
    //Variables para el movimiento
    public float f_MaxSpeed = 4f;
    public float f_ArriveRadius = 2f;
    public float f_MaxForce = 6f;

    public Vector3 TargetPosition;
    
    //Objetivo dee nuestro agente
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
        switch (currentTarget)
        {
            //Se mueve el agente a donde damos clic
            case SteeringTarget.mouse:
                if (Input.GetMouseButtonDown(0))
                {
                    TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                break;

            
        }
    }
    

    private void FixedUpdate()
    {
        //Vector3 TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TargetPosition.z = 0.0f;    //Le pone la z de la camara

        Vector3 v3SteeringForce = Vector3.zero;

            

        //Usamos el arrive para asignarlo al ultimo clic del mouse
        v3SteeringForce = Arrive(TargetPosition);
        //DrawGizmos

        //Aceleración ignora la masa
        myRigidbody.AddForce(v3SteeringForce, ForceMode.Acceleration);

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

    

}
