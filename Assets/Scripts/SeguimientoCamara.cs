using UnityEngine;

public class SeguimientoCamara : MonoBehaviour
{
    // Referencia al personaje que queremos que siga la cámara
    public Transform personaje;

    // Suavidad del movimiento de la cámara (valores bajos = más rápido)
    public float smoothTime = 0.3f;

    // Posición inicial relativa al personaje (x=izquierda/derecha, y=altura, z=profundidad)
    public Vector3 offset = new Vector3(0, 2, -5);

    // Rango de movimiento de la cámara (mínimo y máximo alejamiento)
    public float distanciaMinima = 2f;
    public float distanciaMaxima = 5f;

    // Capa de objetos que consideramos obstáculos (para evitar que tapen al personaje)
    public LayerMask capaObstaculos;

    // Margen de seguridad para no rozar obstáculos
    public float margenSeguridad = 0.3f;

    // Variables internas para el cálculo
    private Vector3 velocity = Vector3.zero;
    private float distanciaActual;
    private Vector3 direccionOriginal;

    void Start()
    {
        // Guardamos la dirección original de la cámara (sin distancia)
        direccionOriginal = offset.normalized;

        // Guardamos la distancia inicial desde el offset
        distanciaActual = offset.magnitude;
    }

    void LateUpdate()
    {
        // Si no tenemos personaje asignado, no hacemos nada
        if (personaje == null) return;

        // Calculamos la posición que queremos ocupar
        Vector3 posicionDeseada = personaje.position + offset;

        // Ajustamos la distancia si hay obstáculos en el camino
        AjustarDistanciaPorObstaculo(ref posicionDeseada);

        // Movemos la cámara suavemente hacia la posición deseada
        transform.position = Vector3.SmoothDamp(
            transform.position,
            posicionDeseada,
            ref velocity,
            smoothTime
        );

        // Hacemos que la cámara mire siempre al personaje
        transform.LookAt(personaje.position);
    }

    // Ajusta la distancia de la cámara si encuentra obstáculos en su línea de visión
    void AjustarDistanciaPorObstaculo(ref Vector3 posicionObjetivo)
    {
        RaycastHit hit;
        // Dirección desde el personaje a la posición actual de la cámara
        Vector3 direccionCamara = (posicionObjetivo - personaje.position).normalized;

        // Distancia actual entre personaje y cámara
        float distanciaOriginal = Vector3.Distance(personaje.position, posicionObjetivo);

        // Lanzamos un rayo para detectar obstáculos
        if (Physics.Raycast(personaje.position, direccionCamara, out hit, distanciaOriginal + margenSeguridad, capaObstaculos))
        {
            // Si hay obstáculo, reducimos la distancia manteniendo el margen de seguridad
            float distanciaSegura = hit.distance - margenSeguridad;
            distanciaActual = Mathf.Clamp(distanciaSegura, distanciaMinima, distanciaMaxima);
        }
        else
        {
            // Si no hay obstáculos, volvemos lentamente a la distancia máxima
            distanciaActual = Mathf.Lerp(distanciaActual, distanciaMaxima, Time.deltaTime * 2f);
        }

        // Actualizamos la posición objetivo con la distancia ajustada
        posicionObjetivo = personaje.position + direccionCamara * distanciaActual;
    }

}