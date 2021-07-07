using System.Collections.Generic;

namespace RdChallenge
{
    public class SuccessManager
    {
        public int Id { get; set; }
        public int Score { get; set; }

        public List<Customer> Customers = new List<Customer>();

        public static List<SuccessManager> BuildSizeEntities(int size, int score)
        {
            var entities = new List<SuccessManager>();

            for (int index = 0; index < size; index++)
                entities.Add(new SuccessManager { Id = index + 1, Score = score });

            return entities;
        }

        public static List<SuccessManager> mapEntities(int[] scores)
        {
            var entities = new List<SuccessManager>();

            for (var i = 0; i < scores.Length; i++)
                entities.Add(new SuccessManager { Id = i + 1, Score = scores[i] });

            return entities;
        }
    }   
}
