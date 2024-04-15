using AvatarController;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable //add it to a concrete namespace
{
//[RequireComponent(typeof(Transform))] //Add this if necessary, delete it otherwise
    public class Interactable_Pushable : Interactable_Base
    {
        [SerializeField] private Transform[] _pushPoints;
        private PlayerController _player;

        #region Public Methods

        public override void Interact()
        {
            base.Interact();

            //TODO: Select closest push point and tp player to it
            Transform t;

            //TODO: Block horizontal movement
            //TODO: Block Jump
            //TODO: Set parent to player
            //TODO: Limites?
            
            

            

        }

        #endregion

        #region Private Methods

        private int GetClosestPushPoint()
        {
            return MathUtils.GetClosestPoint(_player.transform.position, _pushPoints);
        }

        #endregion

    }
}
