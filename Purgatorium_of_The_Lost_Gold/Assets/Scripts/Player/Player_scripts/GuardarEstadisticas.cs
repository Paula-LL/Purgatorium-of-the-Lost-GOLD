using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardarEstadisticas : MonoBehaviour
{
    public PlayerStats estadisticasAGuardar; 
    public static GuardarEstadisticas Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void OnDestroy()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void GuardarStats()
    {
        estadisticasAGuardar = new PlayerStats();
        estadisticasAGuardar = Player_controller.instance.currentPlayerStats;
    }
}
