using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GraphQLParser
{
    public sealed class ConcurrentDictionaryCache : ILexemeCache
    {
        private readonly ConcurrentDictionary<int, object> _cache = new ConcurrentDictionary<int, object>();
        private readonly ConcurrentDictionary<int, string> _intCache = new ConcurrentDictionary<int, string>();
        private readonly object _listLock = new object();

        public void Clear()
        {
            _cache.Clear();
            _intCache.Clear();
        }

        public bool AllowIntCache { get; set; }

        public string GetName(string source, int start, int end)
        {
            if (start == end)
                return string.Empty;

            var hash = StringHelper.GetHashCode(source, start, end);

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
                if (StringHelper.Equals(str, source, start, end))
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
                lock (_listLock)
                {
                    // comparison by value among all elements of the list
                    foreach (var element in list)
                        if (StringHelper.Equals(element, source, start, end))
                            return element; // cache hit!

                    // an even rarer cache miss - repeated hash collision
                    var result = source.Substring(start, end - start);
                    list.Add(result);
                    return result;
                }
            }
            else
                throw new NotSupportedException();
        }

        public string GetInt(string source, int start, int end)
        {
            if (!AllowIntCache || end - start > 9)
                return source.Substring(start, end - start);

            var hash = StringHelper.ParseInt(source, start, end);

            if (!_intCache.TryGetValue(hash, out var value))
            {
                // copy into locals to suppress too early closure allocation of Func<int, string>
                var localSource = source;
                var localStart = start;
                var localEnd = end;
                value = _intCache.GetOrAdd(hash, _ => localSource.Substring(localStart, localEnd - localStart));
            }

            return value;
        }
    }
}
