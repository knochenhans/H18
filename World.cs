using Godot;
using System;

public partial class World : Node2D
{
	Vector2I startPos = new Vector2I(5, 28);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set Cursor

		var cursor = ResourceLoader.Load<CompressedTexture2D>("res://Pointer2x.png");

		Input.SetCustomMouseCursor(cursor, Input.CursorShape.Arrow, new Vector2(cursor.GetWidth() / 2, cursor.GetHeight() / 2));

		// Load map

		TileMap tileMap = GetNode<TileMap>("TileMap");

		tileMap.LoadMap("res://Level1.Level");

		Player player = GetNode<Player>("Player");

		player.Position = ToGlobal(tileMap.MapToLocal(startPos));

		Camera2D camera = GetNode<Camera2D>("Camera2D");
		camera.UpdateLimits();
	}
}
