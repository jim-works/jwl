namespace jwl;

using jwl.SyntaxTree;

public class PrintVisitor : Visitor
{
    private int indention = 0;
    
    private void print(string message) {
        string prefix = "";
        for (int i = 0; i < indention; i++) prefix += "\t";
        System.Console.WriteLine($"{prefix}{message}");
    }
    
    //program structure
    public override void Visit(ProgramAST ast) {
        print("ProgramAST");
    }
    public override void Visit(FileAST ast) {
        print("FileAST");
    }
    //modules
    public override void Visit(DeclareModuleAST ast) {
        print($"ModDecAST {ast.Name}");
    }
    public override void Visit(ImportModuleAST ast) {
        print($"ImportModAST {ast.Name}");
    }
    //statements
    public override void Visit(StatementAST ast) {
        print($"StatementAST {ast.Type}");
    }
    public override void Visit(AssignmentStatementAST ast) {
        print($"Assignment");
    }
    public override void Visit(DeclarationStatementAST ast) {
        print($"Declaration {ast.variable}");
    }
    public override void Visit(CallAST ast) {
        print($"Call");
    }
    public override void Visit(BlockAST ast) {
        print($"Block");
    }
    //Expressions
    public override void Visit(ExpressionAST ast) {
        print("Expression");
    }
    public override void Visit(ArithmeticAST ast) {
        print("Arithmetic");
    }
    public override void Visit(FunctionDefintionAST ast) {
        print($"Function Definition {ast.ReturnType?.ToString() ?? "inferred return type"}");
    }
    public override void Visit(ClassDefinitionAST ast) {
        print("Class Definition");
    }
    public override void Visit(VarDecAST ast)
    {
        print($"Variable Dec {ast.VariableName} : {ast.TypeName?.ToString() ?? "inferred type"}");
    }
}