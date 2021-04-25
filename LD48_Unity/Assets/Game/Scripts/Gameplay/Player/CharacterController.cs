using DUCK.Tween;
using LD48.Gameplay.Camera;
using LD48.Save;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LD48.Gameplay.Player
{
	public class CharacterController : MonoBehaviour
	{
		public static CharacterController Instance { get; private set; }

		[Header("References")]
		[SerializeField]
		private Rigidbody rigidbody;

		[SerializeField]
		private UnityEngine.Camera camera;
		public static UnityEngine.Camera Camera => Instance.camera;

		[Header("Config")]
		[SerializeField]
		private float movementSpeed = 2f;

		[SerializeField]
		private float movementUpMultiplier = 0.5f;

		[SerializeField]
		private float lookSpeed = 10f;

		[SerializeField]
		private float lookDamp = 10f;

		[SerializeField]
		private float boostStrength = 10f;

		[SerializeField]
		private float boostCooldown = 3f;

		[SerializeField]
		private float cameraModifier = 0.5f;

		[SerializeField]
		private Vector2 lookClamp = new Vector2(90f, 90f);

		[SerializeField]
		private float cameraDampMovement = 0.2f;

		[SerializeField]
		private float[] swimSpeedUpdateModifiers = { };

		private InputAction movementAction;
		private InputAction boostAction;
		private InputAction verticalMovementAction;
		private InputAction lookAction;

		private Vector3 movementInput;
		private Vector3 rotationInput;

		private CustomAnimation dragBoostAnimation;

		private float xRotation;
		private Vector3 cameraVelocity;
		private float lastBoostTime;

		private void Start()
		{
			Instance = this;

			transform.position = SaveData.Instance.PlayerData.PlayerPosition;
			transform.rotation = SaveData.Instance.PlayerData.PlayerRotation;
			camera.transform.rotation = SaveData.Instance.PlayerData.CameraRotation;

			//Input Setup
			var map = new InputActionMap("Character Controller");

			lookAction = map.AddAction("look", binding: "<Mouse>/delta");
			movementAction = map.AddAction("move", binding: "<Gamepad>/leftStick");

			boostAction = map.AddAction("boost", binding: "<Keyboard>/leftShift");

			lookAction.AddBinding("<Gamepad>/rightStick").WithProcessor("scaleVector2(x=15, y=15)");

			movementAction.AddCompositeBinding("Dpad")
				.With("Up", "<Keyboard>/w")
				.With("Down", "<Keyboard>/s")
				.With("Left", "<Keyboard>/a")
				.With("Right", "<Keyboard>/d");

			verticalMovementAction = map.AddAction("Vertical Movement");
			verticalMovementAction.AddCompositeBinding("Dpad")
				.With("Up", "<Keyboard>/space")
				.With("Up", "<Gamepad>/buttonSouth");

			boostAction.performed += HandleBoost;

			movementAction.Enable();
			lookAction.Enable();
			verticalMovementAction.Enable();
			boostAction.Enable();

			dragBoostAnimation = new CustomAnimation(UpdateDrag, 0f, rigidbody.drag, boostCooldown);
		}

		private void Update()
		{
			//Input
			var movementDelta = movementAction.ReadValue<Vector2>();

			movementInput.x = movementDelta.x;
			movementInput.z = movementDelta.y;
			movementInput.y = verticalMovementAction.ReadValue<Vector2>().y * movementUpMultiplier;

			var calculatedSpeed = movementSpeed;

			if (PictureTaker.IsActive)
			{
				calculatedSpeed *= cameraModifier;
			}

			calculatedSpeed *= swimSpeedUpdateModifiers[SaveData.Instance.UpgradeData.SwimSpeed];

			rigidbody.AddForce(camera.transform.TransformDirection(movementInput) * calculatedSpeed * rigidbody.drag * Time.deltaTime, ForceMode.VelocityChange);
			camera.transform.position = Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraVelocity, cameraDampMovement * Time.deltaTime);

			rotationInput = lookAction.ReadValue<Vector2>();

			var mouseX = rotationInput.x * lookSpeed * Time.deltaTime;
			var mouseY = rotationInput.y * lookSpeed * Time.deltaTime;

			xRotation -= mouseY;
			xRotation = Mathf.Clamp(xRotation, lookClamp.x, lookClamp.y);

			var cameraRotation = Quaternion.Euler(xRotation, transform.eulerAngles.y, 0f);
			camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation , cameraRotation, lookDamp * Time.deltaTime);
			transform.Rotate(Vector3.up * mouseX);
		}

		private void HandleBoost(InputAction.CallbackContext obj)
		{
			if (!(lastBoostTime < Time.time - boostCooldown) || PictureTaker.IsActive) return;
			dragBoostAnimation.Play();
			var boostDirection = movementInput.sqrMagnitude > 0.01f ? camera.transform.TransformDirection(movementInput).normalized : camera.transform.forward;
			rigidbody.AddForce(boostDirection * boostStrength, ForceMode.VelocityChange);
			lastBoostTime = Time.time;
		}

		private void UpdateDrag(float drag)
		{
			rigidbody.drag = drag;
		}
	}
}