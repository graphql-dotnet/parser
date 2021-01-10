using System;

namespace GraphQLParser
{
    internal static class StringHelper
    {
        /// <summary>
        /// Determines the equivalence of the string <paramref name="str"/> to a substring from <paramref name="source"/>
        /// defined by the <paramref name="start"/> and <paramref name="end"/> index.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static bool Equals(string str, ReadOnlySpan<char> source, int start, int end)
        {
            if (str.Length != end - start)
                return false;

            for (int i = 0; i < str.Length; ++i)
                if (str[i] != source[start + i])
                    return false;

            return true;
        }

        /// <summary>
        /// Determines the hash code of the substring from the <paramref name="source"/> specified by
        /// the <paramref name="start"/> and <paramref name="end"/> index.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int GetHashCode(ReadOnlySpan<char> source, int start, int end)
        {
            if (start > source.Length || end > source.Length || start < 0 || end < 0)
                throw new IndexOutOfRangeException();

            int num1 = 5381;
            int num2 = num1;
            int num3;
            end -= 1;

            for (int i = start; i <= end; i += 2)
            {
                num3 = source[i];
                num1 = (num1 << 5) + num1 ^ num3;
                if (i == end)
                    break;
                int num4 = source[i + 1];
                if (num4 != 0)
                    num2 = (num2 << 5) + num2 ^ num4;
                else
                    break;
            }

            return num1 + num2 * 1566083941;
        }

        /// <summary>
        /// Gets the integer value of the substring from the <paramref name="source"/> specified
        /// by the <paramref name="start"/> and <paramref name="end"/> index.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int ParseInt(ReadOnlySpan<char> source, int start, int end)
        {
            if (end - start > 9)
                throw new NotSupportedException();

            int current = 0;

            if (source[start] == '-')
            {
                ++start;
                for (int i = end - 1, power = 1; i >= start; --i, power *= 10)
                {
                    current += (source[i] - '0') * power;
                }
                return -current;
            }
            else
            {
                for (int i = end - 1, power = 1; i >= start; --i, power *= 10)
                {
                    current += (source[i] - '0') * power;
                }
                return current;
            }
        }
    }
}
