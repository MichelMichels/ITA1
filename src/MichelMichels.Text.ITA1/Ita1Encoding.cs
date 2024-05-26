using MichelMichels.Text.ITA1;
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
    private const byte SWITCH_FIGURES = 0x08;
    private const byte SWITCH_LETTERS = 0x10;

    private static readonly byte[] _bytes = [
        0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
        0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F
    ];
    private static readonly char[] _letterSetChars = [
        '\0',  'A',  'E',  '\r', 'Y',  'U',  'I',  'O',  '?',  'J',  'G',  'H',  'B',  'C',  'F',  'D',
         ' ', '\n',  'X',   'Z', 'S',  'T',  'W',  'V',  '?',  'K',  'M',  'L',  'R',  'Q',  'N',  'P',
    ];
    private static readonly char[] _figureSetChars = [
        '\0',  '1',  '2',  '\r', '3',  '4',  '?',  '5',  ' ',  '6',  '7',  '+',  '8',  '9',  '?',  '0',
         '?', '\n',  ',',   ':', '.',  '?',  '?', '\'',  '?',  '(',  ')',  '=',  '-',  '/',  '?',  '%',
    ];

    public override int GetByteCount(char[] chars, int index, int count)
    {
        int byteCount = 0;
        CharacterMode currentMode = CharacterMode.Letters;

        for (int i = index; i < count; i++)
        {
            char c = chars[i];

            bool isCharFoundInLetterSet = _letterSetChars.Contains(c);
            bool isCharFoundInFiguresSet = _figureSetChars.Contains(c);

            if (!isCharFoundInLetterSet && !isCharFoundInFiguresSet)
            {
                continue;
            }

            if (currentMode is CharacterMode.Letters)
            {
                if (isCharFoundInLetterSet)
                {
                    // Letter byte
                    byteCount++;
                }
                else if (isCharFoundInFiguresSet)
                {
                    // Switch to figures set first
                    byteCount += 1;

                    // Figure byte
                    byteCount += 1;

                    // Switch mode to Figures
                    currentMode = CharacterMode.Figures;
                }

                // Stop evaluating
                continue;
            }
            else if (currentMode is CharacterMode.Figures)
            {
                if (isCharFoundInFiguresSet)
                {
                    // Figure bytes
                    byteCount++;
                }
                else if (isCharFoundInLetterSet)
                {
                    // Switch to letters set first
                    byteCount += 1;

                    // Letter byte
                    byteCount += 1;

                    // Switch mode to Letters
                    currentMode = CharacterMode.Letters;
                }

                // Stop evaluating
                continue;
            }
        }

        return byteCount;
    }

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        CharacterMode currentMode = CharacterMode.Letters;

        List<byte> result = [];

        for (int i = charIndex; i < charCount; i++)
        {
            char c = chars[i];

            bool isCharFoundInLetterSet = _letterSetChars.Contains(c);
            bool isCharFoundInFiguresSet = _figureSetChars.Contains(c);

            if (!isCharFoundInLetterSet && !isCharFoundInFiguresSet)
            {
                continue;
            }

            if (currentMode is CharacterMode.Letters)
            {
                if (isCharFoundInLetterSet)
                {
                    result.Add(GetLetterByte(c));
                }
                else if (isCharFoundInFiguresSet)
                {
                    result.Add(SWITCH_FIGURES);
                    result.Add(GetFigureByte(c));

                    currentMode = CharacterMode.Figures;
                }

                continue;
            }
            else if (currentMode is CharacterMode.Figures)
            {
                if (isCharFoundInFiguresSet)
                {
                    result.Add(GetFigureByte(c));
                }
                else if (isCharFoundInLetterSet)
                {
                    result.Add(SWITCH_LETTERS);
                    result.Add(GetLetterByte(c));

                    currentMode = CharacterMode.Letters;
                }

                continue;
            }
        }

        result.CopyTo(bytes);

        return result.Count;
    }

    public override int GetCharCount(byte[] bytes, int index, int count)
    {
        int result = 0;
        CharacterMode currentMode = CharacterMode.Letters;

        for (int i = index; i < count; i++)
        {
            byte b = bytes[i];

            if (currentMode is CharacterMode.Letters)
            {
                if (b == SWITCH_FIGURES)
                {
                    currentMode = CharacterMode.Figures;
                    continue;
                }
            }

            if (currentMode is CharacterMode.Figures)
            {
                if (b == SWITCH_LETTERS)
                {
                    currentMode = CharacterMode.Letters;
                    continue;
                }
            }

            result++;
        }

        return result;
    }

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        CharacterMode currentMode = CharacterMode.Letters;

        int charCount = 0;

        for (int i = byteIndex; i < byteCount; i++)
        {
            byte b = bytes[i];

            if (currentMode is CharacterMode.Letters)
            {
                if (b == SWITCH_FIGURES)
                {
                    currentMode = CharacterMode.Letters;
                    continue;
                }

                chars[charIndex] = _letterSetChars[b];
                charCount++;
            }

            if (currentMode is CharacterMode.Figures)
            {
                if (b == SWITCH_LETTERS)
                {
                    currentMode = CharacterMode.Letters;
                    continue;
                }

                chars[charIndex] = _figureSetChars[b];
                charCount++;
            }

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

    private static byte GetLetterByte(char c)
    {
        return (byte)Array.IndexOf(_letterSetChars, c);
    }
    private static byte GetFigureByte(char c)
    {
        return (byte)Array.IndexOf(_figureSetChars, c);
    }
}
