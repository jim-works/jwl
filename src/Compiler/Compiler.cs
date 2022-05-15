namespace jwl;

public class Compiler {
    public void Compile(string path, string dest) {
        Lexer lexer = new Lexer(new NamedDisplay("Lexer"));
        using (TokenSequence tokens = lexer.Process(path)) {
            foreach(Token tok in tokens) {
                System.Console.WriteLine($"{tok.GetType()}: {tok.ToString()}");
            }
        }
        using (TokenSequence tokens = lexer.Process(path)) {
            Parser parser = new Parser(new NamedDisplay("Parser"));
            parser.Process(tokens).Accept(new PrintVisitor());
        }
    }
}