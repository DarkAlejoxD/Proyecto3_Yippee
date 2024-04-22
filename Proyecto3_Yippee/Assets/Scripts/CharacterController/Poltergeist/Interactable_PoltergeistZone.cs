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
        [SerializeField] private Rigidbody _objectAttached;

        public Rigidbody ObjectAttached => _objectAttached;
        public bool StartedUseGravity { get; private set; }
        #endregion

        #region Unity Logic
        protected override void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
            StartedUseGravity = _objectAttached.useGravity;
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