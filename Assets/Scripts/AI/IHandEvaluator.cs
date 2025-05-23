
public interface IHandEvaluator
{

    public enum ValueComparitor
    {
        Equal,
        Greater,
        Less
    }
    bool Evaluate();
}