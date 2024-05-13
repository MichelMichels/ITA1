using System.Text;

namespace MichelMichels.Text.ITA1.Tests;

[TestClass()]
public class Ita1EncodingTests
{
    [TestMethod]
    public void GetBytes_HelloWorld()
    {
        // Arrange
        Encoding ita1 = new Ita1Encoding();

        // Act
        byte[] bytes = ita1.GetBytes("HELLO WORLD");

        // Assert
        Assert.IsTrue(AreArraysEqual(bytes, [0x0B, 0x02, 0x1B, 0x1B, 0x07, 0x10, 0x16, 0x07, 0x1C, 0x1B, 0x0F]));
    }

    [TestMethod]
    public void GetString_HelloWorld()
    {
        // Arrange
        byte[] input = [0x0B, 0x02, 0x1B, 0x1B, 0x07, 0x10, 0x16, 0x07, 0x1C, 0x1B, 0x0F];
        Encoding ita1 = new Ita1Encoding();

        // Act
        string value = ita1.GetString(input);

        // Assert
        Assert.AreEqual("HELLO WORLD", value);
    }

    static bool AreArraysEqual(ReadOnlySpan<byte> a1, ReadOnlySpan<byte> a2)
    {
        return a1.SequenceEqual(a2);
    }

    [TestMethod()]
    [DataRow("HELLO WORLD", 11)]
    [DataRow("HELLO WORLD 1", 14)]
    [DataRow("A", 1)]
    [DataRow("1", 2)]
    [DataRow("1A1", 6)]
    public void GetByteCountTest(string value, int byteCount)
    {
        // Arrange
        Encoding ita1 = new Ita1Encoding();

        // Act
        int count = ita1.GetByteCount(value);

        // Assert
        Assert.AreEqual(byteCount, count);
    }
}