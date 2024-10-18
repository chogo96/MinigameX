using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WNC.HYPEPOLY
{
    public class AnimationScenarist : MonoBehaviour
    {
        public List<float> values;
        public List<string> names;
        public ParticleSystem eventPS;
        Animator animator;
        float startDelay;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            if (animator == null) animator = GetComponentInChildren<Animator>();

            if (names.Count > 0)
            {
                for (int i = 0; i < names.Count; i++)
                {
                    if (names[i] == "Delay")
                    {
                        startDelay = values[i];
                        Invoke("SetAnimatorEnabled", values[i]);
                    }
                    if (names[i] == "Timer")
                    {
                        Invoke("SetTimer", values[i] + startDelay);
                    }
                    else animator.SetFloat(names[i], values[i]);
                }
            }
        }
        private void FixedUpdate()
        {
            if (names.Count > 0)
            {
                for (int i = 0; i < names.Count; i++)
                {
                    if (names[i] != "Delay" && names[i] != "Timer") animator.SetFloat(names[i], values[i]);
                }
            }
        }
        void SetAnimatorEnabled()
        {
            animator.SetBool("Enabled", true);
        }
        void SetTimer()
        {
            animator.SetTrigger("Timer");
            Invoke("SetTimer", values[names.IndexOf("Timer")]);
        }
        public void EventPS()
        {
            eventPS.Play();
        }
    }
}