using Terraria;
using ValksStructures.Content.Items;

namespace ValksStructures;

public class VModSystem : ModSystem
{
    public static event Action Update;

    public static int BuildTickRate { get; set; } = 1;

    static readonly List<Action> actions = new();

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

    public static void AddAction(Action action) => actions.Add(action);
    public static void StartActions() => Update += ExecuteAction;
    public static void ExecuteAllActions()
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
