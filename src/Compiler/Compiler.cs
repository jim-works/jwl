namespace jwl;

public class Compiler {
    public void Compile(string path, string dest) {
        Lexer lexer = new Lexer(new NamedDisplay("Lexer"));
        TokenSequence tokens = lexer.Process(path);
        foreach(Token tok in tokens.Sequence()) {
            System.Console.WriteLine($"{tok.GetType()}: {tok.ToString()}");
        }
    }
}