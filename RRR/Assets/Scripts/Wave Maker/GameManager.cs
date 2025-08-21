using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Nixian.Waves
{
    public class GameManager : MonoBehaviour
    {
        public List<WaveReciver> goals;

        public float finishCooldown = 0f;

        public Button win;
        public Slider completeSlider;
        bool hasFinished = false;

        private void LateUpdate()
        {
            if (goals.All(x => x.isDone))
            {
                finishCooldown += Time.deltaTime;

                if (!hasFinished && finishCooldown > 1.0f)
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
                    win.interactable = true;

                    hasFinished = true;

                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Game/Level Complete");
                }
            }
            else
            {
                finishCooldown -= Time.deltaTime;
                finishCooldown = Mathf.Clamp01(finishCooldown);
            }

            completeSlider.value = finishCooldown;
        }
    }
}