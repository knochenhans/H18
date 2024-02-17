using Godot;
using System;

public partial class TileMap : Godot.TileMap
{
	public void LoadMap(string filePath)
	{
		using var hexFile = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);

		int x = 0;
		int y = 0;

		var map_width = 72;
		var tileset_width = 16;

		ulong offset = 4772;

		hexFile.Seek(offset);

		while (!hexFile.EofReached())
		{
			int _ = hexFile.Get8();
			int tileID = hexFile.Get8() - 1;
			SetCell(0, new Vector2I(x, y), 0, new Vector2I(tileID % tileset_width, tileID / tileset_width));
			x++;

			if (x >= map_width)
			{
				x = 0;
				y++;
			}
		}
	}

	private Vector2I GetNextTile(Vector2I pos, int step)
	{
		var newPosX = (pos.X + step) % 16;
		var newPosY = pos.Y + (pos.X + step) / 16;

		return new Vector2I(newPosX, newPosY);
	}

	public void HitTile(Vector2I pos)
	{
		DestroyTile(pos);
	}

	public void DestroyTile(Vector2I pos)
	{
		var tileData = GetCellTileData(0, pos);
		if (tileData != null)
		{
			var customData = (int)tileData.GetCustomData("destructable");

			if (customData != 0)
			{
				var atlasPos = GetCellAtlasCoords(0, pos);

				SetCell(0, pos, 0, GetNextTile(atlasPos, customData));
			}
		}
	}
}
