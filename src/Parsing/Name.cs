namespace jwl;

using System.Collections.Generic;

public class Name {
    //jwl.test.name -> [jwl, test, name]
    public List<string> Names;
    public Name(List<string> names) { 
        this.Names = names;
    }
    public override string ToString()
    {
        string res = "";
        for (int i = 0; i < Names.Count; i++) {
            res += Names[i];
            if (i != Names.Count - 1) {
                res += ".";
            }
        }
        return res;
    }
}