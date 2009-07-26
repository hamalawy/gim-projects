using System;

namespace GIM.CastleContrib.Tests.PropertyInjectionFacility {
    public class AnswerToLifeUniverseAndEverything {
        public readonly int Value = 42;
    }
    public class QuestionOfLifeUniverseAndEverything {
        public AnswerToLifeUniverseAndEverything TheAnswer { get; set; }
    }
}