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
    public Image healthFill; 
    public int maxHealth = 100;
    public int attackDamage = 20;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool isDead = false;
    private int currentHealth;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        currentHealth = maxHealth;
        if (healthBar != null) healthBar.value = 1f;
        
        UpdateHealthBarColor();
        
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

        //Ground
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

        // Attack - Simple Attack
        if (Input.GetKeyDown(attackKey))
        {
            animator.SetTrigger("Attack");
            SimpleAttack();
        }
    }

    void SimpleAttack()
    {
        if (attackPoint == null) return;
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        foreach(Collider2D hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                PlayerController enemy = hit.GetComponent<PlayerController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
        
        // Update health bar color
        UpdateHealthBarColor();
            
        animator.SetTrigger("Hurt");
        
        if (currentHealth <= 0) Die();
    }

    void UpdateHealthBarColor()
    {
        if (healthFill != null)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            
            if (healthPercent >= 0.61f) 
            {
                healthFill.color = Color.green;
            }
            else if (healthPercent >= 0.31f) 
            {
                healthFill.color = Color.yellow;
            }
            else 
            {
                healthFill.color = Color.red;
            }
        }
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
        
        // Tell GameManager who won
        string winnerName = (gameObject.name == "Orc") ? "Soldier" : "Orc";        
        GameManager.instance.ShowVictory(winnerName);
    }

    public void RestartGame()
    {
        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}