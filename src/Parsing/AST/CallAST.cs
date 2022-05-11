namespace jwl.SyntaxTree;

public class CallAST : AST {
    public string Name;
    public CallArgumentsAST? args;


    public CallAST(string name) {
        this.Name = name;
    }
    
    public override void Accept(Visitor v)
    {
        throw new System.NotImplementedException();
    }
}