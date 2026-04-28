using UnityEngine;

public class Engine : MonoBehaviour
{
    private DragAndDrop currentTire;
    private Animator animator;
    private MinigameController minigame;
    private bool isMinigameActive = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        minigame = FindObjectOfType<MinigameController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentTire != null && !isMinigameActive)
        {
            isMinigameActive = true;

            if (minigame != null)
            {
                minigame.StartMinigame(this, currentTire);
            }
        }
    }

    public void SetTire(DragAndDrop tire)
    {
        currentTire = tire;

        if (animator != null)
            animator.SetBool("HasTire", true);
    }

    public void TireLaunched()
    {
        currentTire = null;
        isMinigameActive = false;

        if (animator != null)
            animator.SetBool("HasTire", false);
    }
}