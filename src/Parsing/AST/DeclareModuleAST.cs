namespace jwl.SyntaxTree;

public class DeclareModuleAST : AST {
    public string Name;

    public DeclareModuleAST(string name) {
        Name = name;
    }

    public override void Accept(Visitor v)
    {
        v.Visit(this);
    }
}