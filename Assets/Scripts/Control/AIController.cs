using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{

    public class AIController : MonoBehaviour
    {
        [SerializeField] float aggroRange = 5f;
        Fighter fighter;
        Health health;
        GameObject player;

        private void Start()
        {
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (health.IsDead()) return;
            
            if (InAttackRange(player) && fighter.CanAttack(player))
            {
                fighter.Attack(player);
            }
            else
            {
                fighter.Cancel();
            }
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
