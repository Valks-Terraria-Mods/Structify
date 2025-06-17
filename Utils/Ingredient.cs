namespace Structify;

public class Ingredient(int itemId, int amount)
{
    public int ItemId { get; set; } = itemId;
    public int Amount { get; set; } = amount;
}
