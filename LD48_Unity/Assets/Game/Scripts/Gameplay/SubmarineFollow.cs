using UnityEngine;
using CharacterController = LD48.Gameplay.Player.CharacterController;

public class SubmarineFollow : MonoBehaviour
{
    [SerializeField]
    private float movementThreshold = 50f;

    [SerializeField]
    private Rigidbody rigidbody;

    [SerializeField]
    private float movementForce = 10f;

    [SerializeField]
    private float turnSpeed = 10f;

    private Transform playerTransform;

    private void Update()
    {
        if (playerTransform == null)
        {
            playerTransform = CharacterController.Instance.transform;
        }
        var positionVector = playerTransform.position - transform.position;

        positionVector.y = 0f;
        if (positionVector.sqrMagnitude > movementThreshold)
        {
            positionVector = positionVector.normalized;
            positionVector = Vector3.Lerp(transform.forward, positionVector, turnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(positionVector);
            rigidbody.AddForce(transform.forward * movementForce * Time.deltaTime, ForceMode.Acceleration);
        }
    }
}
