using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string saveFile = "save";

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L)) { Load(); }
            if (Input.GetKeyDown(KeyCode.S)) { Save(); }
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(saveFile);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(saveFile);
        }
    }
}
