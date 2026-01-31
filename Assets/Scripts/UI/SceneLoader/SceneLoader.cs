using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    public Animator transitionAnimator;
    public float transitionTime = 1f;

    private void Awake()
    {
        // Configuración del Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(Transition(nextSceneIndex));
    }

    public void LoadLevelByName(string name)
    {
        StartCoroutine(Transition(name));
    }

    IEnumerator Transition(int levelIndex)
    {
        // 1. Iniciar animación de salida
        transitionAnimator.SetTrigger("StartFade");
        
        // 2. Esperar a que termine la animación
        yield return new WaitForSeconds(transitionTime);
        
        // 3. Cargar la escena
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator Transition(string levelName)
    {
        transitionAnimator.SetTrigger("StartFade");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelName);
    }
}