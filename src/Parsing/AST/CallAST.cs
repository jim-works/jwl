namespace jwl.SyntaxTree;

public class CallAST : AST {
    public Name Name;
    public CallArgumentsAST? args;


    public CallAST(Name name) {
        this.Name = name;
    }
    
    public override void Accept(Visitor v)
    {
        throw new System.NotImplementedException();
    }
}