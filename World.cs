using Godot;
using System;

public enum WeaponStateEnum
{
	Idle,
	Firing,
	Loading
}

public enum ThingsEnum
{
	Pistol,
	AmmoPistol
}

public class Weapon
{
	private AudioStream fireSound;

	private double cooldownCurrent = 0;
	private double cooldownMax;

	private WeaponStateEnum state = WeaponStateEnum.Idle;

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
		if (state == WeaponStateEnum.Idle)
			state = WeaponStateEnum.Firing;
	}

	public void StopFiring()
	{
		if (state == WeaponStateEnum.Firing)
		{
			cooldownCurrent = 0;
			state = WeaponStateEnum.Idle;
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

public class ThingLoader
{
	private World world;
	private TileMap tileMap;
	private Godot.Collections.Array<Thing> things;

	public ThingLoader(World world, TileMap tileMap)
	{
		this.world = world;
		this.tileMap = tileMap;

		things = new Godot.Collections.Array<Thing>();
	}

	public void AddWeapon(ThingsEnum thingType, Vector2I thingPos)
	{
		var thingInstance = GD.Load<PackedScene>("res://WeaponSprite.tscn").Instantiate<WeaponSprite>();
		thingInstance.Position = tileMap.ToGlobal(tileMap.MapToLocal(thingPos));
		thingInstance.Position = new Vector2(thingInstance.Position.X - 9, thingInstance.Position.Y - 9);
		world.AddChild(thingInstance);
	}
	public void AddAmmo(ThingsEnum thingType, Vector2I thingPos, int ammoAmount)
	{
		var thingInstance = GD.Load<PackedScene>("res://AmmoSprite.tscn").Instantiate<AmmoSprite>();
		thingInstance.Position = tileMap.ToGlobal(tileMap.MapToLocal(thingPos));
		thingInstance.Position = new Vector2(thingInstance.Position.X - 9, thingInstance.Position.Y - 9);
		thingInstance.ammoAmount = ammoAmount;
		world.AddChild(thingInstance);
	}
}

public partial class World : Node2D
{
	Vector2I startPos = new Vector2I(4, 28);

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

		Camera2D camera = GetNode<Camera2D>("../Camera2D");
		camera.UpdateLimits();

		var thingLoader = new ThingLoader(this, tileMap);
		thingLoader.AddWeapon(ThingsEnum.Pistol, new Vector2I(6, 28));
		thingLoader.AddAmmo(ThingsEnum.AmmoPistol, new Vector2I(7, 28), 80);
	}
}
