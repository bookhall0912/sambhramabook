namespace SambhramaBook.Application.Services;

public interface IBlobStorageService
{
    /// <summary>
    /// Generates a SAS token for direct upload to Azure Blob Storage
    /// </summary>
    /// <param name="fileName">The name of the file to upload</param>
    /// <param name="contentType">The content type of the file</param>
    /// <returns>SAS URL for direct upload</returns>
    Task<string> GenerateUploadSasUrlAsync(string fileName, string contentType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the public URL for a blob (via CDN if configured)
    /// </summary>
    /// <param name="blobName">The name of the blob</param>
    /// <returns>Public URL to access the blob</returns>
    string GetBlobUrl(string blobName);

    /// <summary>
    /// Deletes a blob from storage
    /// </summary>
    /// <param name="blobName">The name of the blob to delete</param>
    Task DeleteBlobAsync(string blobName, CancellationToken cancellationToken = default);
}

