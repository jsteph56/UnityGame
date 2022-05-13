using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] float expPoints = 0;

        public void GainExperience(float exp)
        {
            expPoints += exp;
        }

        public float GetExpPoints()
        {
            return expPoints;
        }
    }
}
