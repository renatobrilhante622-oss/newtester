namespace MuReborn.Core;
public sealed class AutoCombatController
{
    public bool Enabled { get; private set; }
    public WorldPosition Anchor { get; private set; }
    public float FarmRadius { get; init; } = 12;
    public void Start(WorldPosition anchor) { Anchor = anchor; Enabled = true; }
    public void Stop() => Enabled = false;
    public PlayerCommand Decide(ActorState player, IReadOnlyList<ActorState> monsters)
    {
        if (!Enabled || player.Health <= 0) return default;
        var target = monsters.Where(m => m.Health > 0 && D2(m.Position,Anchor) <= FarmRadius*FarmRadius)
            .OrderBy(m => D2(m.Position,player.Position)).ThenBy(m=>m.Id).FirstOrDefault();
        if (target is null) return Move(player.Position,Anchor);
        if (D2(player.Position,target.Position) <= 4f) return new(0,0,true,0);
        return Move(player.Position,target.Position);
    }
    private static PlayerCommand Move(WorldPosition a, WorldPosition b)
    {
        float x=b.X-a.X,z=b.Z-a.Z,length=MathF.Sqrt(x*x+z*z);
        return length < .05f ? default : new(x/length,z/length,false,0);
    }
    private static float D2(WorldPosition a,WorldPosition b) => (a.X-b.X)*(a.X-b.X)+(a.Z-b.Z)*(a.Z-b.Z);
}
