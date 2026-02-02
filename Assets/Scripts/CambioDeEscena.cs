using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para gestionar escenas

public class CambioDeEscena : MonoBehaviour
{
    [Header("Configuración del Cambio")]
    [Tooltip("Escribe aquí el nombre EXACTO de la escena a la que quieres viajar.")]
    public string nombreDeLaEscena;

    [Tooltip("Escribe el Tag del objeto que puede activar este cambio (ej. 'Player').")]
    public string tagDelObjeto = "Player";

    [Header("Opciones")]
    [Tooltip("Marca esto si quieres que el cambio sea al tocar (Trigger) o al chocar (Colisión física).")]
    public bool usarTrigger = true;

    // ---------------------------------------------------------
    // OPCIÓN 1: DETECCIÓN POR TRIGGER (ATRAVESAR) - RECOMENDADO
    // ---------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (usarTrigger)
        {
            // Verificamos si lo que entró en el trigger es el objeto especificado
            if (other.CompareTag(tagDelObjeto))
            {
                CargarNivel();
            }
        }
    }

    // ---------------------------------------------------------
    // OPCIÓN 2: DETECCIÓN POR COLISIÓN (CHOCAR SÓLIDO)
    // ---------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!usarTrigger)
        {
            // En colisiones, accedemos al tag a través del gameObject
            if (other.gameObject.CompareTag(tagDelObjeto))
            {
                CargarNivel();
            }
        }
    }

    // Lógica para cargar la escena
    void CargarNivel()
    {
        if (!string.IsNullOrEmpty(nombreDeLaEscena))
        {
            Debug.Log("Cambiando a la escena: " + nombreDeLaEscena);
            SceneManager.LoadScene(nombreDeLaEscena);
        }
        else
        {
            Debug.LogError("¡Error! El nombre de la escena está vacío en el Inspector.");
        }
    }
}