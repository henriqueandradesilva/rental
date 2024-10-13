using Application.Services.Interfaces;
using CrossCutting.Settings;
using Firebase.Storage;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services;

public class FireBaseService : IFireBaseService
{
    public string FirebaseStorageBucket { get; }

    protected string BucketName { get; set; }

    public FireBaseService(
        IOptions<FirebaseStorageSettings> firebaseStorageSettings)
    {
        FirebaseStorageBucket = firebaseStorageSettings.Value.Url;
    }

    public async Task<string> UploadFileAsync(
        byte[] fileBytes,
        string fileName)
    {
        var firebaseStorage = new FirebaseStorage(FirebaseStorageBucket);

        return await firebaseStorage.Child($"{BucketName}/{fileName}")
            .PutAsync(new MemoryStream(fileBytes));
    }

    public IFireBaseService AddBucket(
        string bucketName)
    {
        BucketName = bucketName;

        return this;
    }
}