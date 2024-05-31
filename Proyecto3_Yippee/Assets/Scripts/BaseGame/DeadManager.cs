using AudioController;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UtilsComplements;

namespace BaseGame //add it to a concrete namespace
{
    public class DeadManager : MonoBehaviour, ISingleton<DeadManager>
    {
        #region Fields
        [Header("References")]
        [SerializeField] private Image _image;

        [Header("Attributes")]
        [SerializeField, Min(0.1f)] private float _appearTime;
        [SerializeField] private AnimationCurve _appearCurve;
        [SerializeField, Min(0.1f)] private float _stayTime;
        [SerializeField] private AnimationCurve _stayCurve;
        [SerializeField, Min(0.1f)] private float _disappearTime;
        [SerializeField] private AnimationCurve _disappearCurve;

        public ISingleton<DeadManager> Instance => this;
        #endregion

        #region Unity Logic
        private void Awake() => Instance.Instantiate();

        private void Start() => _image.enabled = false;

        private void OnDestroy() => Instance.RemoveInstance();
        #endregion

        #region Static Methods
        public static void ActivateDead()
        {
            if (!Singleton.TryGetInstance(out DeadManager manager))
            {
                GameManager.ResetGame();
                return;
            }

            manager.StartCoroutine(manager.DeadCoroutine());
        }
        #endregion

        #region Private Methods


        private IEnumerator DeadCoroutine()
        {
            Color color = _image.color;
            color.a = 0;
            _image.color = color;
            _image.enabled = true;

            GameManager.Player?.BlockMovement();

            #region AppearLogic
            for (float i = 0; i <= _appearTime; i += Time.deltaTime)
            {
                color.a = _appearCurve.Evaluate(i / _appearTime);
                _image.color = color;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            #endregion

            #region Make ReappearLogic
            float halfStay = _stayTime / 2;

            if(Singleton.TryGetInstance(out AudioManager audioMan))
            {
                //TODO
                //audioMan.PlayOneShot("", );
            }
            yield return new WaitForSeconds(halfStay);

            GameManager.ResetGame();

            yield return new WaitForSeconds(halfStay);
            #endregion

            #region Dissapear
            for (float i = 0; i <= _disappearTime; i += Time.deltaTime)
            {
                color.a = _appearCurve.Evaluate(i / _disappearTime);
                _image.color = color;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            #endregion
            GameManager.Player?.UnBlockMovement();
        }
        #endregion
    }
}
