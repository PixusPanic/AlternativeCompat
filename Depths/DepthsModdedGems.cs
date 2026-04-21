using AlternativeCompat.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using TheDepths.Dusts;
using TheDepths.Tiles;

namespace AlternativeCompat.Depths
{
    [JITWhenModsEnabled(AlternativeCompat.depths)]
    public class DepthsModdedGems
    {
        [JITWhenModsEnabled(AlternativeCompat.depths)]
        public class DepthsGemSystem : ModSystem
		{
            /// <summary>
            /// The number of merging shale tiles added by The Depths.
            /// </summary>
            internal static int _originalShaleTileCount;

			/// <summary>
			/// Tiles that merge with all shale tiles.
			/// </summary>
			internal static readonly List<int> _mergingShaleTiles = new List<int>
            {
                /*ModContent.TileType<ShaleBlock>(),
                ModContent.TileType<Shalestone>(),
                ModContent.TileType<ShalestoneAmethyst>(),
                ModContent.TileType<ShalestoneDiamond>(),
                ModContent.TileType<ShalestoneEmerald>(),
                ModContent.TileType<ShalestoneRuby>(),
                ModContent.TileType<ShalestoneSapphire>(),
                ModContent.TileType<ShalestoneTopaz>()*/
            };

            /// <summary>
            /// The frequencies of each tile in <see cref="_mergingShaleTiles"/> past <see cref="_originalShaleTileCount"/>.
            /// </summary>
            private static readonly List<float> _customFrequencies = new();

			/// <summary>
			/// The generated texture for each added shale tile.
			/// </summary>
			private static readonly Dictionary<int, Asset<Texture2D>> _tileTypeToAsset = new();

			/// <summary>
			/// The texture of <see cref="Shalestone"/>.
			/// </summary>
			private static Texture2D _shaleTexture;

			/// <summary>
			/// The int[] in <see cref="TheDepthsWorldGen"/> that holds all loaded gems (vanilla's gems by default).
			/// </summary>
			private static MethodInfo _gemsList;

			public override bool IsLoadingEnabled(Mod mod)
			{
                if (!ModLoader.HasMod("TheDepths")) return false;

                if (ModLoader.TryGetMod("TheDepths", out var depths)) {
                    TheDepths = depths;

                    if (depths.TryFind("ShaleBlock", out ModTile shale))
                        _mergingShaleTiles.Add(shale.Type);
                    if (depths.TryFind("Shalestone", out ModTile shalestone))
                        _mergingShaleTiles.Add(shalestone.Type);
                    if (depths.TryFind("ShalestoneAmethyst", out ModTile shalestoneAmethyst))
                        _mergingShaleTiles.Add(shalestoneAmethyst.Type);
                    if (depths.TryFind("ShalestoneDiamond", out ModTile shalestoneDiamond))
                        _mergingShaleTiles.Add(shalestoneDiamond.Type);
                    if (depths.TryFind("ShalestoneEmerald", out ModTile shalestoneEmerald))
                        _mergingShaleTiles.Add(shalestoneEmerald.Type);
                    if (depths.TryFind("ShalestoneRuby", out ModTile shalestoneRuby))
                        _mergingShaleTiles.Add(shalestoneRuby.Type);
                    if (depths.TryFind("ShalestoneSapphire", out ModTile shalestoneSapphire))
                        _mergingShaleTiles.Add(shalestoneSapphire.Type);
                    if (depths.TryFind("OnyxShalestone", out ModTile shalestoneOnyx))
                        _mergingShaleTiles.Add(shalestoneOnyx.Type);
                }

				if (ModTileTextureBypass.Failed)
				{
					mod.Logger.Info("Not loading The Depths shale gems because ModTile texture bypassing failed.");
					return false;
				}

				if (!LoadGemsMethod())
				{
					mod.Logger.Info("Not loading The Depths shale gems because the generation method could not be found.");
					return false;
				}

				return true;
			}
            private static Mod TheDepths;
            private static Assembly depthsAssembly;
            private static Type depthsGen;

