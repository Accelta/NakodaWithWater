using UnityEngine;

public class QuestHubTrigger : MonoBehaviour
{
    public IQuest assignedQuest;
    public LevelManager levelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Display a prompt to accept the quest (UI logic here)
            // If player accepts:
            levelManager.AssignQuest(assignedQuest);
        }
    }
}
