using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseAdapter : IAuthService, IDatabaseService
{
    private FirebaseAuth auth = FirebaseAuth.DefaultInstance;
    
    private DatabaseReference dbReference 
    {
        get { return FirebaseDatabase.DefaultInstance.RootReference; }
    }

    public async Task<string> Login(string email, string password)
    {
        await auth.SignInWithEmailAndPasswordAsync(email, password);
        
        await auth.CurrentUser.ReloadAsync();

        if (!auth.CurrentUser.IsEmailVerified)
        {
            auth.SignOut();
            throw new System.Exception("Please verify your email before continuing. (Check your inbox and spam folder!)");
        }

        return auth.CurrentUser.UserId;
    }

    public async Task<string> Register(string email, string password, string username)
    {
        await auth.CreateUserWithEmailAndPasswordAsync(email, password);
        
        await SaveUserName(auth.CurrentUser.UserId, username);

        await auth.CurrentUser.SendEmailVerificationAsync();

        string newUserId = auth.CurrentUser.UserId;
        
        auth.SignOut();

        return newUserId;
    }

    public void Logout() => auth.SignOut();
    public string GetCurrentUserId() => auth.CurrentUser?.UserId;

    public async Task SendPasswordResetEmail(string email)
    {
        await auth.SendPasswordResetEmailAsync(email);
    }

    public async Task SaveUserScore(string userId, int score)
    {
        try
        {
            int currentHighScore = await GetUserHighScore(userId);

            if (score > currentHighScore)
            {
                await dbReference.Child("users").Child(userId).Child("highScore").SetValueAsync(score);
                Debug.Log($"New High Score of {score} synced to Firebase successfully.");
            }
            else
            {
                Debug.Log($"Score {score} did not beat existing High Score of {currentHighScore}. Keeping old score.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to sync score: {e.Message}");
        }
    }

    public async Task SaveUserName(string userId, string userName)
    {
        try
        {
            await dbReference.Child("users").Child(userId).Child("name").SetValueAsync(userName);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save user name: {e.Message}");
        }
    }

    public async Task<System.Collections.Generic.List<LeaderboardEntry>> GetTopHighScores(int limit)
    {
        var leaderboard = new System.Collections.Generic.List<LeaderboardEntry>();

        try
        {
            var snapshot = await dbReference.Child("users").OrderByChild("highScore").LimitToLast(limit).GetValueAsync();

            if (snapshot.Exists)
            {
                foreach (var child in snapshot.Children)
                {
                    LeaderboardEntry entry = new LeaderboardEntry();
                    
                    if (child.Child("name").Exists)
                        entry.userName = child.Child("name").Value.ToString();
                    else
                        entry.userName = "Unknown Player";

                    if (child.Child("highScore").Exists)
                        entry.score = int.Parse(child.Child("highScore").Value.ToString());
                    else
                        entry.score = 0;

                    leaderboard.Insert(0, entry);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to fetch leaderboard: {e.Message}");
        }

        return leaderboard;
    }

    public async Task<int> GetUserHighScore(string userId)
    {
        var snapshot = await dbReference.Child("users").Child(userId).Child("highScore").GetValueAsync();
        if (snapshot.Exists)
        {
            return int.Parse(snapshot.Value.ToString());
        }
        return 0;
    }
}