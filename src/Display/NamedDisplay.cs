namespace jwl;

using System;

public class NamedDisplay : IDisplay {
    public string Name {get; init;}
    public NamedDisplay(string name) {
        this.Name = name;
    }
    public void Print(string message) {
        Console.WriteLine($"[{Name}] {message}");
    }
    public void Print(string message, string filepath, FilePosition highlight)
    {
        Print($"{message} at {filepath}({highlight.line},{highlight.character})");
    }
    public void Print(string message, FileRange highlight) {
        Print($"{message} at {highlight.file}({highlight.startInclusive.line},{highlight.startInclusive.character})");
    }
}