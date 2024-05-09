using System;
using UnityEngine;

namespace Mechanism
{
    [DisallowMultipleComponent]
    public abstract class AbsInterruptor : MonoBehaviour
    {
        protected enum InterruptorStyle
        {
            Once,
            On_Off,
            Repeat
        }

        protected bool Activated { get; private set; }
        protected abstract InterruptorStyle InterStyle { get; }

        public Action Activate;
        public Action Deactivate;

        private void Start()
        {
            Activated = false;
        }

        protected void KeyPressed()
        {
            switch (InterStyle)
            {
                case InterruptorStyle.Once:
                    {
                        if (Activated)
                            return;

                        Activated = true;
                        Activate?.Invoke();
                    }
                    break;
                case InterruptorStyle.On_Off:
                    {
                        if (Activated)
                        {
                            Activated = false;
                            Deactivate?.Invoke();
                        }
                        else
                        {
                            Activated = true;
                            Activate?.Invoke();
                        }
                    }
                    break;
                case InterruptorStyle.Repeat:
                    {
                        Activate?.Invoke();
                    }
                    break;
            }
        }
    }
}
