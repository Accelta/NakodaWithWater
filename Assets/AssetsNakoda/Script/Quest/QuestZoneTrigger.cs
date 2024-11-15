using UnityEngine;

public class QuestZoneTrigger : MonoBehaviour
{
    public IQuest zoneQuest;
    public LevelManager levelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && levelManager.IsQuestActive(zoneQuest))
        {
            levelManager.StartQuest(zoneQuest);
        }
    }
}
