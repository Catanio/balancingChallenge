using Microsoft.VisualStudio.TestTools.UnitTesting;
using RdChallenge;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BalancingTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void Scenario_1()
        {
            var css = SuccessManager.mapEntities(new[] { 60, 20, 95, 75 });
            var customers = Customer.mapEntities(new[] { 90, 20, 70, 40, 60, 10 });
            var csAway = new List<int> { 2, 4 };

            Assert.AreEqual(1, customerSuccessBalancing(css, customers, csAway));
        }

        [TestMethod]
        public void Scenario_2()
        {
            var css = SuccessManager.mapEntities(new[] { 11, 21, 31, 3, 4, 5 });
            var customers = Customer.mapEntities(new[] { 10, 10, 10, 20, 20, 30, 30, 30, 20, 60 });
            var csAway = new List<int> { };

            Assert.AreEqual(0, customerSuccessBalancing(css, customers, csAway));
        }

        [TestMethod, Timeout(100)]
        public void Scenario_3()
        {
            var css = SuccessManager.BuildSizeEntities(1000, 0);
            css[998].Score = 100;

            var customers = Customer.BuildSizeEntities(10000, 10);
            var csAway = new List<int> { 1000 };

            Assert.AreEqual(999, customerSuccessBalancing(css, customers, csAway));
        }

        [TestMethod]
        public void Scenario_4()
        {
            var css = SuccessManager.mapEntities(new[] { 1, 2, 3, 4, 5, 6 });
            var customers = Customer.mapEntities(new[] { 10, 10, 10, 20, 20, 30, 30, 30, 20, 60 });
            var csAway = new List<int> { };

            Assert.AreEqual(0, customerSuccessBalancing(css, customers, csAway));
        }


        [TestMethod]
        public void Scenario_5()
        {
            // should this throw an error? premisse says that all managers have different levels
            var css = SuccessManager.mapEntities(new[] { 100, 2, 3, 3, 4, 5 });
            var customers = Customer.mapEntities(new[] { 10, 10, 10, 20, 20, 30, 30, 30, 20, 60 });
            var csAway = new List<int> { };

            Assert.AreEqual(1, customerSuccessBalancing(css, customers, csAway));
        }

        [TestMethod]
        public void Scenario_6()
        {
            var css = SuccessManager.mapEntities(new[] { 100, 99, 88, 3, 4, 5 });
            var customers = Customer.mapEntities(new[] { 10, 10, 10, 20, 20, 30, 30, 30, 20, 60 });
            var csAway = new List<int> { 1, 3, 2 };

            Assert.AreEqual(0, customerSuccessBalancing(css, customers, csAway));
        }

        [TestMethod]
        public void Scenario_7()
        {
            var css = SuccessManager.mapEntities(new[] { 100, 99, 88, 3, 4, 5 });
            var customers = Customer.mapEntities(new[] { 10, 10, 10, 20, 20, 30, 30, 30, 20, 60 });
            var csAway = new List<int> { 4, 5, 6 };

            Assert.AreEqual(3, customerSuccessBalancing(css, customers, csAway));
        }

        int customerSuccessBalancing(List<SuccessManager> SuccessManagers, List<Customer> customers, List<int> csAway)
        {
            foreach (var csAwayId in csAway)
                SuccessManagers.Remove(SuccessManagers.Find(x => x.Id == csAwayId));

            SuccessManagers = SuccessManagers.OrderByDescending(manager => manager.Score).ToList();

            foreach (var customer in customers)
            {
                var chosenManager = SuccessManagers[0];

                // the highest successManager isn't enough
                if (customer.Score > chosenManager.Score)
                    continue;

                foreach (var manager in SuccessManagers)
                    if (customer.Score <= manager.Score)
                        chosenManager = manager;

                chosenManager.Customers.Add(customer);
            }

            SuccessManagers = SuccessManagers.OrderByDescending(manager => manager.Customers.Count).ToList();

            if (SuccessManagers[0].Customers.Count == SuccessManagers[1].Customers.Count)
                return 0;

            return SuccessManagers[0].Id;
        }
    }
}
