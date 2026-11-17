using TMPro;
using UnityEngine;

// Se conecta a los UnityEvents de GameManager y WaveSpawner desde el inspector
public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    public void SetLives(int value) => livesText.text = "Vidas: " + value;
    public void SetWave(int value) => waveText.text = "Oleada " + value;
    public void ShowGameOver() => gameOverPanel.SetActive(true);
    public void ShowVictory() => victoryPanel.SetActive(true);
}
