// Author: George Mauer
// Code samples for presentation You Can't Dance the Lambda
// Slide: Time For Review, Time For Review (cont'd), The Main Event
// Example of using an old-style delegate to filter an array

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

        public static Array DynamicArrayFilter(Array array, Delegate predicate) {
            Queue newArray = new Queue();
            for (int i = 0; i < array.Length; i++)
                if ((bool)predicate.DynamicInvoke(array.GetValue(i).ToString()))
                    newArray.Enqueue(array.GetValue(i));
            return newArray.ToArray();
        }

        public static T[] ModernArrayFilter<T>(T[] array, Func<T, bool> predicate) {
            List<T> newArray = new List<T>();
            for (int i = 0; i < array.Length; i++)
                if (predicate(array[i]))
                    newArray.Add(array[i]);
            return newArray.ToArray();
        }

    }


    [TestFixture]
    public class When_filtering_array_of_people_for_friends {
        readonly string[] _peoplePresent = new[] { "Mike", "Xavier", "Methusela", "Frank", "Napoleon" };
        readonly string[] _friends = new[] { "Mike", "Henry", "Frank", "Jen" };
        
        private bool IsInFriendsList(string name) {
            return _friends.Contains(name);
        }
    
        [Test]
        public void Can_filter_array_of_friends_with_explicit_delegate() {
            ArrayFilterExample.StringPredicateDelegate predicate
                = new ArrayFilterExample.StringPredicateDelegate(IsInFriendsList);
            var friendsPresent = ArrayFilterExample.ArrayFilter(_peoplePresent, predicate);
            CollectionAssert.AreEquivalent(new[] { "Mike", "Frank" }, friendsPresent);
        }
        
        [Test]
        public void Can_filter_array_of_friends_with_anonymous_delegate() {
            var friendsPresent = ArrayFilterExample.ArrayFilter(_peoplePresent,
                delegate(string name) { return _friends.Contains(name); });
            CollectionAssert.AreEquivalent(new[] { "Mike", "Frank" }, friendsPresent);
        }
        
        [Test]
        public void Can_filter_array_of_friends_with_explicit_lambda() {
            var friendsPresent = ArrayFilterExample.ModernArrayFilter(_peoplePresent,
                new Func<string, bool>(delegate(string name) { return _friends.Contains(name); }));
            CollectionAssert.AreEquivalent(new[] { "Mike", "Frank" }, friendsPresent);
        }
        
        [Test]
        public void Can_filter_array_of_friends_with_lambda() {
            var friendsPresent = ArrayFilterExample.ModernArrayFilter(_peoplePresent,
                name => _friends.Contains(name));
            CollectionAssert.AreEquivalent(new[] { "Mike", "Frank" }, friendsPresent);
        }

        [Test]
        public void Can_filter_array_of_friends_with_LINQ() {
            var friendsPresent = _peoplePresent.Where(name => _friends.Contains(name));
            CollectionAssert.AreEquivalent(new[] { "Mike", "Frank" }, friendsPresent);
        }
    }
}
