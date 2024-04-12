using System;
using UnityEngine;

namespace AvatarController.Data
{
    [CreateAssetMenu(fileName = "New PlayerData", menuName = "GameData/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        private const int SPACES = 10;
        #region Movement Fields
        [Header("Movement Attributes")]
        [SerializeField, Space(SPACES)] private PlayerMovementData _defaultMovement;
        [SerializeField, Space(SPACES)] private PlayerMovementData _pushingMovement;
        [SerializeField, Space(SPACES)] private PlayerMovementData _crounchMovement;
        [SerializeField, Space(SPACES)] private PlayerMovementData _onAirMovement;

        public PlayerMovementData DefaultMovement => _defaultMovement;
        public PlayerMovementData PushingMovement => _pushingMovement;
        public PlayerMovementData CrounchMovement => _crounchMovement;
        public PlayerMovementData OnAirMovement => _onAirMovement;
        #endregion
    }

    [Serializable]
    public class PlayerMovementData
    {
        [SerializeField] internal float _acceleration = 20;
        [SerializeField] internal float _linearDecceleration = 10;
        [SerializeField] internal float _minSpeedToMove = 2;
        [SerializeField] internal float _maxSpeed = 5;
        //[SerializeField] internal bool _canSprint = true;
        //[SerializeField] internal float _sprintMaxSpeed = 5;
        [SerializeField] internal bool _canJump = true;

        public float Acceleration => _acceleration;
        public float LinearDecceleration => _linearDecceleration;
        public float MinSpeedToMove => _minSpeedToMove;
        public float MaxSpeed => _maxSpeed;
        //public bool CanSprint => _canSprint;
        //public float SprintMaxSpeed => _sprintMaxSpeed;
        public bool CanJump => _canJump;
    }
}
