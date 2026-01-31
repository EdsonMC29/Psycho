using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransicionEscenasUI : MonoBehaviour
{
    public static TransicionEscenasUI Instance;
    [Header("Disolver")]

    public CanvasGroup disolverCanvasGroup;
    public float tiempoDisolverEntrada;
    public float tiempoDisolverSalida;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        DisolverEntrada();
    }
    private void DisolverEntrada()
    {
        LeanTween.alphaCanvas(disolverCanvasGroup, 0f, tiempoDisolverEntrada).setOnComplete(() =>
        {
            disolverCanvasGroup.blocksRaycasts = false;
            disolverCanvasGroup.interactable = false;
        });
    }
    // Sobrecarga para INDEX (números)
    public void DisolverSalida(int indexEscena)
    {
        IniciarAnimacionSalida(() =>
        {
            SceneManager.LoadScene(indexEscena);
        });
    }

    // Sobrecarga para NOMBRE (strings)
    public void DisolverSalida(string nombreEscena)
    {
        IniciarAnimacionSalida(() =>
        {
            SceneManager.LoadScene(nombreEscena);
        });
    }

    // Método privado que contiene la lógica de LeanTween
    private void IniciarAnimacionSalida(System.Action accionAlTerminar)
    {
        disolverCanvasGroup.blocksRaycasts = true;
        disolverCanvasGroup.interactable = true;

        LeanTween.alphaCanvas(disolverCanvasGroup, 1f, tiempoDisolverSalida)
            .setIgnoreTimeScale(true) // Importante para que funcione desde el menú de pausa
            .setOnComplete(() =>
            {
                accionAlTerminar?.Invoke();
            });
    }
}
