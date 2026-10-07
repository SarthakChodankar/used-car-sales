using UsedCarSales.Application.Interfaces;

namespace UsedCarSales.Infrastructure.Storage;

public class LocalFileStorage : IFileStorage
{
    private const string UploadFolder = "uploads/cars";
    private readonly string _rootPath;

    public LocalFileStorage(string rootPath) => _rootPath = Path.GetFullPath(rootPath);

    public async Task<string> SaveAsync(Stream content, string extension, CancellationToken ct = default)
    {
        var directory = Path.Combine(_rootPath, "uploads", "cars");
        Directory.CreateDirectory(directory);

        // Random file name: never trust the name the user uploaded.
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(directory, fileName);

        await using var file = new FileStream(fullPath, FileMode.CreateNew);
        await content.CopyToAsync(file, ct);

        return $"/{UploadFolder}/{fileName}";
    }

    public void Delete(string relativePath)
    {
        var fullPath = Path.GetFullPath(Path.Combine(_rootPath, relativePath.TrimStart('/')));

        // Safety: only delete files that are inside our upload root.
        if (!fullPath.StartsWith(_rootPath, StringComparison.OrdinalIgnoreCase)) return;

        if (File.Exists(fullPath)) File.Delete(fullPath);
    }
}