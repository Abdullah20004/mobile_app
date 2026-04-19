using System.Threading.Tasks;
using System.Collections.Generic;

public struct LeaderboardEntry
{
    public string userName;
    public int score;
}

public interface IDatabaseService
{
    Task SaveUserScore(string userId, int score);
    Task<int> GetUserHighScore(string userId);
    Task SaveUserName(string userId, string userName);
    Task<List<LeaderboardEntry>> GetTopHighScores(int limit);
}