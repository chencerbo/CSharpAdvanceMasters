using Contracts;
using System.ComponentModel;

namespace Pets
{
    [DisplayName("Bird")]
    public class Bird : Pet
    {
        public override string Name => "Tweety";

        public override int Age => 4;

        public override void Talk()
        {
            Console.WriteLine("Tweet...");
        }
        public void Fly()
        {
            Console.WriteLine("Flying...");
        }
    }
}
