namespace Structify;

public class Ingredient(short itemId, int amount)
{
    public short ItemId { get; set; } = itemId;
    public int Amount { get; set; } = amount;
}
