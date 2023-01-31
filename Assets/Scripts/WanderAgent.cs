using UnityEngine;

public class WanderAgent : MonoBehaviour
{
    public float f_RadioCirculo = 1;
    public float f_ProbVuelta = 0.05f;
    public float f_MaxRadio = 5;

    public float f_Masa = 15;
    public float f_VelMax = 3;
    public float f_FuerzaMax = 15;

    private Vector3 v3velocidad;
    private Vector3 v3FuerzWander;
    private Vector3 v3objetivo;

    private void Start()
    {
        v3velocidad = Random.onUnitSphere;
        v3FuerzWander = GetRandomFuerzaWander();
    }

    private void Update()
    {
        var velDeseada = GetWanderFuerza();
        velDeseada = velDeseada.normalized * f_VelMax;

        var FuerzaDireccion = velDeseada - v3velocidad;
        FuerzaDireccion = Vector3.ClampMagnitude(FuerzaDireccion, f_FuerzaMax);
        FuerzaDireccion /= f_Masa;

        v3velocidad = Vector3.ClampMagnitude(v3velocidad + FuerzaDireccion, f_VelMax);
        transform.position += v3velocidad * Time.deltaTime;
        transform.forward = v3velocidad.normalized;
    }

    private Vector3 GetWanderFuerza()
    {
        if (transform.position.magnitude > f_MaxRadio)
        {
            var direccionCentro = (v3objetivo - transform.position).normalized;
            v3FuerzWander = v3velocidad.normalized + direccionCentro;
        }
        else if (Random.value < f_ProbVuelta)
        {
            v3FuerzWander = GetRandomFuerzaWander();
        }

        return v3FuerzWander;
    }

    private Vector3 GetRandomFuerzaWander()
    {
        var CentroCir = v3velocidad.normalized;
        var puntoRandom = Random.insideUnitCircle;

        var desplacamiento = new Vector3(puntoRandom.x, puntoRandom.y) * f_RadioCirculo;
        desplacamiento = Quaternion.LookRotation(v3velocidad) * desplacamiento;

        var wanderForce = CentroCir + desplacamiento;
        return wanderForce;
    }
}
