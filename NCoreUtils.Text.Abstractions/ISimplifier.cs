namespace NCoreUtils.Text
{
    public interface ISimplifier
    {
        char Delimiter { get; }
        string Simplify(string source);
    }
}