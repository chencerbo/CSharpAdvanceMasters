using Contracts;
using System.ComponentModel;

namespace Pets
{
    [DisplayName("Cat")]
    public class Cat : Pet
    {
        public override string Name => "Kitty";

        public override int Age => 3;

        public override void Talk()
        {
            Console.WriteLine("Meow...");
        }

        public void Climb()
        {
            Console.WriteLine("Climbing...");
        }
    }
}
