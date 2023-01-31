using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSteeringBehaviors : MonoBehaviour
{
    //Vector2 currentPosition = Vector2.zero;
    //Vector2 currentVelocity = Vector2.zero;

    public Rigidbody myRigidbody = null;

    //Vector2 TargetPosition = Vector2.zero;
    
    public float fmaxSpeed = 1.0f;
    public float fmaxForce = 0.5f;
    //Cuanto fixedDeltatime en el futuro usara para las funciones Pursuit y Evade
    public float fPredictionSteps = 10f;

    public enum SteeringBehavior { Seek, Flee, Pursue, Evade, Arrive}
    public SteeringBehavior currentBehavior = SteeringBehavior.Seek;

    GameObject PursuitTarget = null; //objetivo a perseguir o evadir segun sea el caso
    Rigidbody PursuitTargetRB = null;// el rigidbody de a quien estamos persiguiendo o evadiendo

    void Start()
    {
        //Usamos la funcion GetComponent para obtener el RigidBody de este agente
        //y asi poder aplicarle las fuerzas resultantes de los steering behaviors
        if (myRigidbody == null) 
        {
            Debug.LogError("No Rigidbody component found for this agent steering behavior");
            return;
        }

    }


    private void OnValidate()
    {
        if (currentBehavior == SteeringBehavior.Pursue ||
            currentBehavior == SteeringBehavior.Evade)
        {
            //Buscamos y asignamos un target rigidbody
            SetPursueTarget();
        }
    }

    public Vector3 Seek(Vector3 in_v3TargetPosition)
    {
        //Direccion deseada es punta ("a donde quiero llegar") - cola (donde estoy ahora)
        Vector3 v3DesiredDirection = in_v3TargetPosition - transform.position;
        Vector3 v3DesiredVelocity = v3DesiredDirection.normalized * fmaxSpeed;
        //Hacemos normalized*speed para que la magnitud de la fuuerza nunca sea mayo a la maxspeed
        Vector3 v3SteeringForce = v3DesiredVelocity - myRigidbody.velocity;
        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, fmaxForce);
        return v3SteeringForce;
    }

    Vector3 Flee(Vector3 in_v3TargetPosition)
    {
        //Dirección deseada es punta "a donde quiero llegar" - cola "donde estoy ahorita"
        Vector3 v3DesiredDirection = -1.0f*(in_v3TargetPosition - transform.position); ;

        //Steering Force
        Vector3 v3DesiredVelocity = v3DesiredDirection.normalized * fmaxSpeed;

        Vector3 v3SteeringForce = v3DesiredVelocity - myRigidbody.velocity;

        return v3SteeringForce;
    }

    void SetPursueTarget()

    {
        Debug.Log("entre a setPursueTarget");
        //Ahora, buscamos un GameObject en la escena que tenga el nombre que nosotros
        //designemos y el cual debera tener un rigidbody para poder aplicarle las funciones
        //pursuit y evade
        PursuitTarget = GameObject.Find("PursuitTarget");
        if(PursuitTarget == null )
        {
            //Entonces no encontró dicho objeto, es un error
            Debug.LogError("No pursuit target gameobject found in scene");
            return;
        }

        PursuitTargetRB = PursuitTarget.GetComponent<Rigidbody>();
        if (PursuitTargetRB == null)
        {
            Debug.LogError("No rigidbody present on gameobjet pursuit target but it should");
            return;
        }

    }

    Vector3 Pursuit(Rigidbody in_target)
    {
        //Hacemos copia de la posicion del objetivo para no modificarla directamente
        Vector3 v3TargetPosition = in_target.transform.position;

        //Añadimos a dicha posicion el movimiento equivalente a fPredicitionSteps
        //-veces el deltaTime. Es decir n-cuadros en el futuro
        v3TargetPosition += in_target.velocity * Time.fixedDeltaTime * fPredictionSteps;
        return Seek(v3TargetPosition);
    }


    void Update()
    {


        //Vector normal es un vector al que cuya magnitud es 1
        //Vector (1,2,5)
        //1*1, 2*2, 5*5 (magnitud del vector) = 30

        //Ctrl + k + c es comentar todo lo seleccionado, y ctrl +k + u, es descomentar todo lo seleccionado
    }

    private void FixedUpdate()
    {
        //Esto lo usabamos para hacer que el mouse fuera la posicion target pero ya no lo usaremos
        Vector3 TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TargetPosition.z =0.0f; // Si no hacemos esto, tendrá la z de la camara

        Vector3 v3SteeringForce = Vector3.zero;
        switch (currentBehavior)
        {
            case SteeringBehavior.Seek:
                v3SteeringForce = Seek(TargetPosition);
                break;
            case SteeringBehavior.Flee:
                v3SteeringForce = Flee(TargetPosition);
                break;
            case SteeringBehavior.Pursue:
                v3SteeringForce = Pursuit(PursuitTargetRB);
                break;
        }
        //Idealmente usariamos ForcecMode de Force para tomar en cuenta la maasa del objeto
        //Aqui ya no usamos el deltaTime por que viene integrado como funcion del addForce
        myRigidbody.AddForce(v3SteeringForce, ForceMode.Force);

        //Hacemos un Clamp para que no exceda la velocidad maxima que pueda tener el agente
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, fmaxSpeed);
        //Ya no es necesario llamar estas lineas, por que el motor de fisicas lo hace por nosotros
    }
}
