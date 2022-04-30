namespace jwl;

public interface IDisplay {
    void Print(string message);
    void Print(string message, string filepath, FilePosition toHighlight);
    void Print(string message, FileRange toHighlight);
}