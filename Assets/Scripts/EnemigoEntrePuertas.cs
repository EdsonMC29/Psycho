using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemigoEntrePuertas : MonoBehaviour
{
    [Header("Configuración de Puertas")]
    public List<Transform> listaDePuertas; 

    [Header("Configuración de Movimiento")]
    public float velocidad = 3f;
    public float alturaFijaY = -3.5f;

    [Header("Configuración de Tiempo Aleatorio")]
    [Tooltip("Tiempo mínimo que esperará (en segundos)")]
    public float tiempoMinimo = 1f; 
    
    [Tooltip("Tiempo máximo que esperará (en segundos)")]
    public float tiempoMaximo = 4f; 

    // Componentes internos
    private SpriteRenderer spriteRenderer;
    private Collider2D col2D; 

    // Usamos Awake para obtener referencias. Awake se ejecuta una sola vez al cargar la escena,
    // lo cual es mejor para el rendimiento que hacerlo cada vez que se activa.
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col2D = GetComponent<Collider2D>();
    }

    // OnEnable se ejecuta CADA VEZ que el objeto se activa (SetActive true)
    void OnEnable()
    {
        // 1. Reseteamos la visibilidad al iniciar (oculto hasta que elija puerta)
        AlternarVisibilidad(false);

        // 2. Comprobación de seguridad
        if (listaDePuertas == null || listaDePuertas.Count < 2)
        {
            Debug.LogError($"⛔ ERROR: El objeto '{gameObject.name}' no tiene suficientes puertas asignadas.");
            this.enabled = false; // Se apaga el script si no hay puertas
            return;
        }

        // 3. Iniciamos el ciclo de nuevo desde cero
        StartCoroutine(CicloDeMovimiento());
    }

    // OnDisable se ejecuta cuando el objeto se apaga
    void OnDisable()
    {
        // Detenemos todas las corrutinas para que no intenten mover al objeto
        // mientras está apagado o cuando se vuelva a encender.
        StopAllCoroutines();
    }

    IEnumerator CicloDeMovimiento()
    {
        // Pequeña espera inicial opcional para asegurar que todo cargó
        yield return null;

        while (true)
        {
            // --- FASE 1: ELEGIR CAMINO ---
            
            // Elegir inicio
            int indiceInicio = Random.Range(0, listaDePuertas.Count);
            Transform puertaInicio = listaDePuertas[indiceInicio];

            // Elegir destino
            int indiceDestino = indiceInicio;
            int intentos = 0;
            while (indiceDestino == indiceInicio && intentos < 100)
            {
                indiceDestino = Random.Range(0, listaDePuertas.Count);
                intentos++;
            }
            Transform puertaDestino = listaDePuertas[indiceDestino];

            // Validar
            if (puertaInicio == null || puertaDestino == null)
            {
                yield return null; 
                continue; 
            }

            // --- FASE 2: APARECER Y MOVERSE ---

            Vector2 posicionDeInicio = new Vector2(puertaInicio.position.x, alturaFijaY);
            Vector2 posicionDeDestino = new Vector2(puertaDestino.position.x, alturaFijaY);

            // Teletransportamos al inicio
            transform.position = posicionDeInicio;
            
            // Hacemos visible al enemigo
            AlternarVisibilidad(true);

            // Bucle de movimiento
            while (Vector2.Distance(transform.position, posicionDeDestino) > 0.1f)
            {
                MirarHaciaObjetivo(posicionDeDestino);
                transform.position = Vector2.MoveTowards(transform.position, posicionDeDestino, velocidad * Time.deltaTime);
                yield return null; 
            }

            // --- FASE 3: ESPERA Y REINICIO ---

            AlternarVisibilidad(false);

            float tiempoAleatorio = Random.Range(tiempoMinimo, tiempoMaximo);
            yield return new WaitForSeconds(tiempoAleatorio);
        }
    }

    void MirarHaciaObjetivo(Vector3 destino)
    {
        if (spriteRenderer == null) return;

        if (destino.x > transform.position.x) spriteRenderer.flipX = false; 
        else spriteRenderer.flipX = true;
    }

    void AlternarVisibilidad(bool estado)
    {
        if(spriteRenderer != null) spriteRenderer.enabled = estado;
        if (col2D != null) col2D.enabled = estado;
    }
}