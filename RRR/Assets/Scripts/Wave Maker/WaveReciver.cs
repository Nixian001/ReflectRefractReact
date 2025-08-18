using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Nixian.Waves 
{
    public class WaveReciver : MonoBehaviour
    {
        public List<Wave> expected;
        public List<Wave> given;
        public bool isDone;

        private void Start()
        {
            expected.Sort();
        }

        private void Update()
        {
            given = given.Distinct().ToList();
            given.Sort();

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
