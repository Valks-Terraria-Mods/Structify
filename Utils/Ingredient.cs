namespace ValksStructures;

public class Ingredient
{
    public short ItemId { get; set; }
    public int Amount { get; set; }

    public Ingredient(short itemId, int amount)
    {
        ItemId = itemId;
        Amount = amount;
    }
}
