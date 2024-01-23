using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    // Class-only //
    protected private int health = 100;             // DEFAULT
    protected private int maxHealth = 100;
    private static HealthManager instance;

    // Public Properties //
    public int Health
    {
        get => health; 
        private set => health = value;
    }
    public static HealthManager Instance
    {
        get => instance;
        private set => instance = value;
    }

    // Events //
    public Action OnHealthChange;       // for GUI and animation use

    #region Startup and Shutdown

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DebugSystem.Log("HealthManager already instantiated!", LogType.Error);
            Destroy(gameObject);
        }
        health = maxHealth;             // CHANGE: If loading from save, load previous health, otherwise set maxhealth
    }

    private void OnDisable()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    #endregion

    #region Health Logic

    public void RemoveHealth(int amount)
    {
        float multi = InstanceFinder.Game_Manager().SelectedDifficulty.DamageTakeMultiplier;
        amount = Mathf.RoundToInt(amount * multi);          // new damage taken depending on difficulty
        int diff = health - amount;
        if (diff > 0)
        {
            health -= amount;
            DebugSystem.Log($"Removed {amount} of health", LogType.Debug);
        }
        else
        {
            health = 0;
            // DEATH LOGIC //
            // death animation
            // game over
            // load previous save
            // etc...      //
            DebugSystem.Log($"Player is dead", LogType.Debug);
        }
        OnHealthChange?.Invoke();
    }

    public void AddHealth(int amount)
    {
        int diff = health + amount;
        if (diff >= 100)
        {
            health = maxHealth;
        }

        else
        {
            health += amount;
        }

        DebugSystem.Log($"Added {amount} of health! Now {health}", LogType.Debug);
        OnHealthChange?.Invoke();
    }

    #endregion
}