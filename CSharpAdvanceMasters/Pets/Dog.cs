using Contracts;
using System.ComponentModel;

namespace Pets
{
    [DisplayName("Dog")]
    public class Dog : Pet
    {
        public override string Name => "Doggo";

        public override int Age => 2;

        public override void Talk()
        {
            Console.WriteLine("Woof...");
        }
        public void Roll()
        {
            Console.WriteLine("Rolling...");
        }
    }
}