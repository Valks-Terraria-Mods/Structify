using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Structify.UI.Elements;

public class BlackScrollbar : UIScrollbar
{
    private readonly Asset<Texture2D> _trackTexture = Main.Assets.Request<Texture2D>("Images/UI/Scrollbar");
    private readonly Asset<Texture2D> _handleTexture = Main.Assets.Request<Texture2D>("Images/UI/ScrollbarInner");

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        CalculatedStyle dimensions = GetDimensions();
        Rectangle handleRectangle = GetHandleRectangle(dimensions);

        // Keep vanilla interaction/drag behavior, then redraw with custom colors.
        base.DrawSelf(spriteBatch);

        DrawBar(spriteBatch, _trackTexture.Value, dimensions.ToRectangle(), new Color(8, 8, 8, 230));
        DrawBar(spriteBatch, _handleTexture.Value, handleRectangle, Color.White);
    }

    private Rectangle GetHandleRectangle(CalculatedStyle dimensions)
    {
        float maxView = MaxViewSize;
        if (maxView <= 0f)
            maxView = 1f;

        return new Rectangle(
            (int)dimensions.X,
            (int)(dimensions.Y + dimensions.Height * (ViewPosition / maxView)) - 3,
            (int)dimensions.Width,
            (int)(dimensions.Height * (ViewSize / maxView)) + 7);
    }

    private static void DrawBar(SpriteBatch spriteBatch, Texture2D texture, Rectangle dimensions, Color color)
    {
        spriteBatch.Draw(
            texture,
            new Rectangle(dimensions.X, dimensions.Y - 6, dimensions.Width, 6),
            new Rectangle(0, 0, texture.Width, 6),
            color);

        spriteBatch.Draw(
            texture,
            new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height),
            new Rectangle(0, 6, texture.Width, 4),
            color);

        spriteBatch.Draw(
            texture,
            new Rectangle(dimensions.X, dimensions.Y + dimensions.Height, dimensions.Width, 6),
            new Rectangle(0, texture.Height - 6, texture.Width, 6),
            color);
    }
}
