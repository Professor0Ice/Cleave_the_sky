using UnityEngine;

public class Engine : MonoBehaviour
{
    private DragAndDrop currentTire;
    public Vector2 launchForce = new Vector2(10f, 8f); // настраивай в инспекторе
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>(); // получаем компонент Animator с этого же объекта
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentTire != null)
        {
            currentTire.Launch(launchForce);
            currentTire = null;

            // Выключаем анимацию
            if (animator != null)
                animator.SetBool("HasTire", false);
        }
    }

    public void SetTire(DragAndDrop tire)
    {
        currentTire = tire;

        // Включаем анимацию
        if (animator != null)
            animator.SetBool("HasTire", true);
    }
}