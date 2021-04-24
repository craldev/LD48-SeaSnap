using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LD48.Gameplay.Player
{
	public class CharacterController : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private Rigidbody rigidbody;

		[SerializeField]
		private UnityEngine.Camera camera;

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
		private Vector2 lookClamp = new Vector2(90f, 90f);

		[SerializeField]
		private float cameraDampMovement = 0.2f;

		private InputAction movementAction;
		private InputAction verticalMovementAction;
		private InputAction lookAction;

		private Vector3 movementInput;
		private Vector3 rotationInput;

		private float xRotation;
		private Vector3 cameraVelocity;

		private void Start()
		{
			//Input Setup
			var map = new InputActionMap("Character Controller");

			lookAction = map.AddAction("look", binding: "<Mouse>/delta");
			movementAction = map.AddAction("move", binding: "<Gamepad>/leftStick");

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

			movementAction.Enable();
			lookAction.Enable();
			verticalMovementAction.Enable();
		}

		private void Update()
		{
			//Input
			var movementDelta = movementAction.ReadValue<Vector2>();

			movementInput.x = movementDelta.x;
			movementInput.z = movementDelta.y;
			movementInput.y = verticalMovementAction.ReadValue<Vector2>().y * movementUpMultiplier;

			rigidbody.AddForce(camera.transform.TransformDirection(movementInput) * movementSpeed * rigidbody.drag * Time.deltaTime, ForceMode.VelocityChange);
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
	}
}