using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void Start()
    {
        // Verificar y crear mundo1Pasado
        if (!PlayerPrefs.HasKey("mundo1Pasado"))
        {
            PlayerPrefs.SetInt("mundo1Pasado", 0); // Valor por defecto
        }

        // Verificar y crear mundo2Pasado
        if (!PlayerPrefs.HasKey("mundo2Pasado"))
        {
            PlayerPrefs.SetInt("mundo2Pasado", 0); // Valor por defecto
        }

        PlayerPrefs.Save(); // Guardar cambios (opcional)
    }
    public void Jugar()
    {
        SceneManager.LoadScene("Base");// Cargamos la escena base
    }

    public void Configuracion()
    {
        SceneManager.LoadScene("Configuracion");// Cargamos la escena configuracion
    }
}
