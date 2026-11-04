namespace MuReborn.Core;
public sealed class FileGameSaveStore : IGameSaveStore
{
    private readonly string root;
    public FileGameSaveStore(string directory) { root=Path.GetFullPath(directory); Directory.CreateDirectory(root); }
    public async Task SaveAsync(string slot, byte[] payload, CancellationToken cancellationToken=default)
    {
        var path=PathFor(slot); var temp=path+"."+Guid.NewGuid().ToString("N")+".tmp";
        try { await File.WriteAllBytesAsync(temp,payload,cancellationToken); File.Move(temp,path,true); }
        finally { if(File.Exists(temp)) File.Delete(temp); }
    }
    public async Task<byte[]?> LoadAsync(string slot,CancellationToken cancellationToken=default)
    {
        var path=PathFor(slot);
        return File.Exists(path) ? await File.ReadAllBytesAsync(path,cancellationToken) : null;
    }
    private string PathFor(string slot)
    {
        if(string.IsNullOrWhiteSpace(slot) || slot.Length>64 || slot.Any(c=>!char.IsAsciiLetterOrDigit(c)&&c!='-'&&c!='_'))
            throw new ArgumentException("Invalid slot",nameof(slot));
        return Path.Combine(root,slot+".sav");
    }
}
