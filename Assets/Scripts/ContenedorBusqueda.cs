using UnityEngine;

public class ContenedorBusqueda : MonoBehaviour
{
    [Header("Estado (No tocar, se asigna solo)")]
    public bool tienePildora = false; 
    public bool yaFueRevisado = false;

    // Referencias que asignará el Manager automáticamente
    [HideInInspector] public GameObject prefabPildoraVisual;
    [HideInInspector] public SanitySystem sistemaCordura;

    private void OnMouseDown()
    {
        // Si ya lo revisamos o el juego está pausado, no hacer nada
        if (yaFueRevisado) return;

        yaFueRevisado = true; // Marcar como revisado

        if (tienePildora)
        {
            Debug.Log("¡Encontraste pastillas!");
            
            // 1. Aumentar cordura
            if (sistemaCordura != null)
            {
                sistemaCordura.IncreaseSanity(20f);
            }

            // 2. Crear el efecto visual de la pildora
            if (prefabPildoraVisual != null)
            {
                Instantiate(prefabPildoraVisual, transform.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("Aquí no hay nada...");
            // Opcional: Sonido de vacío o feedback visual de "Vacío"
        }
    }
}