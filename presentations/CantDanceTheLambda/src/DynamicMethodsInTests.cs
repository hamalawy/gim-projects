using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using System.IO;

namespace CantDanceTheLambda {
    public class PredatorEncounterStrategy : IEncounterStrategy {
        public string EncounterResponse { get { return "Flee"; } }
    }
    public class PreyEncounterStrategy : IEncounterStrategy {
        public string EncounterResponse { get { return "Feed"; } }
    }
    public interface IEncounterStrategy {
        string EncounterResponse { get; }
    }
    public class Animal {

        private string _currently;
        private IEncounterStrategy _preyStrategy;
        private IEncounterStrategy _predatorStrategy;
        public Animal(IEncounterStrategy predatorStrategy, IEncounterStrategy preyStrategy) {
            _preyStrategy = preyStrategy;
            _predatorStrategy = predatorStrategy;
        }
        public string Status {
            get { return _currently+"ing"; }
        }
        public void Encounter(string animalType) {
            if (animalType.ToUpperInvariant() == "TIGER")
                _currently = _predatorStrategy.EncounterResponse;
            else
                _currently = _preyStrategy.EncounterResponse;
        }
    }
    [TestFixture]
    public class DynamicMethosInTests {
        Action<Animal> when = _ => { };
        Action<Animal> then = _ => { };
        [SetUp] public void arrange_scenario() {
            when = _ => { }; then = _ => { }; }
        [TearDown] public void execute_scenario() {
            //given
            var animal = new Animal(new PredatorEncounterStrategy(), new PreyEncounterStrategy());
            when(animal);
            then(animal);
        }
        [Test] public void when_encountering_a_tiger_flee() {
            when = a => a.Encounter("tiger");
            then = a => Assert.AreEqual("Fleeing", a.Status);
        }
        [Test] public void when_encountering_a_turtle_feed() {
            when = a => a.Encounter("turtle");
            then = a => Assert.AreEqual("Feeding", a.Status);
        }
    }
}
