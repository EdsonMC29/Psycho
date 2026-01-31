using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemporizadorBarras : MonoBehaviour
{
    public enum EstadoBarra
    {
        Inactivo,
        Duracion,
        Cooldown
    }

    [SerializeField] private Image image;

    private EstadoBarra estado = EstadoBarra.Inactivo;
    private float tiempoMaximo;
    private float tiempoActual;

    public bool EstaOcupado => estado != EstadoBarra.Inactivo;

    private void Update()
    {
        if (estado == EstadoBarra.Inactivo) return;

        if (estado == EstadoBarra.Duracion)
            BajarBarra();

        if (estado == EstadoBarra.Cooldown)
            SubirBarra();
    }

    void BajarBarra()
    {
        tiempoActual -= Time.deltaTime;
        image.fillAmount = tiempoActual / tiempoMaximo;

        if (tiempoActual <= 0)
        {
            tiempoActual = 0;
            image.fillAmount = 0;
            estado = EstadoBarra.Inactivo;
        }
    }

    void SubirBarra()
    {
        tiempoActual += Time.deltaTime;
        image.fillAmount = tiempoActual / tiempoMaximo;

        if (tiempoActual >= tiempoMaximo)
        {
            tiempoActual = tiempoMaximo;
            image.fillAmount = 1;
            estado = EstadoBarra.Inactivo;
        }
    }

    public void IniciarDuracion(float duracion)
    {
        tiempoMaximo = duracion;
        tiempoActual = duracion;
        image.fillAmount = 1;
        estado = EstadoBarra.Duracion;
    }

    public void IniciarCooldown(float cooldown)
    {
        tiempoMaximo = cooldown;
        tiempoActual = 0;
        image.fillAmount = 0;
        estado = EstadoBarra.Cooldown;
    }
}
