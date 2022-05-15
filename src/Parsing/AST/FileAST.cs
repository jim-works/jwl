namespace jwl.SyntaxTree;

using System.Collections.Generic;

public class FileAST : AST {
    
    public DeclareModuleAST? ModDeclaration;
    public List<ImportModuleAST>? Imports;
    public List<StatementAST> Statements = new List<StatementAST>();
    
    public FileAST(ProgramAST parent) {
        Parent = parent;
    }

    public override void Accept(Visitor v)
    {
        if (ModDeclaration != null) {
            ModDeclaration.Accept(v);
        }
        if (Imports != null) {
            foreach (var item in Imports)
            {
                item.Accept(v);
            }
        }
        foreach (var item in Statements) {
            item.Accept(v);
        }
        v.Visit(this);
    }

}