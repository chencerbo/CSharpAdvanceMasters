using Contracts;
using System.ComponentModel;

namespace Pets
{
    [DisplayName("Fish")]
    public class Fish : Pet
    {
        public override string Name => "Goldy";

        public override int Age => 10;

        public override void Talk()
        {
            Console.WriteLine("Glub...");
        }
        public void Swim()
        {
            Console.WriteLine("Swimming...");
        }
    }
}
