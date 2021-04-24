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
		private Camera camera;

		[Header("Config")]
		[SerializeField]
		private float movementSpeed;

		[SerializeField]
		private float lookSpeed;

		[SerializeField]
		private Vector2 lookClamp = new Vector2(360, 180);

		private InputAction movementAction;
		private InputAction verticalMovementAction;
		private InputAction lookAction;

		private Vector3 movementInput;
		private Vector3 rotationInput;

		private float xRotation;

		private void Start()
		{
			//Input Setup
			var map = new InputActionMap("Character Controller");

			lookAction = map.AddAction("look", binding: "<Mouse>/delta");
			movementAction = map.AddAction("move", binding: "<Gamepad>/leftStick");
			verticalMovementAction = map.AddAction("Vertical Movement");

			lookAction.AddBinding("<Gamepad>/rightStick").WithProcessor("scaleVector2(x=15, y=15)");

			movementAction.AddCompositeBinding("Dpad")
				.With("Up", "<Keyboard>/w")
				.With("Down", "<Keyboard>/s")
				.With("Left", "<Keyboard>/a")
				.With("Right", "<Keyboard>/d");

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
			movementInput.y = verticalMovementAction.ReadValue<Vector2>().y;

			rotationInput = lookAction.ReadValue<Vector2>();

			rigidbody.AddForce(camera.transform.TransformDirection(movementInput) * movementSpeed * rigidbody.drag * Time.deltaTime, ForceMode.VelocityChange);
		}

		private void LateUpdate()
		{
			var mouseX = rotationInput.x * lookSpeed * Time.deltaTime;
			var mouseY = rotationInput.y * lookSpeed * Time.deltaTime;

			xRotation -= mouseY;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);

			camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
			transform.Rotate(Vector3.up * mouseX);
		}
	}
}