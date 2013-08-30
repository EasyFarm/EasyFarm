using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TreeSharp
{
    internal class TreeExample
    {
        private static Composite ExampleTree()
        {
            // This allows for quick prototyping of behaviors without needing to override the composite types
            // to implement simple logic. More advanced logic may require overrides to handle them.
            return new PrioritySelector(
                new Decorator(ret => 15 > 20,
                    new Action(action => Console.WriteLine("15 > 20"))),
                new Decorator(ret => 21 > 20,
                    new Action(action => Console.WriteLine("21 > 20")))
                );
        }


        private static void WalkTree(Composite root, object rootContext = null)
        {
            // This function is meant as an EXAMPLE ONLY!
            // It is not the best way to handle walking the tree, but will suffice for simple usages.
            
            // Start must always be called first. Typically the root behavior will have a null context. Change if needed.
            root.Start(rootContext);

            // Run the tree forever...
            while (true)
            {
                try
                {
                    root.Tick(null);
                }
                catch (ThreadAbortException)
                {
                    // Assuming we wanted this, make the tree stop walking, regardless of where it is.
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    // Handle this exception, either stop the tree, or just eat it and continue.
                }


                // If we're still running the current composite, then we don't need to reset, we want the tree to pick up
                // where it left off last tick.
                if (root.LastStatus != RunStatus.Running)
                {
                    // Reset the tree, and begin the execution all over...
                    root.Stop(rootContext);
                    root.Start(rootContext);
                }

                // Change/remove, do whatever you want. This is an example remember!
                Thread.Sleep(10);
            }

            root.Stop(rootContext);
        }
    }
}
