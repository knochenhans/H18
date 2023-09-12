using Godot;
using System;

public partial class node_2d : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var cursor = ResourceLoader.Load<CompressedTexture2D>("res://Pointer2x.png");

		Input.SetCustomMouseCursor(cursor, Input.CursorShape.Arrow, new Vector2(cursor.GetWidth() / 2, cursor.GetHeight() / 2));
	}
}