            // JIT issues`
            private static bool LoadGemsMethod()
			{
                if (TheDepths == null) return false;
                depthsAssembly = TheDepths.GetType().Assembly;

                // Because DepthGems is internal, reflection needs to be used
                foreach (Type type in depthsAssembly.GetTypes())
                {
                    if (type.Name == "DepthsGen") depthsGen = type;
                }

                _gemsList = depthsGen.GetMethods
                    (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
					.FirstOrDefault(m => m.Name.Contains("Gems"));

                return _gemsList != null;
			}
			
			private static Hook _gemsListHook;

            public override void Load()
			{
				_originalShaleTileCount = _mergingShaleTiles.Count;

                if (_gemsList != null)
				{
					_gemsListHook = new(_gemsList, (Func<Func<int[]>, int[]>)((orig) =>
                    {
                        var gemList = orig() ?? [];

                        // Add the new gem tiles, then make sure there aren't any copies of the same ID
                        var newGems = _mergingShaleTiles.Skip(_originalShaleTileCount);
                        return gemList.Concat(newGems).Distinct().ToArray();
                    }));
					_gemsListHook.Apply();
				}
			}
                    
                    
			public override void Unload()
			{
				if (_gemsList != null)
					_gemsListHook?.Undo();

				_tileTypeToAsset?.Clear();
				_mergingShaleTiles?.Clear();
				_shaleTexture = null;
				_gemsList = null;
			}

			/*public override void SetStaticDefaults()
			{
				if (_mergingShaleTiles.Count != _originalShaleTileCount)
				{
					//UpdateTileMerging();
				}
			}

			/// <summary>
			/// Updates all shale tile merging to merge with newly-added tiles.
			/// </summary>
			private static void UpdateTileMerging()
			{
				foreach (int baseMergingTile in _mergingShaleTiles)
				{
					foreach (int otherMergingTile in _mergingShaleTiles)
					{
						if (baseMergingTile == otherMergingTile)
						{
							continue;
						}

						Main.tileMerge[baseMergingTile][otherMergingTile] = true;
					}
				}
			}*/

			/// <summary>
			/// Adds a type of gem to spawn in The Depths.
			/// </summary>
			/// <param name="mod">The mod to add the new tile to.</param>
			/// <param name="gemItemId">The item ID of the gem item. Needed for drops. (Ex: <see cref="ItemID.Amethyst"/>)</param>
			/// <param name="gemTileId">The tile ID of the "embedded" gem tile. (Ex: <see cref="TileID.Amethyst"/>)</param>
			/// <param name="gemBaseTileId">The tile ID of the tile <paramref name="gemTileId"/> is based off of. (Ex: <see cref="TileID.Stone"/>)</param>
			/// <param name="frequency">How much of this gem should spawn. Scales with <see cref="Main.maxTilesX"/>. For reference, Amethyst uses <c>0.5f</c>, while Diamond uses <c>0.05f</c>.</param>
			internal static void AddNewTile(Mod mod, int gemItemId, int gemTileId, int gemBaseTileId, float frequency)
			{
				string internalName = ItemLoader.GetItem(gemItemId).Name;
				ShaleGemTile instance = new(gemItemId, gemTileId, internalName);
				mod.AddContent(instance);
				_mergingShaleTiles.Add(instance.Type);
				//_customFrequencies.Add(frequency);
				Main.QueueMainThreadAction(() => GenerateTextureForTile(mod, instance.Type, gemTileId, gemBaseTileId, internalName));
			}

			private static void GenerateTextureForTile(Mod mod, int tileType, int gemTileId, int gemBaseTileId, string gemInternalName)
			{
				static Texture2D GetTileTexture(int id)
				{
					string path = id < TileID.Count ? $"Terraria/Images/Tiles_{id}" : TileLoader.GetTile(id).Texture;
					return ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad).Value;
				}

				_shaleTexture ??= GetTileTexture(ModContent.TileType<Shalestone>());
				Texture2D gems = TextureHelper.GetOverlaidTexture(GetTileTexture(gemBaseTileId), GetTileTexture(gemTileId));
				Texture2D shaleGems = TextureHelper.OverlayTextures(_shaleTexture, gems);
				_tileTypeToAsset[tileType] = TextureHelper.CreateAssetFromTexture(shaleGems, $"ModCompatTweaks/Assets/TheDepths/Shalestone{gemInternalName}", mod);
			}

			internal static Asset<Texture2D> GetTexture(ushort type)
			{
				// If this fails, let it throw.
				return _tileTypeToAsset[type];
			}
		}

        /// <summary>
        /// A general imitation of "The Depth"'s Shale gem tiles.
        /// </summary>
        [Autoload(false)]
        [JITWhenModsEnabled(AlternativeCompat.depths)]
        public class ShaleGemTile : ModTile, ILoadOwnTexture
        {
            private readonly int _gemItemId;
            private readonly int _gemTileId;
            private readonly string _gemInternalName;

            public ShaleGemTile(int gemItemId, int gemTileId, string gemInternalName)
            {
                _gemItemId = gemItemId;
                _gemTileId = gemTileId;
                _gemInternalName = gemInternalName;
            }

            public override string Name => $"Shalestone{_gemInternalName}";

