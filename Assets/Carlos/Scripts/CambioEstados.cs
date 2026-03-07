using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CambioEstados : MonoBehaviour
{
    [Header("REFERENCIA AL JUGADOR")]
    public GameObject playerTarget; 
    private Animator playerAnimator;

    [Header("OBJETOS DE MÁSCARA (Visuales)")]
    public GameObject objetoMascaraIra;      // Objeto que aparece al presionar 1
    public GameObject objetoMascaraMiedo;    // Objeto que aparece al presionar 2
    public GameObject objetoMascaraFelicidad;// Objeto que aparece al presionar 3

    [Header("AUDIO")]
    public AudioSource audioSource;
    public AudioClip sonidoExplosionPuerta;
    public AudioClip sonidoMascaraIra;
    public AudioClip sonidoMascaraMiedo;
    public AudioClip sonidoMascaraFelicidad;

    [Header("EFECTOS VISUALES (Particle System de la Escena)")]
    public ParticleSystem particleExplosion; 

    [Header("UI - Imágenes Overlay")]
    public Image imagenIra;
    public Image imagenMiedo;     
    public Image imagenFelicidad;

    [Header("UI - Configuración de Color")]
    public Color colorIra = new Color(1f, 0f, 0f, 0.5f);        
    public Color colorMiedo = new Color(0f, 0f, 1f, 0.5f);      
    public Color colorFelicidad = new Color(1f, 1f, 0f, 0.5f);  

    [Header("UI - Configuración de Fades")]
    [Range(0.1f, 3f)] public float duracionFadeIn = 0.5f;
    [Range(0.1f, 3f)] public float duracionFadeOut = 0.5f;

    [Header("Listas de Objetos")]
    public List<GameObject> listaEnemigos;
    public List<GameObject> listaPuertas;

    [Header("Duración Total de Efectos")]
    [SerializeField] private float duracionIra = 10f;
    [SerializeField] private float duracionMiedo = 20f;
    [SerializeField] private float duracionFelicidad = 25f;

    [Header("Cooldowns")]
    [SerializeField] private float cooldownIra = 10f;
    [SerializeField] private float cooldownMiedo = 20f;
    [SerializeField] private float cooldownFelicidad = 25f;

    private bool emocionActiva = false;
    private string emocionActual = "";

    [Header("UI - Temporizadores")]
    [SerializeField] private TemporizadorBarras temporizadorBarrasIra;
    [SerializeField] private TemporizadorBarras temporizadorBarrasMiedo;
    [SerializeField] private TemporizadorBarras temporizadorBarrasFeliz;

    void Start()
    {
        if (playerTarget == null) return;
        playerAnimator = playerTarget.GetComponent<Animator>();
        
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (particleExplosion != null) particleExplosion.Stop();

        // Inicializar imágenes de UI
        InicializarImagen(imagenIra, colorIra);
        InicializarImagen(imagenMiedo, colorMiedo);
        InicializarImagen(imagenFelicidad, colorFelicidad);

        // Asegurarnos que las máscaras físicas empiecen desactivadas
        if (objetoMascaraIra != null) objetoMascaraIra.SetActive(false);
        if (objetoMascaraMiedo != null) objetoMascaraMiedo.SetActive(false);
        if (objetoMascaraFelicidad != null) objetoMascaraFelicidad.SetActive(false);

        PlayerUnifiedRelay relay = playerTarget.GetComponent<PlayerUnifiedRelay>();
        if (relay == null) relay = playerTarget.AddComponent<PlayerUnifiedRelay>();
        relay.managerEstados = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            IntentarActivarEmocion("Ira", duracionIra, cooldownIra, temporizadorBarrasIra, imagenIra, colorIra, sonidoMascaraIra, objetoMascaraIra);
        
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
            IntentarActivarEmocion("Miedo", duracionMiedo, cooldownMiedo, temporizadorBarrasMiedo, imagenMiedo, colorMiedo, sonidoMascaraMiedo, objetoMascaraMiedo);
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
            IntentarActivarEmocion("Felicidad", duracionFelicidad, cooldownFelicidad, temporizadorBarrasFeliz, imagenFelicidad, colorFelicidad, sonidoMascaraFelicidad, objetoMascaraFelicidad);
    }

    void IntentarActivarEmocion(string nombre, float duracion, float cooldown, TemporizadorBarras temp, Image img, Color col, AudioClip clip, GameObject mascaraFisica)
    {
        if (emocionActiva) return;
        if (temp != null && temp.EstaOcupado) return;

        if (audioSource != null && clip != null) audioSource.PlayOneShot(clip);
        StartCoroutine(EmocionCoroutine(nombre, duracion, cooldown, temp, img, col, mascaraFisica));
    }

    IEnumerator EmocionCoroutine(string emocion, float duracionTotal, float cooldown, TemporizadorBarras temporizador, Image imgOverlay, Color colorObjetivo, GameObject mascaraFisica)
    {
        emocionActiva = true;
        emocionActual = emocion;
        
        // ACTIVAR MÁSCARA FÍSICA Y ANIMATOR
        if(mascaraFisica != null) mascaraFisica.SetActive(true);
        if(playerAnimator != null) playerAnimator.SetBool(emocion, true);
        
        if(temporizador != null) temporizador.IniciarDuracion(duracionTotal);

        StartCoroutine(RutinaFadeIn(imgOverlay, colorObjetivo));

        if (emocion == "Felicidad") ToggleEnemigos(false);

        yield return new WaitForSeconds(duracionTotal);

        yield return StartCoroutine(RutinaFadeOut(imgOverlay));

        if (emocion == "Felicidad") ToggleEnemigos(true);

        // DESACTIVAR MÁSCARA FÍSICA Y ANIMATOR
        if(mascaraFisica != null) mascaraFisica.SetActive(false);
        if(playerAnimator != null) playerAnimator.SetBool(emocion, false);

        emocionActiva = false;
        emocionActual = "";
        if(temporizador != null) temporizador.IniciarCooldown(cooldown);
    }

    public void ProcesarChoqueFisico(Collision2D collision)
    {
        if (listaPuertas.Contains(collision.gameObject) && emocionActual == "Ira")
        {
            if (audioSource != null && sonidoExplosionPuerta != null)
                audioSource.PlayOneShot(sonidoExplosionPuerta);

            if (particleExplosion != null && playerTarget != null)
            {
                particleExplosion.transform.position = playerTarget.transform.position;
                particleExplosion.Play(); 
            }

            GameObject puerta = collision.gameObject;
            listaPuertas.Remove(puerta);
            Destroy(puerta);
        }
    }

    void ToggleEnemigos(bool estado)
    {
        foreach (GameObject enemigo in listaEnemigos)
            if (enemigo != null) enemigo.SetActive(estado);
    }

    void InicializarImagen(Image img, Color colorBase)
    {
        if (img == null) return;
        img.color = new Color(colorBase.r, colorBase.g, colorBase.b, 0f);
        img.gameObject.SetActive(false);
    }

    IEnumerator RutinaFadeIn(Image img, Color col)
    {
        if (img == null) yield break;
        img.gameObject.SetActive(true);
        float t = 0;
        while (t < duracionFadeIn) {
            t += Time.deltaTime;
            img.color = Color.Lerp(new Color(col.r, col.g, col.b, 0f), col, t / duracionFadeIn);
            yield return null;
        }
    }

    IEnumerator RutinaFadeOut(Image img)
    {
        if (img == null) yield break;
        Color inicial = img.color;
        Color final = new Color(inicial.r, inicial.g, inicial.b, 0f);
        float t = 0;
        while (t < duracionFadeOut) {
            t += Time.deltaTime;
            img.color = Color.Lerp(inicial, final, t / duracionFadeOut);
            yield return null;
        }
        img.gameObject.SetActive(false);
    }

    public void ProcesarTrigger(Collider2D other)
    {
        if (other.CompareTag("Enemigo") && emocionActual != "Miedo" && emocionActual != "Felicidad")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

public class PlayerUnifiedRelay : MonoBehaviour
{
    public CambioEstados managerEstados;
    private void OnCollisionEnter2D(Collision2D collision) { if (managerEstados != null) managerEstados.ProcesarChoqueFisico(collision); }
    private void OnTriggerEnter2D(Collider2D other) { if (managerEstados != null) managerEstados.ProcesarTrigger(other); }
}