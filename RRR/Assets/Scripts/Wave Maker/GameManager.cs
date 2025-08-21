using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nixian.Waves
{
    public class GameManager : MonoBehaviour
    {
        public List<WaveReciver> goals;

        public GameObject winScreen;
        bool hasFinished = false;

        private void LateUpdate()
        {
            if (goals.All(x => x.isDone) && !hasFinished)
            {
                int high = PlayerPrefs.GetInt("Levels");

                string n = SceneManager.GetActiveScene().name;
                n = n.Substring(n.Length - 3);
                int l = int.Parse(n);
                l++; // Levels start from 0

                if (high <= l)
                {
                    PlayerPrefs.SetInt("Levels", l);
                    PlayerPrefs.Save();
                }

                ClickAndDrag.Instance.canDrag = false;
                winScreen.SetActive(true);

                hasFinished = true;

                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Game/Level Complete");
            }
        }
    }
}