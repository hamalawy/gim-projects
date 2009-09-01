using System;
using System.Linq;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace CantDanceTheLambda {
    public class ArrayFilterExample {
        public delegate bool StringPredicateDelegate(string str);
        
        public static Array ArrayFilter(Array array, StringPredicateDelegate predicate) {
            Queue newArray = new Queue();
            for (int i = 0; i < array.Length; i++)
                if (predicate(array.GetValue(i).ToString()))
                    newArray.Enqueue(array.GetValue(i));
            return newArray.ToArray();
        }
        #region AnonymousDelegates
        public static Array DynamicArrayFilter(Array array, Delegate predicate) {
            Queue newArray = new Queue();
            for (int i = 0; i < array.Length; i++)
                if ((bool)predicate.DynamicInvoke(array.GetValue(i).ToString()))
                    newArray.Enqueue(array.GetValue(i));
            return newArray.ToArray();
        }
        #region ModernArrayFilter
        public static T[] ModernArrayFilter<T>(T[] array, Func<T, bool> predicate) {
            List<T> newArray = new List<T>();
            for (int i = 0; i < array.Length; i++)
                if (predicate(array[i]))
                    newArray.Add(array[i]);
            return newArray.ToArray();
        }
        #endregion
        #endregion
    }

    
    [TestFixture]
    public class When_filtering_array_of_people_for_friends {
        readonly string[] _peoplePresent = new[] { "Mike", "Xavier", "Methusela", "Justen", "Napoleon" }; 
        readonly string[] _friends = new[] {"Mike", "Henry", "Justen", "Jen"};
        private bool IsInFriendsList(string name) {
            return _friends.Contains(name);
        }
        [Test] public void Can_filter_array_of_friends_with_explicit_delegate() {
            ArrayFilterExample.StringPredicateDelegate predicate
                = new ArrayFilterExample.StringPredicateDelegate(IsInFriendsList);
            var friendsPresent = ArrayFilterExample.ArrayFilter(_peoplePresent, predicate);
            CollectionAssert.AreEquivalent(new[] { "Mike", "Justen" }, friendsPresent);
        }
        [Test] public void Can_filter_array_of_friends_with_anonymous_delegate() {
            var friendsPresent = ArrayFilterExample.ArrayFilter(_peoplePresent,
                delegate(string name) { return _friends.Contains(name); }); 
            CollectionAssert.AreEquivalent(new[] { "Mike", "Justen" }, friendsPresent);
        }
        [Test] public void Can_filter_array_of_friends_with_explicit_lambda() {
            var friendsPresent = ArrayFilterExample.ModernArrayFilter(_peoplePresent,
                new Func<string, bool>(delegate(string name) { return _friends.Contains(name); })); 
            CollectionAssert.AreEquivalent(new[] { "Mike", "Justen" }, friendsPresent);
        }
        [Test] public void Can_filter_array_of_friends_with_lambda() {
            var friendsPresent = ArrayFilterExample.ModernArrayFilter(_peoplePresent,
                name => _friends.Contains(name));
            CollectionAssert.AreEquivalent(new[] { "Mike", "Justen" }, friendsPresent);
        }
        [Test] public void Can_filter_array_of_friends_with_LINQ() {
            var friendsPresent = _peoplePresent.Where(name => _friends.Contains(name));
            CollectionAssert.AreEquivalent(new[] { "Mike", "Justen" }, friendsPresent);
        }
    }
}
