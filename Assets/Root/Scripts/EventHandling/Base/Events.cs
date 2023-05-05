namespace YagizAyer.Root.Scripts.EventHandling.Base
{
    public enum Channels
    {
        Null = 0,
        Movement = 10,  // input
        Interact = 20,  // input
        Record = 30,    // input
        ConversationStart = 40,
        ConversationPrompt = 50,
        ConversationResponse = 60,
        ConversationEnd = 70,
    }
}