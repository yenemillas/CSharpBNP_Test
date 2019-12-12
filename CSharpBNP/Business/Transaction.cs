using CSharpBNP.Model;

namespace CSharpBNP.Business
{
    /// <summary>
    /// Transaction
    /// J'ai décidé de traiter le controle de la limite et du nombre de transaction dans deux fonctions disctints,
    /// car pour la réalisation de ce test en WPF on ne prend pas en compte des appels serveur
    /// </summary>
    public class Transaction : ITransaction
    {
        /// <summary>
        /// CheckTransactionCount
        /// Test supplémentaires à réaliser :
        ///  - Le cas où le nombre de transaction est supérieur a la limite 
        ///  - Le cas où le nombre de transaction égal zéro
        ///  J'ai décidé arbitrairement de retourner StateEnum.None pour le test
        /// </summary>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public StateEnum CheckTransactionCount(int max, int count)
        {
            if (count == max)
            {
                return StateEnum.Accepted;
            }
            else if (count <= max)
            {
                return StateEnum.Pending;
            }
            else
            {
                return StateEnum.None;
            }
        }

        /// <summary>
        /// CheckTransactionLimit
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public StateEnum CheckTransactionLimit(int sum, int limit)
        {
            if (sum < limit)
            {
                return StateEnum.Accepted;
            }
            else
            {
               return StateEnum.Rejected;
            }
        }
    }
}
