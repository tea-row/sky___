using UnityEngine;

public class Finish_line : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Score += 100;
                UIManager.instance.ShowNotification("You win!");
            }
        }
    }
}