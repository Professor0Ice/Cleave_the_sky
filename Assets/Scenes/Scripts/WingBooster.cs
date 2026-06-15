using UnityEngine;

public class WingsBooster : MonoBehaviour
{
    [Header("Flight Settings")]
    public float flightDuration = 3f;
    public float flightSpeed = 30f;
    public Vector2 flightDirection = Vector2.right;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tire"))
        {
            TireFlight tireFlight = other.GetComponent<TireFlight>();

            if (tireFlight != null)
            {
                tireFlight.StartFlight(flightDuration, flightSpeed, flightDirection);
                Destroy(gameObject);
                Debug.Log("🪽 КРЫЛЫШКИ! Полёт на " + flightDuration + " секунд!");
            }
        }
    }
}