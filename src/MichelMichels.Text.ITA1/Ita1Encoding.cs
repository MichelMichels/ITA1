using System.Text;

namespace MichelMichels.Text;

/*
 * 
 * Every operation starts with assuming the current character mode equals 'Letters'.
 * F.e. if bytes get converted to chars, it is assumed that the first byte is contained in 
 * the letter set. The same is assumed when converting chars into bytes. If a the chars 
 * start with a figure, there will be a '0x08' byte at the start to switch to the figures  
 * character set.
 * 
 */
public class Ita1Encoding : Encoding
{
    private readonly OriginalBaudotLetterSet _originalBaudotLetterSet = new();
    private readonly OriginalBaudotFigureSet _originalBaudotFigureSet = new();

    public override int GetByteCount(char[] chars, int index, int count)
    {
        int byteCount = 0;
        Ita1CharacterSet currentCharacterSet = _originalBaudotLetterSet;

        for (int i = index; i < count; i++)
        {
            char c = chars[i];

            bool isCharFoundInCurrentSet = currentCharacterSet.Characters.Contains(c);
            if (!isCharFoundInCurrentSet)
            {
                Ita1CharacterSet otherSet = currentCharacterSet == _originalBaudotLetterSet ? _originalBaudotFigureSet : _originalBaudotLetterSet;
                if (!otherSet.Characters.Contains(c))
                {
                    continue;
                }

                // Switch set and count the switch character byte
                currentCharacterSet = otherSet;
                byteCount += 1;
            }

            // Count the character byte
            byteCount += 1;
        }

        return byteCount;
    }

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        Ita1CharacterSet currentCharacterSet = _originalBaudotLetterSet;

        List<byte> result = [];

        for (int i = charIndex; i < charCount; i++)
        {
            char c = chars[i];

            bool isCharFoundInCurrentSet = currentCharacterSet.Characters.Contains(c);
            if (!isCharFoundInCurrentSet)
            {
                Ita1CharacterSet otherSet = currentCharacterSet == _originalBaudotLetterSet ? _originalBaudotFigureSet : _originalBaudotLetterSet;
                if (!otherSet.Characters.Contains(c))
                {
                    continue;
                }

                // Add switch character and switch character set
                result.Add(currentCharacterSet.SwitchCharacter);
                currentCharacterSet = otherSet;
            }

            // Add the character byte
            result.Add(currentCharacterSet.GetByte(c));
        }

        result.CopyTo(bytes);

        return result.Count;
    }

    public override int GetCharCount(byte[] bytes, int index, int count)
    {
        Ita1CharacterSet currentCharacterSet = _originalBaudotLetterSet;
        int result = 0;

        for (int i = index; i < count; i++)
        {
            byte b = bytes[i];

            if (b == currentCharacterSet.SwitchCharacter)
            {
                currentCharacterSet = currentCharacterSet == _originalBaudotLetterSet ? _originalBaudotFigureSet : _originalBaudotLetterSet;
                continue;
            }

            result++;
        }

        return result;
    }

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        Ita1CharacterSet currentCharacterSet = _originalBaudotLetterSet;

        int charCount = 0;

        for (int i = byteIndex; i < byteCount; i++)
        {
            byte b = bytes[i];

            if (b == currentCharacterSet.SwitchCharacter)
            {
                // Switch the current character set
                currentCharacterSet = currentCharacterSet == _originalBaudotLetterSet ? _originalBaudotFigureSet : _originalBaudotLetterSet;
                continue;
            }

            // Add the character
            chars[charIndex] = currentCharacterSet.Characters[b];
            charCount++;
            charIndex++;
        }

        return charCount;
    }

    public override int GetMaxByteCount(int charCount)
    {
        /*
         * The worst case scenario for ITA1 is a constant switch of letters and figures, 
         * which would result in 2 bytes per character; 1 byte for switching charsets and 
         * 1 byte for the character. This results in 2 bytes for 1 char.         
         */

        return charCount * 2;
    }

    public override int GetMaxCharCount(int byteCount)
    {
        /*
         * The worst case scenario for ITA1 is 1 char per byte. This would mean that there 
         * is no switching of charsets.
         */

        return byteCount;
    }
}