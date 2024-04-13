using System;
using UnityEngine;

namespace AvatarController.Data
{
    [CreateAssetMenu(fileName = "New PlayerData", menuName = "GameData/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        #region Player Movement Nested Class
        [Serializable]
        public class PlayerMovementData
        {
            [Header("Linear Attributes")]
            [SerializeField] internal float _acceleration = 20;
            [SerializeField] internal float _linearDecceleration = 10;
            [SerializeField] internal float _minSpeedToMove = 2;
            [SerializeField] internal float _maxSpeed = 5;
            //[SerializeField] internal bool _canSprint = true;
            //[SerializeField] internal float _sprintMaxSpeed = 5;
            [SerializeField] internal bool _canJump = true;

            [Header("Rotation Attributes")]
            [SerializeField, Min(0.1f)] internal float _angularAcceleration;
            [SerializeField] internal float _maxAngularSpeed;

            public float Acceleration => _acceleration;
            public float LinearDecceleration => _linearDecceleration;
            public float MinSpeedToMove => _minSpeedToMove;
            public float MaxSpeed => _maxSpeed;
            //public bool CanSprint => _canSprint;
            //public float SprintMaxSpeed => _sprintMaxSpeed;
            public bool CanJump => _canJump;
        }
        #endregion

        #region Jump Nested Class
        [Serializable]
        public class JumpValues
        {
            [SerializeField, HideInInspector, Min(0.1f)] private float _minHeight;
            [SerializeField, Min(0.1f)] private float _maxHeight;
            [SerializeField, Min(0.1f), HideInInspector] private float _timePressed;
            [SerializeField, Min(0.1f), HideInInspector] private float _timeToReachHeight;
            [SerializeField, Min(0.01f)] private float _coyoteTime;

            [Header("DEBUG")]
            public bool DEBUG_drawHeight;
            public bool DEBUG_drawCurve;
            [Range(0, 50)] public int DEBUG_definitionOfJump;
            [Tooltip("Draws the definition depending on the percentage of the max speed the player has")]
            [Range(0, 1)] public float DEBUG_forwardMovementPct;
            [HideInInspector] public bool DEBUG_drawCoyote;

            public float MinHeight => _minHeight;
            public float MaxHeight => _maxHeight;
            public float TimePressed => _timePressed;
            public float TimeToReachHeight => _timeToReachHeight;
            public float CoyoteTime => _coyoteTime;
        }
        #endregion

        private const int SPACES = 10;
        #region Movement Fields
        [Header("Movement Attributes")]
        [SerializeField, Space(SPACES)] private PlayerMovementData _defaultMovement;
        [SerializeField, Space(SPACES), HideInInspector] private PlayerMovementData _pushingMovement;
        [SerializeField, Space(SPACES)] private PlayerMovementData _crounchMovement;
        [SerializeField, Space(SPACES)] private PlayerMovementData _onAirMovement;

        public PlayerMovementData DefaultMovement => _defaultMovement;
        public PlayerMovementData PushingMovement => _pushingMovement;
        public PlayerMovementData CrounchMovement => _crounchMovement;
        public PlayerMovementData OnAirMovement => _onAirMovement;
        #endregion

        #region JumpValues
        [SerializeField, Space(SPACES)] JumpValues _jumpValues;

        public JumpValues DefaultJumpValues => _jumpValues;
        #endregion
    }
}
