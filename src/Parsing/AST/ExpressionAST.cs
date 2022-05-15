namespace jwl.SyntaxTree;

using System.Diagnostics;

public class ExpressionAST : AST
{
    public enum StatementType
    {
        CALL,
        ARITHMETIC,
        CLASS_DEF,
        FUNCTION_DEF
    }
    public StatementType Type { get; private set; }

    public CallAST? call;
    public ArithmeticAST? arithmetic;
    public ClassDefinitionAST? classDef;
    public FunctionDefintionAST? functionDef;

    public ExpressionAST(CallAST ast)
    {
        Type = StatementType.CALL;
        call = ast;
    }
    public ExpressionAST(ArithmeticAST ast)
    {
        Type = StatementType.ARITHMETIC;
        arithmetic = ast;
    }
    public ExpressionAST(ClassDefinitionAST ast)
    {
        Type = StatementType.CLASS_DEF;
        classDef = ast;
    }
    
    public ExpressionAST(FunctionDefintionAST ast)
    {
        Type = StatementType.FUNCTION_DEF;
        functionDef = ast;
    }
    public override void Accept(Visitor v)
    {
        switch (Type)
        {
            case StatementType.ARITHMETIC:
                Debug.Assert(arithmetic != null);
                arithmetic.Accept(v);
                break;
            case StatementType.CALL:
                Debug.Assert(call != null);
                call.Accept(v);
                break;
            case StatementType.CLASS_DEF:
                Debug.Assert(classDef != null);
                classDef.Accept(v);
                break;
            case StatementType.FUNCTION_DEF:
                Debug.Assert(functionDef != null);
                functionDef.Accept(v);
                break;
        }
        v.Visit(this);
    }
}