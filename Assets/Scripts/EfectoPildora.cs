using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EfectoPildora : MonoBehaviour
{
    [Header("Configuración de Animación")]
    public float velocidadX = 2f;      // Velocidad de avance
    public float escalaFinal = 2f;     // Qué tan grande se pone
    public float duracionVida = 1.5f;  // Cuánto tiempo dura antes de desaparecer

    private float tiempoActual = 0f;
    private Vector3 escalaInicial;
    private SpriteRenderer spriteRenderer;
    private Color colorInicial;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        escalaInicial = transform.localScale;
        colorInicial = spriteRenderer.color;
    }

    void Update()
    {
        tiempoActual += Time.deltaTime;
        float porcentaje = tiempoActual / duracionVida; // Va de 0 a 1

        // 1. Mover en eje X
        transform.Translate(Vector2.right * velocidadX * Time.deltaTime);

        // 2. Agrandar (Lerp entre escala inicial y final)
        float escala = Mathf.Lerp(escalaInicial.x, escalaInicial.x * escalaFinal, porcentaje);
        transform.localScale = new Vector3(escala, escala, 1f);

        // 3. Volverse transparente (Lerp en el canal Alpha)
        Color nuevoColor = colorInicial;
        nuevoColor.a = Mathf.Lerp(1f, 0f, porcentaje);
        spriteRenderer.color = nuevoColor;

        // 4. Destruir al final
        if (tiempoActual >= duracionVida)
        {
            Destroy(gameObject);
        }
    }
}