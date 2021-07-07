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
            // ERROR: should this throw an error? premisse says that all managers have different levels
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

        int customerSuccessBalancing(List<SuccessManager> successManagers, List<Customer> customers, List<int> csAway)
        {
            /*
             * Premissas que vão contra os testes unitários
             */
            // Teste 3

            // Teste 5
            //throw new Exception("Todos os CSs têm níveis diferentes");
            
            
            if (successManagers.Count > 1000)
                throw new Exception("O número de CSs deve estar entre 0 e 1.000");

            if (customers.Count > 1000000) 
                throw new Exception("O número de Clientes deve estar entre 0 e 1.000.000");

            if (successManagers.Max(manager => manager.Id) > 1000 || successManagers.Exists(manager => manager.Id < 0))
                throw new Exception("os Ids dos CSs devem estar entre 0 e 1.000");

            if (customers.Max(Customer => Customer.Id) > 1000000 || customers.Exists(customer => customer.Id < 0))
                throw new Exception("Os Ids do cliente devem estar entre 0 e 1.000.000");

            if (successManagers.Max(manager => manager.Score) > 10000 || successManagers.Exists(manager => manager.Score < 0))
                throw new Exception("O nível do cs deve estar entre 0 e 10.000");

            if (customers.Max(Customer => Customer.Score) > 100000 || customers.Exists(customer => customer.Score < 0))
                throw new Exception("O tamanho do cliente deve estar entre 0 e 100.000");
            
            //if()

            // TODO: (improvement) make an attribute "away" or copy list so the original one is unaffected
            foreach (var csAwayId in csAway)
                successManagers.Remove(successManagers.Find(x => x.Id == csAwayId));

            successManagers = successManagers.OrderBy(manager => manager.Score).ToList();
            customers = customers.OrderBy(manager => manager.Score).ToList();

            Stack<SuccessManager> managerStack = new Stack<SuccessManager>(successManagers);
            Stack<Customer> customerStack = new Stack<Customer>(customers);

            SuccessManager lastValidManager = null;
            while (customerStack.Count > 0)
            {
                var customer = customerStack.Pop();

                // TODO: Improve logic readability
                // Trivial case / border case
                if (managerStack.Count > 0 && managerStack.Peek().Score < customer.Score)
                    if (lastValidManager is null || (lastValidManager != null && lastValidManager.Score < customer.Score))
                        break;

                while(managerStack.Count > 0 && managerStack.Peek().Score >= customer.Score)
                    lastValidManager = managerStack.Pop();

                if (lastValidManager.Score >= customer.Score)
                    lastValidManager.Customers.Add(customer);

            }

            successManagers = successManagers.OrderByDescending(manager => manager.Customers.Count).ToList();

            var unoccupiedManagers = 0;
            foreach(var manager in successManagers)
                if (manager.Customers.Count == 0)
                    unoccupiedManagers++;

            if (unoccupiedManagers == (successManagers.Count / 2))
                throw new Exception("Valor máximo de t = n / 2 arredondado para baixo");

            if (successManagers[0].Customers.Count == successManagers[1].Customers.Count)
                return 0;

            return successManagers[0].Id;
        }
    }
}
