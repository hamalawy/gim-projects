// Author: George Mauer
// Code samples for presentation You Can't Dance the Lambda
// Slide: Conditionals With Hashes
// Demonstrates how to loosely couple conditional logic to implementation by storing it in an associative array
using System;
using System.Collections.Generic;
using System.Linq;

namespace CantDanceTheLambda {

    #region Strategy classes used in example
    public interface IStrategy {
        void ApplyTo(string code);
    }
    public class Strategy1 : IStrategy {
        public void ApplyTo(string code) { }
    }
    public class Strategy2 : IStrategy {
        public void ApplyTo(string code) { }
    }
    public class Strategy3 : IStrategy {
        public void ApplyTo(string code) { }
    }
    #endregion
    
    
    public class StrategyUser {
        #region Using a switch statement - ick
        public void DecideViaSwitch(string code) {
            switch (code) {
                case "S1":
                    new Strategy1().ApplyTo(code); break;
                case "S2":
                    new Strategy2().ApplyTo(code); break;
                case "S3":
                    new Strategy3().ApplyTo(code); break;
            }
        }
        #endregion


        #region Using a hash of matching coditions and actions
        IDictionary<string, Action<string>> _actionHash =
            new Dictionary<string, Action<string>>() {
                {"S1", x=>new Strategy1().ApplyTo(x)},
                {"S2", x=>new Strategy2().ApplyTo(x)},
                {"S3", x=>new Strategy3().ApplyTo(x)}, };
        public StrategyUser(IDictionary<string, Action<string>> actionHash) {
            _actionHash = actionHash ?? _actionHash;
        }

        public void DecideViaHash(string code) {
            if (_actionHash.Keys.Contains(code))
                _actionHash[code](code);

            //Or if you want to get crazy:
            // _actionHash.FirstOrDefault(x => x.Key == code).DoIfNotNull(kv => kv.Value(code));
        }
        #endregion


        #region Using a hash of string predicates and actions !
        IDictionary<Func<string, bool>, Action<string>> _predicateActionHash
            = new Dictionary<Func<string, bool>, Action<string>>() {
                {s=>s.Equals("S1"), 
                    x =>new Strategy1().ApplyTo(x)},
                {s=>s.StartsWith("S"), 
                    x => {/* User reflection to create a Strategy*/}},
                {s=>true, 
                    x => Console.WriteLine("Don't know how to handle code {0}", x)},
            };
        public void RunFirstMatchingPredicateAction(string code) {
            _predicateActionHash.First(kv => kv.Key(code)).Value.Invoke(code);
        }
        public void RunAllMatchingPredicateActions(string code) {
            _predicateActionHash.Where(kv => kv.Key(code)).ToList().ForEach(kv => kv.Value(code));
        }
        #endregion
    }
}
