using UnityEngine;

public class ColorChanger : MonoBehaviour
{

    public Color nuevoColor;

    public void Start()
    {
        // Se recoge todos los objetos dl juego con el tag Suelo
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag("Suelo");
        // Recorremos los objetos, cogemos su renderer y se cambiamos el color del material
        foreach (GameObject prefab in prefabs)
        {
            Renderer renderer = prefab.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = nuevoColor;
            }
        }
    }
}