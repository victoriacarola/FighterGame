using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public KeyCode leftKey, rightKey, jumpKey, attackKey;
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 1.5f;
    public LayerMask enemyLayers;
    public Slider healthBar;
    public int maxHealth = 100;
    public int attackDamage = 20;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool isDead = false;
    private int currentHealth;
    private Vector3 originalScale;

    [Header("Victory Screen")]
    public GameObject victoryScreen;
    public UnityEngine.UI.Text victoryText;

    void Start()
{
    rb = GetComponent<Rigidbody2D>();
    originalScale = transform.localScale;
    currentHealth = maxHealth;
    if (healthBar != null) healthBar.value = 1f;
    
    // Auto-connect restart button if not set up in inspector
    GameObject restartBtn = GameObject.Find("RestartButton");
    if (restartBtn != null)
    {
        Button btn = restartBtn.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(RestartGame);
        }
    }
}

    void Update()
    {
        if (isDead) return;

        // Ground check
        isGrounded = transform.position.y <= 0.5f;

        // Movement
        float move = 0;
        if (Input.GetKey(leftKey)) move = -1;
        else if (Input.GetKey(rightKey)) move = 1;

        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
        
        if (move != 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(move));
            Vector3 newScale = originalScale;
            newScale.x = Mathf.Abs(newScale.x) * Mathf.Sign(move);
            transform.localScale = newScale;
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        // Jump
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Attack - SIMPLE VERSION
        if (Input.GetKeyDown(attackKey))
        {
            animator.SetTrigger("Attack");
            Debug.Log(gameObject.name + " attacking!");
            
            // Immediate attack for testing
            SimpleAttack();
        }
    }

    void SimpleAttack()
    {
        if (attackPoint == null) return;
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        Debug.Log(gameObject.name + " found " + hits.Length + " targets");
        
        foreach(Collider2D hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                PlayerController enemy = hit.GetComponent<PlayerController>();
                if (enemy != null)
                {
                    Debug.Log(gameObject.name + " HIT " + hit.name + "!");
                    enemy.TakeDamage(attackDamage);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage! Health: " + currentHealth);
        
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
            
        animator.SetTrigger("Hurt");
        
        if (currentHealth <= 0) Die();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    void Die()
{
    if (isDead) return; // Prevent multiple death calls
    
    isDead = true;
    animator.SetTrigger("Die");
    Debug.Log(gameObject.name + " died!");
    
    // Debug GameManager
    if (GameManager.instance == null)
    {
        Debug.LogError("GameManager instance is NULL!");
        return;
    }
    
    Debug.Log("GameManager found, calling ShowVictory...");
    
    // Tell GameManager who won
    string winnerName = (gameObject.name == "Orc") ? "Soldier" : "Orc";
    Debug.Log("Winner should be: " + winnerName);
    
    GameManager.instance.ShowVictory(winnerName);
}

    

    public void RestartGame()
    {
    // Reload the current scene
    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}