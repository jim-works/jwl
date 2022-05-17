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
        display.Print($"Syntax error: {message}\n\t{on.ToString()}", on.range);
    }

    private void expectSemicolon(TokenSequence input) {
        Token next = input.Next();
        if (next is not TokenSemicolon) {
            syntaxError("expected ;", next);
        }
    }
    private void consume<T>(TokenSequence input, string errorMessage) where T : Token {
        if (input.Peek() is not T) {
            syntaxError(errorMessage, input.Peek());
            return;
        }
        input.Next(); //eat symbol
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
        //todo: change to not require keyword fn for function (increase token lookahead)
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
        input.Next(); //eat fn
        consume<TokenLeftParen>(input, "expected (");
        if (input.Peek() is TokenComma) {
            syntaxError("unexpected ,", input.Peek()); 
            input.Next();
            return null;
        }
        FunctionDefintionAST fn = new FunctionDefintionAST();
        //parse arguments
        do {
            if (input.Peek() is TokenComma) {
                input.Next(); //eat ,
            }
            if (parseFunctionArg(input) is not VarDecAST arg) {
                syntaxError("expected function argument", input.Peek());
                return null;
            }
            fn.Args.Add(arg);
        } while (input.Peek() is TokenComma);
        consume<TokenRightParen>(input, "expected )");
        if (input.Peek() is TokenSymbol explicitArrow && explicitArrow.symbol == "->") {
            //type annotation
            input.Next();
            if (parseName(input) is Name typeName) {
                fn.ReturnType = typeName;
            } else {
                syntaxError("expected return type name", input.Peek());
                return null;
            }
        } else if (input.Peek() is not TokenSymbol inferArrow || inferArrow.symbol != "=>") {
            syntaxError("expected =>", input.Peek());
            return null;
        } else {
            input.Next(); //eat =>
        }
        return fn;
    }
    private VarDecAST? parseFunctionArg(TokenSequence input) {
        if (parseName(input) is not Name name) {
            syntaxError("expected name", input.Peek());
            return null;
        }
        if (input.Peek() is TokenColon) {
            input.Next(); //eat :
            //type
            if (parseName(input) is not Name typeName) {
                syntaxError("expected type name", input.Peek());
                return null;
            }
            return new VarDecAST(name, typeName);
        }
        return new VarDecAST(name);
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