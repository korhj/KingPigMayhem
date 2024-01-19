using TMPro;
using UnityEngine;

public class ScoreTextUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        Player.Instance.OnScoreIncrease += (object sender, Player.OnScoreIncreaseEventArgs e) =>
        {
            scoreText.text = e.score.ToString();
        };
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
