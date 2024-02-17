using Godot;
using System;
using System.Collections.Generic;

public partial class Character : CharacterBody2D
{
	protected const float Gravity = 200.0f;
	protected const int WalkSpeed = 50;

	private static readonly int numSectors = 10;
	private readonly float sectorSize = 2 * Mathf.Pi / numSectors;

	protected AnimatedSprite2D AnimationSpriteNode;
	protected AudioStreamPlayer2D WeaponSoundNode;

	protected enum CharacterMovementStateEnum
	{
		Idle,
		Walking,
		Aiming,
		Ladder
	}

	protected enum CharacterCurrentInputEnum
	{
		Left,
		Right,
		Up,
		Down,
		Fire
	}

	protected bool[] CharacterCurrentInput;

	protected Dictionary<WeaponTypeEnum, bool> WeaponsOwned = new();
	protected Dictionary<WeaponTypeEnum, int> WeaponsAmmo = new();

	protected Vector2 CurrentTargetPosition { get; set; }

	protected Weapon[] WeaponSlots { get; set; } = new Weapon[7];

	protected CharacterMovementStateEnum CharacterMovementState { get; set; } = CharacterMovementStateEnum.Idle;

	protected WeaponTypeEnum characterWeaponState;
	protected WeaponTypeEnum CharacterWeaponState
	{
		get => characterWeaponState;
		set
		{
			characterWeaponState = value;
			switch (CharacterWeaponState)
			{
				case WeaponTypeEnum.None:
					AnimationSuffix = "-empty";
					break;
				case WeaponTypeEnum.Pistol:
					AnimationSuffix = "-pistol";
					break;
			}
		}
	}

	protected int Orientation { get; set; } = -1;

	protected string AnimationSuffix { get; set; } = "";

	public override void _Ready()
	{
		base._Ready();

		CharacterWeaponState = WeaponTypeEnum.None;

		AnimationSpriteNode = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		CharacterCurrentInput = new bool[Enum.GetNames(typeof(CharacterCurrentInputEnum)).Length];
	}

	protected void StartFiringCurrentWeapon(Vector2 targetPos)
	{
		WeaponSlots[0].StartFiring(targetPos);
	}

	protected void StopFiringCurrentWeapon()
	{
		WeaponSlots[0].StopFiring();
	}

	public void AddWeapon(Weapon weapon)
	{
		AddChild(weapon);
		WeaponsOwned[weapon.WeaponType] = true;
		WeaponsAmmo[weapon.WeaponType] = 0;
		WeaponSlots[0] = weapon;
	}

	// Check if a given radian value falls into a sector
	public int GetSector(float radianValue)
	{
		// Ensure the radian value is within [0, 2*pi] range
		radianValue = Mathf.Wrap(radianValue, 0, 2 * Mathf.Pi);

		// Calculate which sector the radian value falls into
		return Mathf.FloorToInt(radianValue / sectorSize);
	}

	public override void _PhysicsProcess(double delta)
	{
		var velocity = Velocity;

		velocity.Y += (float)delta * Gravity;

		if (CharacterCurrentInput[(int)CharacterCurrentInputEnum.Left])
		{
			velocity.X = -WalkSpeed;

			CharacterMovementState = CharacterMovementStateEnum.Walking;
			AnimationSpriteNode.Play("walk-left" + AnimationSuffix);
			Orientation = -1;
		}
		else if (CharacterCurrentInput[(int)CharacterCurrentInputEnum.Right])
		{
			velocity.X = WalkSpeed;

			CharacterMovementState = CharacterMovementStateEnum.Walking;
			AnimationSpriteNode.Play("walk-right" + AnimationSuffix);
			Orientation = 1;
		}
		else
		{
			CharacterMovementState = CharacterMovementStateEnum.Idle;

			// if (orientation < 0)
			// 	animationSprite.Play("idle-left");
			// else
			// 	animationSprite.Play("idle-right");

			velocity.X = 0;
		}

		if (CharacterMovementState == CharacterMovementStateEnum.Idle)
		{
			var currentTargetDirection = (CurrentTargetPosition - Position).Normalized();
			var angle = Math.PI + Math.Atan2(currentTargetDirection.X, currentTargetDirection.Y);

			var sector = GetSector((float)angle);

			if (sector >= 0 && sector < 5)
			{
				AnimationSpriteNode.Play("aim-left" + AnimationSuffix);
				AnimationSpriteNode.Frame = 4 - sector;
				// animationSprite.Offset = new Godot.Vector2(-4, -4);
			}
			else
			{
				AnimationSpriteNode.Play("aim-right" + AnimationSuffix);
				AnimationSpriteNode.Frame = 4 - (sector - 5);
				// animationSprite.Offset = new Godot.Vector2(0, -4);
			}
		}

		Velocity = velocity;

		// "MoveAndSlide" already takes delta time into account.
		MoveAndSlide();

		if (CharacterCurrentInput[(int)CharacterCurrentInputEnum.Fire])
			StartFiringCurrentWeapon(CurrentTargetPosition);
		else
			StopFiringCurrentWeapon();
	}
}
