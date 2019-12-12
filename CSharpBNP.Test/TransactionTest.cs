using System.Collections.Generic;
using CSharpBNP.Business;
using CSharpBNP.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpBNP.Test
{
    [TestClass]
    public class TransactionTest
    {
        [TestMethod]
        public void TestCheckTransactionCount_Accepted()
        {
            var transaction = Ioc.Get<ITransaction>();
            StateEnum state = transaction.CheckTransactionCount(3,3);
            Assert.AreEqual(StateEnum.Accepted, state);

        }

        [TestMethod]
        public void TestCheckTransactionCount_Pending()
        {
            var transaction = Ioc.Get<ITransaction>();
            StateEnum state = transaction.CheckTransactionCount(2,1);
            Assert.AreEqual(StateEnum.Pending, state);
        }

        [TestMethod]
        public void TestCheckTransactionLimit_Accepted()
        {
            var transaction = Ioc.Get<ITransaction>();
            StateEnum state = transaction.CheckTransactionLimit(500,1000 );
            Assert.AreEqual(StateEnum.Accepted, state);
        }

        [TestMethod]
        public void TestCheckTransactionLimit_Rejected()
        {
            var transaction = Ioc.Get<ITransaction>();
            StateEnum state = transaction.CheckTransactionLimit(600,500);
            Assert.AreEqual(StateEnum.Rejected, state);
        }

    }
}
