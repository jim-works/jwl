namespace jwl.SyntaxTree;

using System.Collections.Generic;

public class BlockAST : AST {
    public List<StatementAST> Statements = new List<StatementAST>();

    public override void Accept(Visitor v)
    {
        foreach (var stmt in Statements)
        {
            stmt.Accept(v);
        }
        v.Visit(this);
    }
}