using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text hpText;
    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private TMP_Text notificationText;

    public static UIManager instance;

    void Awake()
    {
        instance = this;
    }

    public void UpdateUI(int currentScore, int currentHP)
    {
        scoreText.text = "Score: " + currentScore;
        hpText.text = "HP: " + currentHP;
    }

    public void ShowNotification(string message)
    {
        notificationText.text = message;
        gameOverPanel.SetActive(true);
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}