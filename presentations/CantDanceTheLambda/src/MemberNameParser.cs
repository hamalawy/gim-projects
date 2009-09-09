// Author: George Mauer
// Code samples for presentation You Can't Dance the Lambda
// Slide: And More!
// Build a class to provide access to names of class members without ever having to use weakly typed strings
using System;
using NUnit.Framework;
using System.Linq.Expressions;
namespace CantDanceTheLambda {
    /// <summary>
    /// An extentension method on an actual object to get its member names
    /// </summary>
    public static partial class ReflectionExtensions {
        public static string MemberName<T, R>(this T obj, Expression<Func<T, R>> expr) {            
            var node = expr.Body as MemberExpression;
            if (object.ReferenceEquals(null, node))
                throw new InvalidOperationException("Expression must be of member access");
            return node.Member.Name;
        }
    }

    /// <summary>
    /// If we don't have an actual object we can still refer to its type with this generic static class
    /// </summary>
    public static class MembersOf<T> {
        public static string GetName<R>(Expression<Func<T,R>> expr) {
            var node = expr.Body as MemberExpression;
            if (object.ReferenceEquals(null, node)) 
                throw new InvalidOperationException("Expression must be of member access");
            return node.Member.Name;
        }
    }

    [TestFixture]
    public class MemberNameParserTests {
        [Test] public void Can_get_property_name_from_instance() {
            var a = new Animal(null, null);
            Assert.AreEqual("Status", a.MemberName(x => x.Status));
        }
        [Test] public void Can_get_property_name_from_class() {
            Assert.AreEqual("Status", MembersOf<Animal>.GetName(x => x.Status));
        }
    }
}
