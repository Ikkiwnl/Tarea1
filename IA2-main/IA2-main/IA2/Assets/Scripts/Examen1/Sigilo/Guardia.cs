//Agente Guardia:
//Guardia en estado normal:
//Debe estar en una posición fija.
//Debe tener un cono de visión de área limitada, en la dirección en la que está "viendo".
//Debe rotar cada cierto tiempo. p.e. cada 5 segundos. La rotación puede ser en ángulos de 45, 90 u otros grados. Pero si se les ocurre algo mejor, inténtenlo.
//Si el Infiltrador entra a su área de visión, el guardia cambia a estado de alerta.

//Guardia en estado de alerta.
//Su cono de visión se vuelve un poco más amplio, pero no más largo/profundo.
//Después de un pequeño periodo de tiempo, se acercará a la última posición donde "vió" al infiltrador, con un Arrive.
//Si al llegar ahí ya no ha visto al infiltrador, volverá a su posición inicial, y al llegar ahí cambiará nuevamente a estado normal.
//Si, al contrario, mientras está en alerta, el infiltrador pasa 1 segundo (tiempo total, no contínuo, es decir, por ejemplo, podrían ser 0.5 segundos primero, y luego otros 0.5 segundos mientras el guardia se mueve durante el estado de alerta) dentro del área de visión del guardia, el guardia pasará al estado de Ataque.

