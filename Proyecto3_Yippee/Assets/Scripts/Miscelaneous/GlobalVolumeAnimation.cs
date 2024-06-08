using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Miscelaneous
{
    [ExecuteAlways]
    [RequireComponent(typeof(Animator))]
    public class GlobalVolumeAnimation : MonoBehaviour
    {
        #region Fields
        [Header("Global Volume")]
        [SerializeField] private Volume _globalVolume;
        private ColorAdjustments _gvColorAdjust;

        [Header("Color Adjusment Control")]
        [SerializeField] private ClampedFloatParameter _contrast;

        private ColorAdjustments GlobalColorAdjust
        {
            get
            {
                if (_gvColorAdjust == null)
                {
                    if (_globalVolume.profile.TryGet(out _gvColorAdjust))
                        return _gvColorAdjust;
                }
                return _gvColorAdjust;
            }
        }

        #endregion    

        #region Unity Logic
        private void Awake()
        {
        }

        private void OnValidate()
        {
            UpdateGlobalVolumeValues();
        }

        private void Update()
        {

        }

        private void UpdateGlobalVolumeValues()
        {
            if(GlobalColorAdjust != null)
            {
            GlobalColorAdjust.contrast = new(10, -100, 100);
                //Write some code here
            }

        }

        private void OnAnimatorIK(int layerIndex)
        {
            UpdateGlobalVolumeValues();
        }
        #endregion
    }
}
