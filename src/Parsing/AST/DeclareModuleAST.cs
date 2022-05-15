namespace jwl.SyntaxTree;

public class DeclareModuleAST : AST {
    public Name Name;

    public DeclareModuleAST(Name name) {
        Name = name;
    }

    public override void Accept(Visitor v)
    {
        v.Visit(this);
    }
}