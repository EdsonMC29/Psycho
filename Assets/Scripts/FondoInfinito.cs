using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FondoInfinito : MonoBehaviour
{
    [Header("Configuración desde el Inspector")]
    [Tooltip("La dirección hacia la que se moverá el fondo. Ej: (-1, 0) es izquierda.")]
    public Vector2 direccionMovimiento = new Vector2(-1, 0);

    [Tooltip("La velocidad a la que se mueven los sprites.")]
    public float velocidad = 2f;

    // Variables internas para el cálculo
    private Vector2 tamanoSprite;
    private Vector2 posicionInicial;
    private Vector2 direccionNormalizada;

    void Start()
    {
        // 1. Obtenemos el SpriteRenderer para saber cuánto mide la imagen en el mundo.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        // 'bounds.size' nos da el tamaño en unidades de mundo, considerando la escala.
        tamanoSprite = sr.bounds.size;

        // 2. Guardamos la posición donde empezó este sprite específico.
        posicionInicial = transform.position;

        // 3. Normalizamos la dirección para que la velocidad sea constante
        // independientemente de si pones (1,0) o (50,0).
        direccionNormalizada = direccionMovimiento.normalized;
    }

    void Update()
    {
        // --- MOVIMIENTO ---
        // Movemos el objeto en la dirección deseada, basado en la velocidad y el tiempo.
        transform.Translate(direccionNormalizada * velocidad * Time.deltaTime);

        // --- COMPROBACIÓN DE REINICIO (LOOP) ---
        // Calculamos qué tan lejos estamos de la posición inicial en cada eje.
        float distanciaX = Mathf.Abs(transform.position.x - posicionInicial.x);
        float distanciaY = Mathf.Abs(transform.position.y - posicionInicial.y);

        // Lógica para movimiento HORIZONTAL (si la dirección X es mayor que la Y)
        if (Mathf.Abs(direccionNormalizada.x) > Mathf.Abs(direccionNormalizada.y))
        {
            // Si nos hemos movido más allá del ancho del sprite...
            if (distanciaX >= tamanoSprite.x)
            {
                // Calculamos el vector de reinicio exacto opuesto al movimiento
                Vector3 offsetReinicio = new Vector3(tamanoSprite.x * Mathf.Sign(direccionNormalizada.x), 0, 0);
                // "Teletransportamos" el sprite hacia atrás exactamente una anchura.
                transform.position -= offsetReinicio;
                
                // Actualizamos la posición inicial para evitar pequeños errores de cálculo a largo plazo
                posicionInicial.x = transform.position.x;
            }
        }
        // Lógica para movimiento VERTICAL (si la dirección Y es mayor o igual que la X)
        else
        {
            // Si nos hemos movido más allá del alto del sprite...
            if (distanciaY >= tamanoSprite.y)
            {
                Vector3 offsetReinicio = new Vector3(0, tamanoSprite.y * Mathf.Sign(direccionNormalizada.y), 0);
                // "Teletransportamos" el sprite hacia atrás exactamente una altura.
                transform.position -= offsetReinicio;

                // Actualizamos la posición inicial
                posicionInicial.y = transform.position.y;
            }
        }
    }
}
