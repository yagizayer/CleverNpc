namespace YagizAyer.Root.Scripts.EventHandling.Base
{
    public enum InputChannels
    {
        Null = 0,
    }

    public enum LevelChannels
    {
        Null = 0,
        LevelLoadEc = 10, // LevelData
        LevelReadyEc = 20, // LevelData
        LevelStartEc = 30, // LevelData
        LevelEndEc = 40, // Bool (true = success, false = fail)
        LevelSuccessfulEc = 50, // LevelScore
        LevelFailedEc = 60, // LevelScore
    }

    public enum ScoreChannels
    {
        Null = 0,
        ScoreChangedEc = 10, // float
        ScoreMultiplierChangedEc = 20 // float
    }

    public enum ShopChannels
    {
        Null = 0,
        ItemPurchaseSuccessfulEc = 10, // ShopItemPurchaseData
        ItemPurchaseUnsuccessfulEc = 20, // ShopItemPurchaseData
        ItemResetSuccessfulEc = 30, // ShopItemResetData
        ItemResetUnsuccessfulEc = 40  // ShopItemResetData
    }

    public enum TutorialChannels
    {
        Null = 0,
        ShowTutorialEc = 10, // string (tutorial name as key)
        HideTutorialEc = 20, // string (tutorial name as key)
    }

    public enum InGameChannels
    {
        Null = 0,
    }
}