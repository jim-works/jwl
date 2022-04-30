namespace jwl;

using System.Collections.Generic;

public class Lexer : CompilerLayer<string, TokenSequence> {
    
    public Lexer(IDisplay display) : base(display) {}
    
    public override TokenSequence Process(string filepath)
    {
        return new TokenSequence(filepath, display);
    }
    public static bool IsSeparator(char c) => c switch {
        '(' or ')' or '{' or '}' or '[' or ']' or '"' or '\'' => true,
        _ => char.IsSeparator(c) || char.IsWhiteSpace(c),
    };
}