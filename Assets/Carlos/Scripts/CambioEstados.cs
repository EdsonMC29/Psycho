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
    private Collider2D playerCollider;

    [Header("UI - Imágenes Overlay")]
    public Image imagenIra;
    public Image imagenMiedo;     
    public Image imagenFelicidad;

    [Header("UI - Configuración de Color")]
    [Tooltip("Ajusta la barra 'A' (Alpha) para la transparencia máxima deseada")]
    public Color colorIra = new Color(1f, 0f, 0f, 0.5f);        
    public Color colorMiedo = new Color(0f, 0f, 1f, 0.5f);      
    public Color colorFelicidad = new Color(1f, 1f, 0f, 0.5f);  

    [Header("UI - Configuración de Fades (Suavizado)")]
    [Range(0.1f, 3f)] public float duracionFadeIn = 0.5f;  // Tiempo para aparecer
    [Range(0.1f, 3f)] public float duracionFadeOut = 0.5f; // Tiempo para desaparecer

    [Header("Listas de Objetos")]
    public List<GameObject> listaEnemigos;
    public List<GameObject> listaPuertas;

    [Header("Duracion Total de Efectos (Incluye fades)")]
    [SerializeField] private float duracionIra = 10f;
    [SerializeField] private float duracionMiedo = 20f;
    [SerializeField] private float duracionFelicidad = 25f;

    [Header("Cooldowns")]
    [SerializeField] private float cooldownIra = 10f;
    [SerializeField] private float cooldownMiedo = 20f;
    [SerializeField] private float cooldownFelicidad = 25f;

    private bool enCooldown = false;
    private bool emocionActiva = false;
    private string emocionActual = "";

    [Header("UI - Temporizadores")]
    [SerializeField] private TemporizadorBarras temporizadorBarrasFeliz;
    [SerializeField] private TemporizadorBarras temporizadorBarrasIra;
    [SerializeField] private TemporizadorBarras temporizadorBarrasMiedo;

    void Start()
    {
        if (playerTarget == null) { Debug.LogError("⛔ ERROR: Asigna el Player Target."); return; }

        playerAnimator = playerTarget.GetComponent<Animator>();
        playerCollider = playerTarget.GetComponent<Collider2D>();

        // Asegurar que empiecen apagadas y transparentes
        InicializarImagen(imagenIra, colorIra);
        InicializarImagen(imagenMiedo, colorMiedo);
        InicializarImagen(imagenFelicidad, colorFelicidad);

        PlayerUnifiedRelay relay = playerTarget.AddComponent<PlayerUnifiedRelay>();
        relay.managerEstados = this; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            ActivarEmocion("Ira", duracionIra, cooldownIra, temporizadorBarrasIra, imagenIra, colorIra);
        
        if (Input.GetKeyDown(KeyCode.Y)) 
            ActivarEmocion("Miedo", duracionMiedo, cooldownMiedo, temporizadorBarrasMiedo, imagenMiedo, colorMiedo);
        
        if (Input.GetKeyDown(KeyCode.H))
            ActivarEmocion("Felicidad", duracionFelicidad, cooldownFelicidad, temporizadorBarrasFeliz, imagenFelicidad, colorFelicidad);
    }

    void ActivarEmocion(string emocion, float duracionTotal, float cooldown, TemporizadorBarras temporizador, Image imgOverlay, Color colorObjetivo)
    {
        if (enCooldown || emocionActiva) return;
        
        // Validación de seguridad: el tiempo total debe ser mayor que la suma de los fades
        if (duracionTotal <= (duracionFadeIn + duracionFadeOut))
        {
            Debug.LogWarning("La duración total del efecto es muy corta para los tiempos de fade configurados.");
            duracionTotal = duracionFadeIn + duracionFadeOut + 0.1f; // Ajuste mínimo de seguridad
        }

        StartCoroutine(EmocionCoroutine(emocion, duracionTotal, cooldown, temporizador, imgOverlay, colorObjetivo));
    }

    IEnumerator EmocionCoroutine(string emocion, float duracionTotal, float cooldown, TemporizadorBarras temporizador, Image imgOverlay, Color colorObjetivo)
    {
        // --- INICIO ---
        emocionActiva = true;
        emocionActual = emocion;
        
        if(playerAnimator != null) playerAnimator.SetBool(emocion, true);
        if(temporizador != null) temporizador.IniciarDuracion(duracionTotal);

        // 1. INICIAR FADE IN (Aparecer suavemente)
        StartCoroutine(RutinaFadeIn(imgOverlay, colorObjetivo));

        switch (emocion)
        {
            case "Felicidad": ToggleEnemigos(false); break;
        }

        // Calcular el tiempo de espera central (Duración total menos lo que tardan los fades)
        float tiempoDeEsperaCentral = duracionTotal - duracionFadeIn - duracionFadeOut;
        
        // Esperar el tiempo de Fade In + el tiempo central
        yield return new WaitForSeconds(duracionFadeIn + tiempoDeEsperaCentral);

        // --- FIN ---
        // 2. INICIAR FADE OUT (Desaparecer suavemente)
        // Usamos 'yield return' aquí para esperar a que termine el desvanecido antes de seguir
        yield return StartCoroutine(RutinaFadeOut(imgOverlay));

        if(playerAnimator != null) playerAnimator.SetBool(emocion, false);

        switch (emocion)
        {
            case "Felicidad": ToggleEnemigos(true); break;
        }

        emocionActiva = false;
        emocionActual = "";

        // COOLDOWN
        enCooldown = true;
        if(temporizador != null) temporizador.IniciarCooldown(cooldown);
        yield return new WaitForSeconds(cooldown);
        enCooldown = false;
    }

    // =========================================================
    // NUEVAS FUNCIONES PARA EL SUAVIZADO (FADES)
    // =========================================================

    // Configura la imagen al inicio para que esté lista pero invisible
    void InicializarImagen(Image img, Color colorBase)
    {
        if (img == null) return;
        // La ponemos del color correcto pero totalmente transparente (Alpha 0)
        img.color = new Color(colorBase.r, colorBase.g, colorBase.b, 0f);
        img.gameObject.SetActive(false);
    }

    // Corrutina para aparecer suavemente
    IEnumerator RutinaFadeIn(Image img, Color colorObjetivo)
    {
        if (img == null) yield break;
        
        img.gameObject.SetActive(true); // Encendemos el objeto
        
        Color colorInicial = new Color(colorObjetivo.r, colorObjetivo.g, colorObjetivo.b, 0f); // Empieza transparente
        img.color = colorInicial;

        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionFadeIn)
        {
            tiempoTranscurrido += Time.deltaTime;
            // Lerp interpola suavemente entre el color transparente y el color objetivo
            img.color = Color.Lerp(colorInicial, colorObjetivo, tiempoTranscurrido / duracionFadeIn);
            yield return null; // Esperar al siguiente frame
        }
        
        // Asegurar que al final quede exactamente del color objetivo
        img.color = colorObjetivo;
    }

    // Corrutina para desaparecer suavemente
    IEnumerator RutinaFadeOut(Image img)
    {
        if (img == null) yield break;

        Color colorActual = img.color;
        Color colorFinalTransparente = new Color(colorActual.r, colorActual.g, colorActual.b, 0f); // Destino: transparente

        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionFadeOut)
        {
            tiempoTranscurrido += Time.deltaTime;
            img.color = Color.Lerp(colorActual, colorFinalTransparente, tiempoTranscurrido / duracionFadeOut);
            yield return null;
        }

        // Asegurar que quede transparente y apagar el objeto
        img.color = colorFinalTransparente;
        img.gameObject.SetActive(false);
    }

    // =========================================================
    // FUNCIONES DE JUEGO EXISTENTES
    // =========================================================
    void ToggleEnemigos(bool estado)
    {
        foreach (GameObject enemigo in listaEnemigos)
        {
            if (enemigo != null) enemigo.SetActive(estado);
        }
    }

    public void ProcesarChoqueFisico(Collision2D collision)
    {
        GameObject objeto = collision.gameObject;
        if (listaPuertas.Contains(objeto) && emocionActiva && emocionActual == "Ira")
        {
            listaPuertas.Remove(objeto); Destroy(objeto); Debug.Log("¡Puerta derribada!");
        }
    }

    public void ProcesarTrigger(Collider2D other)
    {
        GameObject objeto = other.gameObject;
        if (objeto.CompareTag("Enemigo"))
        {
            if (emocionActiva && emocionActual == "Miedo") { return; }
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

public class PlayerUnifiedRelay : MonoBehaviour
{
    public CambioEstados managerEstados;
    private void OnCollisionEnter2D(Collision2D collision) { if (managerEstados != null) managerEstados.ProcesarChoqueFisico(collision); }
    private void OnTriggerEnter2D(Collider2D other) { if (managerEstados != null) managerEstados.ProcesarTrigger(other); }
}