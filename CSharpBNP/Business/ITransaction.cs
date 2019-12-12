using CSharpBNP.Model;

namespace CSharpBNP.Business
{
    public interface ITransaction
    {
        StateEnum CheckTransactionCount(int max, int count);

        StateEnum CheckTransactionLimit(int sum, int limit);
    }
}