            Asset<Texture2D> ILoadOwnTexture.GetTexture()
            {
                return DepthsGemSystem.GetTexture(Type);
            }

            public override void SetStaticDefaults()
            {
                Main.tileSolid[Type] = true;
                Main.tileMergeDirt[Type] = true;
                //Main.tileBlockLight[Type] = true;
                //Main.tileLighted[Type] = false;
                Main.tileSpelunker[Type] = true;
                DustType = ModContent.DustType<ShaleDust>();
                HitSound = SoundID.Tink;
                MinPick = 65;

                RegisterItemDrop(_gemItemId);

                //LocalizedText originalText = Lang._mapLegendCache[MapHelper.TileToLookup(_gemTileId, 0)];
                ModTile originalTile = TileLoader.GetTile(_gemTileId);
                MapTile mapTile = MapTile.Create((ushort)_gemTileId, byte.MaxValue, 0);
                Color color = MapHelper.GetMapTileXnaColor(ref mapTile);

                AddMapEntry(color, Language.GetOrRegister(originalTile.Mod.GetLocalizationKey("MapObject." + originalTile.Name)));

                // Tile merging is handled in DepthsGemSystemGlobalTile
            }

            public override void NumDust(int i, int j, bool fail, ref int num)
            {
                num = fail ? 1 : 3;
            }
        }

        [JITWhenModsEnabled(AlternativeCompat.depths)]
        public class DepthsGemSystemGlobalTile : GlobalTile
		{
            public override void SetStaticDefaults()
			{
                if (DepthsGemSystem._mergingShaleTiles.Count == 0) return;

                if (DepthsGemSystem._mergingShaleTiles.Count != DepthsGemSystem._originalShaleTileCount)
                {
                    foreach (int baseMergingTile in DepthsGemSystem._mergingShaleTiles)
                    {
                        foreach (int otherMergingTile in DepthsGemSystem._mergingShaleTiles)
                        {
                            Main.tileMerge[baseMergingTile][otherMergingTile] = true;
                            Main.tileMerge[otherMergingTile][baseMergingTile] = true;
                        }
                    }
                }
            }
		}

        [JITWhenModsEnabled(AlternativeCompat.depths)]
        public class DepthsAddModdedGems : ModSystem
		{
            public void AddShaleGem(int gemItemId, int gemTileId, int gemBaseTileId, float frequency = 0)
            {
                DepthsGemSystem.AddNewTile(Mod, gemItemId, gemTileId, gemBaseTileId, frequency);
            }

            public override void OnModLoad()
            {
				if (ModLoader.TryGetMod("Avalon", out var avalon)) {
					if (avalon.TryFind("Peridot", out ModItem peridotItem) && avalon.TryFind("Peridot", out ModTile peridotTile))
                        AddShaleGem(peridotItem.Type, peridotTile.Type, TileID.Stone, 0.4f);
                    if (avalon.TryFind("Tourmaline", out ModItem tourmalineItem) && avalon.TryFind("Tourmaline", out ModTile tourmalineTile))
                        AddShaleGem(tourmalineItem.Type, tourmalineTile.Type, TileID.Stone, 0.4f);
                    if (avalon.TryFind("Zircon", out ModItem zirconItem) && avalon.TryFind("Zircon", out ModTile zirconTile))
                        AddShaleGem(zirconItem.Type, zirconTile.Type, TileID.Stone, 0.4f);
                }
                if (ModLoader.TryGetMod("ThoriumMod", out var thorium))
				{
                    if (thorium.TryFind("SmoothCoal", out ModItem coalItem) && thorium.TryFind("SmoothCoal", out ModTile coalTile))
                        AddShaleGem(coalItem.Type, coalTile.Type, TileID.Stone, 0.2f);
                    if (thorium.TryFind("Aquamarine", out ModItem aquamarineItem) && thorium.TryFind("Aquamarine", out ModTile aquamarineTile))
                        AddShaleGem(aquamarineItem.Type, aquamarineTile.Type, TileID.Stone, 0.4f);
                    if (thorium.TryFind("Opal", out ModItem opalItem) && thorium.TryFind("Opal", out ModTile opalTile))
                        AddShaleGem(opalItem.Type, opalTile.Type, TileID.Stone, 0.4f);
                }
				if (ModLoader.TryGetMod("Verdant", out var verdant) &&
					verdant.TryFind("AquamarineItem", out ModItem aquamarineVeItem) && verdant.TryFind("Aquamarine", out ModTile aquamarineVeTile))
					AddShaleGem(aquamarineVeItem.Type, aquamarineVeTile.Type, TileID.Stone);
            }
        }
	}
}