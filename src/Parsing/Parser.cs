namespace jwl;

using jwl.SyntaxTree;
using System.Collections.Generic;

public class Parser : CompilerLayer<TokenSequence, AST> {
    public ProgramAST Program;
    public Parser(IDisplay display) : base(display) {
        Program = new ProgramAST();
    }
    public override AST Process(TokenSequence input)
    {
        FileAST file = new FileAST(Program);
        if (Program.Files == null) {
            Program.Files = new List<FileAST>();
        }
        Program.Files.Add(file);
        file.ModDeclaration = parseDeclareModule(input);
        file.Imports = parseImportModules(input);
        StatementAST? statement = parseStatement(input);
        while (statement != null) {
            file.Statements.Add(statement);
            statement = parseStatement(input);
        }
        return Program;
    }

    private void syntaxError(string message, Token on) {
        display.Print($"Syntax error: {message}", on.range);
    }

    private void expectSemicolon(TokenSequence input) {
        Token next = input.Next();
        if (next is not TokenSemicolon) {
            syntaxError("expected ;", next);
        }
    }

    private DeclareModuleAST? parseDeclareModule(TokenSequence input) {
        if (input.Peek() is TokenName modKW && modKW.symbol == "module") {
            //we have module keyword
            input.Next(); //eat keyword
            Token next = input.Next();
            if (next is TokenName name) {
                DeclareModuleAST ast = new DeclareModuleAST(name.symbol);
                expectSemicolon(input);
                return ast;
            }
            //expected name
            syntaxError("expected name", next);
            return null;
            
        }
        return null;
    }
    private List<ImportModuleAST> parseImportModules(TokenSequence input) {
        List<ImportModuleAST> imports = new List<ImportModuleAST>();
        while(input.Peek() is TokenName importKW && importKW.symbol == "import") {
            input.Next(); //eat kw
            Token next = input.Next();
            if (next is TokenName name) {
                ImportModuleAST ast = new ImportModuleAST(name.symbol);
                expectSemicolon(input);
                imports.Add(ast);
                continue;
            }
            //expected name
            syntaxError("expected name", next);
            continue;
        }
        return imports;
    }

    private StatementAST? parseStatement(TokenSequence input) {
        return null;
    }
}