using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovimeintoPersonaje : MonoBehaviour
{
    // Par�metros de movimiento b�sicos
    public float velocidadMovimiento = 8f;
    public float fuerzaSalto = 10f;
    public float velocidadCorriendo = 13f;
    public float velocidadRotacion = 12f;

    // Configuraci�n para detecci�n de suelo (salto)
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
        // Inicializaci�n del Rigidbody y bloqueo de rotaci�n f�sica
        cuerpoRigido = GetComponent<Rigidbody>();
        cuerpoRigido.freezeRotation = true;
    }

    void Update()
    {
        // Actualizaci�n de l�gica de movimiento y detecci�n de suelo cada frame
        Movimiento();
        ChequearSuelo();
    }

    void ChequearSuelo()
    {
        // Detecci�n de contacto con el suelo usando una esfera de colisi�n
        bool enSuelo = Physics.CheckSphere(
            puntoSuelo.position,
            radioDeteccionSuelo,
            capaSuelo,
            QueryTriggerInteraction.Ignore
        );

        Debug.Log($"En suelo: {enSuelo}");

        // Actualiza el estado de salto seg�n la detecci�n de suelo
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
            puedeSaltar = false; // Bloquea saltos m�ltiples consecutivos
        }
    }

    void FixedUpdate()
    {
        // Aplicaci�n de f�sica con velocidad ajustada por estado (caminar/correr)
        float velocidadActual = estaCorriendo ? velocidadCorriendo : velocidadMovimiento;

        Vector3 velocidadCalculada = direccionMovimiento * velocidadActual;
        cuerpoRigido.velocity = new Vector3(
            velocidadCalculada.x,
            cuerpoRigido.velocity.y,
            velocidadCalculada.z
        );

        // Rotaci�n del personaje si hay direcci�n de movimiento
        if (direccionMovimiento != Vector3.zero)
        {
            RotarPersonaje();
        }
    }

    void RotarPersonaje()
    {
        // Rotaci�n suavizada hacia la direcci�n de movimiento
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotacionObjetivo,
            velocidadRotacion * Time.deltaTime
        );
    }

}