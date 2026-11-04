namespace MuReborn.Core;
public sealed class OfflineWorld : IWorldSimulation
{
    private readonly Dictionary<Guid, ActorState> actors = new();
    private readonly Dictionary<Guid, PlayerCommand> commands = new();
    private readonly HashSet<Guid> monsters = new();
    private long tick;
    public WorldSnapshot Snapshot => new(tick, actors.Values.OrderBy(a => a.Id).ToArray());
    public void AddPlayer(ActorState actor) => Add(actor, false);
    public void AddMonster(ActorState actor) => Add(actor, true);
    private void Add(ActorState actor, bool monster)
    {
        if (actor.MaxHealth <= 0 || actor.Health < 0 || actor.Health > actor.MaxHealth) throw new ArgumentOutOfRangeException(nameof(actor));
        if (!actors.TryAdd(actor.Id, actor)) throw new InvalidOperationException("Duplicate actor");
        if (monster) monsters.Add(actor.Id);
    }
    public void SubmitCommand(Guid playerId, PlayerCommand command)
    {
        if (actors.ContainsKey(playerId) && !monsters.Contains(playerId)) commands[playerId] = command;
    }
    public void Step(TimeSpan fixedDelta)
    {
        float dt = (float)fixedDelta.TotalSeconds;
        if (!float.IsFinite(dt) || dt <= 0 || dt > .1f) throw new ArgumentOutOfRangeException(nameof(fixedDelta));
        foreach (var (id, cmd) in commands.OrderBy(p => p.Key))
        {
            if (!actors.TryGetValue(id, out var actor) || actor.Health == 0) continue;
            float x = float.IsFinite(cmd.MoveX) ? Math.Clamp(cmd.MoveX, -1, 1) : 0;
            float z = float.IsFinite(cmd.MoveY) ? Math.Clamp(cmd.MoveY, -1, 1) : 0;
            float mag = MathF.Sqrt(x*x+z*z);
            if (mag > 1) { x /= mag; z /= mag; }
            var p = actor.Position;
            actors[id] = actor with { Position = new(p.X+x*4*dt,p.Y,p.Z+z*4*dt) };
            if (!cmd.BasicAttack) continue;
            var nearest = monsters.Where(m => actors[m].Health > 0)
                .Select(m => (Id:m, Dist:Distance2(actors[id].Position,actors[m].Position)))
                .Where(m => m.Dist <= 4.84f).OrderBy(m=>m.Dist).ThenBy(m=>m.Id).FirstOrDefault();
            if (nearest.Id == Guid.Empty) continue;
            var target = actors[nearest.Id];
            actors[nearest.Id] = target with { Health = Math.Max(0,target.Health-10) };
        }
        commands.Clear();
        tick++;
    }
    private static float Distance2(WorldPosition a, WorldPosition b) => (a.X-b.X)*(a.X-b.X)+(a.Z-b.Z)*(a.Z-b.Z);
}
