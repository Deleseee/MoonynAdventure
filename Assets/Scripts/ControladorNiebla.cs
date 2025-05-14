using UnityEngine;

public class ControladorNiebla : MonoBehaviour
{
    void Start()
    {
        // Habilitar la niebla
        RenderSettings.fog = true;

        // Parametros de la niebla
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogColor = Color.gray;
        RenderSettings.fogDensity = 0.04f; // Ajustar densidad de la niebla
    }
}