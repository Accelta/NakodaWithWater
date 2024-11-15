public interface IQuest
{
    string QuestName { get; }
    bool IsComplete { get; }
    void StartQuest();
    void CompleteQuest();
}
