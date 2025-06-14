namespace ValksStructures;

public class GameQueue : ModSystem
{
    public static event Action Update;

    public static int BuildTickRate { get; set; } = 1;

    private static readonly List<Action> _actions = [];

    private int _count;

    public override void PreUpdateEntities()
    {
        _count++;

        if (_count >= BuildTickRate)
        {
            _count = 0;
            Update?.Invoke();
        }
    }

    public static void Enqueue(Action action) => _actions.Add(action);

    public static void ExecuteSlowly() => Update += ExecuteAction;

    public static void ExecuteInstantly()
    {
        foreach (Action action in _actions)
            action();

        _actions.Clear();

        ModContent.GetInstance<ValksStructures>().IsCurrentlyBuilding = false;
    }

    private static void ExecuteAction()
    {
        if (_actions.Count == 0)
        {
            Update -= ExecuteAction;
            ModContent.GetInstance<ValksStructures>().IsCurrentlyBuilding = false;
            return;
        }

        _actions[0]();
        _actions.RemoveAt(0);
    }
}
