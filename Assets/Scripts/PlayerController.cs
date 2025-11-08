using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Attack Settings")]
    [SerializeField] private LayerMask enemyLayers;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    [Header("Health Settings")]
    public Image healthBar;
    private float currentHealth = 100f;
    private float maxHealth = 100f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAttack();
    }

    void HandleMovement()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (move != 0)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);

        if (move > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (move < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }
    }

    void HandleAttack()
    {
        // Customize keys per character
        if (gameObject.name.Contains("Soldier"))
        {
            if (Input.GetKeyDown(KeyCode.F) && Time.time >= nextAttackTime)
            {
                animator.SetTrigger("Attack");
                Debug.Log("Soldier - Attack Triggered");
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        else if (gameObject.name.Contains("Orc"))
        {
            if (Input.GetKeyDown(KeyCode.K) && Time.time >= nextAttackTime)
            {
                animator.SetTrigger("Attack");
                Debug.Log("Orc - Attack Triggered");
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    // Animation Event
    public void OnAttackHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"{gameObject.name} hit {enemy.name}");
            enemy.GetComponent<PlayerController>().TakeDamage(10);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        animator.SetTrigger("Hurt");
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float fill = currentHealth / maxHealth;
            healthBar.fillAmount = fill;

            if (fill > 0.6f)
                healthBar.color = Color.green;
            else if (fill > 0.4f)
                healthBar.color = Color.yellow;
            else
                healthBar.color = Color.red;
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        Debug.Log(gameObject.name + " has died!");
        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
