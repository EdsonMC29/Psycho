using UnityEngine;
using UnityEngine.UI;

public class SanitySystem : MonoBehaviour
{
    [Header("Configuración de Cordura")]
    public float currentSanity = 100f;
    public float maxSanity = 100f;
    public float sanityDecayRate = 2f; 
    
    [Header("Umbrales de Fases")]
    public float phase1Threshold = 75f;
    public float phase2Threshold = 50f;
    public float phase3Threshold = 25f;
    
    [Header("UI Barra")]
    public Slider sanityBar;
    public Image sanityFill;

    [Header("UI Fin de Juego")]
    public GameObject panelMuerte; // Asigna aquí tu Panel desde el Inspector
    
    [Header("Colores")]
    public Color normalColor = Color.green;
    public Color warningColor = Color.yellow;
    public Color criticalColor = new Color(1f, 0.5f, 0f); 
    public Color finalColor = Color.red;
    
    private int currentPhase = 0;
    private bool isGameOver = false; // Para evitar que se ejecute varias veces
    
    void Start()
    {
        // Aseguramos que el tiempo corra y el panel esté apagado al iniciar
        Time.timeScale = 1f;
        if (panelMuerte != null) panelMuerte.SetActive(false);

        currentSanity = maxSanity;
        UpdateUI();
    }
    
    void Update()
    {
        if (isGameOver) return; // Si ya perdimos, no hacemos nada más

        // La cordura baja con el tiempo
        DecreaseSanity(sanityDecayRate * Time.deltaTime);
        
        UpdateUI();
        CheckPhaseChanges();

        // Verificar si la cordura llegó a cero
        if (currentSanity <= 0)
        {
            GameOver();
        }
    }
    
    public void IncreaseSanity(float amount)
    {
        if (isGameOver) return;
        currentSanity = Mathf.Clamp(currentSanity + amount, 0, maxSanity);
    }
    
    public void DecreaseSanity(float amount)
    {
        currentSanity = Mathf.Clamp(currentSanity - amount, 0, maxSanity);
    }
    
    void GameOver()
    {
        isGameOver = true;
        Debug.Log("☠ CORDURA AGOTADA: Juego Pausado");

        if (panelMuerte != null)
        {
            panelMuerte.SetActive(true); // Activa el panel
        }

        Time.timeScale = 0f; // Pausa todo el movimiento y física del juego
    }

    void UpdateUI()
    {
        if (sanityBar != null)
        {
            sanityBar.value = currentSanity / maxSanity;
        }
        
        if (sanityFill != null)
        {
            if (currentSanity > phase1Threshold)
                sanityFill.color = normalColor;
            else if (currentSanity > phase2Threshold)
                sanityFill.color = warningColor;
            else if (currentSanity > phase3Threshold)
                sanityFill.color = criticalColor;
            else
                sanityFill.color = finalColor;
        }
    }
    
    void CheckPhaseChanges()
    {
        int newPhase = GetCurrentPhase();
        
        if (newPhase != currentPhase)
        {
            currentPhase = newPhase;
            OnPhaseChange(currentPhase);
        }
    }
    
    int GetCurrentPhase()
    {
        if (currentSanity > phase1Threshold) return 0;
        if (currentSanity > phase2Threshold) return 1;
        if (currentSanity > phase3Threshold) return 2;
        return 3;
    }
    
    void OnPhaseChange(int phase)
    {
        switch (phase)
        {
            case 0: Debug.Log("✓ TODO NORMAL"); break;
            case 1: Debug.Log("⚠ FASE 1: Las cosas se ponen raras..."); break;
            case 2: Debug.Log("⚠⚠ FASE 2: La realidad se distorsiona"); break;
            case 3: Debug.Log("☠ FASE FINAL: Punto crítico!"); break;
        }
    }
}