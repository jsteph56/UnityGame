using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
       [SerializeField] float weaponRange = 2f;
       [SerializeField] float weaponSpeed = 1.21f;
       [SerializeField] float weaponDamage = 5f;
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null || target.IsDead()) return;
            
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            
            if (timeSinceLastAttack > weaponSpeed)
            {
                //This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Event
        void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) <= weaponRange;
        }

        public bool CanAttack(GameObject currentTarget)
        {
            if (currentTarget == null) return false;

            Health testTarget = currentTarget.GetComponent<Health>(); 
            return !testTarget.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}