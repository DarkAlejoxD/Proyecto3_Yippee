using System;
using Unity.Collections;
using UnityEngine;
using UtilsComplements;

namespace GhostView //add it to a concrete namespace
{
    public class GhostViewManager : MonoBehaviour, ISingleton<GhostViewManager>
    {
        #region Fields

        [Serializable]
        public class PrototypeColorValues
        {
            public Color AppearColor;
            public Color DissapearColor;

            [Min(0.1f)] public float AppearTime;
            [Min(0.1f)] public float Staytime;
            [Min(0.1f)] public float DisapearTime;
        }

        public ISingleton<GhostViewManager> Instance => this;

        public static Action<Vector3, float> OnActivateGhostView;

        public PrototypeColorValues _proto;
        public static PrototypeColorValues Proto
        {
            get
            {
                if (!ISingleton<GhostViewManager>.TryGetInstance(out var manager))
                    return null;
                return manager._proto;
            }
        }
        #endregion

        #region Unity Logic
        private void Awake() => Instance.Instantiate();

        private void OnDestroy() => Instance.RemoveInstance();
        #endregion

        #region Static Methods
        public static void RequestGhostView(Vector3 origin, float radius)
        {
            if (!ISingleton<GhostViewManager>.TryGetInstance(out var manager))
                return;

            manager.ActivateGhostView(origin, radius);
        }
        #endregion

        #region Public Methods
        public void ActivateGhostView(Vector3 origin, float radius)
        {
            OnActivateGhostView?.Invoke(origin, radius);
        }
        #endregion
    }
}
