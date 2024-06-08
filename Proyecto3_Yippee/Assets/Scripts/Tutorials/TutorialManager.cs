using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using BaseGame;
using InputController;
using UtilsComplements;

namespace Tutorials
{
    /// <summary>
    /// It's a bit hardcoded this class, but I think it will acomplish its duty.
    /// </summary>
    public class TutorialManager : MonoBehaviour, ISingleton<TutorialManager>
    {
        private enum ControllerStyle
        {
            Keyboard,
            Gamepad
        }

        #region Fields
        [Header("Tutorial InputReference")]
        [SerializeField] private GameObject _keyboardPanelRef;
        [SerializeField] private GameObject _controllerPanelRef;

        [Header("Dive InputReference")]
        [SerializeField] private GameObject _diveKeyRef;
        [SerializeField] private GameObject _diveConRef;

        [Header("Climb InputReference")]
        [SerializeField] private GameObject _climbKeyRef;
        [SerializeField] private GameObject _climbConRef;

        [Header("PhantomVision InputReference")]
        [SerializeField] private GameObject _ghostKeyRef;
        [SerializeField] private GameObject _ghostConRef;

        [Header("Poltergeist InputReference")]
        [SerializeField, Tooltip("Activation")] private GameObject _polter1KeyRef;
        [SerializeField, Tooltip("Activation")] private GameObject _polter1ConRef;
        [SerializeField, Tooltip("Selection")] private GameObject _polter2KeyRef;
        [SerializeField, Tooltip("Selection")] private GameObject _polter2ConRef;
        [SerializeField, Tooltip("Movement")] private GameObject _polter3KeyRef;
        private bool _polterMode = false;
        private int _controlIndex = 0;

        [Header("Attributes")]
        [SerializeField, Min(0.1f)] private float _timePressedThreshold = 2;
        private float _timeControl = 0;

        private List<GameObject> _allTutorials;
        private bool _isAppearing = false;

        private Menus _menuInputs;
        private ControllerStyle _controlStyle = ControllerStyle.Keyboard;
        private ControllerStyle _lastStyle = ControllerStyle.Keyboard;

        public ISingleton<TutorialManager> Instance => this;
        #endregion

        #region Unity Logic
        private void Awake()
        {
            Instance.Instantiate();
            _keyboardPanelRef.SetActive(false);
            _controllerPanelRef.SetActive(false);

            _menuInputs = new();
            _menuInputs.Tutorials.Enable();
            _polterMode = false;

            _allTutorials = new List<GameObject>();

            _allTutorials.Add(_keyboardPanelRef);
            _allTutorials.Add(_controllerPanelRef);

            _allTutorials.Add(_diveKeyRef);
            _allTutorials.Add(_diveConRef);

            _allTutorials.Add(_climbKeyRef);
            _allTutorials.Add(_climbConRef);

            _allTutorials.Add(_ghostKeyRef);
            _allTutorials.Add(_ghostConRef);

            _allTutorials.Add(_polter1KeyRef);
            _allTutorials.Add(_polter1ConRef);
            _allTutorials.Add(_polter2KeyRef);
            _allTutorials.Add(_polter2ConRef);
            _allTutorials.Add(_polter3KeyRef);

            Deactivate();
        }

        private void Update()
        {
            //First check if the Tutorials are Active
            if (!_isAppearing)
                return;

            CheckControllerStyleUpdate();
            CheckTriggerUpdate();
        }

        private void OnDestroy() => Instance.RemoveInstance();
        #endregion

        #region Public Methods
        public void TUTORIAL_ActivateDive()
        {
            _diveKeyRef.SetActive(true);
            _diveConRef.SetActive(true);

            Activate();
        }
        public void TUTORIAL_ActivateClimb()
        {
            _climbKeyRef.SetActive(true);
            _climbConRef.SetActive(true);
            Activate();
        }
        public void TUTORIAL_ActivateGhostMode()
        {
            _ghostKeyRef.SetActive(true);
            _ghostConRef.SetActive(true);
            Activate();
        }
        public void TUTORIAL_ActivatePoltergeist()
        {
            _controlIndex = 1;
            _polterMode = true;
            _polter1KeyRef.SetActive(true);
            _polter2ConRef.SetActive(true);
            Activate();
        }
        #endregion

