namespace jwl.SyntaxTree;

public abstract class Visitor {
    //public abstract void Visit(AST t);
    //program structure
    public abstract void Visit(ProgramAST ast);
    public abstract void Visit(FileAST ast);
    //modules
    public abstract void Visit(DeclareModuleAST ast);
    public abstract void Visit(ImportModuleAST ast);
    //statements
    public abstract void Visit(StatementAST ast);
    public abstract void Visit(AssignmentStatementAST ast);
    public abstract void Visit(DeclarationStatementAST ast);
    public abstract void Visit(CallAST ast);
}