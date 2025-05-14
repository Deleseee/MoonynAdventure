using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Config : MonoBehaviour
{
    //Metodo para volver a la escena anterior o al menu, dependiendo de si estas en la abse o has entrado desde el menu
    public void Volver()
    {
        if (PlayerPrefs.GetInt("Config") == 1)
        {
            PlayerPrefs.SetInt("Config", 0);
            SceneManager.LoadScene(PlayerPrefs.GetString("mundo"));
        }
        else
        {
            PlayerPrefs.SetInt("Config", 0);
            SceneManager.LoadScene("Menu");
        }
    }
}