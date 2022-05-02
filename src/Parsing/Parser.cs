namespace jwl;
using jwl.SyntaxTree;

public class Parser : CompilerLayer<TokenSequence, AST> {
    
    public Parser(IDisplay display) : base(display) {}
    public override AST Process(TokenSequence input)
    {
        throw new System.NotImplementedException();
    }
}