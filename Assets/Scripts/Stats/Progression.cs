using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progress", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;
        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass currClass in characterClasses)
            {
                if (currClass.characterClass == characterClass)
                {
                    return currClass.health[level - 1];
                }
            }

            return 0;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public float[] health;
 
        }
    }
}
