using Godot;
using System;

public partial class Character : CharacterBody2D
{
	protected const float Gravity = 200.0f;
	protected const int WalkSpeed = 50;

	protected AnimatedSprite2D animationSprite;
	protected TileMap tileMap;
	protected AudioStreamPlayer2D weaponSound;

	protected enum CharacterMovementStateEnum
	{
		Idle,
		Walking,
		Aiming
	}

	protected enum CharacterCurrentWeaponEnum
	{
		None,
		Pistol,
		Uzi,
		Minigun,
		Shotgun,
		Bazooka,
		Missile
	}

	protected Weapon[] weapons;

	protected CharacterMovementStateEnum characterMovementState = CharacterMovementStateEnum.Idle;
	protected CharacterCurrentWeaponEnum characterWeaponState = CharacterCurrentWeaponEnum.Pistol;

	protected int orientation = -1;

	public override void _Ready()
	{
		base._Ready();

		animationSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		tileMap = GetNode<TileMap>("/root/World/TileMap");
		weaponSound = GetNode<AudioStreamPlayer2D>("WeaponSound");

		weapons = new Weapon[] { new Pistol() };
	}

	protected void StartFiringCurrentWeapon(Godot.Vector2 targetPos)
	{
		if (weapons[0].Ready())
		{
			weapons[0].StartFiring();
			weaponSound.Stream = weapons[0].GetFireSound();
			weaponSound.Play();
			var clickedCellPos = tileMap.LocalToMap(targetPos);

			tileMap.DestroyTile(clickedCellPos);
		}
	}

	protected void StopFiringCurrentWeapon()
	{
		weapons[0].StopFiring();
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		for (int i = 0; i < weapons.Length; i++)
		{
			weapons[i].Update(delta);
		}
	}
}
