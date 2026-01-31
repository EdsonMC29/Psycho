using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{

    public void IrAEscena()
    {
        SceneManager.LoadScene("Escena_cambio");
    }

    public void IrHabitacion()
    {
        SceneManager.LoadScene("Habitacion");
    }

    public void IrAlNivel()
    {
        SceneManager.LoadScene("Nivel1");
    }
}

