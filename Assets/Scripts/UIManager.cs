using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private RectTransform gameOverPanel;
    [SerializeField] private Button playAgainButton;

    private void Awake()
    {
        gameOverPanel.gameObject.SetActive(false);
        
        Field.OnGameLost += HandleGameLost;
        Field.OnGameWon += HandleGameWon;
        playAgainButton.onClick.AddListener(ResetGame);
    }

    private void OnDestroy()
    {
        Field.OnGameLost -= HandleGameLost;
        Field.OnGameWon -= HandleGameWon;
        playAgainButton.onClick.RemoveAllListeners();
    }

    private void HandleGameWon()
    {
        gameOverPanel.gameObject.SetActive(true);
        gameOverText.text = "You Won!";
    }

    private void HandleGameLost()
    {
        gameOverPanel.gameObject.SetActive(true);
        gameOverText.text = "You Lost!";
    }

    public void ResetGame()
    {
        gameOverPanel.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
    
}
