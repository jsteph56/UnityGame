using System;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;

namespace RPG.Control
{

    public class AIController : MonoBehaviour
    {
        [SerializeField] float aggroRange = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float dwellTime = 3f;
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        
        [SerializeField] PatrolPath patrolPath; 


        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;

        Vector3 guardLocation;
        Quaternion startingRotation;
        float timeSinceLastAggro = Mathf.Infinity;
        float timeSpentDwelling = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Start()
        {
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();

            guardLocation = transform.position;
            startingRotation = transform.rotation;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRange(player) && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastAggro <= suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                IdleBehaviour();
            }

            UpdateTimers();
        }

        private void IdleBehaviour()
        {
            if (patrolPath == null)
            {
                guardBehaviour();
            }
            else
            {
                patrolBehaviour();
            }
        }

        private void UpdateTimers()
        {
            timeSinceLastAggro += Time.deltaTime;
            timeSpentDwelling += Time.deltaTime;
        }

        private void patrolBehaviour()
        {
            Vector3 nextPosition = guardLocation;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSpentDwelling = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            
            if (timeSpentDwelling > dwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private void guardBehaviour()
        {
            
            mover.StartMoveAction(guardLocation, patrolSpeedFraction);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastAggro = 0;
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
