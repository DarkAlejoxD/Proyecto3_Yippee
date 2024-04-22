using UnityEngine;

namespace InputController
{
    public struct InputValues
    {
        public Vector2 MoveInput { get; internal set; }
        public bool JumpInput { get; internal set; }
        public bool CrounchDiveInput { get; internal set; }
        public bool InteractInput { get; internal set; }
        public bool GhostViewInput { get; internal set; }
        public bool SprintInput { get; internal set; }
        public bool CancelInput { get; internal set; }

        public void ResetInputs()
        {
            MoveInput = Vector2.zero;
            JumpInput = false;
            CrounchDiveInput = false;
            InteractInput = false;
            GhostViewInput = false;
            SprintInput = false;
            CancelInput = false;
        }
    }
}
