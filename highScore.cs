using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private bool isUpdating = false;

    private void Awake()
    {
        entryContainer = transform.Find("entryContainer");
        if (entryContainer == null) return;

        entryTemplate = entryContainer.Find("entryTemplate");
        if (entryTemplate == null) return;

        entryTemplate.gameObject.SetActive(false);
    }

    private async void OnEnable()
    {
        if (isUpdating) return;
        isUpdating = true;

        try
        {
            if (entryTemplate == null)
            {
                Debug.LogWarning("Leaderboard Template is missing or destroyed!");
                return;
            }
            foreach (Transform child in entryContainer)
            {
                if (child != null && child != entryTemplate)
                {
                    Destroy(child.gameObject);
                }
            }

            if (DataManagementFacade.Instance == null) return;

            List<LeaderboardEntry> leaderboard = await DataManagementFacade.Instance.DatabaseService.GetTopHighScores(10);

            if (this == null || entryTemplate == null) return;
            float templateHeight = 35f;

            for (int i = 0; i < leaderboard.Count; i++)
            {
                LeaderboardEntry data = leaderboard[i];
                Transform entryTransform = Instantiate(entryTemplate, entryContainer);
                RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
                entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
                entryTransform.gameObject.SetActive(true);

                int rank = i + 1;
                string rankString = rank switch { 1 => "1ST", 2 => "2ND", 3 => "3RD", _ => rank + "TH" };

                SetTextValue(entryTransform, "Position", rankString);
                SetTextValue(entryTransform, "Score", data.score.ToString());
                SetTextValue(entryTransform, "PLayerName", data.userName);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Leaderboard Refresh interrupted: " + e.Message);
        }
        finally
        {
            isUpdating = false;
        }
    }

    private void SetTextValue(Transform parent, string childName, string value)
    {
        Transform child = parent.Find(childName);
        if (child != null)
        {
            var textComp = child.GetComponent<TextMeshProUGUI>();
            if (textComp != null) textComp.text = value;
        }
    }
}
