using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCamera : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float verticalClamp = 80f; // Макс. угол по вертикали

    [Header("Input")]
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference rotateAction;

    private Vector2 lookInput;
    private float xRotation;
    private float yRotation;

    private void OnEnable()
    {
        lookAction.action.Enable();
        rotateAction.action.Enable();
    }

    private void OnDisable()
    {
        lookAction.action.Disable();
        rotateAction.action.Disable();
    }

    private void Update()
    {
        // Вращать камеру только при зажатой ПКМ
        if (rotateAction.action.IsPressed())
        {
            lookInput = lookAction.action.ReadValue<Vector2>();
            RotateCamera();
        }
    }

    private void RotateCamera()
    {
        float mouseX = lookInput.x * rotationSpeed * Time.deltaTime;
        float mouseY = lookInput.y * rotationSpeed * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
