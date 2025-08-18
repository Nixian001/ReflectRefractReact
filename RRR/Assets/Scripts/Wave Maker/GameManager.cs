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

        private void LateUpdate()
        {
            if (goals.All(x => x.isDone))
            {
                int high = PlayerPrefs.GetInt("Levels");

                string n = SceneManager.GetActiveScene().name;
                n = n.Substring(n.Length - 3);
                int l = int.Parse(n);
                l++; // Levels start from 0

                if (high !>= l)
                {
                    PlayerPrefs.SetInt("Levels", l);
                    PlayerPrefs.Save();
                }


                winScreen.SetActive(true);
            }
        }
    }
}