using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 offset;
    private Rigidbody2D rb;
    private bool isPlacedOnEngine = false;
    private Engine currentEngine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        if (isPlacedOnEngine) return;
        isDragging = true;
        offset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
    }

    void OnMouseDrag()
    {
        if (isPlacedOnEngine) return;
        if (isDragging)
        {
            Vector2 newPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            transform.position = newPos;
        }
    }

    public void Launch(Vector2 force)
    {
        if (!isPlacedOnEngine) return; // можно запускать только установленную шину

        isPlacedOnEngine = false;
        rb.bodyType = RigidbodyType2D.Dynamic; // возвращаем обычную физику
        rb.gravityScale = 1;
        rb.AddForce(force, ForceMode2D.Impulse);
        Debug.Log("Запуск! Сила: " + force);
    }

    void OnMouseUp()
    {
        if (isPlacedOnEngine) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        bool onEngine = false;
        Collider2D engineCollider = null;
        foreach (var hit in hits)
        {
            if (hit.gameObject.name == "Engine" && hit.isTrigger)
            {
                onEngine = true;
                engineCollider = hit;
                break;
            }
        }

        if (onEngine && !isPlacedOnEngine)
        {
            isPlacedOnEngine = true;
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            Transform engine = engineCollider.transform;
            transform.position = new Vector2(engine.position.x, engine.position.y + 0.5f);

            currentEngine = engine.GetComponent<Engine>();
            if (currentEngine != null)
            {
                currentEngine.SetTire(this);
            }
        }
        else
        {
            rb.gravityScale = 1;
        }
        isDragging = false;
    }
}