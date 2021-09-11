using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwoPiGrid.Configuration;

namespace TwoPiGrid.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            return char.ToUpper(s[0]) + s.Substring(1);
        }
        
        public static string ToLowerFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            return char.ToLower(s[0]) + s.Substring(1);
        }
        
        public static string RemoveNonAlphanumericOrUnderscoresAndEnsureFirstIsNotNumeric(this string s)
        {
            var str = new string(s.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());
            try
            {
                var firstLetterOrUnderscore = str.First(c => char.IsLetter(c) || c == '_');
                var index = str.IndexOf(firstLetterOrUnderscore);
                str = str.Substring(index);
            }
            catch (InvalidOperationException)
            {
                str = "";
            }
            return str;
        }

        public static string RemoveNonAlphanumericOrUnderscoresAndEnsureFirstIsNotNumericOrUnderscore(this string s)
        {
            var str = new string(s.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());
            try
            {
                var firstLetterOrUnderscore = str.First(c => char.IsLetter(c));
                var index = str.IndexOf(firstLetterOrUnderscore);
                str = str.Substring(index);
            }
            catch (InvalidOperationException)
            {
                str = "";
            }
            return str;
        }
    }

    public static class EnumeratorExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerator enumerator)
        {
            while(enumerator.MoveNext())
                yield return (T) enumerator.Current;
        }

        public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> enumerator)
        {
            while(enumerator.MoveNext())
                yield return enumerator.Current;
        }
    }
    
    public static class ValueTypesExtension
    {
        internal static string ExtractType(this CustomCellField.ValueTypes valueType)
        {
            return valueType.ToString().Substring(1);
        }
    }
}
