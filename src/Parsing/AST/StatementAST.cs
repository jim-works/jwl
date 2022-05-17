namespace jwl.SyntaxTree;

using System.Diagnostics;

public class StatementAST : AST {
    public enum StatementType
    {
        ASSIGNMENT,
        DECLARATION,
        CALL,
        BLOCK,
    }
    public StatementType Type {get; private set;}
    
    public AssignmentStatementAST? assignment;
    public DeclarationStatementAST? declaration;
    public CallAST? call;
    public BlockAST? block;

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
    public StatementAST(BlockAST ast) {
        Type = StatementType.BLOCK;
        block = ast;
    }
    public override void Accept(Visitor v)
    {
        switch (Type) {
            case StatementType.ASSIGNMENT:
                Debug.Assert(assignment != null);
                assignment.Accept(v);
                break;
            case StatementType.DECLARATION:
                Debug.Assert(declaration != null);
                declaration.Accept(v);
                break;
            case StatementType.CALL:
                Debug.Assert(call != null);
                call.Accept(v);
                break;
            case StatementType.BLOCK:
                Debug.Assert(block != null);
                block.Accept(v);
                break;
        }
        v.Visit(this);
    }
}