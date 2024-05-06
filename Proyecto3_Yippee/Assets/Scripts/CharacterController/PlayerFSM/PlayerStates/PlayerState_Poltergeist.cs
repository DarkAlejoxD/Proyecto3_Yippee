﻿using UnityEngine;
using Unity.VisualScripting;
using AvatarController.Data;
using InputController;
using Poltergeist;
using UtilsComplements;
using static UtilsComplements.AsyncTimer;

namespace AvatarController.PlayerFSM
{
    using FSM;

    /// <summary>
    /// This class duty is to check the inputs and the poltergeist states
    /// </summary>
    public class PlayerState_Poltergeist : PlayerState
    {
        private enum PoltergeistStates
        {
            Selecting,
            Manipulating
        }

        private enum SelectState
        {
            None,
            Selected,
            Mantein,
            ManteinSelected
        }

        private const float SELECTION_TIME_IN_BETWEEN = 0.5f;
        private const float WAIT_MANTEIN = 1f;

        private PlayerPoltergeist _poltergeistController;
        private PoltergeistManager _poltManager;

        private PoltergeistStates _currentState;
        private FSM_Default<SelectState> _selectionBrain;

        private bool _canChangeState = false;
        private float _inputHandler;
        private float _timeControl;

        private Poltergeist_Item Item
        {
            get => _poltergeistController.Item;
            set => _poltergeistController.Item = value;
        }

        public override string Name => "Poltergeist";

        public PlayerState_Poltergeist(PlayerController playerController) : base(playerController)
        {
            _currentState = PoltergeistStates.Selecting;
            SelectionFSMInit();

            if (!playerController.TryGetComponent(out _poltergeistController))
            {
                _poltergeistController = playerController.AddComponent<PlayerPoltergeist>();
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _currentState = PoltergeistStates.Selecting;
            _selectionBrain.ForceChange(SelectState.None);

            _canChangeState = false;
            _playerController.StartCoroutine(
                TimerCoroutine(_playerController.DataContainer.DefPoltValues.PoltergeistSpamCD,
                () => _canChangeState = true));

            _playerController.StartPoltergeist();

            CameraPolter.ActivatePolterMode();

            Item = null;

            if (Singleton.TryGetInstance(out PoltergeistManager manager))
            {
                _poltManager = manager;
            }
            _poltManager.StartPoltergeist(_playerController.transform,
                                         Data.DefPoltValues.PoltergeistRadius);
        }

        public override void OnPlayerStay(InputValues inputs)
        {
            switch (_currentState)
            {
                case PoltergeistStates.Selecting:
                    Vector2 input = inputs.PoltergeistXZAxis;

                    if (input.magnitude < 0)
                    {
                        _inputHandler = 0;
                    }
                    else
                    {
                        float dot = Vector2.Dot(input, Vector2.right);
                        if (input.magnitude < 1)
                            dot *= input.magnitude;

                        _inputHandler = dot;
                    }

                    _selectionBrain.OnStay();
                    Debug.Log(_selectionBrain.Name);

                    if (Item == null)
                        return;

                    if (inputs.SelectDeselectInput)
                    {
                        _currentState = PoltergeistStates.Manipulating;
                        Item.Manipulate();
                    }
                    break;
                case PoltergeistStates.Manipulating:
                    _poltergeistController.PoltergeistModeUpdate(inputs.PoltergeistXZAxis,
                                                                 inputs.PoltergeistYAxis);

                    if (inputs.SelectDeselectInput)
                    {
                        _currentState = PoltergeistStates.Selecting;
                        _poltManager.UpdateOrder(Item);
                        Item.NoManipulating();
                    }
                    break;
            }

            //_playerController.OnGhostView?.Invoke(inputs.GhostViewInput);??

            if (inputs.Cancel)
            {
                _playerController.RequestChangeState(PlayerStates.OnGround);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            CameraPolter.DeactivatePolterMode();
            _playerController.EndPoltergeist();
            _poltManager.EndPoltergeist();
        }

        public override bool CanAutoTransition()
        {
            return _canChangeState;
        }

        private void PickNewItem()
        {
            int dir = _inputHandler > 0 ? 1 : -1;
            Item = _poltManager.GetNext(dir);
        }

        private void SelectionFSMInit()
        {
            _selectionBrain = new();

            State none = new(() =>
            { }, () =>
            { }, () =>
            { }, "NONE");

            State selected = new(() =>
            { _timeControl = 0; }, () =>
            { _timeControl += Time.deltaTime; }, () =>
            { }, "Selected");

            State mantein = new(() =>
            { _timeControl = 0; }, () =>
            { _timeControl += Time.deltaTime; }, () =>
            { }, "Mantein");

            //This state is a "virtual state" due to My FSM cannot transition to the same state
            State manteinSelected = new(() =>
            { }, () =>
            { }, () =>
            { }, "ManteinSelected");

            Transition pressed = new(() =>
            {
                if (Mathf.Abs(_inputHandler) > Data.DefPoltValues.JoystickThreshold)
                    return true;

                return false;
            }, PickNewItem);

            Transition released = new(() =>
            {
                if (Mathf.Abs(_inputHandler) < Data.DefPoltValues.JoystickThreshold)
                    return true;

                return false;
            });
            Transition stillPressed = new(() =>
            {
                return _timeControl > WAIT_MANTEIN;
            }, PickNewItem);
            Transition manteinPressed = new(() =>
            {
                return _timeControl > SELECTION_TIME_IN_BETWEEN;
            }, PickNewItem);
            Transition manteinReset = new(() =>
            {
                return true;
            });

            _selectionBrain.SetRoot(SelectState.None, none);
            _selectionBrain.AddState(SelectState.Selected, selected);
            _selectionBrain.AddState(SelectState.Mantein, mantein);
            _selectionBrain.AddState(SelectState.ManteinSelected, manteinSelected);

            _selectionBrain.AddAutoTransition(SelectState.None, pressed, SelectState.Selected);
            _selectionBrain.AddAutoTransition(SelectState.Selected, stillPressed, SelectState.Mantein);
            _selectionBrain.AddAutoTransition(SelectState.Mantein, manteinPressed, SelectState.ManteinSelected);
            _selectionBrain.AddAutoTransition(SelectState.ManteinSelected, manteinReset, SelectState.Mantein);

            _selectionBrain.AddAutoTransition(SelectState.Selected, released, SelectState.None);
            _selectionBrain.AddAutoTransition(SelectState.Mantein, released, SelectState.None);

            _selectionBrain.OnEnter();
        }
    }
}