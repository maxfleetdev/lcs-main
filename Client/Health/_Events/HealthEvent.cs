using UnityEngine;

public class HealthEvent : MonoBehaviour
{
    [Tooltip("True = Add; False = Remove")]
    [SerializeField] private bool addToHealth;
    [SerializeField] private int amount;

    public void HealthChange()
    {
        // Get Info //
        HealthManager health = InstanceFinder.Health_Manager();
        switch (addToHealth)
        {
            // Add To Health //
            case true:
                health.AddHealth(amount);
                break;

            // Remove From Health //
            case false:
                health.RemoveHealth(amount);
                break;
        }
    }
}
