using UnityEngine;

public class SeguimientoCamara : MonoBehaviour
{
    // Referencia al personaje que queremos que siga la c�mara
    public Transform personaje;

    // Suavidad del movimiento de la c�mara (valores bajos = m�s r�pido)
    public float smoothTime = 0.3f;

    // Posici�n inicial relativa al personaje (x=izquierda/derecha, y=altura, z=profundidad)
    public Vector3 offset = new Vector3(0, 2, -5);

    // Rango de movimiento de la c�mara (m�nimo y m�ximo alejamiento)
    public float distanciaMinima = 2f;
    public float distanciaMaxima = 5f;

    // Capa de objetos que consideramos obst�culos (para evitar que tapen al personaje)
    public LayerMask capaObstaculos;

    // Margen de seguridad para no rozar obst�culos
    public float margenSeguridad = 0.3f;

    // Variables internas para el c�lculo
    private Vector3 velocity = Vector3.zero;
    private float distanciaActual;
    private Vector3 direccionOriginal;

    void Start()
    {
        // Guardamos la direcci�n original de la c�mara (sin distancia)
        direccionOriginal = offset.normalized;

        // Guardamos la distancia inicial desde el offset
        distanciaActual = offset.magnitude;
    }

    void LateUpdate()
    {
        // Si no tenemos personaje asignado, no hacemos nada
        if (personaje == null) return;

        // Calculamos la posici�n que queremos ocupar
        Vector3 posicionDeseada = personaje.position + offset;

        // Ajustamos la distancia si hay obst�culos en el camino
        AjustarDistanciaPorObstaculo(ref posicionDeseada);

        // Movemos la c�mara suavemente hacia la posici�n deseada
        transform.position = Vector3.SmoothDamp(
            transform.position,
            posicionDeseada,
            ref velocity,
            smoothTime
        );

        // Hacemos que la c�mara mire siempre al personaje
        transform.LookAt(personaje.position);
    }

    // Ajusta la distancia de la c�mara si encuentra obst�culos en su l�nea de visi�n
    void AjustarDistanciaPorObstaculo(ref Vector3 posicionObjetivo)
    {
        RaycastHit hit;
        // Direcci�n desde el personaje a la posici�n actual de la c�mara
        Vector3 direccionCamara = (posicionObjetivo - personaje.position).normalized;

        // Distancia actual entre personaje y c�mara
        float distanciaOriginal = Vector3.Distance(personaje.position, posicionObjetivo);

        // Lanzamos un rayo para detectar obst�culos
        if (Physics.Raycast(personaje.position, direccionCamara, out hit, distanciaOriginal + margenSeguridad, capaObstaculos))
        {
            // Si hay obst�culo, reducimos la distancia manteniendo el margen de seguridad
            float distanciaSegura = hit.distance - margenSeguridad;
            distanciaActual = Mathf.Clamp(distanciaSegura, distanciaMinima, distanciaMaxima);
        }
        else
        {
            // Si no hay obst�culos, volvemos lentamente a la distancia m�xima
            distanciaActual = Mathf.Lerp(distanciaActual, distanciaMaxima, Time.deltaTime * 2f);
        }

        // Actualizamos la posici�n objetivo con la distancia ajustada
        posicionObjetivo = personaje.position + direccionCamara * distanciaActual;
    }

}