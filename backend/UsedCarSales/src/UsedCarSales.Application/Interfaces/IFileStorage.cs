namespace UsedCarSales.Application.Interfaces;

public interface IFileStorage
{
    /// <summary>Saves the file and returns its web path, e.g. /uploads/cars/abc.jpg</summary>
    Task<string> SaveAsync(Stream content, string extension, CancellationToken ct = default);

    void Delete(string relativePath);
}