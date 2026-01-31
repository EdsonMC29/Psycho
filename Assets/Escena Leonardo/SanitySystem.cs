using UnityEngine;
using UnityEngine.UI;

public class SanitySystem : MonoBehaviour
{
    [Header("Configuración de Cordura")]
    public float currentSanity = 100f;
    public float maxSanity = 100f;
    public float sanityDecayRate = 2f; // Baja 2 puntos por segundo
    
    [Header("Umbrales de Fases")]
    public float phase1Threshold = 75f;
    public float phase2Threshold = 50f;
    public float phase3Threshold = 25f;
    
    [Header("UI")]
    public Slider sanityBar;
    public Image sanityFill;
    
    [Header("Colores")]
    public Color normalColor = Color.green;
    public Color warningColor = Color.yellow;
    public Color criticalColor = new Color(1f, 0.5f, 0f); // Naranja
    public Color finalColor = Color.red;
    
    private int currentPhase = 0;
    
    void Start()
    {
        currentSanity = maxSanity;
        UpdateUI();
    }
    
    void Update()
    {
        // La cordura baja con el tiempo
        DecreaseSanity(sanityDecayRate * Time.deltaTime);
        
        UpdateUI();
        CheckPhaseChanges();
    }
    
    public void IncreaseSanity(float amount)
    {
        currentSanity = Mathf.Clamp(currentSanity + amount, 0, maxSanity);
        Debug.Log("Cordura aumentada: " + currentSanity);
    }
    
    public void DecreaseSanity(float amount)
    {
        currentSanity = Mathf.Clamp(currentSanity - amount, 0, maxSanity);
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
            case 0:
                Debug.Log("✓ TODO NORMAL");
                break;
            case 1:
                Debug.Log("⚠ FASE 1: Las cosas se ponen raras...");
                break;
            case 2:
                Debug.Log("⚠⚠ FASE 2: La realidad se distorsiona");
                break;
            case 3:
                Debug.Log("☠ FASE FINAL: Punto crítico!");
                break;
        }
    }
}