namespace jwl;

public record struct FileRange(FilePosition startInclusive, FilePosition endExclusive, string file) {
    //creates single character
    public static FileRange single(FilePosition start, string filepath) {
        return new(start, new(start.line, start.character), filepath);
    }
}