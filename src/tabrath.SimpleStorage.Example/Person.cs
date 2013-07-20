using System;
using System.Collections.Generic;

namespace tabrath.SimpleStorage.Example
{
    [Serializable]
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Person> Children { get; set; }
        public Person Mother { get; set; }
        public Person Father { get; set; }

        public IEnumerable<Person> Siblings
        {
            get
            {
                var siblings = new List<Person>();

                foreach (var sibling in Mother.Children)
                    if (!siblings.Contains(sibling))
                        siblings.Add(sibling);

                foreach (var sibling in Father.Children)
                    if (!siblings.Contains(sibling))
                        siblings.Add(sibling);

                return siblings;
            }
        }

        public Person()
        {
            Children = new List<Person>();
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Age);
        }
    }
}
