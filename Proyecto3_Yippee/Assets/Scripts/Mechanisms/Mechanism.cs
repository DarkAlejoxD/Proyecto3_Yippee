using UnityEngine;
using UnityEngine.Events;
using BaseGame;
using static UtilsComplements.AsyncTimer;

namespace Mechanisms
{
    [ExecuteAlways]
    public class Mechanism : MonoBehaviour, IMechanism, IResetable
    {
        internal enum MechanismType
        {
            On_Off,
            ContinuousSignal,
            JustOn,
            Timed
        }

        [Header("References")]
        private KeyManager _keyManager;

        [Header("Events")]
        [SerializeField] private UnityEvent _activate;
        [SerializeField] private UnityEvent _deactivate;

        [Header("Type")]
        [SerializeField] private MechanismType _mechanismType = MechanismType.JustOn;
        [SerializeField, HideInInspector, Min(0.1f)] private float _time = 0.1f;
        private bool _isActive;
        private bool _lastSignal;

        public bool IsActive => _isActive;

        #region Unity Logic
        private void Awake()
        {
            _isActive = false;
            _lastSignal = false;
            _deactivate?.Invoke();
        }

        private void OnEnable()
        {
            _keyManager = GetComponentInParent<KeyManager>();
            _keyManager.AddToMechanismList(this);
        }

        private void OnDisable()
        {
            _keyManager.RemoveFromMechanismList(this);
        }

        public void Reset()
        {
            _isActive = false;
            _deactivate?.Invoke();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Techinally, this will be called when an interruptor suffer a change of its state, so...
        /// </summary>
        /// <param name="signal"></param>
        public void ReceiveSignal(bool signal)
        {
            switch (_mechanismType)
            {
                case MechanismType.On_Off:
                    {
                        if (!_lastSignal && signal)
                        {
                            if (_isActive)
                            {
                                _deactivate?.Invoke();
                                _isActive = false;
                            }
                            else
                            {
                                _activate?.Invoke();
                                _isActive = true;
                            }
                        }
                        _lastSignal = signal;
                    }
                    break;
                case MechanismType.ContinuousSignal:
                    {
                        if (_isActive)
                        {
                            if (signal) // Unexpected case, but...
                                return;

                            _deactivate?.Invoke();
                            _isActive = false;
                        }
                        else
                        {
                            if (!signal)
                                return;

                            _activate?.Invoke();
                            _isActive = true;
                        }
                    }
                    break;
                case MechanismType.JustOn:
                    {
                        if (_isActive)
                            return;

                        if (!signal)
                            return;

                        _activate?.Invoke();
                        _isActive = true;
                    }
                    break;
                case MechanismType.Timed:
                    {
                        if (_isActive)
                        {
                            if (signal)
                                return;

                            _isActive = false;
                            StartCoroutine(TimerCoroutine(_time, () => _deactivate?.Invoke()));
                        }
                        else
                        {
                            if (!signal)
                                return;

                            StopAllCoroutines();
                            _isActive = true;
                            _activate?.Invoke();
                        }
                    }
                    break;
            }
        }
        #endregion
    }
}

#if UNITY_EDITOR
namespace Mechanisms
{
    using UnityEditor;

    [CustomEditor(typeof(Mechanism))]
    public class MechanismEditor : Editor
    {
        private Mechanism _this;
        private SerializedProperty _type;
        private SerializedProperty _timer;

        private void OnEnable()
        {
            _this = (Mechanism)target;

            _type = serializedObject.FindProperty("_mechanismType");
            _timer = serializedObject.FindProperty("_time");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Mechanism.MechanismType type = (Mechanism.MechanismType)_type.enumValueIndex;

            if (type == Mechanism.MechanismType.Timed)
            {
                _timer.floatValue = EditorGUILayout.FloatField("Timer: ", _timer.floatValue);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif