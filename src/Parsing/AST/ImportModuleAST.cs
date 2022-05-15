namespace jwl.SyntaxTree;

public class ImportModuleAST : AST {
    public Name Name;

    public ImportModuleAST(Name name) {
        Name = name;
    }

    public override void Accept(Visitor v)
    {
        v.Visit(this);
    }
}