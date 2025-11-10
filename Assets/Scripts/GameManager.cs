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
    
    public bool gameEnded = false; // Changed to public so PlayerController can check it

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("GameManager instance created");
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
            Debug.Log("Victory screen hidden at start");
        }
        else
        {
            Debug.LogError("Victory screen not assigned!");
        }
    }

    public void ShowVictory(string winnerName)
    {
        Debug.Log("ShowVictory called with winner: " + winnerName);
        
        if (gameEnded) 
        {
            Debug.Log("Game already ended, ignoring...");
            return;
        }
        
        gameEnded = true;
        
        // Freeze both characters
        FreezeAllPlayers();
        
        if (victoryScreen == null)
        {
            Debug.LogError("VictoryScreen is null!");
            return;
        }
        
        if (victoryText == null)
        {
            Debug.LogError("VictoryText is null!");
            return;
        }
        
        victoryText.text = winnerName + " Wins!";
        victoryScreen.SetActive(true);
        Debug.Log("VICTORY SCREEN SHOULD BE VISIBLE NOW: " + winnerName + " Wins!");
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
            
            Debug.Log("Froze player: " + player.gameObject.name);
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}