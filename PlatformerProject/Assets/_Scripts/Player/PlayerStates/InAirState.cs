public class InAirState : PlayerState
{
    private bool isJumping;
    private bool isFalling;
    private float normalGravityScale;
    private const float fallingGravityMultiplier = 1.3f; // Tăng 30%

    public InAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
        : base(player, stateMachine, playerData, animBoolName)
    {
        normalGravityScale = player.RB.gravityScale;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.RB.velocity.y < 0 && !isFalling)
        {
            isFalling = true;
            player.RB.gravityScale = normalGravityScale * fallingGravityMultiplier;
        }
        else if (player.RB.velocity.y >= 0 && isFalling)
        {
            isFalling = false;
            player.RB.gravityScale = normalGravityScale;
        }

        // ... existing code ...
    }

    public override void Exit()
    {
        base.Exit();
        player.RB.gravityScale = normalGravityScale;
    }

    // ... existing code ...
}