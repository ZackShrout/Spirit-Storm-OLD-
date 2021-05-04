using UnityEngine;
using System.Collections;
using System;
using SpiritStorm.Saving;

namespace SpiritStorm.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        public event Action onExperienceGained;

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                GainExperience(5f);
            }
        }
#endif

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
        }

        public float GetPoints()
        {
            return experiencePoints;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}