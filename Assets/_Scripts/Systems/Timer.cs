using System;
using System.Collections;
using UnityEngine;

namespace _Scripts.Systems
{
    public class Timer : MonoBehaviour
    {
        public int timer;

        public Action<int> onTimerChanged;
        public Action onTimesUp;

        public void StartTimer(int time) => StartCoroutine(StartCountDownCoroutine(time));

        private IEnumerator StartCountDownCoroutine(int time)
        {
            timer = time;
            while (timer > 0)
            {
                yield return new WaitForSeconds(1f);
                timer--;
                onTimerChanged.Invoke(timer);
            }
            onTimesUp.Invoke();
        }
    }
}