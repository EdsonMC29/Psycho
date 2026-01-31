using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [Header("Configuración de Escena")]
    public string firstLevelName = "Comic";
    public void PlayGame()
    {

        // Solo llamamos a la transición, ella se encarga de
        TransicionEscenasUI.Instance.DisolverSalida(firstLevelName);
    }
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void LoadScene(string sceneName)
    {
        TransicionEscenasUI.Instance.DisolverSalida(sceneName);
    }
}