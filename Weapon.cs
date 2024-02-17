using Godot;

public partial class Weapon : Node
{
	[Signal]
	public delegate void FireEventHandler(Character owner, Vector2 targetPos);

	[Export]
	public WeaponTypeEnum WeaponType { get; set; }

	[Export]
	public int Damage { get; set; }

	[Export]
	public AudioStream FireSound { get; set; }

	[Export]
	public double Cooldown { get; set; }

	Vector2 TargetPos { get; set; }

	AudioStreamPlayer2D FireSoundNode { get; set; }

	private AudioStream fireSound;

	public WeaponStateEnum State
	{
		get => state; set
		{
			state = value;
			if (state == WeaponStateEnum.Firing)
			{
				OnFire();
			}
		}
	}

	private WeaponStateEnum state = WeaponStateEnum.Idle;

	public override void _Ready()
	{
		base._Ready();

		FireSoundNode = GetNode<AudioStreamPlayer2D>("FireSound");
	}

	public void StartFiring(Vector2 targetPos)
	{
		TargetPos = targetPos;

		if (State == WeaponStateEnum.Idle)
			State = WeaponStateEnum.Firing;
	}

	public void StopFiring()
	{
		if (State == WeaponStateEnum.Firing)
			State = WeaponStateEnum.Idle;
	}

	private void OnFire()
	{
		FireSoundNode.Stream = FireSound;
		FireSoundNode.Play();
		EmitSignal(SignalName.Fire, GetParent<Character>(), TargetPos);
	}
}