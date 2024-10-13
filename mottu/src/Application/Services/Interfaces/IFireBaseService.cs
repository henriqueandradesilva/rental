using System.Threading.Tasks;

namespace Application.Services.Interfaces;

public interface IFireBaseService
{
    string FirebaseStorageBucket { get; }

    IFireBaseService AddBucket(
        string bucketName);

    Task<string> UploadFileAsync(
        byte[] arrayByte,
        string fileName);
}