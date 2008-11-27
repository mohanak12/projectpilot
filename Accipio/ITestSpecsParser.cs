using System.IO;

namespace Accipio
{
    public interface ITestSpecsParser
    {
        TestSpecs Parse(Stream stream);
    }
}