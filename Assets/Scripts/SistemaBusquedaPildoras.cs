using UnityEngine;
using System.Collections.Generic; // Necesario para usar Listas

public class SistemaBusquedaPildoras : MonoBehaviour
{
    [Header("Referencias")]
    public SanitySystem sanitySystem; // Arrastra aquí tu script de cordura
    public GameObject prefabPildora;  // Arrastra aquí el prefab con el script 'EfectoPildora'

    [Header("Objetos Interactuables")]
    [Tooltip("Arrastra aquí tus 5 objetos de la escena")]
    public List<ContenedorBusqueda> listaDeObjetos; 

    void Start()
    {
        InicializarBusqueda();
    }

    void InicializarBusqueda()
    {
        if (listaDeObjetos.Count < 5)
        {
            Debug.LogWarning("Se recomienda tener al menos 5 objetos en la lista.");
        }

        // 1. Limpiamos cualquier estado previo
        foreach (var obj in listaDeObjetos)
        {
            obj.tienePildora = false;
            obj.yaFueRevisado = false;
            obj.prefabPildoraVisual = prefabPildora; // Le pasamos la referencia del prefab
            obj.sistemaCordura = sanitySystem;       // Le pasamos la referencia de cordura
        }

        // 2. Barajamos la lista (Algoritmo Fisher-Yates) para que sea aleatorio
        // Esto desordena la lista temporalmente para elegir al azar
        for (int i = 0; i < listaDeObjetos.Count; i++)
        {
            ContenedorBusqueda temp = listaDeObjetos[i];
            int randomIndex = Random.Range(i, listaDeObjetos.Count);
            listaDeObjetos[i] = listaDeObjetos[randomIndex];
            listaDeObjetos[randomIndex] = temp;
        }

        // 3. Asignamos TRUE a los primeros 3 objetos de la lista barajada
        // Como la lista está desordenada, siempre serán objetos distintos.
        int cantidadPildoras = 3;
        
        for (int i = 0; i < listaDeObjetos.Count; i++)
        {
            if (i < cantidadPildoras)
            {
                listaDeObjetos[i].tienePildora = true;
                // Debug.Log(listaDeObjetos[i].name + " tiene pildora."); // Descomentar para hacer trampas y probar
            }
        }
    }
}