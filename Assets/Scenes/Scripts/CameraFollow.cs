using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("Zoom")]
    public float minZoom = 3f;      // минимальный размер камеры (близко)
    public float maxZoom = 10f;     // максимальный размер камеры (далеко)
    public float zoomSpeed = 2f;    // скорость зума
    public float defaultZoom = 5f;  // начальный зум

    private Camera cam;
    private float currentZoom;

    void Start()
    {
        cam = GetComponent<Camera>();
        currentZoom = defaultZoom;
        cam.orthographicSize = currentZoom;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Слежение за целью
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.unscaledDeltaTime  // unscaledDeltaTime чтобы работало при паузе
        );

        // Зум колёсиком мыши
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            cam.orthographicSize = currentZoom;
        }
    }
}