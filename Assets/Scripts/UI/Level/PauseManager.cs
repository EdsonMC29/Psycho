using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    [Header("Configuracion de Sonido")]
    public AudioMixer mixer; // Mixer

    [Header("Configuracion de Menu Pausa")]
    public GameObject objetoMenuPausa;
    private bool juegoPausado = false;

    private void Awake()
    {
        // Singleton para acceso global
        if (Instance == null)
        {
            Instance = this;
            // Opcional: DontDestroyOnLoad(gameObject); si quieres que la pausa sea global
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Detecta la tecla P
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        juegoPausado = true;
        objetoMenuPausa.SetActive(true);

        // Detener el tiempo
        Time.timeScale = 0f;

        // ESTO PAUSA TODO EL AUDIO DEL JUEGO
        mixer.SetFloat("SFX", -80f);
    }

    public void Reanudar()
    {
        juegoPausado = false;
        objetoMenuPausa.SetActive(false);

        // Devuelve el tiempo a la normalidad
        Time.timeScale = 1f;
        // ESTO REANUDA EL AUDIO
        mixer.SetFloat("SFX", -0f);
    }
    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;

        if (mixer != null)
        {
            mixer.SetFloat("SFX", 0f);
        }

        int escenaActual = SceneManager.GetActiveScene().buildIndex;

        if (TransicionEscenasUI.Instance != null)
        {
            TransicionEscenasUI.Instance.DisolverSalida(escenaActual);
        }
        else
        {
            SceneManager.LoadScene(escenaActual);
        }
    }   
    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        if (TransicionEscenasUI.Instance != null)
        {
            TransicionEscenasUI.Instance.DisolverSalida(0);
            SceneManager.LoadScene("MenuInicio");
        }
    }
}