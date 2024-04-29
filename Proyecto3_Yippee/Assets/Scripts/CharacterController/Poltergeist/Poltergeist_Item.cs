using UnityEngine;
using UtilsComplements;

namespace Poltergeist
{
    [DisallowMultipleComponent]
    public class Poltergeist_Item : MonoBehaviour
    {
        private void OnEnable()
        {
            if (Singleton.TryGetInstance(out PoltergeistManager manager))
            {
                manager.AddPoltergeist(this);
            }
        }

        private void OnDisable()
        {
            if (Singleton.TryGetInstance(out PoltergeistManager manager))
            {
                manager.RemovePoltergeist(this);
            }
        }
    }
}