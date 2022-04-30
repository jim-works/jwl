namespace jwl;

using System.Collections.Generic;
using System.IO;

public class TokenSequence : System.IDisposable
{

    private StreamReader reader;
    private IDisplay display;
    private string filePath;
    private int line;
    private int character;
    private char prev;

    public TokenSequence(string filePath, IDisplay display)
    {
        reader = new(filePath);
        this.filePath = filePath;
        line = 0;
        character = 0;
    }
    public IEnumerable<Token> Sequence()
    {
        while (HasNext())
        {
            yield return Next();
        }
        yield return new TokenEOF(new(new(line, character), new(line, character), filePath));
        yield break;
    }
    public bool HasNext()
    {
        return !reader.EndOfStream;
    }
    public void Dispose()
    {
        reader.Dispose();
    }
    public Token Next()
    {
        if (!HasNext())
        {
            return new TokenEOF(new(new(line, character), new(line, character), filePath));
        }
        char reading = getChar();
        //skip all whitespace
        while (char.IsWhiteSpace(reading))
        {
            if (!HasNext())
            {
                return new TokenEOF(new(new(line, character), new(line, character), filePath));
            }
            reading = getChar();
        }
        int startLine = line;
        int startChar = character;
        //single characters
        switch (reading)
        {
            case '"':
                return new TokenQuote(FileRange.single(new(line, character), filePath));
            case '\'':
                return new TokenApostrophe(FileRange.single(new(line, character), filePath));
            case '.':
                return new TokenDot(FileRange.single(new(line, character), filePath));
            case ',':
                return new TokenComma(FileRange.single(new(line, character), filePath));
            case ';':
                return new TokenSemicolon(FileRange.single(new(line, character), filePath));
            case ':':
                return new TokenColon(FileRange.single(new(line, character), filePath));
            case '(':
                return new TokenLeftParen(FileRange.single(new(line, character), filePath));
            case ')':
                return new TokenRightParen(FileRange.single(new(line, character), filePath));
            case '{':
                return new TokenLeftCurly(FileRange.single(new(line, character), filePath));
            case '}':
                return new TokenRightCurly(FileRange.single(new(line, character), filePath));
            case '[':
                return new TokenLeftSquare(FileRange.single(new(line, character), filePath));
            case ']':
                return new TokenRightSquare(FileRange.single(new(line, character), filePath));
        }
        System.Text.StringBuilder sb = new();
        if (TokenName.ValidFirstChar(reading))
        {
            //lex name
            sb.Append(reading);
            while (HasNext() && TokenName.ValidBodyChar((char)reader.Peek())) {
                sb.Append(getChar());
            }
            return new TokenName(new FileRange(new(startLine,startChar), new(line, character), filePath), sb.ToString());
        }
        while(HasNext() && !TokenSymbol.IsSeparator((char)reader.Peek())) {
            //lex symbol
            sb.Append(getChar());
        }
        if (sb.Length == 0) {
            //skip unneeded separator
            getChar();
            return Next();
        }
        return new TokenSymbol(new FileRange(new(startLine,startChar),new(line,character), filePath), sb.ToString());
    }
    //gets next char, incrementing line and character
    //make sure you aren't at end of stream before calling
    private char getChar()
    {
        char reading = (char)reader.Read();
        character++;
        if (reading == '\r' || reading == '\n')
        {
            line++;
            character = 0;
        }
        return reading;
    }
}