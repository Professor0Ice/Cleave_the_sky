using UnityEngine;

public class CameraFollow : MonoBehaviour
{
<<<<<<< Updated upstream
    public Transform target;        // объект, за которым следим (шина)
    public float smoothSpeed = 5f;  // плавность следования
    public Vector3 offset = new Vector3(0, 0, -10); // отступ (Z = -10 для 2D)
    public Vector2 minBounds;       // минимальные границы камеры (X, Y)
    public Vector2 maxBounds;       // максимальные границы камеры
=======
    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public float smoothSpeedX = 8f;
    public float smoothSpeedY = 4f;
    public Vector3 offset = new Vector3(0, 2, -10);

    [Header("Vertical Limits")]
    public float maxHeightAboveTarget = 5f;  // насколько камера отстаёт вверх при прыжке
    public float groundY = -3f;              // минимальный Y камеры

    [Header("Zoom")]
    public float minZoom = 3f;
    public float maxZoom = 10f;
    public float zoomSpeed = 2f;
    public float defaultZoom = 5f;

    private Camera cam;
    private float currentZoom;

    void Start()
    {
        cam = GetComponent<Camera>();
        currentZoom = defaultZoom;
        cam.orthographicSize = currentZoom;
    }
>>>>>>> Stashed changes

    void LateUpdate()
    {
        if (target == null) return;

<<<<<<< Updated upstream
        Vector3 desiredPosition = target.position + offset;
        // Ограничиваем камеру в заданных пределах
        float clampX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        float clampY = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
        Vector3 clampedPosition = new Vector3(clampX, clampY, desiredPosition.z);

        transform.position = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed * Time.deltaTime);
=======
        // --- Зум ---
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            cam.orthographicSize = currentZoom;
        }

        // --- X: всегда следуем ---
        float targetX = target.position.x + offset.x;
        float newX = Mathf.Lerp(transform.position.x, targetX, smoothSpeedX * Time.unscaledDeltaTime);

        // --- Y: следуем но с задержкой при прыжке ---
        float targetY = target.position.y + offset.y;

        // Камера не должна опускаться ниже уровня земли
        float minCameraY = groundY;
        float clampedTargetY = Mathf.Max(targetY, minCameraY);

        // Плавное движение по Y
        float newY = Mathf.Lerp(transform.position.y, clampedTargetY, smoothSpeedY * Time.unscaledDeltaTime);

        transform.position = new Vector3(newX, newY, offset.z);
>>>>>>> Stashed changes
    }
}