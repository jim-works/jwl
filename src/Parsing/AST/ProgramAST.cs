namespace jwl.SyntaxTree;

using System.Collections.Generic;

public class ProgramAST : AST {
    public List<FileAST> Files = new List<FileAST>();

    public override void Accept(Visitor v)
    {
        foreach (FileAST f in Files) {
            f.Accept(v);
        }
        v.Visit(this);
    }
}