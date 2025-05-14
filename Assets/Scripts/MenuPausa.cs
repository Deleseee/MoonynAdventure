using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    // Variable para activar/desactivar la funcionalidad del botón ESC
    public bool activarESC = true;

    // Nombre de la escena actual (por defecto "Base")
    string nombreScene = "Base";

    void Update()
    {
        // Comprueba si se pulsa la tecla ESC y está activada la funcionalidad
        if (activarESC && Input.GetKeyDown(KeyCode.Escape))
        {
            // Guarda el nombre de la escena actual en PlayerPrefs
            nombreScene = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("mundo", nombreScene);

            // Llama al método para cargar la escena de configuración
            CargarEscenaConfiguracion();
        }
    }

    // Método público para cargar la escena de configuración
    public void CargarEscenaConfiguracion()
    {
        // Configura el estado de configuración en PlayerPrefs si no está establecido
        if (PlayerPrefs.GetInt("Config") == 0)
        {
            PlayerPrefs.SetInt("Config", 1);
        }

        // Verifica si la escena de configuración existe y puede cargarse
        if (Application.CanStreamedLevelBeLoaded("Configuracion"))
        {
            // Carga la escena de configuración y reanuda el tiempo del juego
            SceneManager.LoadScene("Configuracion");
            Time.timeScale = 1f;
        }
        
    }
}