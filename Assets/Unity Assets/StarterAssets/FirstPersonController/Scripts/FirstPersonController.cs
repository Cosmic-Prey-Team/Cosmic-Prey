using System.Collections;
using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
	[RequireComponent(typeof(PlayerInput))]
#endif

	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;
		[Tooltip("The height the player can jump in SPACE")]
		public float SpaceJumpHeight = 3.0f;
		[Tooltip("The gravity value in outer space. Should still fall down a bit, but very slowly")]
		public float SpaceGravity = -15f;
		private float CurrentGravity;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;
		public float JetpackTimeout = 0.20f;
		private bool hasJumpedJetpack = false;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		[Header("Movement on Ship")]
		[SerializeField] ShipController _ship;
		private MovePlayerWithShip movePlayerWithShip;

		[SerializeField] private bool _canMove = true;
		[SerializeField] private bool _atHelm = false;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		public static bool triggerJump;

		Vector3 inputDirection;

        /*[Header("Jetpack/Space Movement")]
		public float JetPackAcceleration = 0.2f; //accelerates 1 unit per second, per second
		public float MaxJetpackVelocity = 0.6f;

		private float _jetpackPushTimer = 5f;
        private Vector2 _jetpackVelocity;
        private Vector3 _lastVel;
        private Vector3 mv; //movement velocity for jetpack
        private float _jetpackTimeoutDelta;*/


        // timeout deltatime
        public static float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		private PlayerState _playerState;

	
#if ENABLE_INPUT_SYSTEM
		private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		private InputHandler _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM
				return _playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}

			_playerState = FindObjectOfType<PlayerState>();
			movePlayerWithShip = _ship.GetComponent<MovePlayerWithShip>();
		}

		private void Start()
		{
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<InputHandler>();
#if ENABLE_INPUT_SYSTEM
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
			//_jetpackTimeoutDelta = JetpackTimeout;
		}

		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			Move();
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				if (_atHelm)
                {
					//transform.Rotate(shipController.rotateSpeed.normalized * _rotationVelocity);
				} 
				else
                {
					transform.Rotate(Vector3.up * _rotationVelocity);
				}
				
			}
		}

		public static float targetSpeed;
		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            //if (movePlayerWithShip.onShip) targetSpeed += _ship.velocity.z;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			
			

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
			
			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}
			
			inputDirection = new Vector3(0f, 0f, 0f);
			if (_canMove)
			{
				
				// normalise input direction
				/*Vector3*/ inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

				// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
				// if there is a move input rotate player when the player is moving
				if (_input.move != Vector2.zero)
				{
					// move
					inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
				}
			}
			// move the player
			//_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

			if (movePlayerWithShip.onShip)
            {
				//CurrentGravity = Gravity;
				//_playerState.SwitchState(ControlState.FirstPerson);
				if (inputDirection.normalized == new Vector3(0f, 0f, 0f))
                {
					_controller.Move(new Vector3(0.0f, _verticalVelocity * Time.deltaTime + _ship.velocity.y, 0.0f));
					//_controller.Move(_ship.transform.forward * _ship.velocity.z);
					
					/*if(_ship.rotating)
						transform.RotateAround(_ship.transform.position, Vector3.up*//*new Vector3(0, 1, 0)*//*, _ship.rotateSpeed);*/
				}
					
                else
                {
					_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + (new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime));
				}
				
			}else
            {
				CurrentGravity = SpaceGravity;

				_playerState.SwitchState(ControlState.SpaceMovement);
			}
		}

		private void JumpAndGravity()
		{
			if (Grounded && _canMove)
			{
				if (!_atHelm)
					_playerState.SwitchState(ControlState.FirstPerson);
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;
				//_jetpackTimeoutDelta = JetpackTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
					
				}
				//Debug.Log(_verticalVelocity);
				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
					Debug.Log("Regular Jump");
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else if (Grounded && !movePlayerWithShip.onShip) //if not on the ship, but maybe an asteroid or another "ground", jump with lower gravity
            {
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout * 3f;
				//_jetpackTimeoutDelta = JetpackTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump (in space)
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(SpaceJumpHeight * -2f * SpaceGravity);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else if (!Grounded && !movePlayerWithShip.onShip)
			{
				_playerState.SwitchState(ControlState.SpaceMovement);
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;
				//_jetpackTimeoutDelta = JetpackTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				if (movePlayerWithShip.onShip)
					_input.jump = false;
			}

			if (movePlayerWithShip.onShip)
            {
				if (_verticalVelocity < _terminalVelocity)// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
				{
					_verticalVelocity += Gravity * Time.deltaTime;
				}
			}
            // else if (!movePlayerWithShip.onShip)
            //{
            //	_verticalVelocity = SpaceGravity;
            //	if (_verticalVelocity < _terminalVelocity)
            //	{
            //		_verticalVelocity += SpaceGravity * Time.deltaTime;
            //	}
            //}

        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}

		public void EnterControlShip()
		{
			_canMove = false;
			_atHelm = true;
			_playerState.SwitchState(ControlState.Ship);
		}

		public void EnterControlPlayer()
		{
			_canMove = true;
			_atHelm = false;
			_playerState.SwitchState(ControlState.FirstPerson);

		}

		/*//private IEnumerator JetpackPush()
        //{
		//	while (_jetpackPushTimer > 0)
        //    {
		//		_controller.Move(inputDirection.normalized * ((_speed * _jetpackPushTimer) * Time.deltaTime) + (new Vector3(0.0f, _verticalVelocity / 5, 0.0f) * Time.deltaTime));
		//		//Jump();
		//		Debug.Log("Tried to jump in space");
		//		_jetpackPushTimer -= 1;
		//		//if (_input.jump)
		//		//	break;
		//
		//	}
		//	yield return new WaitForSeconds(1f);
		//
		//	_jetpackPushTimer = 5f;
		//}
*/
		/*private void JetpackMovement()
        {
			mv = _lastVel;
			if (_input.jump)
			{
				//mv.y += JetPackAcceleration * Time.deltaTime;
				mv.x = inputDirection.normalized.x;
				mv.z = inputDirection.normalized.z;
				Debug.Log("Blasting off...");
				//if (mv.y > MaxJetpackVelocity) mv.y = MaxJetpackVelocity;
				if (mv.x > MaxJetpackVelocity) mv.x = MaxJetpackVelocity;
				if (mv.z > MaxJetpackVelocity) mv.z = MaxJetpackVelocity;
				hasJumpedJetpack = true;
			}
			else
            {
				mv.y += SpaceGravity * Time.deltaTime;
				_verticalVelocity += SpaceGravity * Time.deltaTime;


				Debug.Log("APPLYING GRAVITY");
				hasJumpedJetpack = false;
			}


			//Debug.Log(mv);
			_controller.Move(inputDirection.normalized + mv * Time.deltaTime);
			_lastVel = _controller.velocity;
        }*/

	}
}