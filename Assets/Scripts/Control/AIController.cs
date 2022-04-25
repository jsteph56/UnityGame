using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{

    public class AIController : MonoBehaviour
    {
        [SerializeField] float aggroRange = 5f;
        [SerializeField] float suspicionTime = 3f;     
        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;

        Vector3 guardLocation;
        float timeSinceLastAggro = Mathf.Infinity;

        private void Start()
        {
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();

            guardLocation = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;
            
            if (InAttackRange(player) && fighter.CanAttack(player))
            {
                timeSinceLastAggro = 0;
                AttackBehaviour();
            }
            else if (timeSinceLastAggro <= suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                GuardBehaviour();
            }

            timeSinceLastAggro += Time.deltaTime;
        }

        private void GuardBehaviour()
        {
            mover.StartMoveAction(guardLocation);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private bool InAttackRange(GameObject player)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < aggroRange;
        }

        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroRange);
        }
    }
}
