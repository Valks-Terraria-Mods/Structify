namespace ValksStructures;

public class VModSystem : ModSystem
{
    public static event Action Update;

    public static int BuildTickRate { get; set; } = 1;

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
}
