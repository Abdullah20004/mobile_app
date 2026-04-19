using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    private PlayerMovement movement;
    public float delay = 6f;
    public GameObject leaderboardUI; 
    AudioManager audioManager;

    private void Awake()
    {
        GameObject audioObj = GameObject.FindGameObjectWithTag("Audio");
        if (audioObj != null) audioManager = audioObj.GetComponent<AudioManager>();
    }

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "jump_obs":
                if (movement.isGrounded) TriggerGameOver();
                break;

            case "slide_obs":
                if (movement.slideTimer <= 0f) TriggerGameOver();
                break;

            case "left_obs":
                if (movement.leftTimer <= 0f) TriggerGameOver();
                break;

            case "right_obs":
                if (movement.rightTimer <= 0f) TriggerGameOver();
                break;
        }
    }

    private void TriggerGameOver()
    {
        if (movement.enabled == false) return;
        movement.enabled = false;

        if (audioManager != null) audioManager.PlaySFX(audioManager.lose);

        Score scoreScript = Object.FindAnyObjectByType<Score>();
        if (scoreScript != null) scoreScript.enabled = false;

        int finalScore = Mathf.FloorToInt(Time.timeSinceLevelLoad);
        Debug.Log("Game Over! Final Score: " + finalScore);

        if (DataManagementFacade.Instance != null)
        {
            DataManagementFacade.Instance.SaveGameOverSession(finalScore);
        }

        if (leaderboardUI != null)
        {
            leaderboardUI.SetActive(true);
        }

        Invoke("Restart", delay);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}