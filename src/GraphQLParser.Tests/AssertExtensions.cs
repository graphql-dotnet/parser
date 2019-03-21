namespace GraphQLParser.Tests
{
    using KellermanSoftware.CompareNetObjects;
    using System;

    public static class AssertExtensions
    {
        public static void DeepEqual<T>(T obj1, T obj2, params Type[] typesToIgnore)
        {
            var compareLogic = new CompareLogic();

            if (typesToIgnore != null)
            {
                foreach (var type in typesToIgnore)
                {
                    compareLogic.Config.TypesToIgnore.Add(type);
                }
            }

            var result = compareLogic.Compare(obj1, obj2);

            if (!result.AreEqual)
                throw new Exception(
                    "Objects are not the same: " +
                    Environment.NewLine + Environment.NewLine +
                    result.DifferencesString);
        }
    }
}
