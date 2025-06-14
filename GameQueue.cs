namespace ValksStructures;

public class GameQueue : ModSystem
{
    public static event Action Update;

    public static int BuildTickRate { get; set; } = 1;

    static readonly List<Action> actions = [];

    int count;

    public override void PreUpdateEntities()
    {
        count++;

        if (count >= BuildTickRate)
        {
            count = 0;
            Update?.Invoke();
        }
    }

    public static void Enqueue(Action action) => actions.Add(action);

    public static void ExecuteSlowly() => Update += ExecuteAction;

    public static void ExecuteInstantly()
    {
        foreach (Action action in actions)
            action();

        actions.Clear();

        ModContent.GetInstance<ValksStructures>().IsCurrentlyBuilding = false;
    }

    static void ExecuteAction()
    {
        if (actions.Count == 0)
        {
            Update -= ExecuteAction;
            ModContent.GetInstance<ValksStructures>().IsCurrentlyBuilding = false;
            return;
        }

        actions[0]();
        actions.RemoveAt(0);
    }
}
