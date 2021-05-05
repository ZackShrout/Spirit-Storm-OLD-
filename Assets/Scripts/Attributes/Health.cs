using SpiritStorm.Saving;
using SpiritStorm.Stats;
using UnityEngine;

namespace SpiritStorm.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = -1f;

        bool isDead = false;

        private void Start()
        {
            GetComponent<BaseStats>().onLevelUp += UpdateFullHealth;
            if (healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }  
        }

        public bool IsDead()
        {
            return isDead;
        }
        
        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if(healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public void GainHealth(GameObject instigator, float healAmount)
        {
            healthPoints = Mathf.Min(healthPoints + healAmount, GetMaxHealth());
        }

        public float GetHealthPoints()
        {
            return healthPoints;
        }

        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void UpdateFullHealth()
        {
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            //trigger death animation
        }

        private void AwardExperience(GameObject instigator)
        {
            AwardEachPartyMemberXP(instigator);
        }

        private void AwardEachPartyMemberXP(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience = null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
        }
    }
}
