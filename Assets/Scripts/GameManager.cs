using UnityEngine;
using UnityEngine.Events;

// Guarda la variable de vida que bajan los enemigos al llegar al final
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int lives = 20;

    public UnityEvent<int> onLivesChanged;
    public UnityEvent onGameOver;

    public int Lives => lives;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        onLivesChanged.Invoke(lives);
    }

    public void LoseLife(int amount)
    {
        if (lives <= 0) return;

        lives = Mathf.Max(0, lives - amount);
        onLivesChanged.Invoke(lives);

        if (lives == 0)
        {
            onGameOver.Invoke();
            Time.timeScale = 0f;
        }
    }
}
