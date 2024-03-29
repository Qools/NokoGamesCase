using System;

public static class EventSystem
{
    public static Action OnStartGame;
    public static void CallStartGame() => OnStartGame?.Invoke();

    public static Action<GameResult> OnGameOver;
    public static void CallGameOver(GameResult gameResult) => OnGameOver?.Invoke(gameResult);

    public static Action OnNewLevelLoad;
    public static void CallNewLevelLoad() => OnNewLevelLoad?.Invoke();

    public static Action OnJoystickButtonUp;
    public static void CallJoystickButtonUp() => OnJoystickButtonUp?.Invoke();

    public static Action OnItemTakenFromSpawnerStorage;
    public static void CallItemTakenFromSpawnerStorage() => OnItemTakenFromSpawnerStorage?.Invoke();

    public static Action OnItemTakenFromTransformerStorage;
    public static void CallItemTakenFromTransformerStorage() => OnItemTakenFromTransformerStorage?.Invoke();

    public static Action OnInventoryFull;
    public static void CallInvetoryFull() => OnInventoryFull?.Invoke();

    public static Action OnInvetoryEmpty;
    public static void CallInventoryEmpty() => OnInvetoryEmpty?.Invoke();
}
