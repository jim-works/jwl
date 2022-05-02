namespace jwl.SyntaxTree;

using System.Diagnostics;

public class StatementAST : AST {
    public enum StatementType
    {
        ASSIGNMENT,
        DECLARATION,
        CALL,
    }
    public StatementType Type {get; private set;}
    
    public AssignmentStatementAST? assignment;
    public DeclarationStatementAST? declaration;
    public CallAST? call;

    public StatementAST(AssignmentStatementAST ast) {
        Type = StatementType.ASSIGNMENT;
        assignment = ast;
    }
    public StatementAST(DeclarationStatementAST ast)
    {
        Type = StatementType.DECLARATION;
        declaration = ast;
    }
    public StatementAST(CallAST ast)
    {
        Type = StatementType.CALL;
        call = ast;
    }
    public override void Accept(Visitor v)
    {
        switch (Type) {
            case StatementType.ASSIGNMENT:
                Debug.Assert(assignment != null);
                v.Visit(assignment);
                break;
            case StatementType.DECLARATION:
                Debug.Assert(declaration != null);
                v.Visit(declaration);
                break;
            case StatementType.CALL:
                Debug.Assert(call != null);
                v.Visit(call);
                break;
        }
    }
}