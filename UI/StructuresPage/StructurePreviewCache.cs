using System.Collections;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using StructureHelper.API;
using StructureHelper.Models;
using Terraria;
using Terraria.ModLoader;

namespace Structify.UI.StructuresPage;

public enum StructurePreviewState
{
    Missing,
    Loading,
    Ready
}

public readonly record struct StructurePreviewResult(Texture2D Texture, StructurePreviewState State);

public static class StructurePreviewCache
{
    private static readonly Dictionary<string, Texture2D> LoadedPreviews = [];
    private static readonly HashSet<string> MissingPreviews = [];

    private static readonly Type StructurePreviewType = typeof(Generator).Assembly.GetType("StructureHelper.Util.StructurePreview");
    private static readonly ConstructorInfo StructurePreviewCtor = StructurePreviewType?.GetConstructor([typeof(string), typeof(StructureData)]);
    private static readonly MethodInfo StructurePreviewGenerateMethod = StructurePreviewType?.GetMethod("GeneratePreview", BindingFlags.Instance | BindingFlags.NonPublic);
    private static readonly Type PreviewRenderQueueType = typeof(Generator).Assembly.GetType("StructureHelper.Util.PreviewRenderQueue");
    private static readonly FieldInfo PreviewRenderQueueField = PreviewRenderQueueType?.GetField("queue", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

    public static StructurePreviewResult GetOrRequestPreview(Structure structure)
    {
        string key = GetPreviewKey(structure);

        if (LoadedPreviews.TryGetValue(key, out Texture2D texture))
            return new StructurePreviewResult(texture, StructurePreviewState.Ready);

        if (MissingPreviews.Contains(key))
            return new StructurePreviewResult(null, StructurePreviewState.Missing);

        Texture2D bundledPreview = TryLoadBundledPreview(structure);
        if (bundledPreview != null)
        {
            LoadedPreviews[key] = bundledPreview;
            return new StructurePreviewResult(bundledPreview, StructurePreviewState.Ready);
        }

        if (structure.Procedural || string.IsNullOrWhiteSpace(structure.Schematic))
        {
            MissingPreviews.Add(key);
            return new StructurePreviewResult(null, StructurePreviewState.Missing);
        }

        try
        {
            Texture2D generatedPreview = TryGenerateStructurePreview(structure);
            if (generatedPreview != null)
            {
                LoadedPreviews[key] = generatedPreview;
                return new StructurePreviewResult(generatedPreview, StructurePreviewState.Ready);
            }
        }
        catch
        {
        }

        return new StructurePreviewResult(null, StructurePreviewState.Missing);
    }

    private static Texture2D TryGenerateStructurePreview(Structure structure)
    {
        Mod mod = ModContent.GetInstance<Structify>();
        string schematicPath = $"Schematics/{structure.Schematic}.shstruct";
        StructureData data = Generator.GetStructureData(schematicPath, mod);

        return TryGenerateViaStructureHelper(data, structure.Schematic);
    }

    private static Texture2D TryGenerateViaStructureHelper(StructureData data, string name)
    {
        if (StructurePreviewCtor == null || StructurePreviewGenerateMethod == null)
            return null;

        object previewInstance = null;

        try
        {
            previewInstance = StructurePreviewCtor.Invoke([name, data]);
            return StructurePreviewGenerateMethod.Invoke(previewInstance, null) as Texture2D;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (previewInstance != null && PreviewRenderQueueField?.GetValue(null) is IList queue)
                queue.Remove(previewInstance);
        }
    }

    private static Texture2D TryLoadBundledPreview(Structure structure)
    {
        Mod mod = ModContent.GetInstance<Structify>();

        foreach (string assetPath in BuildCandidateAssetPaths(structure))
        {
            string fullAssetPath = $"{mod.Name}/{assetPath}";
            if (!ModContent.HasAsset(fullAssetPath))
                continue;

            return ModContent.Request<Texture2D>(fullAssetPath, AssetRequestMode.ImmediateLoad).Value;
        }

        return null;
    }

    private static IEnumerable<string> BuildCandidateAssetPaths(Structure structure)
    {
        if (!string.IsNullOrWhiteSpace(structure.Schematic))
        {
            yield return $"Content/Previews/{structure.Schematic}";
            yield break;
        }

        string displayNameKey = NormalizeDisplayName(structure.DisplayName);
        if (!string.IsNullOrWhiteSpace(displayNameKey))
        {
            yield return $"Content/Previews/{displayNameKey}";

            if (structure.Procedural)
                yield return $"Content/Items/{displayNameKey}";
        }
    }

    private static string NormalizeDisplayName(string displayName)
    {
        return string.Concat(displayName.Where(char.IsLetterOrDigit));
    }

    private static string GetPreviewKey(Structure structure)
    {
        if (!string.IsNullOrWhiteSpace(structure.Schematic))
            return $"schematic:{structure.Schematic}";

        return $"name:{NormalizeDisplayName(structure.DisplayName)}";
    }
}
