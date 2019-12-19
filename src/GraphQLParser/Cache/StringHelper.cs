using System;

namespace GraphQLParser
{
    internal static class StringHelper
    {
        public static bool Equals(string str, string source, int start, int end)
        {
            if (str.Length != end - start)
                return false;

            for (int i = 0; i < str.Length; ++i)
                if (str[i] != source[start + i])
                    return false;

            return true;
        }

        public static unsafe bool EqualsUnsafe(string str, string source, int start, int end)
        {
            if (str.Length != end - start)
                return false;

            fixed (char* fix1 = str)
            {
                fixed (char* fix2 = source)
                {
                    char* ptr1 = fix1;
                    char* ptr2 = fix2 + start;
                    while (*ptr1 == *ptr2)
                    {
                        ++ptr1;
                        if (*ptr1 == '\0') return true;
                        ++ptr2;
                    }
                    return false;
                }
            }
        }

        public static int GetHashCode(string source, int start, int end)
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

        public static unsafe int GetHashCodeUnsafe(string source, int start, int end)
        {
            if (start > source.Length || end > source.Length || start < 0 || end < 0)
                throw new IndexOutOfRangeException();

            fixed (char* chPtr = source)
            {
                int num1 = 5381;
                int num2 = num1;
                int num3;
                char* endPtr = chPtr + end - 1;
                for (char* chPtr2 = chPtr + start; (num3 = *chPtr2) != 0 && chPtr2 <= endPtr; chPtr2 += 2)
                {
                    num1 = (num1 << 5) + num1 ^ num3;
                    if (chPtr2 == endPtr)
                        break;
                    int num4 = chPtr2[1];
                    if (num4 != 0)
                        num2 = (num2 << 5) + num2 ^ num4;
                    else
                        break;
                }
                return num1 + num2 * 1566083941;
            }
        }
    }
}
