using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform camera;
    [SerializeField] private float speed;
    private InputAction stickMovement;

    private void Awake()
    {
        stickMovement = InputSystem.actions.FindAction("StickMove");
    }

    private void Update()
    {
        Vector3 direction;
        //direction.x = stickMovement.ReadValue<Vector2>().x;
        //direction.y = cameraPivot.transform.position.y;
        //direction.z = stickMovement.ReadValue<Vector2>().y;

        direction = stickMovement.ReadValue<Vector2>().x * camera.right + stickMovement.ReadValue<Vector2>().y * camera.up;
        direction.y = cameraPivot.transform.position.y;

        cameraPivot.position += direction * speed * Time.deltaTime;
    }
}
