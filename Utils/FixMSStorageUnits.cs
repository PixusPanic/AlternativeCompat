using AlternativeCompat.Confection.MStorage;
using AlternativeCompat.Contagion.MStorage;
using AlternativeCompat.Depths.MStorage;
using MagicStorage.Components;
using Terraria;
using Terraria.ModLoader;

namespace AlternativeCompat.Utils
{
    // Okay, so I looked at TEStorageUnit again, and there’s a massive oversight that I, well, overlooked
    // It still checks if the ID of the tile is StorageUnit, so it thinks that custom Storage Units are invalid
    // So the tile entity ends up deleting itself ~absoluteArquarian
    /// <summary>
    /// A simple function to try and get pass a hardcoded limitation in Magic Storage for custom storage unit tiers -
    /// under normal circumstances, their tile entities are deleted when a world is unloaded and loaded back in
    /// </summary>
    [ExtendsFromMod("MagicStorage")]
    public class FixMSStorageUnits : TEStorageUnit
    {
        public override bool ValidTile(in Tile tile) => tile.TileFrameX % 36 == 0 && tile.TileFrameY % 36 == 0 &&
            ((ModLoader.HasMod(AlternativeCompat.avalon) && Baccilite(tile)) ||
            (ModLoader.HasMod(AlternativeCompat.depths) && Arquerite(tile)) ||
            (ModLoader.HasMod(AlternativeCompat.confection) && Neapolinite(tile)));

        [JITWhenModsEnabled(AlternativeCompat.avalon)]
        private static bool Baccilite(Tile tile) => TileLoader.GetTile(tile.TileType) is BacciliteStorageUnitTile;

        [JITWhenModsEnabled(AlternativeCompat.depths)]
        private static bool Arquerite(Tile tile) => TileLoader.GetTile(tile.TileType) is ArqueriteStorageUnitTile;

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private static bool Neapolinite(Tile tile) => TileLoader.GetTile(tile.TileType) is NeapoliniteStorageUnitTile;
    }
}