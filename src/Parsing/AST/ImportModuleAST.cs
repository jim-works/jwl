namespace jwl.SyntaxTree;

public class ImportModuleAST : AST {
    public string Name;

    public ImportModuleAST(string name) {
        Name = name;
    }

    public override void Accept(Visitor v)
    {
        v.Visit(this);
    }
}