using Godot;
using System;

public partial class Player : Character
{
	// Number of sectors
	private static readonly int numSectors = 10;

	// Calculate the size of each sector in radians
	private readonly float sectorSize = 2 * Mathf.Pi / numSectors;

	// Check if a given radian value falls into a sector
	public int GetSector(float radianValue)
	{
		// Ensure the radian value is within [0, 2*pi] range
		radianValue = Mathf.Wrap(radianValue, 0, 2 * Mathf.Pi);

		// Calculate which sector the radian value falls into
		return Mathf.FloorToInt(radianValue / sectorSize);
	}

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		var velocity = Velocity;

		velocity.Y += (float)delta * Gravity;

		if (Input.IsActionPressed("left"))
		{
			velocity.X = -WalkSpeed;

			characterMovementState = CharacterMovementStateEnum.Walking;
			orientation = -1;
		}
		else if (Input.IsActionPressed("right"))
		{
			velocity.X = WalkSpeed;

			characterMovementState = CharacterMovementStateEnum.Walking;
			orientation = 1;
		}
		else
		{
			characterMovementState = CharacterMovementStateEnum.Idle;

			velocity.X = 0;
		}

		string animationSuffix = "";

		switch (characterWeaponState)
		{
			case CharacterCurrentWeaponEnum.None:
				animationSuffix = "";
				break;
			case CharacterCurrentWeaponEnum.Pistol:
				animationSuffix = "_pistol";
				break;
		}

		if (characterMovementState == CharacterMovementStateEnum.Walking)
		{
			if (velocity.X < 0)
			{
				animationSprite.Play("walk_left" + animationSuffix);
				animationSprite.Offset = new Godot.Vector2(-4, 0);
			}
			else if (velocity.X > 0)
			{
				animationSprite.Play("walk_right" + animationSuffix);
				animationSprite.Offset = new Godot.Vector2(0, 0);
			}
		}
		else if (characterMovementState == CharacterMovementStateEnum.Idle)
		{
			if (orientation < 0)
				animationSprite.Play("idle_left");
			else
				animationSprite.Play("idle_right");
			var direction = (GetGlobalMousePosition() - Position).Normalized();
			var angle = Math.PI + Math.Atan2(direction.X, direction.Y);

			var sector = GetSector((float)angle);

			if (sector >= 0 && sector < 5)
			{
				animationSprite.Play("aim_left" + animationSuffix);
				animationSprite.Frame = 4 - sector;
				animationSprite.Offset = new Godot.Vector2(-4, -4);
			}
			else
			{
				animationSprite.Play("aim_right" + animationSuffix);
				animationSprite.Frame = 4 - (sector - 5);
				animationSprite.Offset = new Godot.Vector2(0, -4);
			}
		}

		Velocity = velocity;

		// "MoveAndSlide" already takes delta time into account.
		MoveAndSlide();

		if (Input.IsActionJustPressed("fire"))
		{
			StartFiringCurrentWeapon(GetGlobalMousePosition());
		}
		else if (Input.IsActionJustReleased("fire"))
		{
			StopFiringCurrentWeapon();
		}
	}
}
