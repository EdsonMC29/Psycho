using UnityEngine;

public enum PillType
{
    Positive,
    Negative
}

public class Pill : MonoBehaviour
{
    public PillType pillType = PillType.Positive;
    public float sanityEffect = 20f;
    public Color pillColor = Color.green;
    
    void Start()
    {
        // CAMBIO 2D: Usamos SpriteRenderer en lugar de Renderer genérico
        // Esto es mucho más eficiente para juegos 2D.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = pillColor;
        }
    }
    
    // CAMBIO PRINCIPAL: Ahora usa la física 2D
    void OnTriggerEnter2D(Collider2D other)
    {
        // Si el jugador toca la pastilla
        if (other.CompareTag("Player"))
        {
            SanitySystem sanitySystem = other.GetComponent<SanitySystem>();
            
            if (sanitySystem != null)
            {
                if (pillType == PillType.Positive)
                {
                    sanitySystem.IncreaseSanity(sanityEffect);
                    Debug.Log("🟢 Pastilla BUENA consumida! +" + sanityEffect);
                }
                else
                {
                    sanitySystem.DecreaseSanity(sanityEffect);
                    Debug.Log("🔴 Pastilla MALA consumida! -" + sanityEffect);
                }
            }
            
            // Destruir la pastilla
            Destroy(gameObject);
        }
    }
}