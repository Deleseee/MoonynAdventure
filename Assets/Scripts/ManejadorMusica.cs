using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManejadorMusica : MonoBehaviour
{
    // Instancia única para que funcione como un DJ exclusivo (singleton)
    public static ManejadorMusica Instancia;

    // Asigna aquí la pista de música de fondo que quieras reproducir
    [SerializeField] private AudioClip musicaFondo;

    // Componentes para controlar el audio y la interfaz
    private AudioSource fuenteAudio;
    private Slider volumenSlider; // Para ajustar volumen en configuración
    private Toggle muteToggle;   // Para activar/desactivar sonido

    void Awake()
    {
        // Si no hay una instancia previa, crea una y persiste entre escenas
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject); // Música que sobrevive a cambios de escena
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
            return;
        }

        // Prepara la caja de sonido con la pista asignada
        gameObject.AddComponent<AudioSource>();
        fuenteAudio = GetComponent<AudioSource>();
        fuenteAudio.clip = musicaFondo;
        fuenteAudio.loop = true; // Repetición infinita
        fuenteAudio.playOnAwake = true; // Empieza a sonar automáticamente

        // Aplica el volumen y estado muteado guardados
        ActualizarConfiguracionAudio();

        // Se suscribe para detectar cuando cargue la escena de configuración
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    // Cuando cambia de escena, verifica si es la de configuración
    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        if (escena.name == "Configuracion")
        {
            BuscarElementosUI(); // Localiza el slider y toggle automáticamente
        }
    }

    // Busca elementos de interfaz en la escena actual
    void BuscarElementosUI()
    {
        // Localiza el primer slider que encuentre (para controlar volumen)
        volumenSlider = FindObjectOfType<Slider>();
        if (volumenSlider != null)
        {
            // Configura el rango y valor inicial del slider
            volumenSlider.minValue = 0f;
            volumenSlider.maxValue = 1f;
            volumenSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
            volumenSlider.onValueChanged.AddListener(ActualizarVolumen);
        }

        // Localiza el primer toggle que encuentre (para muteo)
        muteToggle = FindObjectOfType<Toggle>();
        if (muteToggle != null)
        {
            // Establece estado inicial según preferencias guardadas
            muteToggle.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
            muteToggle.onValueChanged.AddListener(ToggleMute);
        }
    }

    // Restaura configuración guardada al iniciar
    void ActualizarConfiguracionAudio()
    {
        float volumen = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        bool musicaActivada = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

        fuenteAudio.volume = volumen;     // Aplica volumen guardado
        fuenteAudio.mute = !musicaActivada; // Aplica estado muteado

        // Si está activa y no suena, empieza a reproducir
        if (musicaActivada && !fuenteAudio.isPlaying)
            fuenteAudio.Play();
    }

    // Cambia el volumen cuando el usuario mueve el slider
    void ActualizarVolumen(float nuevoVolumen)
    {
        fuenteAudio.volume = nuevoVolumen;
        PlayerPrefs.SetFloat("MusicVolume", nuevoVolumen); // Guarda el ajuste
    }

    // Cambia estado muteado cuando el usuario interactúa con el toggle
    void ToggleMute(bool activado)
    {
        fuenteAudio.mute = !activado; // Invierte el estado (true = sonido activo)
        PlayerPrefs.SetInt("MusicEnabled", activado ? 1 : 0); // Guarda preferencia
    }

    // Limpieza al cerrar la aplicación
    void OnApplicationQuit()
    {
        SceneManager.sceneLoaded -= AlCargarEscena; // Elimina la suscripción
    }
}