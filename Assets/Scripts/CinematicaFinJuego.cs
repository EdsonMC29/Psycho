using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CinematicaFinJuego : MonoBehaviour
{
    [Header("Referencias")]
    public VideoPlayer reproductorVideo; // Tu componente Video Player
    public GameObject botonGracias;      // El botón de UI que dice "Gracias"
    
    [Header("Configuración")]
    public string nombreEscenaMenu = "MenuInicio";

    void Start()
    {
        // 1. Al iniciar, ocultamos el botón para que no estorbe
        if (botonGracias != null)
            botonGracias.SetActive(false);

        // 2. Nos suscribimos al evento: "Avísame cuando el video termine"
        if (reproductorVideo != null)
            reproductorVideo.loopPointReached += AlTerminarVideo;
    }

    // Esta función se ejecuta automáticamente cuando el video acaba
    void AlTerminarVideo(VideoPlayer vp)
    {
        // Mostramos el botón
        botonGracias.SetActive(true);

        // IMPORTANTE: Aseguramos que el mouse sea visible para poder hacer clic
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Esta función la llamaremos desde el botón
    public void IrAlMenu()
    {
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}