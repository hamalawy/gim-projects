using System;
using System.Collections.Generic;
using System.Linq;

namespace CantDanceTheLambda {

    public interface IStrategy {
        void ApplyTo(StrategyUser user);
    }
    public class Strategy1 : IStrategy {
        public void ApplyTo(StrategyUser user) { }
    }
    public class Strategy2 : IStrategy {
        public void ApplyTo(StrategyUser user) { }
    }
    public class Strategy3 : IStrategy {
        public void ApplyTo(StrategyUser user) { }
    }
    public class StrategyUser {
        public void UseWithSwitch(string code) {
            switch (code) {
                case "S1":
                    new Strategy1().ApplyTo(this); break;
                case "S2":
                    new Strategy2().ApplyTo(this); break;
                case "S3":
                    new Strategy3().ApplyTo(this); break;
            }
        }
        IDictionary<string, Action<StrategyUser>> _actionHash = 
            new Dictionary<string, Action<StrategyUser>>() {
                {"S1", x=>new Strategy1().ApplyTo(x)},
                {"S2", x=>new Strategy2().ApplyTo(x)},
                {"S3", x=>new Strategy3().ApplyTo(x)}, };
        public StrategyUser(IDictionary<string, Action<StrategyUser>> actionHash) {
            _actionHash = actionHash ?? _actionHash;
        }
        public void UseWithHash(string code) {
            if (_actionHash.Keys.Contains(code))
                _actionHash[code](this);
            
            //Or Even:
            // _actionHash.FirstOrDefault(x => x.Key == code).DoIfNotNull(kv => kv.Value(this));
        }
        IDictionary<Func<string, bool>, Action<string, StrategyUser>> _predicateActionHash 
            = new Dictionary<Func<string, bool>, Action<string, StrategyUser>>() {
                {s=>s.Equals("S1"), (s,x)=>new Strategy1().ApplyTo(x)},
                {s=>s.StartsWith("S"), (s, x) => {/* User reflection to create a Strategy*/}},
                {s=>true, (s,x)=> Console.WriteLine("Don't know how to handle code {0}", s)},
            };
        public void RunFirstMatchingPredicateAction(string code) {
            _predicateActionHash.First(kv => kv.Key(code)).Value.Invoke(code, this);
        }
        public void RunAllMatchingPredicateActions(string code) { 
            _predicateActionHash.Where(kv=>kv.Key(code)).ToList().ForEach(kv=>kv.Value(code, this));
        }
    }
}
