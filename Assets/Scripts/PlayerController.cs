using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 15f;

    [Header("Control Keys")]
    public KeyCode leftKey, rightKey, jumpKey, attackKey;

    [Header("Combat Settings")]
    public int maxHealth = 100;
    public int attackDamage = 10;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    [Header("UI References")]
    public Slider healthBar;
    public Image healthFill;

    [Header("References")]
    public Animator animator;

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

        if (healthBar != null)
            healthBar.value = 1f;
    }

    void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleJump();

        if (Input.GetKeyDown(attackKey))
        {
            animator.SetTrigger("Attack");
            Debug.Log(gameObject.name + " - Attack Triggered");
        }
    }

    void HandleMovement()
    {
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
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    public void PerformAttack()
{
    Debug.Log("PerformAttack called for: " + gameObject.name);
    
    if (attackPoint == null)
    {
        Debug.LogError("AttackPoint is null!");
        return;
    }
    
    // Detect enemies in range
    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    Debug.Log("Found " + hitEnemies.Length + " enemies in range");
    
    // Damage them
    foreach(Collider2D enemy in hitEnemies)
    {
        if (enemy.gameObject != this.gameObject)
        {
            Debug.Log("Hit: " + enemy.name);
            PlayerController enemyPC = enemy.GetComponent<PlayerController>();
            if (enemyPC != null)
            {
                enemyPC.TakeDamage(attackDamage);
            }
            else
            {
                Debug.Log("No PlayerController on: " + enemy.name);
            }
        }
    }
}

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        float healthPercent = (float)currentHealth / maxHealth;

        if (healthBar != null)
            healthBar.value = healthPercent;

        if (healthFill != null)
        {
            if (healthPercent > 0.6f) healthFill.color = Color.green;
            else if (healthPercent > 0.4f) healthFill.color = Color.yellow;
            else healthFill.color = Color.red;
        }

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        rb.linearVelocity = Vector2.zero;
        Invoke(nameof(DisablePlayer), 2f);
    }

    void DisablePlayer()
    {
        this.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
