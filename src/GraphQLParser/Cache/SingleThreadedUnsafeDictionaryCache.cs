using System;
using System.Collections.Generic;

namespace GraphQLParser
{
    public sealed class SingleThreadedUnsafeDictionaryCache : ILexemeCache
    {
        private readonly Dictionary<int, object> _cache = new Dictionary<int, object>();

        public void Clear() => _cache.Clear();

        public string GetInt(string source, int start, int end)
        {
            return source.Substring(start, end - start);
        }

        public string GetName(string source, int start, int end)
        {
            if (start == end)
                return string.Empty;

            var hash = StringHelper.GetHashCodeUnsafe(source, start, end);

            if (!_cache.TryGetValue(hash, out var value))
            {
                // absolutely new string
                string result = source.Substring(start, end - start);
                _cache[hash] = result;
                return result;
            }
            else if (value is string str)
            {
                // the string is already in cache, we need to compare
                if (StringHelper.EqualsUnsafe(str, source, start, end))
                {
                    return str; // cache hit!
                }
                else
                {
                    // cache miss - hashes are the same but the actual values of the strings are different so need to allocate list with both strings
                    var result = source.Substring(start, end - start);
                    var list = new List<string> { str, result };
                    _cache[hash] = list;
                    return result;
                }
            }
            else if (value is List<string> list)
            {
                // comparison by value among all elements of the list
                foreach (var element in list)
                    if (StringHelper.EqualsUnsafe(element, source, start, end))
                        return element; // cache hit!

                // an even rarer cache miss - repeated hash collision
                var result = source.Substring(start, end - start);
                list.Add(result);
                return result;
            }
            else
                throw new NotSupportedException();
        }
    }
}
