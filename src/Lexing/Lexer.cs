namespace jwl;

using System.Collections.Generic;
using System.IO;

public class Lexer : CompilerLayer<string, TokenSequence> {
    
    public Lexer(IDisplay display) : base(display) {}
    
    public override TokenSequence Process(string filepath)
    {
        return new TokenSequence(this, filepath, display);
    }

    //advances the reader using getChar() and returns the next token
    public Token LexNext(StreamReader reader, ref FilePosition pos, string filePath)
    {
        if (!hasNextChar(reader))
        {
            return new TokenEOF(new(new(pos.line, pos.character + 1), new(pos.line, pos.character + 1), filePath));
        }
        char reading = getChar(reader, ref pos);
        //boop bop
        //skip all whitespace and separators. returns EOF if needed
        while (char.IsWhiteSpace(reading) || char.IsSeparator(reading))
        {
            if (!hasNextChar(reader))
            {
                //recursive call to handle EOF
                return LexNext(reader, ref pos, filePath);
            }
            reading = getChar(reader, ref pos);
        }
        int startLine = pos.line;
        int startChar = pos.character;
        //single characters
        switch (reading)
        {
            case '"':
                return new TokenQuote(FileRange.single(pos, filePath));
            case '\'':
                return new TokenApostrophe(FileRange.single(pos, filePath));
            case '.':
                return new TokenDot(FileRange.single(pos, filePath));
            case ',':
                return new TokenComma(FileRange.single(pos, filePath));
            case ';':
                return new TokenSemicolon(FileRange.single(pos, filePath));
            case ':':
                return new TokenColon(FileRange.single(pos, filePath));
            case '(':
                return new TokenLeftParen(FileRange.single(pos, filePath));
            case ')':
                return new TokenRightParen(FileRange.single(pos, filePath));
            case '{':
                return new TokenLeftCurly(FileRange.single(pos, filePath));
            case '}':
                return new TokenRightCurly(FileRange.single(pos, filePath));
            case '[':
                return new TokenLeftSquare(FileRange.single(pos, filePath));
            case ']':
                return new TokenRightSquare(FileRange.single(pos, filePath));
        }
        System.Text.StringBuilder sb = new();
        //lex name
        if (TokenName.ValidFirstChar(reading))
        {
            sb.Append(reading);
            while (hasNextChar(reader) && TokenName.ValidBodyChar((char)reader.Peek()))
            {
                sb.Append(getChar(reader, ref pos));
            }
            //character+1 since the end is exclusive
            return new TokenName(new FileRange(new(startLine, startChar), new(pos.line, pos.character + 1), filePath), sb.ToString());
        }
        //this should be covered by the eof check, single char tokens, and whitespace check
        System.Diagnostics.Debug.Assert(hasNextChar(reader));
        System.Diagnostics.Debug.Assert(!TokenSymbol.IsSeparator(reading));
        //lex symbol
        //add first character of symbol
        sb.Append(reading);
        while (hasNextChar(reader) && !TokenSymbol.IsSeparator((char)reader.Peek()))
        {
            sb.Append(getChar(reader, ref pos));
        }
        if (sb.Length == 0)
        {
            //skip unneeded separator
            getChar(reader, ref pos);
            return LexNext(reader, ref pos, filePath);
        }
        //character+1 since the end is exclusive
        return new TokenSymbol(new FileRange(new(startLine, startChar), new(pos.line, pos.character + 1), filePath), sb.ToString());
    }
    //gets next char, incrementing line and character
    //make sure you aren't at end of stream before calling
    private char getChar(TextReader reader, ref FilePosition pos)
    {
        char reading = (char)reader.Read();
        pos.character++;
        //CR LF line endings
        if (reading == '\r' && (char)reader.Peek() == '\n')
        {
            reading = (char)reader.Read(); //eat \n
            //-1 so that the next read starts at 0
            pos.character = -1;
            pos.line++;
            return reading;
        }
        if (reading == '\r' || reading == '\n')
        {
            pos.line++;
            //-1 so that the next read starts at 0
            pos.character = -1;
        }
        return reading;
    }
    private bool hasNextChar(StreamReader reader)
    {
        return !reader.EndOfStream;
    }
}