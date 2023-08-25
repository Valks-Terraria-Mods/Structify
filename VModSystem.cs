namespace ValksStructures;

public class VModSystem : ModSystem
{
    public static event Action Update;

    public override void PreUpdateEntities()
    {
        Update?.Invoke();
    }
}
