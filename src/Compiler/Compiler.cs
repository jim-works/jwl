namespace jwl;

public class Compiler {
    public void Compile(string path, string dest) {
        Lexer lexer = new Lexer(new NamedDisplay("Lexer"));
        using (TokenSequence tokens = lexer.Process(path)) {
            foreach(Token tok in tokens) {
                System.Console.WriteLine($"{tok.GetType()}: {tok.ToString()}");
            }
        }
    }
}