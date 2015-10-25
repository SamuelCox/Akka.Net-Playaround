using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;
using Akka.Actor;

namespace akkatest
{
    class Program
    {
        static void Main(string[] args)
        {
            var system = ActorSystem.Create("MySystem");

            var greeter = system.ActorOf<ConsumerActor>("Consumer");
            greeter.Tell(new Work((x) => x*x));
            greeter.Tell(new ExitMessage("System Shutdown."));
            Console.ReadLine();
        }
    }

    public class Work
    {
        public Work(Func<int,int> lambda)
        {
            work = lambda;
        }
        public Func<int, int> work { get; private set; }
    }

    public class ExitMessage
    {
        public ExitMessage(string exitReason)
        {
            ExitReason = exitReason;
        }
        public string ExitReason {get; set;}
    }

    public class ConsumerActor : ReceiveActor
    {
        public ConsumerActor()
        {
            Receive<Work>(work =>
                Console.WriteLine("Result of calling function with x = 5, {0}", work.work(5)));
            Receive<ExitMessage>(exit =>
                Console.WriteLine("Consumer was sent an exit command with argument of: {0}", exit.ExitReason));
        }
    }
}
