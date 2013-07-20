using System;
using System.Collections.Generic;
using System.Linq;

namespace tabrath.SimpleStorage.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            PersonTest();
            NumbersTest();

            Console.ReadLine();
        }

        // Test writing an array of numbers to disk and read back to prove that they match.
        private static void NumbersTest()
        {
            long[] numbers = new long[4096 * 1024];
            var random = new Random(Environment.TickCount);

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = random.Next();
            }

            numbers.Write("numbers.data", CompressionAlgorithm.Deflate);
            var reloaded = SimpleStorage.Read<long[]>("numbers.data", CompressionAlgorithm.Deflate);

            Console.WriteLine("Numbers = {0}", numbers.SequenceEqual(reloaded) ? "equal" : "not equal");
        }

        // Test writing an object to disk and read back, showing both original and reloaded.
        private static void PersonTest()
        {
            var dad = new Person { Name = "John Doe", Age = 52 };
            var mom = new Person { Name = "Jane Doe", Age = 48 };
            var me = new Person { Name = "Johnny Doe", Age = 23, Mother = mom, Father = dad };
            var sister = new Person { Name = "Jenny Doe", Age = 26, Mother = mom, Father = dad };
            var brother = new Person { Name = "Jerome Doe", Age = 19, Mother = mom, Father = dad };

            dad.Children.AddRange(me, sister, brother);
            mom.Children.AddRange(me, sister, brother);

            Console.Write("Original ");
            Dump(me);

            me.Write("people.data", CompressionAlgorithm.GZip);
            var reloaded = SimpleStorage.Read<Person>("people.data", CompressionAlgorithm.GZip);

            Console.Write("Reloaded ");
            Dump(reloaded);
        }

        // Dump person object to screen.
        static void Dump(Person person)
        {
            Console.WriteLine("Person\n\tName: {0}\n\tAge: {1}\n\tMother: {2}\n\tFather: {3}",
                person.Name, person.Age, (person.Mother != null) ? person.Mother.ToString() : "Unkown", (person.Father != null) ? person.Father.ToString() : "Unknown");
            
            Console.WriteLine("\tChildren:");
            
            foreach (var child in person.Children)
                Console.WriteLine("\t\t{0}", child);

            foreach (var sibling in person.Siblings)
                Console.WriteLine("\t\t{0}", sibling);
        }
    }

    // Extension class for List<T>
    public static class ListExtensions
    {
        /// <summary>
        /// Adds elements to the list.
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="list">this</param>
        /// <param name="elements">Elements to add.</param>
        public static void AddRange<T>(this List<T> list, params T[] elements)
        {
            list.AddRange(elements);
        }
    }
}
