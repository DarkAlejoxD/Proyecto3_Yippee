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
            [SerializeField] internal float _rotationLerp = 5;
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
            public float RotationLerp => _rotationLerp;
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
            [SerializeField] private float _gravityMultiplier = 2.0f;
            [SerializeField] private float _downGravityMultiplier = 1.5f;

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
            public float GravityMultiplier => _gravityMultiplier;
            public float DownGravityMultiplier => _downGravityMultiplier;
        }
        #endregion 
        
        #region Dive Nested Class
        [Serializable]
        public class DiveValues
        {
            [SerializeField] private float _startingSpeed;
            [SerializeField] private float _airDeceleration;
            [SerializeField] private float _groundDeceleration;

            
            public float StartingSpeed => _startingSpeed;
            public float AirDeceleration => _airDeceleration;
            public float GroundDeceleration => _groundDeceleration;
        }
        #endregion

        #region Interaction Nested Class
        [Serializable]
        public class InteractionValues
        {
            [SerializeField] private float _interactionRange;
            //[SerializeField] private float _interactionCooldown;


            public float InteractionRange => _interactionRange;
            //public float InteractionCooldown => _interactionCooldown;
            
        }
        #endregion

        private const int SPACES = 6;
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

        [SerializeField, Space(SPACES)] DiveValues _diveValues;
        
        public JumpValues DefaultJumpValues => _jumpValues;
        public DiveValues DefaultDiveValues => _diveValues;

        [Space]
        [Header("Interaction Attributes")]
        [SerializeField, Space(SPACES)] InteractionValues _interactionValues;
        public InteractionValues DefaultInteractionValues => _interactionValues;
        

        #endregion
    }
}
