namespace jwl;

public abstract class CompilerLayer<Input, Output> {
    protected IDisplay display;
    public CompilerLayer(IDisplay display) {
        this.display = display;
    }
    public abstract Output Process(Input input);

}