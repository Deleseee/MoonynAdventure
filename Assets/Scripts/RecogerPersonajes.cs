using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RecogerPersonajes : MonoBehaviour
{
    [Header("Configuración")]
    public int totalPersonajesBuenos = 6; // Número total de aliados a rescatar en el nivel
    public Text contadorTexto;             // Referencia al texto UI que muestra el contador
    public Text textoGanar;                // Referencia al texto UI de victoria

    private int personajesRecogidos = 0;    // Contador interno de aliados rescatados
    private string escenaActual;           // Almacena el nombre del nivel actual

    void Start()
    {
        // Al iniciar, obtenemos el nombre de la escena actual para determinar el mundo
        escenaActual = SceneManager.GetActiveScene().name;
        ActualizarContador(); // Actualizamos el contador visual inicial
    }

    void Update()
    {
        // Comprobamos si hemos recolectado todos los personajes buenos
        if (personajesRecogidos >= totalPersonajesBuenos)
        {
            textoGanar.text = "Has salvado a todos tus compañeros ya puedes volver a la base";
            personajesRecogidos = 0; // Reiniciamos el contador para posibles reinicios

            // Actualizamos el progreso según el mundo actual en el que estemos
            if (escenaActual == "Mundo1")
            {
                PlayerPrefs.SetInt("mundo1Pasado", 1); // Marcamos el mundo 1 como completado
            }
            else if (escenaActual == "Mundo2")
            {
                PlayerPrefs.SetInt("mundo2Pasado", 1); // Marcamos el mundo 2 como completado
            }
            else if (escenaActual == "Mundo3")
            {
                PlayerPrefs.SetInt("mundo3Pasado", 1); // Marcamos el mundo 3 como completado
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Colisión con un personaje aliado (bueno)
        if (other.gameObject.CompareTag("Bueno"))
        {
            personajesRecogidos++; // Incrementamos el contador
            Destroy(other.gameObject); // Eliminamos el personaje del escenario
            ActualizarContador(); // Actualizamos el contador en pantalla
        }
        // Colisión con un enemigo (malo)
        else if (other.gameObject.CompareTag("Malo"))
        {
            // Guardamos mensaje de derrota y cargamos pantalla de selección de mundos
            PlayerPrefs.SetString("MensajeDerrota", "Te capturaron los malvados alienígenas! Inténtalo de nuevo");
            SceneManager.LoadScene("Mundos");
        }
    }

    // Actualiza el texto del contador en la interfaz de usuario
    void ActualizarContador()
    {
        if (contadorTexto != null)
        {
            contadorTexto.text = personajesRecogidos.ToString();
        }
    }
}