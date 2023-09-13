namespace Dota2GSI.UnitTests.Nodes;

public abstract class JsonDeserializeTests
{
    protected static string LoadFile(string fileName)
    {
        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", fileName);
        
        return File.ReadAllText(path);
    }
}