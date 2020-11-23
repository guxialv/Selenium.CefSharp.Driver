using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Selenium.CefSharp.Driver
{
    /// <summary>
    ///     集合逻辑类型
    /// </summary>
    public enum LogicType
    {
        AllRight,
        AnyWrong,
        AllWrong,
        AnyRight
    }

    /// <summary>
    ///     自定义的扩展方法
    /// </summary>
    public static class ExtendEnumerable
    {
        private static readonly Random random = new Random(unchecked((int)DateTime.Now.Ticks));

        public static Regex UnsafeColumnMatcher = new Regex("[ ()\\[ \\] \\^ *×――(^)$%~!@#$…&%￥+=<>《》!！??？:：•`·、。，；,.;\"‘’“”]");
        public static string ReplaceErrorChars(string input)
        {
            return UnsafeColumnMatcher.Replace(input, "");
        }
        public static string ReplaceSplitString(string input, string splitchar)
        {
            if (input == null)
                return "";
            input = input.Replace("\"\"", "\"");
            return input.Trim('"');
        }


        public static List<T> IListConvert<T>(this IList list) where T : class
        {
            var newlist = new List<T>();
            foreach (var item in list)
            {
                newlist.Add(item as T);
            }
            return newlist;
        }
        public static string GenerateRandomString(int length)
        {
            var checkCode = string.Empty;

            var random = new Random();

            for (var i = 0; i < length; i++)
            {
                var number = random.Next();

                char code;
                if (number % 2 == 0)
                {
                    code = (char)('0' + (char)(number % 10));
                }
                else
                {
                    code = (char)('A' + (char)(number % 26));
                }

                checkCode += code.ToString();
            }

            return checkCode;
        }

        public static void KeepRange<T>(this IList<T> collection, int start, int end)
        {
            if (start < 0)
                start = 0;
            if (end >= collection.Count)
                end = collection.Count;

            for (int i = collection.Count - 1; i > end; i--)
            {
                var item = collection[i];
                collection.Remove(item);

            }
            for (int i = 0; i < start; i++)
            {
                var item = collection[i];
                collection.Remove(item);
            }
        }

        //public static IFreeDocument MergeToDocument(this IEnumerable<IFreeDocument> docs)
        //{
        //    var keys = docs.GetKeys();
        //    var doc = new FreeDocument();
        //    foreach (var key in keys)
        //    {
        //        doc[key] = docs.Select(d => d[key]).FirstOrDefault(d => d != null);
        //    }
        //    return doc;
        //}


        public static IEnumerable<T> Init<T>(this IEnumerable<T> items, Func<T, bool> init = null)
        {
            var count = 0;
            foreach (var item in items)
            {
                if (count == 0)
                    if (init != null && init(item) == false)
                    {

                        yield break;
                    }
                count++;

                yield return item;
            }
        }

        //public static IFreeDocument MergeQuery(this IFreeDocument document, IFreeDocument doc2, string columnNames)
        //{
        //    if (doc2 == null || string.IsNullOrWhiteSpace(columnNames))
        //        return document;

        //    var columns = columnNames.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        //    if (columnNames.ToString() == "*")
        //        columns = doc2.Keys.ToArray();
        //    foreach (var column in columns)
        //    {
        //        document.SetValue(column, doc2[column]);
        //    }
        //    return document;
        //}

        public static Dictionary<string, string> ToDict(string parameter, char split = ' ')
        {
            var dict = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(parameter))
                return dict;
            foreach (var item in parameter.Split('\n'))
            {
                var items = item.Split(split);
                if (items.Length != 2)
                    continue;
                dict[items[0]] = dict[items[1]];
            }
            return dict;
        }

        public static List<int> GetRandomInts(int min, int max, int mount)
        {
            var values = new List<int>(mount);
            var index = 0;
            while (index < mount)
            {
                var value = random.Next(min, max);
                if (!values.Contains(value))
                {
                    values.Add(value);
                    index++;
                }
            }
            return values;
        }

        public static int GetRandonInt(int min, int max)
        {
            return random.Next(min, max);
        }

        #region Constants and Fields

        private static Type lastType;

        private static PropertyInfo[] propertys;

        #endregion

        #region Public Methods

        public static void OrThrows(this bool condition, string message = null)
        {
            if (!condition)
            {
                throw new Exception(message ?? "Something error");
            }
        }

        public static bool LogicCheck(this IEnumerable<bool> checks, LogicType type)
        {
            switch (type)
            {
                case LogicType.AllRight:
                    return checks.All(d => d);
                case LogicType.AllWrong:
                    return checks.All(d => d == false);
                case LogicType.AnyRight:
                    return checks.Any(d => d);
                case LogicType.AnyWrong:
                    return checks.Any(d => d == false);
            }
            return false;
        }

        public static void AddRange<K, V>(this IDictionary<K, V> source, IDictionary<K, V> value)
        {
            if (value == null)
                return;
            foreach (var d in value)
            {
                source.SetValue(d.Key, d.Value);
            }
        }


        public static void AddRange<T>(this IList<T> source, IEnumerable<T> items)
        {
            if (source == null || items == null)
                return;

            foreach (var d in items)
            {
                source.Add(d);
            }
        }

        public static int MaxSameCount<T>(this IEnumerable<T> source, Func<T, T, bool> issame, out T maxvalue)
        {
            var dict = new Dictionary<T, int>();
            foreach (var t in source)
            {
                var add = false;
                foreach (var i in dict)
                {
                    if (issame(i.Key, t))
                    {
                        dict[i.Key]++;
                        add = true;
                        break;
                    }
                }
                if (add == false)
                    dict.Add(t, 1);
            }
            maxvalue = default(T);
            if (dict.Any() == false)
            {
                return 0;
            }
            var maxcount = 0;

            foreach (var i in dict)
            {
                if (i.Value > maxcount)
                {
                    maxcount = i.Value;
                    maxvalue = i.Key;
                }
            }

            return maxcount;
        }

        public static int MaxSameCount<T>(this IEnumerable<T> source, out T maxvalue)
        {
            var dict = new Dictionary<T, int>();
            foreach (var t in source)
            {
                if (dict.ContainsKey(t))
                    dict[t]++;
                else
                {
                    dict[t] = 0;
                }
            }
            maxvalue = default(T);
            if (dict.Any() == false)
            {
                return 0;
            }
            var maxcount = 0;

            foreach (var i in dict)
            {
                if (i.Value > maxcount)
                {
                    maxcount = i.Value;
                    maxvalue = i.Key;
                }
            }

            return maxcount;
        }


        public static int MaxSameCount<T>(this IEnumerable<T> source)
        {
            var dict = new Dictionary<T, int>();
            foreach (var t in source)
            {
                if (dict.ContainsKey(t))
                    dict[t]++;
                else
                {
                    dict[t] = 0;
                }
            }
            if (dict.Any() == false)
                return 0;
            return dict.Max(d => d.Value);
        }

        public static IDictionary<string, object> Clone(this IDictionary<string, object> old)
        {
            var dict = new Dictionary<string, object>();
            foreach (var o in old)
            {
                dict.Add(o.Key, o.Value);
            }
            return dict;
        }
        public static IEnumerable<int> SelectAll<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> selector)
        {
            var item = source.ToList();
            for (var i = 0; i < item.Count; i++)
            {
                var p = item[i];
                if (selector(p))
                {
                    yield return i;
                }
            }
        }


        /// <summary>
        ///     获取序列中的最大元素值，并将其所在编号返回
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="maxindex"></param>
        /// <returns></returns>
        public static int Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector,
            out int maxindex)
        {
            var t = 0;
            var index = 0;
            double max = 0;
            foreach (var item in source)
            {
                var i = selector(item);
                if (max < i)
                {
                    max = i;
                    index = t;
                }
                t++;
            }
            maxindex = index;
            return maxindex;
        }

        public static int MaxSameLength<T>(this IEnumerable<T> enumerable, T sameItem, out int start)
        {
            if (enumerable == null)
            {
                start = 0;

                return 0;
            }


            var dictionary = new Dictionary<int, int>();


            var count = 0;
            var len = 0;
            foreach (var item in enumerable)
            {
                if (!Equals(sameItem, item))
                {
                    if (count != 0)
                    {
                        dictionary.Add(count - len, len);
                    }


                    len = 0;
                }
                else
                {
                    len++;
                }
                count++;
            }


            var max = 0;
            var maxindex = 0;
            foreach (var i in dictionary)
            {
                if (i.Value > max)
                {
                    max = i.Value;
                    maxindex = i.Key;
                }
            }
            start = maxindex;

            return max;
        }

        public static T Search<T>(this IDictionary<string, T> dict, string name)
        {
            foreach (var item in dict)
            {
                if (item.Key.Contains(name)) return item.Value;
            }
            return default(T);
        }


        /// <summary>
        ///     对序列中的每个元素执行方法
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="method"></param>
        public static void Execute<TSource>(this IEnumerable<TSource> source, Action<TSource> method)
        {
            foreach (var d in source)
            {
                method(d);
            }
        }

        /// <summary>
        ///     对序列中的每个元素执行方法
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="method"></param>
        public static IEnumerable<TSource> ExecuteReturn<TSource>(
            this IEnumerable<TSource> source, Action<TSource> method)
        {
            foreach (var d in source)
            {
                method(d);
                yield return d;
            }
        }


        public static IEnumerable<KeyValuePair<string, object>> Filter(
            this IDictionary<string, object> dict, IEnumerable<string> names)
        {
            return names.Select(name => new KeyValuePair<string, object>(name, dict[name]));
        }

        public static T Get<T>(this IDictionary<string, object> dat, string key)
        {
            if (!dat.ContainsKey(key))
            {
                return default(T);
            }

            var item = default(T);
            var data = dat[key];

            var type = typeof(T);
            try
            {
                if (type.IsEnum)
                {
                    var s = data.ToString();

                    var res = (T)Enum.Parse(typeof(T), s);
                    return (res);
                }
                item = (T)Convert.ChangeType(dat[key], typeof(T));
            }
            catch (Exception)
            {
            }

            return item;
        }
        // Extends String.Join for a smooth API.
        public static string Join(this string separator, IEnumerable<object> values)
        {
            return string.Join(separator, values);
        }


        public static T GetRandom<T>(this IEnumerable<T> enumerable)
        {
            var enumerable1 = enumerable as T[] ?? enumerable.ToArray();
            var l = enumerable1.Count();
            if (l == 0)
            {
                return default(T);
            }
            return enumerable1.ElementAt(GetRandonInt(0, l - 1));
        }

        public static IEnumerable GetRange(this IEnumerable enumerable, int start, int end)
        {
            var e = enumerable.GetEnumerator();
            while (start > 0)
            {
                e.MoveNext();
            }
            while (end > 0)
            {
                yield return e.Current;
                e.MoveNext();
            }
        }

        public static void IncreaseSet<TK>(this IDictionary<TK, int> dat, TK k)
        {
            if (dat == null)
            {
                return;
            }
            if (dat.ContainsKey(k))
            {
                dat[k]++;
            }
            else
            {
                dat.Add(k, 1);
            }
        }

        /// <summary>
        ///     对集合实现删除操作
        /// </summary>
        /// <typeparam name="TSource">元素类型</typeparam>
        /// <param name="source">要删除的元素列表</param>
        /// <param name="filter">过滤器</param>
        /// <param name="method">删除时执行的委托</param>
        public static IEnumerable<TSource> RemoveElements<TSource>(
            this IList<TSource> source, Func<TSource, bool> filter, Action<TSource> method = null)
        {
            var indexs = (from d in source where filter(d) select source.IndexOf(d)).ToList();
            indexs.Sort();
            for (var i = indexs.Count - 1; i >= 0; i--)
            {
                if (method != null)
                {
                    method(source[indexs[i]]);
                }
                yield return source[indexs[i]];
                source.RemoveAt(indexs[i]);
            }
        }

        public static void RemoveElements<K, V>(
            this IDictionary<K, V> source, Func<K, bool> filter, Action<V> method = null)
        {
            var indexs = (from d in source where filter(d.Key) select d.Key).ToList();

            for (var i = indexs.Count - 1; i >= 0; i--)
            {
                if (method != null)
                {
                    method(source[indexs[i]]);
                }

                source.Remove(indexs[i]);
            }
        }

        public static void RemoveAll<TSource>(
            this IList<TSource> source)

        {
            source.RemoveElementsNoReturn(d => true);
        }

        /// <summary>
        ///     对集合实现删除操作
        /// </summary>
        /// <typeparam name="TSource">元素类型</typeparam>
        /// <param name="source">要删除的元素列表</param>
        /// <param name="filter">过滤器</param>
        /// <param name="method">删除时执行的委托</param>
        public static void RemoveElementsNoReturn<TSource>(
            this IList<TSource> source, Func<TSource, bool> filter, Action<TSource> method = null)
        {
            var indexs = (from d in source where filter(d) select source.IndexOf(d))?.ToList();
            if (!indexs.Any())
                return;
            indexs.Sort();
            for (var i = indexs.Count - 1; i >= 0; i--)
            {
                if (source.Count <= indexs[i])
                    continue;
                method?.Invoke(source[indexs[i]]);
                if (source.Count <= indexs[i])
                    continue;
                source.RemoveAt(indexs[i]);
            }
        }

        public static void RemoveElementsWithValue<K, V>(
            this IDictionary<K, V> source, Func<V, bool> filter, Action<K> method = null)
        {
            var indexs = (from d in source where filter(d.Value) select d.Value).ToList();

            for (var i = indexs.Count - 1; i >= 0; i--)
            {
                var key = source.FirstOrDefault(d => d.Value.Equals(indexs[i])).Key;
                if (method != null)
                {
                    method(key);
                }

                source.Remove(key);
            }
        }

        public static void Set<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

        public static void SetValue<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

        public static IEnumerable<int> Range(int start, Func<int, bool> contineFunc, int interval)
        {
            for (var i = start; contineFunc(i); i += interval)
            {
                yield return i;
            }
        }


        #endregion
    }
}