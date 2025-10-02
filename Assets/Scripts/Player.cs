using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float forcePower;
    [SerializeField]
    private float xInput;

    [SerializeField]
    private int score;
    public int Score { get { return score; } set { score = value; } }

    [Header("Level Boundaries")]
    private float minX;
    private float maxX;

    [Header("Dependencies")]
    public LevelGenerator levelGenerator;

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Score = 0;

        currentHealth = maxHealth;

        if (levelGenerator != null)
        {
            float halfLevelWidth = (levelGenerator.numberOfLanes - 1) * 0.5f * levelGenerator.laneWidth;
            minX = -halfLevelWidth;
            maxX = halfLevelWidth;
        }
        else
        {
            Debug.LogError("LevelGenerator is not assigned in the Player script!", this.gameObject);

            minX = -10f;
            maxX = 10f;
        }
    
    UIManager.instance.UpdateUI(Score, currentHealth);
    }

void Update()
    {
        if (isDead) return;

        xInput = Input.GetAxis("Horizontal");

        score += (int)(Time.deltaTime * 10);
        UIManager.instance.UpdateUI(score, currentHealth);
    }

    void FixedUpdate()
    {
        if (isDead) return;

        rb.AddForce(xInput * Vector3.right * forcePower * Time.fixedDeltaTime, ForceMode.VelocityChange);
       
        Vector3 currentPosition = rb.position;

        currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
        rb.position = currentPosition;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        UIManager.instance.UpdateUI(Score, currentHealth);

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void MoveLeftOrRight()
    {
        xInput = Input.GetAxis("Horizontal");
        rb.AddForce(xInput * Vector3.right * forcePower);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            score += 100;

            Debug.Log("Collected a coin! Score: " + score);

            Destroy(other.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(1);

            Destroy(collision.gameObject);
        }
    }

    void Die()
    {
        isDead = true;
        UIManager.instance.ShowNotification("Game Over!");

        GetComponent<MeshRenderer>().enabled = false;
        rb.isKinematic = true;
        this.enabled = false;
    }
}