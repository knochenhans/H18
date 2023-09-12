using Godot;
using System;

public partial class TileMap : Godot.TileMap
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		string filePath = "res://Level1.Level";

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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
