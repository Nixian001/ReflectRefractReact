using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.VisualScripting;

namespace Nixian.Waves 
{
    public class WaveReciver : MonoBehaviour
    {
        public List<Wave> expected;
        public List<Wave> given;
        public bool isDone;

        private void Start()
        {
            expected = BubbleSort(expected);
        }

        public static List<Wave> BubbleSort(List<Wave> waves)
        {
            int i, j;
            int N = waves.Count;

            for (j = N - 1; j > 0; j--)
            {
                for (i = 0; i < j; i++)
                {
                    if ((int)waves[i].color > (int)waves[i + 1].color)
                    {
                        var temp = waves[i];
                        waves[i] = waves[i + 1];
                        waves[i + 1] = temp;
                    }
                }
            }

            return waves;
        }

        private void Update()
        {
            given = given.Distinct().ToList();
            given = BubbleSort(given);

            if (expected.Count != given.Count)
            {
                isDone = false;
            }
            else
            {
                for (int i = 0; i < expected.Count; i++)
                {
                    if (expected[i] != given[i])
                    {
                        isDone = false;
                        given.Clear();
                        return;
                    }
                }
                isDone = true;
            }
            given.Clear();
        }
    }
}
