using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public List<Button> buttons;

    void Start()
    {
        int highLev = 0;

        if (PlayerPrefs.HasKey("Levels"))
        {
            highLev = PlayerPrefs.GetInt("Levels");
        }
        else
        {
            PlayerPrefs.SetInt("Levels", 0);
            PlayerPrefs.Save();
        }

        buttons.ForEach(x => x.interactable = false);

        for (int i = 0; i <= highLev; i++)
        {
            buttons[i].interactable = true;
            buttons[i].onClick.AddListener(delegate { LoadLevel(i-1); } );
        }
    }

    private void LoadLevel(int lvl)
    {
        SceneLoader.LoadScene($"Level_{lvl.ToString().PadLeft(3, '0')}");
    }
}
