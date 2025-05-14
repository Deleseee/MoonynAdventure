using UnityEngine;

public class SeguimientoMusica : MonoBehaviour
{
    [Header("Configuraci�n")] // Uso de Header para organizar variables en el inspector [[4]]
    public Transform transformCamara; // Referencia a la transformada de la c�mara
    public float distancia = 5f; // Distancia desde la c�mara al sistema de part�culas
    public Vector3 ajustePosicion = new Vector3(0, 2f, 0); // Ajuste fino de posici�n

    // Variables privadas para gesti�n interna
    private ParticleSystem sistemaParticulas;
    private Quaternion rotacionOriginal; // Guarda la rotaci�n inicial del objeto

    void Start()
    {
        // Inicializaci�n del sistema de part�culas y rotaci�n
        sistemaParticulas = GetComponent<ParticleSystem>();
        rotacionOriginal = transform.rotation;

        // Asigna autom�ticamente la c�mara principal si no est� definida
        if (transformCamara == null)
            transformCamara = Camera.main.transform;
    }

    void LateUpdate()
    {
        // C�lculo de posici�n frente a la c�mara usando operaciones vectoriales
        Vector3 posicionObjetivo = transformCamara.position +
                                  transformCamara.forward * distancia +
                                  ajustePosicion;

        // Movimiento suavizado mediante interpolaci�n lineal (Lerp)
        transform.position = Vector3.Lerp(
            transform.position,
            posicionObjetivo,
            Time.deltaTime * 5f // Factor de suavizado
        );

        // Mant�n la rotaci�n original para evitar rotaciones indeseadas
        transform.rotation = rotacionOriginal;
    }
}