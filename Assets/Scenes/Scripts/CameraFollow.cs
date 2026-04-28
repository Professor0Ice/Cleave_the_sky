using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // объект, за которым следим (шина)
    public float smoothSpeed = 5f;  // плавность следования
    public Vector3 offset = new Vector3(0, 0, -10); // отступ (Z = -10 для 2D)
    public Vector2 minBounds;       // минимальные границы камеры (X, Y)
    public Vector2 maxBounds;       // максимальные границы камеры

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        // Ограничиваем камеру в заданных пределах
        float clampX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        float clampY = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
        Vector3 clampedPosition = new Vector3(clampX, clampY, desiredPosition.z);

        transform.position = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed * Time.deltaTime);
    }
}