namespace jwl.SyntaxTree;

using System.Collections.Generic;

public class FunctionDefintionAST : AST
{
    public Name? ReturnType;
    public List<VarDecAST> Args = new List<VarDecAST>();
    public StatementAST? Body;

    public override void Accept(Visitor v)
    {
        foreach (var arg in Args)
        {
            arg.Accept(v);
        }
        Body?.Accept(v);
        v.Visit(this);
    }
}