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
        Assert.IsTrue(AreArraysEqual<byte>(bytes, [0x0B, 0x02, 0x1B, 0x1B, 0x07, 0x10, 0x16, 0x07, 0x1C, 0x1B, 0x0F]));
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

    [TestMethod]
    [DataRow(new byte[] { 0x0B, 0x02, 0x1B, 0x1B, 0x07, 0x10, 0x16, 0x07, 0x1C, 0x1B, 0x0F }, "HELLO WORLD")]
    [DataRow(new byte[] { 0x0 }, "\0")]
    public void GetString(byte[] input, string expectedResult)
    {
        // Arrange
        Encoding ita1 = new Ita1Encoding();

        // Act
        string value = ita1.GetString(input);

        // Assert
        Assert.AreEqual(expectedResult, value);
    }

    [TestMethod]
    public void BigByteArrayTest()
    {
        // Arrange
        Ita1Encoding ita1 = new();
        byte[] bytes = new byte[1024];
        char[] chars = new char[1024];

        for (int i = 0; i < 1024; i++)
        {
            bytes[i] = 0x0;
            chars[i] = '\0';
        }

        // Act
        char[] result = ita1.GetChars(bytes);

        // Assert
        Assert.IsTrue(AreArraysEqual<char>(result, chars));
    }

    static bool AreArraysEqual<T>(ReadOnlySpan<T> a1, ReadOnlySpan<T> a2)
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

    [TestMethod]
    [DataRow(new byte[] { 0x0B, 0x02, 0x1B, 0x1B, 0x07, 0x10, 0x16, 0x07, 0x1C, 0x1B, 0x0F }, 11)]
    [DataRow(new byte[] { 0x08, 0x01 }, 1)]
    [DataRow(new byte[] { 0x08, 0x01, 0x10, 0x01 }, 2)]
    public void GetCharCountTest(byte[] bytes, int expectedCharCount)
    {
        // Arrange
        Encoding ita1 = new Ita1Encoding();

        // Act
        int count = ita1.GetCharCount(bytes);

        // Assert
        Assert.AreEqual(expectedCharCount, count);
    }

    [TestMethod]
    [DataRow("hello", new byte[] { })]
    [DataRow("Hello", new byte[] { 0x0B })]
    public void LowerCaseTest(string input, byte[] expectedBytes)
    {
        // Arrange
        Encoding ita1 = new Ita1Encoding();

        // Act
        byte[] bytes = ita1.GetBytes(input);

        // Assert
        Assert.IsTrue(AreArraysEqual<byte>(expectedBytes, bytes));
    }

    [TestMethod]
    public void FileWriteTest()
    {
        // Arrange
        Ita1Encoding ita1Encoding = new();
        string filePath = "output.txt";

        // Act
        File.WriteAllText(filePath, "HELLO WORLD", ita1Encoding);

        // Assert
    }

    [TestMethod]
    public void FileReadTest()
    {
        // Arrange
        Ita1Encoding ita1 = new();
        string filePath = "read.txt";

        // Act
        string contents = File.ReadAllText(filePath, ita1);

        // Assert
        Assert.AreEqual("HELLO WORLD", contents);
    }

    [TestMethod]
    public void StreamReaderTest()
    {
        // Arrange
        string path = "read.txt";
        Ita1Encoding encoding = new();

        using StreamReader sr = new(path, encoding, detectEncodingFromByteOrderMarks: true);

        // Act
        sr.ReadToEnd();

        // Assert

    }

    [TestMethod]
    public void StreamReader_ByteArray_Test()
    {
        // Arrange
        Ita1Encoding encoding = new();

        byte[] bytes = new byte[1024];
        for (int i = 0; i < 1024; i++)
        {
            bytes[i] = 0x0;
        }

        using MemoryStream ms = new(bytes);
        using StreamReader sr = new(ms, encoding, detectEncodingFromByteOrderMarks: false);

        // Act
        sr.ReadToEnd();

        // Assert

    }
}