using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextoPantallaBase : MonoBehaviour
{
    public Text textoPantalla;
    void Start()
    {
        //Comprobamos que la variable se 1 si s eha pasado el mundo 3 y aparecera ese mensaje, si no aparecera otro
        if (PlayerPrefs.GetInt("mundo3Pasado", 0) == 1)
        {
            textoPantalla.text = "Has salvado a todos tus compañeros de los malvados alienigenas!!";
        }
        else
        {
            textoPantalla.text = "No has salvado a todos tus compañeros de los malvados alienigenas!! Ves a la nave a salvarlos!!";
        }
    }

}
