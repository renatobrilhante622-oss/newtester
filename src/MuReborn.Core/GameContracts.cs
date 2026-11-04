namespace MuReborn.Core;

public readonly record struct WorldPosition(float X, float Y, float Z);
public readonly record struct PlayerCommand(float MoveX, float MoveY, bool BasicAttack, int SkillSlot);
public sealed record ActorState(Guid Id, WorldPosition Position, int Level, int Health, int MaxHealth);
public sealed record WorldSnapshot(long Tick, IReadOnlyList<ActorState> Actors);

/// <summary>Engine-independent simulation; an offline or authoritative server host owns its lifetime.</summary>
public interface IWorldSimulation
{
    WorldSnapshot Snapshot { get; }
    void SubmitCommand(Guid playerId, PlayerCommand command);
    void Step(TimeSpan fixedDelta);
}

/// <summary>Versioned local persistence boundary; no server trusts local saves as authoritative.</summary>
public interface IGameSaveStore
{
    Task SaveAsync(string slot, byte[] payload, CancellationToken cancellationToken = default);
    Task<byte[]?> LoadAsync(string slot, CancellationToken cancellationToken = default);
}

public interface IGameInputSource
{
    PlayerCommand ReadCommand();
}
