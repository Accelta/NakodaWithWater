using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    private List<IQuest> activeQuests = new List<IQuest>();
    private Dictionary<IQuest, GameObject> questBarriers = new Dictionary<IQuest, GameObject>();

    public void AssignQuest(IQuest quest)
    {
        if (!activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);
            // Show quest in UI or set quest status as active
        }
    }

    public bool IsQuestActive(IQuest quest)
    {
        return activeQuests.Contains(quest);
    }

    public void StartQuest(IQuest quest)
    {
        quest.StartQuest();
    }

    public void CompleteQuest(IQuest quest)
    {
        if (activeQuests.Contains(quest))
        {
            activeQuests.Remove(quest);
            GameObject barrier;
            if (questBarriers.TryGetValue(quest, out barrier))
            {
                Destroy(barrier);  // Remove access barriers on completion
            }
        }
    }
}
