using Godot;
using System;

public enum WeaponState
{
	Idle,
	Firing,
	Loading
}

public class Weapon
{
	private AudioStream fireSound;

	private double cooldownCurrent = 0;
	private double cooldownMax;

	private WeaponState state = WeaponState.Idle;

	public Weapon(string fireSoundFilename, double cooldown)
	{
		fireSound = ResourceLoader.Load<AudioStream>("res://Sounds/" + fireSoundFilename);
		cooldownMax = cooldown;
		cooldownCurrent = cooldown;
	}

	public AudioStream GetFireSound()
	{
		return fireSound;
	}

	public void Update(double delta)
	{
		if (cooldownCurrent < cooldownMax)
		{
			cooldownCurrent += delta;
		}
		else
		{
			if (cooldownCurrent >= cooldownMax)
				cooldownCurrent = cooldownMax;
		}
	}

	public void StartFiring()
	{
		if (state == WeaponState.Idle)
			state = WeaponState.Firing;
	}

	public void StopFiring()
	{
		if (state == WeaponState.Firing)
		{
			cooldownCurrent = 0;
			state = WeaponState.Idle;
		}
	}

	public bool Ready()
	{
		return (cooldownCurrent == cooldownMax);
	}
}

public class Pistol : Weapon
{
	public Pistol() : base("Sound.Abk.16.Pistol  .wav", 0.2)
	{
	}
}

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
