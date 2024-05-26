namespace MichelMichels.Text;

public abstract class Ita1CharacterSet
{
    public abstract char[] Characters { get; }
    public abstract byte SwitchCharacter { get; }
    public byte GetByte(char c)
    {
        return (byte)Array.IndexOf(Characters, c);
    }
}
