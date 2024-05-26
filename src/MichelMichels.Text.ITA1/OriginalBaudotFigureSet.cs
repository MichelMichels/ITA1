namespace MichelMichels.Text;

public class OriginalBaudotFigureSet : Ita1CharacterSet
{
    public override char[] Characters => [
        '\0',  '1',  '2',  '\r', '3',  '4',  '?',  '5',  ' ',  '6',  '7',  '+',  '8',  '9',  '?',  '0',
         '?', '\n',  ',',   ':', '.',  '?',  '?', '\'',  '?',  '(',  ')',  '=',  '-',  '/',  '?',  '%',
    ];

    public override byte SwitchCharacter => 0x10;
}
