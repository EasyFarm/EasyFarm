using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EasyFarm.Tests.Parsing;
using Ploeh.AutoFixture;
using Xunit;

namespace EasyFarm.Tests.Caskets
{
    public class BrownCasketTests
    {
        [Fact]
        public void Test()
        {
            // Setup fixture
            var messages = new ChatMessages
            (
                "You have a hunch that the first digit is odd."
            );
            var sut = new BrownCasket();
            // Exercise system
            var result = sut.Process(messages);
            // Verify outcome
            Assert.All(result.Answers, AssertFirstDigitOdd);
            // Teardown
        }

        private static void AssertFirstDigitOdd(int value)
        {
            var regex = new Regex(@"(\d)(\d)");
            var matches = regex.Match(value.ToString());
            Assert.True(matches.Success, "Digit is not a two-digit number.");
            Assert.NotEqual(1, matches.Groups.Count);
            var firstDigit = int.Parse(matches.Groups[1].Value);
            Assert.Equal(1, firstDigit % 2);
        }
    }

    public class ChatMessages
    {
        public string[] Messages { get; set; }

        public ChatMessages(params string[] messages)
        {
            Messages = messages ?? new string[0];
        }
    }

    public class BrownCasket
    {
        public BrownCasketResult Process(ChatMessages messages)
        {
            List<int> availableAnswers = Enumerable.Range(10, 90).ToList();

            if (availableAnswers.Min() < 10)
                throw new InvalidOperationException("Casket available answers starts at 10.");

            if (availableAnswers.Max() > 99)
                throw new InvalidOperationException("Casket available answers ends at 99.");

            if (messages.Messages.Any(x => x.Contains("odd")))
            {
                return new BrownCasketResult
                {
                    Answers = availableAnswers.Where(x => (((int)(x / 10.0)) % 2) == 1).ToList()
                };
            }

            return new BrownCasketResult();
        }
    }

    public class BrownCasketResult
    {
        public IEnumerable<int> Answers { get; set; } = new List<int>();
    }
}
