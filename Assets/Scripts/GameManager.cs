using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("Victory Screen")]
    public GameObject victoryScreen;
    public TMP_Text victoryText;
    
    public bool gameEnded = false; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false);
        }
    }

    // SHows the Victoryscreen
    public void ShowVictory(string winnerName)
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;

        FreezeAllPlayers();

        if (victoryScreen == null)
        {
            return;
        }

        if (victoryText == null)
        {
            return;
        }

        victoryText.text = winnerName + " Wins!";
        victoryScreen.SetActive(true);
    }

    void FreezeAllPlayers()
    {
        // Find and freeze all players
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players)
        {
            // Stop movement
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0;
            }
            
            // Disable input
            player.enabled = false;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}