using UnityEngine;

public class ContenedorBusqueda : MonoBehaviour
{
    [Header("Configuración de Audio")]
    public AudioSource audioSource; // Arrastra el AudioSource aquí
    public AudioClip sonidoPildoraEncontrada; // El sonido de éxito
    public AudioClip sonidoContenedorVacio;   // Opcional: Sonido de "no hay nada"

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
            
            // 1. Reproducir Audio de éxito
            ReproducirSonido(sonidoPildoraEncontrada);

            // 2. Aumentar cordura
            if (sistemaCordura != null)
            {
                sistemaCordura.IncreaseSanity(20f);
            }

            // 3. Crear el efecto visual de la pildora
            if (prefabPildoraVisual != null)
            {
                Instantiate(prefabPildoraVisual, transform.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("Aquí no hay nada...");
            // Reproducir sonido de vacío si se desea
            ReproducirSonido(sonidoContenedorVacio);
        }
    }

    // Método auxiliar para reproducir audio de forma segura
    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}