//Guardia en estado de ataque.
//Utilizará el steering behavior de pursuit hacia el infiltrador durante 5 segundos.
//Si toca al infiltrador, lo destruye (deben poder re-aparecer al infiltrador después de que lo destruyan!), debe volver a su posición inicial y al hacerlo, volver al estado Normal.
//Si se acaban los 5 segundos y no destruyó al infiltrador, debe volver a su posición inicial y al hacerlo, volver al estado Normal.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardia : MonoBehaviour
{
    // Variables para el tipo componentes que se usaran 
    [Header("GO&Transforms")]
    public Rigidbody myRigidbody;
    public Rigidbody objRigidbody;
    public Transform t_player;
    public Transform t_head;
    private Vector2 v_playerVector;

    //Variables para dar la rotacion a nuestro guardia
    [Header("Rotation")]
    [Range(0f, 360f)]
    public float f_visionAngleBar = 30f;
    public float f_visionAngle = 30f;
    public float f_visionDistance = 10f;
    public float f_rotationAngle=45.0f;
    public float f_rotationAux=0;

    //Declaramos las variables para los tiempos de ataque y guardia de nuestro personaje
    [Header("Timers")]
    public float f_rotateTime = 0;
    public float f_alertTime = 0;
    public float f_atackTime = 0;

    //Declaramos las variables para el movimiento de el guardia
    [Header("Movement")]
    public float f_MaxSpeed = 4f;
    public float f_ArriveRadius = 2f;
    public float f_MaxForce = 6f;
    public float f_PredictionsSteps = 2f;
    public Vector3 TargetPosition;

    enum SteeringTarget {  player }
    [SerializeField] SteeringTarget currentTarget = SteeringTarget.player;



    private bool b_once = true;
    private bool b_detected = false;
    private bool b_alertState;
    public bool b_atackState;
    public bool bUseArrive;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        f_visionAngle = f_visionAngleBar;

    }

    private void Update()
    {
        //Se inicia el contador para los giros
        f_rotateTime += Time.deltaTime;

        //Si ve al objetivo auenta el angulo de vision si no se mantiene normal
        f_visionAngleBar = b_detected ? f_visionAngle + 20 : f_visionAngle;

        switch (currentTarget)
        {

            case SteeringTarget.player:

                break;
        }
        //Si pasa del tiempo establecido vuelve a su estado normal
        if (f_rotateTime >= 5)
        {
            Normal_State();
        }

        
        //Si detecta al infiltrado entra en modo alerta y cuenta cuanto tiempo esta detectado
        if (b_detected == true)
        {
            Alert_State();
            f_alertTime += Time.deltaTime;

        }
        else
            b_alertState = false;

        //Si lo detecta mas de un segundo empieza la persecucion y entra el modo ataque
        if(f_alertTime >= 1)
        {
            b_atackState = true;
            f_atackTime += Time.deltaTime;
        }
        
        //Punta cola para obtener la posicion
        v_playerVector = t_player.position - t_head.position;

        //Sirve para ddetectar Si entro el inflitrador dentro de su vista
        if (Vector3.Angle(v_playerVector.normalized, t_head.right) < f_visionAngleBar * 0.5f)
        {
            if(v_playerVector.magnitude < f_visionDistance)
                b_detected=true;
            
        }
        else
            b_detected = false;
        
        //Si lo sigue por mas de 6 segundos y no lo destruye termina la persecucion
        if (f_atackTime >= 6)
        {
            Atack_State();
        }
        //Respawnea al infiltrador al presionar la tecla espacio
        if (Input.GetKeyDown("space"))
        {
            objRigidbody.gameObject.SetActive(true);
        }


    }
    //Si chocan entra en esta funcion
    private void OnCollisionEnter(Collision collision)
    {

        //Si esta en persecucion lo desactiva, tambien desactiva al infiltrador y regresa al guardia al punto 0,0
        if(b_atackState == true)
        {
            objRigidbody.gameObject.SetActive(false);
            b_atackState=false;
        }
    }

    private void FixedUpdate()
    {

        if (b_alertState == true)
        {
            TargetPosition = t_player.position;
            //Desactiva giro
            f_rotateTime = 0;

        }  

        TargetPosition.z = 0.0f;    //Le pone la z de la camara

        Vector3 v3SteeringForce = Vector3.zero;

        v3SteeringForce = b_atackState ? Pursuit(objRigidbody) :Arrive(TargetPosition);
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

    private float ArriveFunction(Vector3 in_v3DesiredDirection)
    {
        float fDistance = in_v3DesiredDirection.magnitude;
        float fDesiredMagnitude = f_MaxSpeed;

        if (fDistance < f_ArriveRadius)
        {
            fDesiredMagnitude = Mathf.InverseLerp(0f, f_ArriveRadius, fDistance);
        }

        return fDesiredMagnitude;
    }
    public Vector3 Seek(Vector3 in_v3TargetPosition)
    {
        //Dirección deseada es punta "a donde quiero llegar" - cola "donde estoy ahorita"
        Vector3 v3DesiredDirection = in_v3TargetPosition - transform.position;
        float fDesiredMagnitude = f_MaxSpeed;

        if (bUseArrive)
        {
            fDesiredMagnitude = ArriveFunction(v3DesiredDirection);
        }

        Vector3 v3DesiredVelocity = v3DesiredDirection.normalized * f_MaxSpeed;

        Vector3 v3SteeringForce = v3DesiredVelocity - myRigidbody.velocity;

        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, f_MaxForce);

        return v3SteeringForce;
    }
    Vector3 Pursuit(Rigidbody in_target)
    {
        Vector3 v3TargetPosition = in_target.transform.position;
        v3TargetPosition += in_target.velocity * Time.fixedDeltaTime * f_PredictionsSteps;

        return Seek(v3TargetPosition);
    }
    //Mostramos el gizmos
    private void OnDrawGizmos()
    {
        if(b_atackState==false)
        {
            if (f_visionAngle <= 0f) return;

        float f_halfVisionAngle = f_visionAngleBar * 0.5f;

        Vector2 v_p1, v_p2;

        v_p1 = PointForAngle(f_halfVisionAngle, f_visionDistance);
        v_p2 = PointForAngle(-f_halfVisionAngle, f_visionDistance);

        Gizmos.color = b_detected ? Color.red: Color.green;
        Gizmos.DrawLine(t_head.position, (Vector2)t_head.position + v_p1);
        Gizmos.DrawLine(t_head.position, (Vector2)t_head.position + v_p2);
        Gizmos.DrawLine((Vector2)t_head.position + v_p1,(Vector2)t_head.position + v_p2);
        }
        

        
    }
    //Creacion para el angulo de vision
    Vector3 PointForAngle(float angle, float distance)
    {
        return t_head.TransformDirection(
            new Vector2(Mathf.Cos(angle*Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)))
            *distance;


    }
    //Sirve para rotar al guardia
    private void Normal_State()
    {

        // Guardar valor de rotacion inicial
        if (b_once == true)
        {
            f_rotationAux = transform.eulerAngles.z;
            b_once = false;
        }
        // Rotar poco a poco de posicion inicial a posicion inicial + la rotacion adicional
        transform.rotation = Quaternion.Euler(0,0,Mathf.Lerp(f_rotationAux, f_rotationAux + f_rotationAngle, f_rotateTime-5.0f));

        if (f_rotateTime >= 6f)
        {
            f_rotateTime = 0;
            b_once = true;
        }
    }
    // Sirve para cuando esta mas de 1 segundo detectado nuestro inflitrador
    private void Alert_State()
    {
        b_alertState = true;
    }

    private void Atack_State()
    {
       
     b_atackState = false;
        TargetPosition = Vector3.zero;
        f_atackTime = 0;
        f_alertTime = 0;
        
    }
    
}
