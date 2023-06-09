namespace YagizAyer.Root.Scripts.EventHandling.Base
{
    public enum Channels
    {
        Null = 0,
        Movement = 10,  // input
        Look = 15,  // input
        Interact = 20,  // input
        Record = 30,    // input
        Cancel = 35,    // input
        Conversating = 40,
        Recording = 50,
        PlayerAnswering = 55,
        NpcThinking = 60,
        NpcAnswering = 70,
        CancelConversating = 80,
        NpcAttacking = 90,
        NpcAttackEnding = 100,
    }
}