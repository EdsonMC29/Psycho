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

    // Ya no usamos un booleano global de cooldown
    private bool emocionActiva = false;
    private string emocionActual = "";

    [Header("UI - Temporizadores")]
    [SerializeField] private TemporizadorBarras temporizadorBarrasIra;
    [SerializeField] private TemporizadorBarras temporizadorBarrasMiedo;
    [SerializeField] private TemporizadorBarras temporizadorBarrasFeliz;

    void Start()
{
    // 1. Verificación de seguridad
    if (playerTarget == null) 
    {
        Debug.LogError("⛔ ERROR: No has asignado el PlayerTarget en el Inspector.");
        return;
    }

    // 2. Obtener Animator
    playerAnimator = playerTarget.GetComponent<Animator>();

    // 3. Configurar imágenes de UI
    InicializarImagen(imagenIra, colorIra);
    InicializarImagen(imagenMiedo, colorMiedo);
    InicializarImagen(imagenFelicidad, colorFelicidad);

    // 4. Configurar el Relay de forma segura
    // Primero revisamos si ya existe para no duplicarlo
    PlayerUnifiedRelay relay = playerTarget.GetComponent<PlayerUnifiedRelay>();
    if (relay == null)
    {
        relay = playerTarget.AddComponent<PlayerUnifiedRelay>();
    }
    relay.managerEstados = this;
}

    void Update()
    {
        // 1. Ira con tecla 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
            IntentarActivarEmocion("Ira", duracionIra, cooldownIra, temporizadorBarrasIra, imagenIra, colorIra);
        
        // 2. Miedo con tecla 2
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
            IntentarActivarEmocion("Miedo", duracionMiedo, cooldownMiedo, temporizadorBarrasMiedo, imagenMiedo, colorMiedo);
        
        // 3. Felicidad con tecla 3
        if (Input.GetKeyDown(KeyCode.Alpha3))
            IntentarActivarEmocion("Felicidad", duracionFelicidad, cooldownFelicidad, temporizadorBarrasFeliz, imagenFelicidad, colorFelicidad);
    }

    void IntentarActivarEmocion(string nombre, float duracion, float cooldown, TemporizadorBarras temp, Image img, Color col)
    {
        // Regla 1: No se puede activar nada si ya hay una emoción en curso (duración)
        if (emocionActiva) return;

        // Regla 2: Solo se bloquea si el temporizador específico está ocupado (en su propio cooldown)
        if (temp != null && temp.EstaOcupado) 
        {
            Debug.Log(nombre + " está en cooldown.");
            return;
        }

        StartCoroutine(EmocionCoroutine(nombre, duracion, cooldown, temp, img, col));
    }

    IEnumerator EmocionCoroutine(string emocion, float duracionTotal, float cooldown, TemporizadorBarras temporizador, Image imgOverlay, Color colorObjetivo)
    {
        emocionActiva = true;
        emocionActual = emocion;
        
        if(playerAnimator != null) playerAnimator.SetBool(emocion, true);
        if(temporizador != null) temporizador.IniciarDuracion(duracionTotal);

        StartCoroutine(RutinaFadeIn(imgOverlay, colorObjetivo));

        if (emocion == "Felicidad") ToggleEnemigos(false);

        float tiempoDeEsperaCentral = duracionTotal - duracionFadeIn - duracionFadeOut;
        yield return new WaitForSeconds(duracionFadeIn + tiempoDeEsperaCentral);

        yield return StartCoroutine(RutinaFadeOut(imgOverlay));

        if(playerAnimator != null) playerAnimator.SetBool(emocion, false);
        if (emocion == "Felicidad") ToggleEnemigos(true);

        emocionActiva = false;
        emocionActual = "";

        // Al terminar, solo este temporizador entra en cooldown
        if(temporizador != null) temporizador.IniciarCooldown(cooldown);
    }

    // --- MÉTODOS DE APOYO (Sin cambios significativos) ---

    void InicializarImagen(Image img, Color colorBase)
    {
        if (img == null) return;
        img.color = new Color(colorBase.r, colorBase.g, colorBase.b, 0f);
        img.gameObject.SetActive(false);
    }

    IEnumerator RutinaFadeIn(Image img, Color colorObjetivo)
    {
        if (img == null) yield break;
        img.gameObject.SetActive(true);
        float t = 0;
        while (t < duracionFadeIn)
        {
            t += Time.deltaTime;
            img.color = Color.Lerp(new Color(colorObjetivo.r, colorObjetivo.g, colorObjetivo.b, 0f), colorObjetivo, t / duracionFadeIn);
            yield return null;
        }
    }

    IEnumerator RutinaFadeOut(Image img)
    {
        if (img == null) yield break;
        Color inicial = img.color;
        Color final = new Color(inicial.r, inicial.g, inicial.b, 0f);
        float t = 0;
        while (t < duracionFadeOut)
        {
            t += Time.deltaTime;
            img.color = Color.Lerp(inicial, final, t / duracionFadeOut);
            yield return null;
        }
        img.gameObject.SetActive(false);
    }

    void ToggleEnemigos(bool estado)
    {
        foreach (GameObject enemigo in listaEnemigos) if (enemigo != null) enemigo.SetActive(estado);
    }

    public void ProcesarChoqueFisico(Collision2D collision)
    {
        if (listaPuertas.Contains(collision.gameObject) && emocionActual == "Ira")
        {
            listaPuertas.Remove(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }

    public void ProcesarTrigger(Collider2D other)
    {
        if (other.CompareTag("Enemigo") && emocionActual != "Miedo")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

public class PlayerUnifiedRelay : MonoBehaviour
{
    public CambioEstados managerEstados;

    private void OnCollisionEnter2D(Collision2D collision) 
    { 
        if (managerEstados != null) managerEstados.ProcesarChoqueFisico(collision); 
    }

    private void OnTriggerEnter2D(Collider2D other) 
    { 
        if (managerEstados != null) managerEstados.ProcesarTrigger(other); 
    }
}