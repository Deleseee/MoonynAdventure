using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mundos : MonoBehaviour
{
    // Botones para acceder a los mundos 2 y 3
    public Button botonMundo2;
    public Button botonMundo3;

    // Colores para mostrar si un mundo está bloqueado o desbloqueado
    public Color colorDesbloqueado = Color.white;
    public Color colorBloqueado = new Color(0.3f, 0.3f, 0.3f, 0.5f);

    // Texto que muestra mensajes de derrota (como recordatorios del juego)
    public Text textoDerrota;

    void Start()
    {
        // Si hay un mensaje de derrota guardado, lo muestra y lo borra
        if (PlayerPrefs.HasKey("MensajeDerrota"))
        {
            if (textoDerrota != null)
            {
                textoDerrota.text = PlayerPrefs.GetString("MensajeDerrota");
                PlayerPrefs.DeleteKey("MensajeDerrota"); // Una vez mostrado, se borra
                PlayerPrefs.Save(); // Guardamos los cambios
            }
        }

        ActualizarEstadoBotones(); // Configura los botones según el progreso
    }

    void ActualizarEstadoBotones()
    {
        // Consultamos en "la libreta de notas" del juego si has completado mundos previos
        bool mundo1Completado = PlayerPrefs.GetInt("mundo1Pasado", 0) == 1; // ¿Terminaste el mundo 1?
        bool mundo2Completado = PlayerPrefs.GetInt("mundo2Pasado", 0) == 1; // ¿Terminaste el mundo 2?

        // Configuramos los botones para que estén bloqueados o desbloqueados
        ConfigurarBoton(botonMundo2, mundo1Completado); // El mundo 2 depende del 1
        ConfigurarBoton(botonMundo3, mundo2Completado); // El mundo 3 depende del 2
    }

    void ConfigurarBoton(Button boton, bool desbloqueado)
    {
        if (boton == null) return;

        // Cambiamos el color y si se puede pulsar según esté desbloqueado
        boton.interactable = desbloqueado; // ¿Puedes hacer clic aquí?
        boton.image.color = desbloqueado ? colorDesbloqueado : colorBloqueado; // Color visual
    }

    // Métodos para cambiar de escena cuando pulsas un botón
    public void Mundo1() => SceneManager.LoadScene("Mundo1");
    public void Volver() => SceneManager.LoadScene("Base");

    // Carga mundos solo si has completado requisitos previos
    public void Mundo2()
    {
        if (PlayerPrefs.GetInt("mundo1Pasado", 0) == 1) // ¿Completaste el mundo 1?
            SceneManager.LoadScene("Mundo2");
    }

    public void Mundo3()
    {
        if (PlayerPrefs.GetInt("mundo2Pasado", 0) == 1) // ¿Completaste el mundo 2?
            SceneManager.LoadScene("Mundo3");
    }
}