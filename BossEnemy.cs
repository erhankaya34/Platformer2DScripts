using System.Collections;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public static BossEnemy Instance;

    [SerializeField] private float speed = 2f;
    private float maxHealth;
    private float currentHealth;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material damagedMaterial;
    private float flashDuration = 0.1f;

    [SerializeField] private float knockBackForceX = 2f, knockBackForceY = 2f;
    private float damage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        if (direction.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
        else if (direction.x > 0 && transform.localScale.x < 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void SetStats(float health, float damage)
    {
        this.maxHealth = health;
        this.damage = damage;
        this.currentHealth = maxHealth;
    }

    public void TakeDamage(float damage, Vector3 attackerPosition)
    {
        currentHealth -= damage;
        Vector2 knockbackDirection = transform.position - attackerPosition;
        knockbackDirection.Normalize();
        rb.AddForce(new Vector2(knockbackDirection.x * knockBackForceX, knockBackForceY), ForceMode2D.Impulse);

        if (currentHealth <= 0)
        {
            AudioManager.instance.PlayAudio(AudioManager.instance.EnemeyDieAS);
            currentHealth = 0;
            animator.SetTrigger("death");
            Destroy(gameObject, 0.4f);
        }
        else
        {
            AudioManager.instance.PlayAudio(AudioManager.instance.EnemyDamageAS);
            spriteRenderer.material = damagedMaterial;
            StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash()
    {
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = originalMaterial;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RestartTrigger"))
        {
            spriteRenderer.material = damagedMaterial;
            currentHealth = 0;
            animator.SetTrigger("death");
            Destroy(gameObject, 0.4f);
            AudioManager.instance.PlayAudio(AudioManager.instance.EnemeyDieAS);
        }
    }
}
