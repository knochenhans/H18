using Godot;
using System;

public partial class Player : Character
{
	private Thing currentThing;

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionPressed("left"))
			CharacterCurrentInput[(int)CharacterCurrentInputEnum.Left] = true;
		else
			CharacterCurrentInput[(int)CharacterCurrentInputEnum.Left] = false;

		if (Input.IsActionPressed("right"))
			CharacterCurrentInput[(int)CharacterCurrentInputEnum.Right] = true;
		else
			CharacterCurrentInput[(int)CharacterCurrentInputEnum.Right] = false;

		if (Input.IsActionPressed("up"))
			CharacterCurrentInput[(int)CharacterCurrentInputEnum.Up] = true;
		else
			CharacterCurrentInput[(int)CharacterCurrentInputEnum.Up] = false;

		if (Input.IsActionPressed("down"))
			CharacterCurrentInput[(int)CharacterCurrentInputEnum.Down] = true;
		else
			CharacterCurrentInput[(int)CharacterCurrentInputEnum.Down] = false;

		if (Input.IsActionJustPressed("fire"))
			CharacterCurrentInput[(int)CharacterCurrentInputEnum.Fire] = true;
		else
			CharacterCurrentInput[(int)CharacterCurrentInputEnum.Fire] = false;

		CurrentTargetPosition = GetGlobalMousePosition();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (Input.IsActionJustPressed("interact"))
			TakeCurrentThing();
	}

	private void TakeCurrentThing()
	{
		// if (currentThing != null)
		// {
		// 	if (currentThing is Thing)
		// 	{
		// 		if (currentThing is WeaponSprite)
		// 			weaponsOwned[currentThing.thingType] = true;
		// 		else if (currentThing is AmmoSprite)
		// 			weaponsAmmo[currentThing.thingType] += (currentThing as AmmoSprite).ammoAmount;

		// 		currentThing.QueueFree();
		// 	}
		// }
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
