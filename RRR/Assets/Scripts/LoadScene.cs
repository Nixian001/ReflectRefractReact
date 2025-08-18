using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    static public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void LoadScene2(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    static public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#elif UNITY_WEBGL
        Application.OpenURL("https://nixian001.itch.io/rrr");
#elif UNITY_STANDALONE_WIN
        Application.Quit();
#endif
    }
}
