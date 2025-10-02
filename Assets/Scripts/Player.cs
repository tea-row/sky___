using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float forcePower;
    [SerializeField]
    private float xInput;

    [SerializeField]
    private int score;
    public int Score { get { return score; } set { score = value; } }

    [SerializeField]
    private int hp = 3;
    public int HP { get { return hp; } set { hp = value; } }

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isDead) return;

        MoveLeftOrRight();

        score += (int)(Time.deltaTime * 10);
        UIManager.instance.UpdateUI(score, hp);
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
            hp -= 1;
            if (hp <= 0)
            {
                Die();
            }
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