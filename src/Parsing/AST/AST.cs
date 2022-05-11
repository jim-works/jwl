namespace jwl.SyntaxTree;

using System.Collections.Generic;

//uses visitor pattern
public abstract class AST {
    public AST? Parent;

    public abstract void Accept(Visitor v);
}