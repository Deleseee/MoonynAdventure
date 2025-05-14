using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovimeintoPersonaje : MonoBehaviour
{
    // Parámetros de movimiento básicos
    public float velocidadMovimiento = 8f;
    public float fuerzaSalto = 10f;
    public float velocidadCorriendo = 13f;
    public float velocidadRotacion = 12f;

    // Configuración para detección de suelo (salto)
    public LayerMask capaSuelo;
    public Transform puntoSuelo;
    public float radioDeteccionSuelo = 0.2f;

    // Variables privadas para control del personaje
    private Rigidbody cuerpoRigido;
    private Vector3 direccionMovimiento;
    private bool estaCorriendo;
    private bool puedeSaltar = true; // Control de estado de salto

    void Start()
    {
        // Inicialización del Rigidbody y bloqueo de rotación física
        cuerpoRigido = GetComponent<Rigidbody>();
        cuerpoRigido.freezeRotation = true;
    }

    void Update()
    {
        // Actualización de lógica de movimiento y detección de suelo cada frame
        Movimiento();
        ChequearSuelo();
    }

    void ChequearSuelo()
    {
        // Detección de contacto con el suelo usando una esfera de colisión
        bool enSuelo = Physics.CheckSphere(
            puntoSuelo.position,
            radioDeteccionSuelo,
            capaSuelo,
            QueryTriggerInteraction.Ignore
        );

        Debug.Log($"En suelo: {enSuelo}");

        // Actualiza el estado de salto según la detección de suelo
        if (enSuelo && !puedeSaltar)
        {
            puedeSaltar = true;
        }
        else if (!enSuelo && puedeSaltar)
        {
            puedeSaltar = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Cambio de escena al colisionar con el objeto "Nave"
        if (collision.gameObject.CompareTag("Nave"))
        {
            SceneManager.LoadScene("Mundos");
        }
    }

    void Movimiento()
    {
        // Captura de entrada del jugador (horizontal/vertical)
        float horizontal = -Input.GetAxisRaw("Horizontal");
        float vertical = -Input.GetAxisRaw("Vertical");
        direccionMovimiento = new Vector3(horizontal, 0, vertical).normalized;
        estaCorriendo = Input.GetKey(KeyCode.LeftShift);
        Debug.Log("Saltar?" + puedeSaltar);

        // Sistema de salto mejorado con control de estado
        if (Input.GetKeyDown(KeyCode.Space) && puedeSaltar)
        {
            Console.WriteLine("Saltando");
            cuerpoRigido.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            puedeSaltar = false; // Bloquea saltos múltiples consecutivos
        }
    }

    void FixedUpdate()
    {
        // Aplicación de física con velocidad ajustada por estado (caminar/correr)
        float velocidadActual = estaCorriendo ? velocidadCorriendo : velocidadMovimiento;

        Vector3 velocidadCalculada = direccionMovimiento * velocidadActual;
        cuerpoRigido.velocity = new Vector3(
            velocidadCalculada.x,
            cuerpoRigido.velocity.y,
            velocidadCalculada.z
        );

        // Rotación del personaje si hay dirección de movimiento
        if (direccionMovimiento != Vector3.zero)
        {
            RotarPersonaje();
        }
    }

    void RotarPersonaje()
    {
        // Rotación suavizada hacia la dirección de movimiento
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotacionObjetivo,
            velocidadRotacion * Time.deltaTime
        );
    }

}