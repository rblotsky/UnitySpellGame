using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    // DATA //
    public GameObject deathUI;
    public float returnToMenuDelay = 4f;


    // FUNCTIONS //
    // Basic Functions
    private void Start()
    {
        // Adds RunPlayerDeath to player death event
        FindObjectOfType<PlayerComponent>().component_CombatEntity.onDeath += RunPlayerDeath;
    }

    public void RunPlayerDeath()
    {
        StartCoroutine(RunDeath());
    }

    public IEnumerator RunDeath()
    {
        // Disables all player components
        PlayerStats.isPlayerPaused = true;
        DataRef.playerReference.component_MoveScript.enabled = false;
        DataRef.playerReference.component_ItemScript.enabled = false;
        DataRef.overlayManagerReference.gameObject.SetActive(false);

        // Instantiates death UI
        Instantiate(deathUI);

        // Waits before returning to menu
        yield return new WaitForSeconds(returnToMenuDelay);

        // Loads Main Menu
        SceneManager.LoadScene(0);
    }
}
