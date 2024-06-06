using InputController;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
        [SerializeField, Tooltip("Movement")] private GameObject _polter3ConRef;
        private bool _polterMode = false;
        private int _controlIndex = 0;

        private List<GameObject> _allTutorials;
        private bool _isAppearing = false;

        private Menus _menuInputs;
        private ControllerStyle _controlStyle = ControllerStyle.Keyboard;

        public ISingleton<TutorialManager> Instance => this;
        #endregion

        #region Unity Logic
        private void Awake()
        {
            Instance.Instantiate();
            InputSystem.onDeviceChange += CheckControllerStyleUpdate;
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
            _allTutorials.Add(_polter3ConRef);

            Deactivate();
        }

        private void Update()
        {
            //First check if the Tutorials are Active
            if (!_isAppearing)
                return;

            CheckTriggerUpdate();
        }

        private void OnDestroy()
        {
            InputSystem.onDeviceChange -= CheckControllerStyleUpdate;
            Instance.RemoveInstance();
        }
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
            gameObject.SetActive(true);
            _isAppearing = true;
        }

        private void Deactivate()
        {
            foreach (var item in _allTutorials)
            {
                item.SetActive(false);
            }
            gameObject.SetActive(false);
            _isAppearing = false;
        }

        private void CheckTriggerUpdate()
        {
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

                        _polter1KeyRef.gameObject.SetActive(false);
                        _polter1ConRef.gameObject.SetActive(false);

                        _polter2KeyRef.SetActive(true);
                        _polter2ConRef.SetActive(true);
                        break;

                    case 2:
                        _controlIndex = 3;

                        _polter2KeyRef.gameObject.SetActive(false);
                        _polter2ConRef.gameObject.SetActive(false);

                        _polter3KeyRef.SetActive(true);
                        _polter3ConRef.SetActive(true);
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

        private void CheckControllerStyleUpdate(InputDevice inputDevice, InputDeviceChange change)
        {
            if (!_isAppearing)
                return;

        }
        #endregion
    }
}
