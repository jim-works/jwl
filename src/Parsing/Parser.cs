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

    private Name? parseName(TokenSequence input) {
        if (input.Peek() is TokenName) {
            List<string> names = new List<string>();
            while (input.Peek() is TokenName) {
                TokenName name = (TokenName)input.Next();
                names.Add(name.symbol);
                if (input.Peek() is TokenDot) {
                    //eat dot
                    input.Next();
                } else {
                    break;
                }
            }
            return new Name(names);
        }
        return null;
    }

    private DeclareModuleAST? parseDeclareModule(TokenSequence input) {
        if (input.Peek() is TokenName modKW && modKW.symbol == "module") {
            //we have module keyword
            input.Next(); //eat keyword
            if (parseName(input) is Name name) {
                DeclareModuleAST ast = new DeclareModuleAST(name);
                expectSemicolon(input);
                return ast;
            }
            //expected name
            syntaxError("expected module name", input.Peek());
            return null;
            
        }
        return null;
    }
    private List<ImportModuleAST> parseImportModules(TokenSequence input) {
        List<ImportModuleAST> imports = new List<ImportModuleAST>();
        while(input.Peek() is TokenName importKW && importKW.symbol == "import") {
            input.Next(); //eat kw
            if (parseName(input) is Name name) {
                ImportModuleAST ast = new ImportModuleAST(name);
                expectSemicolon(input);
                imports.Add(ast);
                continue;
            }
            //expected name
            syntaxError("expected module name", input.Peek());
            continue;
        }
        return imports;
    }

    private StatementAST? parseStatement(TokenSequence input) {
        Token next = input.Peek();
        if (next is TokenEOF) return null;
        if (next is TokenSemicolon) {
            input.Next(); //eat ;
            return parseStatement(input);
        }
        if (next is TokenName name) {
            if (name.symbol == "let") {
                DeclarationStatementAST? dec = parseDeclaration(input);
                return dec == null ? null : new StatementAST(dec);
            }
            
        }
        syntaxError("expected name", next);
        return null;
    }
    private ExpressionAST? parseExpression(TokenSequence input) {
        if (input.Peek() is TokenName name) {
            if (name.symbol == "fn") {
                FunctionDefintionAST? f =  parseFunctionDef(input);
                return f == null ? null : new ExpressionAST(f);
            }
        }
        return null;
    }
    private FunctionDefintionAST? parseFunctionDef(TokenSequence input) {
        if (input.Peek() is not TokenName name || name.symbol != "fn") {
            syntaxError("expected keyword 'fn'", input.Peek());
            return null;
        }
    }
    private DeclarationStatementAST? parseDeclaration(TokenSequence input) {
        if (input.Peek() is not TokenName kw || kw.symbol != "let") {
            syntaxError("expected keyword 'let'", input.Peek());
            return null;
        }
        input.Next(); //eat let
        if (parseName(input) is not Name name) {
            syntaxError("expected variable name", input.Peek());
            return null;
        }
        //expect =
        if (input.Peek() is not TokenSymbol symbol || symbol.symbol != "=")
        {
            syntaxError("expected '='", input.Peek());
            return null;
        }
        Token beforeExpr = input.Next(); //eat =, store token here for error message if there's a malformed rhs
        if (parseExpression(input) is not ExpressionAST rhs)
        {
            syntaxError("expected expression", beforeExpr);
            return null;
        }
        return new DeclarationStatementAST(name, rhs);
        
    }

}