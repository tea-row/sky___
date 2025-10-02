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

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Score = 0;

        currentHealth = maxHealth;

        UIManager.instance.UpdateUI(Score, currentHealth);
    }

    void Update()
    {
        if (isDead) return;

        MoveLeftOrRight();

        score += (int)(Time.deltaTime * 10);
        UIManager.instance.UpdateUI(score, currentHealth);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(1);
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