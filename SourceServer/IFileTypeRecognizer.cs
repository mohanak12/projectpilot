namespace SourceServer
{
    public interface IFileTypeRecognizer
    {
        string RecognizeFileType(string filePath);
    }
}