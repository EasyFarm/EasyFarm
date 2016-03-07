using System;
using System.Collections;
using CuttingEdge.Conditions;

namespace EasyFarm.Tests
{
    public static class ConditionExtensions
    {
        public static void IsAllOfType<TCollection>(this ConditionValidator<TCollection> validator, IEnumerable collection, Type type) where TCollection : IEnumerable
        {
            CustomAssertions.IsAllOfType(collection, type);
        }
    }
}