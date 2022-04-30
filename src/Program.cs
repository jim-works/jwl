using System;

namespace jwl;

class Program
{
    static void Main(string[] args)
    {
        Compiler compiler = new Compiler();
        compiler.Compile("test.jwl", "out");
    }
}
