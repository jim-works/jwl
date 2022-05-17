namespace jwl.SyntaxTree;

public class VarDecAST : AST {
    public Name VariableName;
    public Name? TypeName;
    public VarDecAST(Name varName) {
        this.VariableName = varName;
    }
    public VarDecAST(Name varName, Name typeName) {
        this.VariableName = varName;
        this.TypeName = typeName;
    }

    public override void Accept(Visitor v)
    {
        v.Visit(this);
    }
}