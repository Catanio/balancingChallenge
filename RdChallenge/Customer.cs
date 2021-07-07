using System.Collections.Generic;

namespace RdChallenge
{
    public class Customer
    {
        public int Id { get; set; }
        public int Score { get; set; }

        public static List<Customer> BuildSizeEntities(int size, int score)
        {
            var entities = new List<Customer>();

            for (int index = 0; index < size; index++)
                entities.Add(new Customer { Id = index + 1, Score = score });

            return entities;
        }

        public static List<Customer> mapEntities(int[] scores)
        {
            var entities = new List<Customer>();

            for (var i = 0; i < scores.Length; i++)
                entities.Add(new Customer { Id = i + 1, Score = scores[i] });

            return entities;
        }
    }
}
