using Godot;
using System;

public partial class cow : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 100;
	[Export]
	public float IdleTimer { get; set; } = 5;
	[Export] 
	float WalkTimer { get; set; } = 2;


	public AnimationTree AnimationTree { get; set; }
	public AnimationNodeStateMachinePlayback StateMachine { get; set; }
	public Sprite2D Sprite { get; set; }
	public Timer Timer { get; set; }

	public Vector2 MoveDirection { get; set; } = Vector2.Zero;

	public CowState CurrentCowState { get; set; } = CowState.IDLE;

	public enum CowState
	{
		IDLE,
		WALK
	}


	public override void _Ready()
	{
		AnimationTree = GetNode<AnimationTree>("AnimationTree");
		StateMachine = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/playback");
		Sprite = GetNode<Sprite2D>("CowSprite");
		Timer = GetNode<Timer>("Timer");

		SelectNewState();
	}

	public override void _Process(double data)
	{
		if(CurrentCowState == CowState.WALK)
		{
			Velocity = MoveDirection * Speed;

		}
		MoveAndSlide();
	}

	public void SelectNewDirection()
	{
		Random rng = new Random();

		MoveDirection = new Vector2(
			rng.Next(-1, 1),
			rng.Next(-1, 1)
			);

		if(MoveDirection.X < 0)
		{
			Sprite.FlipH = true;
		}
		else if(MoveDirection.X > 0)
		{
			Sprite.FlipH = false;
		}
	}

	public void SelectNewState()
	{
		if(CurrentCowState == CowState.IDLE)
		{
			StateMachine.Travel("Walk");
			CurrentCowState = CowState.WALK;
			SelectNewDirection();

			Timer.Start(WalkTimer);
		}
		else if(CurrentCowState == CowState.WALK)
		{
			StateMachine.Travel("Idle");
			CurrentCowState = CowState.IDLE;
			Timer.Start(IdleTimer);
		}
	}

	private void _on_timer_timeout()
	{
		SelectNewState();
	}
}
