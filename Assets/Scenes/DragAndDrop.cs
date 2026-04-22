using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 offset;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("Старт");
    }

    void OnMouseDown()
    {
        isDragging = true;
        Debug.Log("Нажатие");
        offset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector2 newPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            transform.position = newPos;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        rb.gravityScale = 1;
    }
}