        #region Private Methods
        private void Activate()
        {
            _isAppearing = true;
            gameObject.SetActive(true);

            if (_lastStyle == ControllerStyle.Gamepad)
                _controllerPanelRef.SetActive(true);
            else
                _keyboardPanelRef.SetActive(true);

            GameManager.GetGameManager().PlayerInstance?.BlockMovement();
            PauseManager.SetCanPause(false);

            _timeControl = Time.time;
            Time.timeScale = 0;
            //Debug.Log("ActivateTutorialCanavs");
        }

        private void Deactivate()
        {
            foreach (var item in _allTutorials)
            {
                item.SetActive(false);
            }
            gameObject.SetActive(false);
            _isAppearing = false;

            GameManager.GetGameManager().PlayerInstance?.UnBlockMovement();
            PauseManager.SetCanPause(true);
            Time.timeScale = 1;
        }

        private void CheckTriggerUpdate()
        {
            if (Time.time < _timeControl + _timePressedThreshold)
                return;

            //Check if the Button was pressed
            bool wasTriggered = _menuInputs.Tutorials.PassTutorial.WasPressedThisFrame();
            if (!wasTriggered)
                return;

            //Check if the tutorials are the Poltergeist Tutorials;
            if (_polterMode)
            {
                switch (_controlIndex)
                {
                    case 1:
                        _controlIndex = 2;

                        _polter1KeyRef.SetActive(false);
                        _polter1ConRef.SetActive(false);

                        _polter2KeyRef.SetActive(true);
                        _polter2ConRef.SetActive(true);
                        break;
                    case 2:
                        _controlIndex = 3;

                        _polter2KeyRef.gameObject.SetActive(false);
                        _polter2ConRef.gameObject.SetActive(false);

                        if (_controlStyle == ControllerStyle.Keyboard)
                            _polter3KeyRef.SetActive(true);
                        else
                            Deactivate();
                        break;
                    case 3:
                        Deactivate();
                        break;
                }
            }
            //If not just Deactivate();
            else
                Deactivate();
        }

        private void CheckControllerStyleUpdate()
        {
            if (!_isAppearing)
                return;

            switch (_controlStyle)
            {
                case ControllerStyle.Keyboard:
                    {
                        bool triggered = _menuInputs.Tutorials.GamepadHandler.WasPressedThisFrame();
                        if (triggered)
                            _controlStyle = ControllerStyle.Gamepad;
                    }
                    break;
                case ControllerStyle.Gamepad:
                    {
                        bool triggered = _menuInputs.Tutorials.KeyboardHandler.WasPressedThisFrame();
                        if (triggered)
                            _controlStyle = ControllerStyle.Keyboard;
                    }
                    break;
            }
        }

        //private void ChangeInputStyle()
        //{
        //    if (Time.time <= _timeControl + _changeCD)
        //        return;

        //    switch (_controlStyle)
        //    {
        //        case ControllerStyle.Keyboard:
        //            if (_lastStyle == ControllerStyle.Gamepad)
        //            {
        //                _timeControl = Time.time;
        //                _lastStyle = ControllerStyle.Keyboard;

        //                _controllerPanelRef.SetActive(false);
        //                _keyboardPanelRef.SetActive(true);
        //            }
        //            break;
        //        case ControllerStyle.Gamepad:
        //            if (_lastStyle == ControllerStyle.Keyboard)
        //            {
        //                _timeControl = Time.time;
        //                _lastStyle = ControllerStyle.Gamepad;
        //                _keyboardPanelRef.SetActive(false);
        //                _controllerPanelRef.SetActive(true);
        //            }
        //            break;
        //    }
        //}
        #endregion
    }
}
