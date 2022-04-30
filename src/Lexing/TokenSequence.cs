namespace jwl;

using System.Collections.Generic;
using System.IO;
using System.Collections;

public class TokenSequence : System.IDisposable, IEnumerable<Token>
{
    //token that will be returned when Peek() is called
    private Lexer lexer;
    private Token nextToken;
    private StreamReader reader;
    private IDisplay display;
    private string filePath;
    private FilePosition pos;

    public TokenSequence(Lexer lexer, string filePath, IDisplay display)
    {
        this.lexer = lexer;
        reader = new(filePath);
        this.filePath = filePath;
        this.display = display;
        //character is -1 so that the next read starts at 0
        pos = new(0,-1);
        //make sure Peek() is ready
        nextToken = lexer.LexNext(reader, ref pos, filePath);
    }
    //returns an IEnumerator of all tokens in the file. Works lazily. Returns an EOF token as the last member of the iterator
    public IEnumerator<Token> GetEnumerator()
    {
        while (HasNext())
        {
            yield return Next();
        }
        yield return Next();
        yield break;
    }
    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator(); 
    }
    public bool HasNext() {
        return nextToken is not TokenEOF;
    }
    public void Dispose()
    {
        reader.Dispose();
    }
    //returns the next token and advances
    public Token Next() {
        Token tmp = nextToken;
        nextToken = lexer.LexNext(reader, ref pos, filePath);
        return tmp;
    }
    //returns the next token without advancing
    public Token Peek() {
        return nextToken;
    }
    
}