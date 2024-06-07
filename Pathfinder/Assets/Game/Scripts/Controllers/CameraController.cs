using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 moveSpeedRange;
    [SerializeField] private float defaultMoveSpeed;
    [SerializeField] private float changeMoveSpeedStep;
    [SerializeField] private float rotationSpeed;

    public static CameraController Instance;

    private float moveSpeed;
    private bool wasClickedOnUi;
    private Vector3 lastMousePosition;
    private Vector3 moveDirection;
    private Vector3 mouseOffset;

    private void Awake()
    {
        Instance = this;

        moveSpeed = defaultMoveSpeed;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            wasClickedOnUi = Utilities.IsCursorOnUi();
        }

        if (Input.GetMouseButton(1) && wasClickedOnUi == false)
        {
            UpdatePosition();
            UpdateRotation();
            UpdateMoveSpeed();
        }

        lastMousePosition = Input.mousePosition;
    }

    private void UpdatePosition()
    {
        moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += transform.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDirection += -transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += -transform.right;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection += transform.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            moveDirection += -transform.up;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            moveDirection += transform.up;
        }

        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    private void UpdateRotation()
    {
        mouseOffset = Input.mousePosition - lastMousePosition;
        float x = rotationSpeed * Time.deltaTime * -mouseOffset.y;
        float y = rotationSpeed * Time.deltaTime * mouseOffset.x;

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x + x, transform.eulerAngles.y + y, transform.eulerAngles.z);
    }

    private void UpdateMoveSpeed()
    {
        if (Input.mouseScrollDelta.y != 0.0f)
        {
            moveSpeed = Mathf.Clamp(moveSpeed + Input.mouseScrollDelta.y * changeMoveSpeedStep, moveSpeedRange.x, moveSpeedRange.y);

            MoveSpeedPanel.Instance.Show(moveSpeed);
        }
    }
}
