using System;
using System.Collections;
using CuttingEdge.Conditions;
using Xunit;

namespace EasyFarm.Tests
{
    public class CustomAssertions
    {
        public static void AssertTypesEqual(
            ICollection expectedTypes,
            ICollection actualInstances)
        {
            Condition.Ensures(expectedTypes)
                .IsNotNull()
                .HasLength(actualInstances.Count)
                .IsAllOfType(expectedTypes, typeof(Type));

            Condition.Ensures(actualInstances).IsNotNull();

            IEnumerator typesEnumerator = expectedTypes.GetEnumerator();
            IEnumerator instancesEnumerator = actualInstances.GetEnumerator();

            while (instancesEnumerator.MoveNext() && typesEnumerator.MoveNext())
            {
                Assert.IsType((Type)typesEnumerator.Current, instancesEnumerator.Current);
            }
        }

        public static void IsAllOfType<TCollection>(TCollection collection, Type type) where TCollection : IEnumerable
        {
            foreach (var value in collection)
            {
                if (!value.GetType().IsSubclassOf(type))
                {
                    Fail($"The Precondition 'All values should be of type {type}' failed. ");
                }
            }
        }

        public static void Fail(string message)
        {
            Assert.True(false, message);
        }

        public static void Fail(string message, params object[] args)
        {
            Assert.True(false, string.Format(message, args));
        }
    }
}