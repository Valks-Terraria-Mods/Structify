namespace Structify;

public class GameQueue : ModSystem
{
    public static event Action Update;

    public static int BuildTickRate { get; set; } = 1;

    public static List<Action> Actions = [];

    private int _tickRate;

    public override void PreUpdateEntities()
    {
        _tickRate++;

        if (_tickRate >= BuildTickRate)
        {
            _tickRate = 0;
            Update?.Invoke();
        }
    }

    public static void Enqueue(Action action) => Actions.Add(action);

    public static void ExecuteSlowly() => Update += ExecuteAction;

    public static void ExecuteInstantly()
    {
        foreach (Action action in Actions)
            action();

        Actions.Clear();

        ModContent.GetInstance<Structify>().IsCurrentlyBuilding = false;
    }

    private static void ExecuteAction()
    {
        if (Actions.Count == 0)
        {
            Update -= ExecuteAction;
            ModContent.GetInstance<Structify>().IsCurrentlyBuilding = false;
            return;
        }

        Actions[0]();
        Actions.RemoveAt(0);
    }
}
