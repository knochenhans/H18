using Godot;
using System;

public partial class Player : Character
{
	private Thing currentThing;

	private static readonly int numSectors = 10;
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

		string animationSuffix = "";

		switch (characterWeaponState)
		{
			case CharacterCurrentWeaponEnum.None:
				animationSuffix = "-empty";
				break;
			case CharacterCurrentWeaponEnum.Pistol:
				animationSuffix = "-pistol";
				break;
		}

		var velocity = Velocity;

		velocity.Y += (float)delta * Gravity;

		if (Input.IsActionPressed("left"))
		{
			velocity.X = -WalkSpeed;

			characterMovementState = CharacterMovementStateEnum.Walking;
			animationSprite.Play("walk-left" + animationSuffix);
			orientation = -1;
		}
		else if (Input.IsActionPressed("right"))
		{
			velocity.X = WalkSpeed;

			characterMovementState = CharacterMovementStateEnum.Walking;
			animationSprite.Play("walk-right" + animationSuffix);
			orientation = 1;
		}
		else
		{
			characterMovementState = CharacterMovementStateEnum.Idle;

			// if (orientation < 0)
			// 	animationSprite.Play("idle-left");
			// else
			// 	animationSprite.Play("idle-right");

			velocity.X = 0;
		}

		if (characterMovementState == CharacterMovementStateEnum.Idle)
		{
			var direction = (GetGlobalMousePosition() - Position).Normalized();
			var angle = Math.PI + Math.Atan2(direction.X, direction.Y);

			var sector = GetSector((float)angle);

			if (sector >= 0 && sector < 5)
			{
				animationSprite.Play("aim-left" + animationSuffix);
				animationSprite.Frame = 4 - sector;
				// animationSprite.Offset = new Godot.Vector2(-4, -4);
			}
			else
			{
				animationSprite.Play("aim-right" + animationSuffix);
				animationSprite.Frame = 4 - (sector - 5);
				// animationSprite.Offset = new Godot.Vector2(0, -4);
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

		if (Input.IsActionJustPressed("interact"))
		{
			TakeCurrentThing();
		}
	}

	private void TakeCurrentThing()
	{
		if (currentThing != null)
		{
			if (currentThing is Thing)
			{
				if (currentThing is WeaponSprite)
					weaponsOwned[currentThing.thingType] = true;
				else if (currentThing is AmmoSprite)
					weaponsAmmo[currentThing.thingType] += (currentThing as AmmoSprite).ammoAmount;

				currentThing.QueueFree();
			}
		}
	}

	public void OnScannerAreaEntered(Area2D area)
	{
		currentThing = (Thing)area;

	}
	public void OnScannerAreaExited(Area2D area)
	{
		if (currentThing == area)
			currentThing = null;
	}
}
