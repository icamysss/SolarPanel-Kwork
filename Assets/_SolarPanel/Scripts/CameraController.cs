using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float verticalAngleLimit = 30f;

    [Header("Ссылки")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private InputActionReference rotateAction;
    [SerializeField] private InputActionReference LookAction;

    private Vector2 inputRotation;
    private float VerticalAngle;
    private float HorizontalAngle;
    

    private void Awake()
    {

        HorizontalAngle = transform.eulerAngles.z;
        VerticalAngle = transform.eulerAngles.x;
    }

    private void OnEnable()
    {
        rotateAction.action.Enable();
        LookAction.action.Enable();
    }

    private void OnDisable()
    {
        rotateAction.action.Disable();
        LookAction.action.Disable();
    }

    private void LateUpdate()
    {
        if (rotateAction.action.IsPressed())
        {
            HandleRotation();
        }
    }

    private void HandleRotation()
    {
        var delta = LookAction.action.ReadValue<Vector2>();
        
        VerticalAngle -= delta.y * rotationSpeed * Time.deltaTime;
        HorizontalAngle += delta.x * rotationSpeed * Time.deltaTime;
        
        VerticalAngle = Mathf.Clamp(VerticalAngle, -verticalAngleLimit, verticalAngleLimit);
        
        transform.localRotation = Quaternion.Euler(0, HorizontalAngle, VerticalAngle);
    }
}