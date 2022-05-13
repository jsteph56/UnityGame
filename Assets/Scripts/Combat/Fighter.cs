using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using RPG.Saving;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
       [SerializeField] Transform rightHandTransform = null;
       [SerializeField] Transform leftHandTransform = null;
       [SerializeField] Weapon defaultWeapon = null;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        Weapon equippedWeapon = null;
        
        private void Start()
        {
            if (equippedWeapon == null) { EquipWeapon(defaultWeapon); }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null || target.IsDead()) return;
            
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }
        public void EquipWeapon(Weapon weapon)
        {
            equippedWeapon = weapon;

            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            
            if (timeSinceLastAttack > equippedWeapon.GetAttackSpeed())
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
            
            if (equippedWeapon.HasProjectile())
            {
                equippedWeapon.FireProjectile(rightHandTransform, leftHandTransform, target, gameObject);
            }
            else target.TakeDamage(gameObject, equippedWeapon.GetDamage());
        }

        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) <= equippedWeapon.GetRange();
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
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public object CaptureState()
        {
            return equippedWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string) state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}