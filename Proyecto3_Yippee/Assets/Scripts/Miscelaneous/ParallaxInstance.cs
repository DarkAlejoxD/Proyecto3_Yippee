using System.Collections.Generic;
using UnityEngine;

namespace BaseGame //add it to a concrete namespace
{
    public class ParallaxInstance : MonoBehaviour
    {
        #region Fields
        [Header("References")]
        [SerializeField] private Transform _maximumHandler;
        [SerializeField] private List<Transform> _dayVariations;
        [SerializeField] private List<Transform> _nightVariations;

        private Transform _currentVariation;
        public Vector3 Distance
        {
            get
            {
                if (!_maximumHandler)
                {
                    Debug.LogError("Check the maximumHandler, it's not assigned");
                    return Vector3.one;
                }

                return _maximumHandler.position - transform.position;
            }
        }
        #endregion    

        #region Unity Logic
        private void Awake()
        {
        }

        private void Update()
        {
        }
        #endregion

        #region Static Methods
        public static void StaticMethod()
        {
        }
        #endregion

        #region Public Methods
        public void ActivateRandomVariation(bool isDay)
        {
            _currentVariation?.gameObject.SetActive(false);

            if (isDay)
            {
                int variations = _dayVariations.Count;
                int randomIndex = Random.Range(0, variations);
                _currentVariation = _dayVariations[randomIndex];
            }
            else
            {
                int variations = _nightVariations.Count;
                int randomIndex = Random.Range(0, variations);
                _currentVariation = _nightVariations[randomIndex];
            }

            _currentVariation?.gameObject.SetActive(true);
        }
        #endregion

        #region Private Methods
        private void PrivateMethod()
        {
        }
        #endregion
    }
}
