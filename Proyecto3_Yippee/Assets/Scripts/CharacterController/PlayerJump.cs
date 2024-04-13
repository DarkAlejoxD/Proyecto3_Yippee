using UnityEngine;
using AvatarController.Data;
using UtilsComplements.Editor;

namespace AvatarController
{
    [RequireComponent(typeof(PlayerController), typeof(CharacterController))]
    public class PlayerJump : MonoBehaviour
    {
        //TODO: Get Design Specifications: For proto regular jump, for >=alpha mantein to higher
        //TODO: Make this class.

        #region Fields
        [Header("References")]
        private PlayerController _controller;
        private CharacterController _characterController;

        [Header("Contro")]
        private float _velocityY;
        private bool _onGround;
        private float _lastTimeInGround;
        private bool _jumped;

        private PlayerData DataContainer => _controller.DataContainer;
        private float Gravity => Physics.gravity.y;
        #endregion

        #region Unity Logic
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            if (_controller == null)
                _controller = GetComponent<PlayerController>();

            _controller.OnJump += OnJump;
        }

        private void OnDisable()
        {
            if (_controller == null)
                _controller = GetComponent<PlayerController>();

            _controller.OnJump -= OnJump;
        }
        #endregion

        #region Public Methods

        private void Update()
        {
            UpdateVy();
        }
        #endregion

        #region Private Methods
        private void UpdateVy()
        {
            float variation = _velocityY * Time.deltaTime;

            CollisionFlags movement = _characterController.Move(new Vector3(0, variation, 0));

            if (movement == (CollisionFlags.Above))
            {
                _velocityY = 0;
            }

            if (movement == CollisionFlags.Below)
            {
                _onGround = true;
                _lastTimeInGround = Time.time;
                _jumped = false;
                _velocityY = 0;
            }
            else
            {
                _onGround = false;
            }

            _velocityY += Gravity * Time.deltaTime;
        }

        private void OnJump(bool active)
        {
            if (!active)
                return;

            if (!CanJump())
                return;

            Jump();
        }

        private bool CanJump()
        {
            if (_onGround)
                return true;

            if (_jumped)
                return false;

            if (Time.time <= _lastTimeInGround + DataContainer.DefaultJumpValues.CoyoteTime)
                return true;

            return false;
        }

        private void Jump()
        {
            _velocityY = GetVelocity();
            _jumped = true;

            #region DEBUG
#if UNITY_EDITOR
            lastPos = transform.position;
            lastDir = transform.forward;
#endif
            #endregion
        }

        private float GetVelocity()
        {
            //VelocityCalculus
            //v^2 - (v0)^2 = 2 * a * Dx
            //V^2 = 0; v0 = ?; a = gravity; Dx = Data
            //v0 = maths.sqrt ( -2 * a * Dx )

            float vel = -2 * Gravity * DataContainer.DefaultJumpValues.MaxHeight;
            return Mathf.Sqrt(Mathf.Abs(vel));


            //OtherVelocityCalculus
            //v = v0 + gt
            //0 = v0 + gt
            //v0 = -gt

            //y = y0 + v0 * t + a/2 * t^2
            //y=Data; y0 = 0; v0=? 
            //return -Gravity * DataContainer.DefaultJumpValues.TimeToReachHeight;
        }
        #endregion

        #region DEBUG
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_controller == null)
            {
                _controller = GetComponent<PlayerController>();
            }

            DrawHeight();
            DrawCurve();
            DrawCoyoteCurve();
        }

        private void DrawHeight()
        {
            GizmosUtilities.DrawSphereProperties sphereProperties =
                new(GizmosUtilities.DrawSphereProperties.DefaultProperty)
                {
                    Radius = 0.3f
                };

            Vector3 height = lastPos + Vector3.up * DataContainer.DefaultJumpValues.MaxHeight;
            Color color = Color.green;
            GizmosUtilities.DrawSphere(height, color, sphereProperties,
                                       DataContainer.DefaultJumpValues.DEBUG_drawHeight);
        }

        #region Curve
        Vector3 lastPos = Vector3.zero;
        Vector3 lastDir = Vector3.zero;

        private void DrawCurve()
        {
            if (Application.isPlaying)
            {
                if (lastPos == Vector3.zero)
                    lastPos = transform.position;
                if (lastDir == Vector3.zero)
                    lastDir = transform.forward;
            }
            else
            {
                lastDir = transform.forward;
                lastPos = transform.position;
            }

            Color color = Color.blue;

            //Time calculus
            //v = v0 + a * t
            //0 = vel + g * t --> t = vel / g
            float time = Mathf.Abs(GetVelocity() / Gravity);

            GizmosUtilities.DrawCurveProperties curveProperties =
                new(GizmosUtilities.DrawCurveProperties.DefaultValues)
                {
                    MinValue = 0,
                    MaxValue = time * 2,
                    DefinitionOfCurve = DataContainer.DefaultJumpValues.DEBUG_definitionOfJump
                };

            GizmosUtilities.DrawCurve(Curve, color, curveProperties,
                                      DataContainer.DefaultJumpValues.DEBUG_drawCurve);
        }

        private void DrawCoyoteCurve()
        {
            //throw new NotImplementedException();
        }

        private Vector3 Curve(float time)
        {
            Vector3 xzPos = Vector3.zero;
            Vector3 yPos = Vector3.zero;

            #region Y Axis
            float vel = GetVelocity();

            float y = lastPos.y + vel * time + Gravity / 2 * time * time;
            yPos = new(0, y, 0);
            #endregion

            #region XY Axis
            float speed = DataContainer.DefaultMovement.MaxSpeed *
                          DataContainer.DefaultJumpValues.DEBUG_forwardMovementPct;

            Vector3 direction = lastDir;
            Vector3 pos = lastPos;

            xzPos = pos + speed * time * direction;
            #endregion

            return xzPos + yPos;
        }

        #endregion
#endif
        #endregion
    }
}

