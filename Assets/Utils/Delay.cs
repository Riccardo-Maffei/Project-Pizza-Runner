using System;
using System.Collections;
using UnityEngine;


namespace Utils {
    public class Delay : MonoBehaviour
    {
        private static Delay _instance;

        private static Delay Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var delayObject = new GameObject("Delay");
                _instance = delayObject.AddComponent<Delay>();
                DontDestroyOnLoad(delayObject);

                return _instance;
            }
        }

        public static void BySeconds(Action function, float seconds)
        {
            Instance.StartCoroutine(DelayByInstruction(function, new WaitForSeconds(seconds)));
        }

        private static IEnumerator DelayByInstruction(Action function, YieldInstruction instruction)
        {
            yield return instruction;
            function();
        }
    }
}
