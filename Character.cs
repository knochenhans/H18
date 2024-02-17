using Godot;
using System.Collections.Generic;

public partial class Character : CharacterBody2D
{
	protected const float Gravity = 200.0f;
	protected const int WalkSpeed = 50;

	protected AnimatedSprite2D AnimationSpriteNode;
	protected AudioStreamPlayer2D WeaponSoundNode;

	protected enum CharacterMovementStateEnum
	{
		Idle,
		Walking,
		Aiming
	}

	protected Dictionary<WeaponTypeEnum, bool> WeaponsOwned = new();
	protected Dictionary<WeaponTypeEnum, int> WeaponsAmmo = new();

	protected Weapon[] WeaponSlots { get; set; } = new Weapon[7];

	protected CharacterMovementStateEnum CharacterMovementState { get; set; } = CharacterMovementStateEnum.Idle;
	protected WeaponTypeEnum CharacterWeaponState { get; set; } = WeaponTypeEnum.None;

	protected int orientation = -1;

	public override void _Ready()
	{
		base._Ready();

		AnimationSpriteNode = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	protected void StartFiringCurrentWeapon(Vector2 targetPos)
	{
		// if (weapons[0].Ready())
		// {
		WeaponSlots[0].StartFiring(targetPos);
		// 	weaponSound.Stream = weapons[0].GetFireSound();
		// 	weaponSound.Play();
		// 	var clickedCellPos = tileMap.LocalToMap(targetPos);

		// 	tileMap.DestroyTile(clickedCellPos);
		// }
	}

	protected void StopFiringCurrentWeapon()
	{
		WeaponSlots[0].StopFiring();
	}


	// public override void _PhysicsProcess(double delta)
	// {
	// 	base._PhysicsProcess(delta);
	// }

	public void AddWeapon(Weapon weapon)
	{
		AddChild(weapon);
		WeaponsOwned[weapon.WeaponType] = true;
		WeaponsAmmo[weapon.WeaponType] = 0;
		WeaponSlots[0] = weapon;
	}
}
