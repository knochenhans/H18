using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{
	private Player player;
	public float FollowSpeed = 5.0f;

	private TileMap tileMap;

	public override void _Ready()
	{
		player = GetNode<Player>("../World/Player");
		tileMap = GetNode<TileMap>("../World/TileMap"); // Adjust the path to your TileMap node.
	}

	public void UpdateLimits()
	{
		var limits = tileMap.GetUsedRect();
		var size = tileMap.TileSet.TileSize;

		LimitLeft = limits.Position.X * size.X;
		LimitTop = limits.Position.Y * size.Y;
		LimitRight = limits.End.X * size.X;
		LimitBottom = limits.End.Y * size.Y;
	}

	public override void _Process(double delta)
	{
		Vector2 targetPosition = player.GlobalPosition;

		// Calculate the camera's position to follow the player.
		GlobalPosition = GlobalPosition.Lerp(targetPosition, FollowSpeed * (float)delta);
	}
}
