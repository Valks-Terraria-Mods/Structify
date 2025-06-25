using Structify.Utils;

namespace Structify.UI;

public static class StructureCatalog
{
    public static List<Structure> All { get; } =
    [
        new Structure
        {
            Schematic = "SmallHouse1",
            DisplayName = "Small House",
            Description = "A small house that comes with a basement.",
            NPCs = 1,
            Cost = 500,
            Offset = 5,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "MediumHouse1",
            DisplayName = "Medium House",
            Description = "A medium house that comes with a basement.",
            NPCs = 2,
            Cost = 1000,
            Offset = 6,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "LargeHouse1",
            DisplayName = "Large House",
            Description = "A large house that comes with a basement and an actic.",
            NPCs = 3,
            Cost = 1500,
            Offset = 6,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "Castle1",
            DisplayName = "Castle",
            Description = "A mostly empty castle for all your storage needs.",
            NPCs = 3,
            Cost = 30000,
            Offset = 15,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "BossArena",
            DisplayName = "Boss Arena",
            Description = "A small section of an arena meant to be stacked adjacently with itself.",
            Cost = 500,
            Offset = 4,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "FishingPond1",
            DisplayName = "Fishing Pond",
            Description = "A small pond surrounded by a small hut on either side.",
            NPCs = 2,
            Cost = 1500,
            Offset = 7,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "Greenhouse1",
            DisplayName = "Greenhouse",
            Description = "The structure comes with many flower pots.",
            NPCs = 1,
            Cost = 2500,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "Greenhouse2",
            DisplayName = "Overgrown House",
            Description = "The interior is empty so you can place what you want.",
            Cost = 3000,
            Offset = 16,
            Authors = [Builders.Grim]
        },
        new Structure
        {
            Schematic = "JungleFarm",
            DisplayName = "Jungle Farm",
            Description = "Several rows of mud and grass usually placed deep inside the jungle.",
            Cost = 500,
            Offset = 4,
            Authors = [Builders.Toast]
        },
        new Structure
        {
            Schematic = "PrisonCell1",
            DisplayName = "Underground Prison Cell",
            Description = "Usually placed deep underground.",
            NPCs = 1,
            Cost = 100,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "PrisonCell2",
            DisplayName = "Wide Prison Cell",
            Description = "Entry is from top. Usually placed near surface underground.",
            NPCs = 1,
            Cost = 100,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "PrisonCell3",
            DisplayName = "Sky Prison Cell",
            Description = "Usually placed in the sky.",
            NPCs = 1,
            Cost = 100,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "Ship1",
            DisplayName = "Boat",
            Description = "A boat to place on the water for fishing perhaps?",
            Cost = 1500,
            Offset = 5,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "TowerGate1",
            DisplayName = "Tower Gate",
            Description = "Defensive structure commonly used to divide biomes.",
            Cost = 1000,
            Offset = 2,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "UndergroundHouse1",
            DisplayName = "Underground House",
            Description = "The interior is empty allowing you to decorate it yourself.",
            Cost = 1000,
            Offset = 1,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "UnderWaterDome",
            DisplayName = "Underwater Dome",
            Description = "A structure you can place deep under water in the sea.",
            Cost = 1000,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "WallOfFleshArena",
            DisplayName = "Wall of Flesh Arena",
            Description = "Can be stacked horizontally.",
            Cost = 500,
            Offset = 5,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            DisplayName = "Hellavator",
            Description = "An elevator to hell.",
            Cost = 10000,
            Authors = [Builders.Valkyrienyanko],
            Procedural = true
        }
    ];
}