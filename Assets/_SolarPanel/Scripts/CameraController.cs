using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float minAngle = -15;
    [SerializeField] private float maxAngle = 35;
   
    [SerializeField] private float zoomSpeed = .1f;
    [SerializeField] private float minDistance = 10f;
    [SerializeField] private float maxDistance = 45;
    
    [Header("Ссылки")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private InputActionReference rotateAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference zoomAction;

    private Vector2 inputRotation;
    private float VerticalAngle;
    private float HorizontalAngle;
    private Vector3 cameraPosition;
    public Vector3 LookPoint;
    
    private float distance;

    public float MinRunTimeDistance
    {
        get => distance;
        set
        {
            if (value <= minDistance)
            {
                distance = minDistance;
            }
            else
            {
                distance = distance >= maxDistance ? maxDistance : value;
            }
        }
    }
    private void Awake()
    {
        distance = minDistance;
        LookPoint = transform.position;
        cameraPosition = cameraTransform.localPosition;
        HorizontalAngle = transform.eulerAngles.z;
        VerticalAngle = transform.eulerAngles.x;
        
    }

    private void OnEnable()
    {
        rotateAction.action.Enable();
        lookAction.action.Enable();
        zoomAction.action.Enable();
    }

    private void OnDisable()
    {
        rotateAction.action.Disable();
        lookAction.action.Disable();
        zoomAction.action.Disable();
    }

    private void LateUpdate()
    {
        cameraTransform.LookAt(LookPoint);
        if (rotateAction.action.IsPressed())
        {
            HandleRotation();
        }
        HandleZoom();
       
    }

    private void HandleZoom()
    {
        var delta = zoomAction.action.ReadValue<Vector2>();
        cameraPosition.x -= -delta.y * zoomSpeed * Time.deltaTime;
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, MinRunTimeDistance, maxDistance);
        cameraTransform.localPosition = cameraPosition;
    }

    public void SetXPosition(float x)
    {
        cameraPosition.x = x;
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, MinRunTimeDistance, maxDistance);
        cameraTransform.localPosition = cameraPosition;
    }
    
    private void HandleRotation()
    {
        var delta = lookAction.action.ReadValue<Vector2>();
        
        VerticalAngle -= delta.y * rotationSpeed * Time.deltaTime;
        HorizontalAngle += delta.x * rotationSpeed * Time.deltaTime;
        
        VerticalAngle = Mathf.Clamp(VerticalAngle, minAngle, maxAngle);
        transform.rotation = Quaternion.Euler(0, HorizontalAngle, VerticalAngle);
    }
}