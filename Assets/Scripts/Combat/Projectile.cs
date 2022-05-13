using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float projectileSpeed = 1;
        [SerializeField] float lifeAfterImpact = .1f;
        [SerializeField] float lifetime = 10f;
        [SerializeField] bool isHoming = false;
        Health target = null;
        GameObject instigator = null;
        float damage = 0;
        
        // Update is called once per frame
        private void Start()
        {
            if (target == null) return;
            transform.LookAt(GetAimLocation());
        }
        
        void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead()) { transform.LookAt(GetAimLocation()); }

            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.damage = damage;
            this.target = target;
            this.instigator = instigator;

            Destroy(gameObject, lifetime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            
            if (targetCapsule == null) { return target.transform.position; }
            return target.transform.position + ((Vector3.up * targetCapsule.height) / 2);
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }
            
            target.TakeDamage(instigator, damage);
            projectileSpeed = 0;
            foreach (GameObject destroy in destroyOnHit) { Destroy(destroy); }
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
