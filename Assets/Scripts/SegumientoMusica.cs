using UnityEngine;

public class SeguimientoMusica : MonoBehaviour
{
    [Header("Configuración")] // Uso de Header para organizar variables en el inspector [[4]]
    public Transform transformCamara; // Referencia a la transformada de la cámara
    public float distancia = 5f; // Distancia desde la cámara al sistema de partículas
    public Vector3 ajustePosicion = new Vector3(0, 2f, 0); // Ajuste fino de posición

    // Variables privadas para gestión interna
    private ParticleSystem sistemaParticulas;
    private Quaternion rotacionOriginal; // Guarda la rotación inicial del objeto

    void Start()
    {
        // Inicialización del sistema de partículas y rotación
        sistemaParticulas = GetComponent<ParticleSystem>();
        rotacionOriginal = transform.rotation;

        // Asigna automáticamente la cámara principal si no está definida
        if (transformCamara == null)
            transformCamara = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Cálculo de posición frente a la cámara usando operaciones vectoriales
        Vector3 posicionObjetivo = transformCamara.position +
                                  transformCamara.forward * distancia +
                                  ajustePosicion;

        // Movimiento suavizado mediante interpolación lineal (Lerp)
        transform.position = Vector3.Lerp(
            transform.position,
            posicionObjetivo,
            Time.deltaTime * 5f // Factor de suavizado
        );

        // Mantén la rotación original para evitar rotaciones indeseadas
        transform.rotation = rotacionOriginal;
    }
}