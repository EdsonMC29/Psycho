using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [Header("Configuración de Límites")]
    public int limitePastillasSobredosis = 15; // Final C: Sobredosis 
    public int limiteAsesinatosMonstruo = 5;   // Final A: Asesino

    [Header("Nombres Exactos de las Escenas")]
    public string escenaFinalA_Monstruo = "Final_A";
    public string escenaFinalB_Escape = "Final_B";
    public string escenaFinalC_Sobredosis = "Final_C";

    [Header("Referencia")]
    // Aquí arrastras el objeto que tenga el script PlayerStats (el falso o el real)
    public PlayerStats statdelJugador;

    void Update()
    {
        // FINAL C: SOBREDOSIS (Colapso mental inmediato)
        if (statdelJugador != null && statdelJugador.totalPastillasConsumidas >= limitePastillasSobredosis){
            //EjecutarFinal(escenaFinalC_Sobredosis);
            Debug.Log("Ejecutando el final C: sobredosis");
        }
    }

    public void EvaluarFinalDeNivel(){
        if (statdelJugador == null){
            Debug.LogError("¡Falta asignar el script PlayerStats en el inspector!");
            return;
        }

        Debug.Log("Evaluando final de nivel...");

        // FINAL A: MONSTRUO
        if (statdelJugador.personasAsesinadas >= limiteAsesinatosMonstruo){
            //EjecutarFinal(escenaFinalA_Monstruo);
            Debug.Log("Ejecutando el final A: Monstruo asesino");
        }
        // FINAL B: CONTROL / ESCAPE
        else{
            //EjecutarFinal(escenaFinalB_Escape);
            Debug.Log("Ejecutando el final B: escape controlado");
        }
    }

    private void EjecutarFinal(string escena){
        if (SceneManager.GetActiveScene().name != escena)
        {
            Debug.Log($"CARGANDO FINAL: {escena}");
            SceneManager.LoadScene(escena);
        }
    }
}