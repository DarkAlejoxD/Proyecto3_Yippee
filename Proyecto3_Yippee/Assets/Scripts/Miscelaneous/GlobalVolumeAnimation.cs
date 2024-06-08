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
        [SerializeField, Range(-100, 100)] private float _contrast;

        private Animator _animator;

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

        private Animator AnimController
        {
            get
            {
                if (_animator == null)
                    _animator = GetComponent<Animator>();

                return _animator;
            }
        }

        #endregion    

        #region Unity Logic //Dont Touch
        private void OnValidate()
        {
            UpdateGlobalVolumeValues();
        }

        private void Update()
        {
            if (UpdateAnimatorValues())            
                UpdateGlobalVolumeValues();            
        }
        #endregion

        #region Animator Logic //Do touch
        private bool UpdateAnimatorValues()
        {
            bool anyChange = false;            

            const string CONTRAST_VALUE = "Contrast";

            float lastContrastValue = AnimController.GetFloat(CONTRAST_VALUE);
            if (lastContrastValue != _contrast)
            {
                AnimController.SetFloat(CONTRAST_VALUE, _contrast);
                anyChange = true;
            }

            /*
            const string ANIM_VALUE = "[Parameter name]";
            float lastParamName = AnimController.GetFloat(ANIM_VALUE);
            if (lastParamName != _newParam)
            {
                AnimController.SetFloat(ANIM_VALUE, _newParam);
                anyChange = true;
            }
            */
            return anyChange;
        }

        private void UpdateGlobalVolumeValues()
        {
            if (GlobalColorAdjust != null)
            {
                GlobalColorAdjust.contrast.Override(_contrast);
                //Write some code here
            }

            Debug.Log("Something Changed");
        }
        #endregion
    }
}
