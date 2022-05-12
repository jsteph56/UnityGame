using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentPrefab;
        static bool hasSpawned;

        private void Awake()
        {
            if (hasSpawned) return;
            
            SpawnPersistentObject();
            hasSpawned = true;
        }

        private void SpawnPersistentObject()
        {
            GameObject persistentObject = Instantiate(persistentPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}