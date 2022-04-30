namespace jwl;

public enum TokenType {
    SYMBOL,
    QUOTE,
    APROSTROPHE,
    DOT,
    COMMA,
    SEMICOLON,
    LEFT_PAREN,
    RIGHT_PAREN,
    LEFT_CURLY,
    RIGHT_CURLY,
    LEFT_SQUARE,
    RIGHT_SQUARE,
}
public abstract record Token(FileRange range);
public record TokenEOF(FileRange range) : Token(range);
public record TokenSymbol(FileRange range, string symbol) : Token(range) {
    public static bool IsSeparator(char c) => c switch
    {
        '(' or ')' or '{' or '}' or '[' or ']' => true,
        _ => char.IsSeparator(c) || char.IsWhiteSpace(c)
    };
}
public record TokenName(FileRange range, string symbol) : TokenSymbol(range, symbol) {
    public static bool ValidFirstChar(char c) => (int) c switch {
        >= (int)'a' and <= (int)'z' => true,
        >= (int)'A' and <= (int)'Z' => true,
        '_' => true,
        _ => false
    };
    public static bool ValidBodyChar(char c) => (int) c switch {
        >= (int)'0' and <= (int)'9' => true,
        _ => ValidFirstChar(c),
    };
}
public record TokenQuote(FileRange range) : Token(range);
public record TokenApostrophe(FileRange range) : Token(range);
public record TokenDot(FileRange range) : Token(range);
public record TokenComma(FileRange range) : Token(range);
public record TokenSemicolon(FileRange range) : Token(range);
public record TokenColon(FileRange range) : Token(range);
public record TokenLeftParen(FileRange range) : Token(range);
public record TokenRightParen(FileRange range) : Token(range);
public record TokenLeftCurly(FileRange range) : Token(range);
public record TokenRightCurly(FileRange range) : Token(range);
public record TokenLeftSquare(FileRange range) : Token(range);
public record TokenRightSquare(FileRange range) : Token(range);
