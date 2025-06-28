using Structify.Utils;

namespace Structify.UI;

public static class StructureCatalog
{
    public static List<Structure> All { get; } =
    [
        new()
        {
            Schematic = "PylonUniversal",
            DisplayName = "Pylon",
            Description = "A pylon structure that changes based on what biome you place it in.",
            Cost = 10000,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "PineTree1",
            DisplayName = "Pine Tree 1",
            Description = "Fully actuated pine tree.",
            Cost = 100,
            Authors = [Builders.Expination]
        },
        new()
        {
            Schematic = "PineTree2",
            DisplayName = "Pine Tree 2",
            Description = "Some leaves are not actuated.",
            Cost = 100,
            Authors = [Builders.Expination]
        },
        new()
        {
            Schematic = "ForestTree1",
            DisplayName = "Forest Tree 1",
            Description = "Some leaves are not actuated.",
            Cost = 100,
            Authors = [Builders.Expination]
        },
        new()
        {
            Schematic = "ForestTree2",
            DisplayName = "Forest Tree 2",
            Description = "Some leaves are not actuated.",
            Cost = 100,
            Authors = [Builders.Expination]
        },
        new()
        {
            Schematic = "JungleTree1",
            DisplayName = "Jungle Tree 1",
            Description = "Some leaves are not actuated.",
            Cost = 100,
            Authors = [Builders.Expination]
        },
        new()
        {
            Schematic = "JungleTree2",
            DisplayName = "Jungle Tree 2",
            Description = "Some leaves are not actuated.",
            Cost = 100,
            Authors = [Builders.Expination]
        },
        new()
        {
            Schematic = "House1_Expination",
            DisplayName = "Medieval House (Front)",
            Description = "A detailed house with 2 rooms and an attic.",
            NPCs = 2,
            Cost = 2000,
            Offset = 1,
            Authors = [Builders.Expination]
        },
        new()
        {
            Schematic = "House2_Expination",
            DisplayName = "Medieval House (Side)",
            Description = "A detailed house with 2 rooms.",
            NPCs = 2,
            Cost = 2000,
            Offset = 1,
            Authors = [Builders.Expination]
        },
        new()
        {
            Schematic = "House3_Expination",
            DisplayName = "Medieval Double House",
            Description = "A detailed house with 4 rooms and a very small attic.",
            NPCs = 3,
            Cost = 2000,
            Offset = 1,
            Authors = [Builders.Expination]
        },
        new()
        {
            Schematic = "House4_Expination",
            DisplayName = "Medieval Blacksmith",
            Description = "A detailed blacksmith with various workstations.",
            NPCs = 1,
            Cost = 2000,
            Offset = 1,
            Authors = [Builders.Expination]
        },
        new()
        {
            Schematic = "SmallHouse1",
            DisplayName = "Small House",
            Description = "A small house that comes with a basement.",
            NPCs = 1,
            Cost = 500,
            Offset = 5,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "MediumHouse1",
            DisplayName = "Medium House",
            Description = "A medium house that comes with a basement.",
            NPCs = 2,
            Cost = 1000,
            Offset = 6,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "LargeHouse1",
            DisplayName = "Large House",
            Description = "A large house that comes with a basement and an actic.",
            NPCs = 3,
            Cost = 1500,
            Offset = 6,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "Castle1",
            DisplayName = "Castle",
            Description = "A mostly empty castle for all your storage needs.",
            NPCs = 3,
            Cost = 30000,
            Offset = 15,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "BossArena",
            DisplayName = "Boss Arena",
            Description = "A small section of an arena meant to be stacked adjacently with itself.",
            Cost = 500,
            Offset = 4,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "FishingPond1",
            DisplayName = "Fishing Pond",
            Description = "A small pond surrounded by a small hut on either side.",
            NPCs = 2,
            Cost = 1500,
            Offset = 7,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "Greenhouse1",
            DisplayName = "Greenhouse",
            Description = "The structure comes with many flower pots.",
            NPCs = 1,
            Cost = 2500,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "Greenhouse2",
            DisplayName = "Overgrown House",
            Description = "The interior is empty so you can place what you want.",
            Cost = 3000,
            Offset = 16,
            Authors = [Builders.Grim]
        },
        new()
        {
            Schematic = "JungleFarm",
            DisplayName = "Jungle Farm",
            Description = "Several rows of mud and grass usually placed deep inside the jungle.",
            Cost = 500,
            Offset = 4,
            Authors = [Builders.Toast]
        },
        new()
        {
            Schematic = "PrisonCell1",
            DisplayName = "Underground Prison Cell",
            Description = "Usually placed deep underground.",
            NPCs = 1,
            Cost = 100,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "PrisonCell2",
            DisplayName = "Wide Prison Cell",
            Description = "Entry is from top. Usually placed near surface underground.",
            NPCs = 1,
            Cost = 100,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "PrisonCell3",
            DisplayName = "Sky Prison Cell",
            Description = "Usually placed in the sky.",
            NPCs = 1,
            Cost = 100,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "Ship1",
            DisplayName = "Boat",
            Description = "A boat to place on the water for fishing perhaps?",
            Cost = 1500,
            Offset = 5,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "TowerGate1",
            DisplayName = "Tower Gate",
            Description = "Defensive structure commonly used to divide biomes.",
            Cost = 1000,
            Offset = 2,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "UndergroundHouse1",
            DisplayName = "Underground House",
            Description = "The interior is empty allowing you to decorate it yourself.",
            Cost = 1000,
            Offset = 1,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "UnderWaterDome",
            DisplayName = "Underwater Dome",
            Description = "A structure you can place deep under water in the sea.",
            Cost = 1000,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            Schematic = "WallOfFleshArena",
            DisplayName = "Wall of Flesh Arena",
            Description = "Can be stacked horizontally.",
            Cost = 500,
            Offset = 5,
            Authors = [Builders.Valkyrienyanko]
        },
        new()
        {
            DisplayName = "Hellavator",
            Description = "An elevator to hell.",
            Cost = 10000,
            Authors = [Builders.Valkyrienyanko],
            Procedural = true
        }
    ];
}