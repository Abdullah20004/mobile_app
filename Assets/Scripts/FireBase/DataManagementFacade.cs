using UnityEngine;

public class DataManagementFacade : MonoBehaviour
{
    public static DataManagementFacade Instance;

    public IAuthService AuthService => authService;
    public IDatabaseService DatabaseService => databaseService;
    
    private IAuthService authService;
    private IDatabaseService databaseService;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            FirebaseAdapter adapter = new FirebaseAdapter();
            authService = adapter;
            databaseService = adapter;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void SaveGameOverSession(int finalScore)
    {
        string userId = authService.GetCurrentUserId();

        PlayerPrefs.SetInt("OfflineScore", finalScore);
        PlayerPrefs.Save();

        if (!string.IsNullOrEmpty(userId))
        {
            await databaseService.SaveUserScore(userId, finalScore);
        }
        else
        {
            Debug.LogWarning("User not logged in. Score saved locally only.");
        }
    }
}