using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public void Home()
    {
        SceneLoader.LoadScene("MainMenu");
    }
    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    public void Next()
    {
        string n = SceneManager.GetActiveScene().name;
        n = n.Substring(n.Length - 3);
        int l = int.Parse(n);
        l++;

        try
        {
            SceneManager.LoadSceneAsync($"Level_{l.ToString().PadLeft(3, '0')}");
        }
        catch
        {
            SceneManager.LoadSceneAsync("EndOfDemo");    
        }
    }
}
