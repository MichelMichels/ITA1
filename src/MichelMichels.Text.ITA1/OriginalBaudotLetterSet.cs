namespace MichelMichels.Text;

public class OriginalBaudotLetterSet : Ita1CharacterSet
{
    public override char[] Characters => [
        '\0',  'A',  'E',  '\r', 'Y',  'U',  'I',  'O',  '?',  'J',  'G',  'H',  'B',  'C',  'F',  'D',
         ' ', '\n',  'X',   'Z', 'S',  'T',  'W',  'V',  '?',  'K',  'M',  'L',  'R',  'Q',  'N',  'P',
    ];

    public override byte SwitchCharacter => 0x08;
}
