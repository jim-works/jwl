namespace jwl.SyntaxTree;

public class DeclarationStatementAST : AST {
    public Name variable;
    public ExpressionAST rhs;

    public DeclarationStatementAST(Name lhs, ExpressionAST rhs) {
        this.variable = lhs;
        this.rhs = rhs;
    }
    public override void Accept(Visitor v)
    {
        rhs.Accept(v);
        v.Visit(this);
    }
}