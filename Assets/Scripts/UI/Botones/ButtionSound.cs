using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtionSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler 
{

    [Header("Sonido Botones")]
    public AudioSource audioSource;
    public AudioClip sonidoClick;
    public AudioClip sonidoHover;
    
    // Se ejecuta al pasar el mouse por encima
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (sonidoHover != null)
        {
            print("Sonido Hover");
            audioSource.PlayOneShot(sonidoHover);
        }
        print("No entro al IF de onPointerEnter");
    }

    // Se ejecuta al hacer clic
    public void OnPointerClick(PointerEventData eventData)
    {
        if (sonidoClick != null)
        {
            print("Sonido Click");
            audioSource.PlayOneShot(sonidoClick);
        }
        print("No entro al IF de onPointerClick");
    }
}
