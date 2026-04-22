using UnityEngine;

public class Engine : MonoBehaviour
{
    private DragAndDrop currentTire; // ссылка на шину, котора€ установлена

    void Update()
    {
        // ≈сли нажата клавиша E и есть установленна€ шина
        if (Input.GetKeyDown(KeyCode.E) && currentTire != null)
        {
            currentTire.Launch(new Vector2(10f, 8f)); // сила полЄта (влево-вправо, вверх)
            currentTire = null; // шина улетела, больше не храним
        }
    }

    // Ётот метод вызовет шина, когда установитс€ на двигатель
    public void SetTire(DragAndDrop tire)
    {
        currentTire = tire;
    }
}