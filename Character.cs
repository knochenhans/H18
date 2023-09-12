using Godot;
using System;

public partial class Character : CharacterBody2D
{
	protected const float Gravity = 200.0f;
	protected const int WalkSpeed = 50;

	protected AnimatedSprite2D animationSprite;
	protected TileMap tileMap;

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

	protected CharacterMovementStateEnum characterMovementState = CharacterMovementStateEnum.Idle;
	protected CharacterCurrentWeaponEnum characterWeaponState = CharacterCurrentWeaponEnum.Pistol;

	protected int orientation = -1;

	public override void _Ready()
	{
		base._Ready();

		animationSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		tileMap = GetNode<TileMap>("/root/World/TileMap");
	}
}
