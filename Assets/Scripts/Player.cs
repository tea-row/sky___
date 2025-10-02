using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float forcePower;
    [SerializeField]
    private float xInput;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveLeftOrRight();
    }
    private void MoveLeftOrRight()
    {
        xInput = Input.GetAxis("Horizontal");
        rb.AddForce (xInput * Vector3.right * forcePower);
    }
}
