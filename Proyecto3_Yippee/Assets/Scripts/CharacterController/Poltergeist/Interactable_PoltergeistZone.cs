using UnityEngine;
using AvatarController;
using Interactable;

namespace Poltergeist
{
    [RequireComponent(typeof(Collider))]
    public class Interactable_PoltergeistZone : Interactable_Base
    {
        #region Fields
        [Header("References")]
        [SerializeField] private PoltergeistObject _objectAttached;
        #endregion

        #region Unity Logic
        protected override void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }
        #endregion

        #region Public Methods
        public override void Interact()
        {
            base.Interact();
            var playerPolt = GameManager.Player.GetComponent<PlayerPoltergeist>();
            playerPolt.TryEnterPoltergeist(this);
        }
        #endregion
    }
}
