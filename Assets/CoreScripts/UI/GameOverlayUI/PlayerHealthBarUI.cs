using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    // DATA //
    // References
    public PlayerComponent player;
    public UIProgressBar progressBar;


    // Startup Functions
    private void Awake()
    {
        // Gets player reference
        player = FindObjectOfType<PlayerComponent>();
    }

    private void OnEnable()
    {
        // Adds event listeners
        player.component_CombatEntity.onHealthModify += UpdateHealth;
        player.onStartUp += UpdateHealth;
    }

    private void OnDisable()
    {
        // Removes event listeners
        player.component_CombatEntity.onHealthModify -= UpdateHealth;
        player.onStartUp -= UpdateHealth;
    }


    // UpdateHealth function
    public virtual void UpdateHealth()
    {
        progressBar.SetValue(player.component_CombatEntity.health);
        progressBar.SetMaxValue(player.component_CombatEntity.maxHealth);
    }
    public virtual void UpdateHealth(int modifyAmount)
    {
        UpdateHealth();
    }
}
