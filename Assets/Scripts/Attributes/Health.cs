using UnityEngine;
using RPG.Stats;
using RPG.Core;
using RPG.Saving;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;
        bool isDead = false;
        Vector3 startingPosition;
        Quaternion startingRotation;
        
        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();
        }
        
        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetPercentage()
        {
            return 100 * (healthPoints / GetComponent<BaseStats>().GetHealth());
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience exp = instigator.GetComponent<Experience>();
            if (exp == null)
            {
                Debug.Log("null exp statement");
                return;
            } 

            exp.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
        }

        private void Die()
        {
            if (isDead) return;
            
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            float health = (float) state;
            healthPoints = health;
        }
    }
}