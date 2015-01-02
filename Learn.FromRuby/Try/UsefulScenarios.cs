using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;

namespace Learn.FromRuby.Try
{
    [TestFixture]
    public class UsefulScenarios
    {
        [Test]
        public void given_empty_collection_when_getting_first_item_should_return_null()
        {
            var names = new List<Person>() {new Person {FirstName = "Todd", LastName = "Meinershagen"}};
            var michaels = names.Where(x => x.FirstName == "Michael");

            michaels.FirstOrDefault().Try(x => x.FirstName).Should().BeNull();
            michaels.FirstOrDefault()
                .Try(x => x.FirstName)
                .Try(x => x.Capitalize())
                .Try(x => x.Reverse())
                .Should()
                .BeNull();
            michaels
                .Try(x => x.FirstOrDefault().FirstName.Capitalize().Reverse()).Should().BeNull();
        }

        class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
