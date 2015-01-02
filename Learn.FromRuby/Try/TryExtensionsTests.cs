using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;

namespace Learn.FromRuby.Try
{
    [TestFixture]
    public class given_function
    {
        [Test]
        public void given_object_when_trying_to_call_function_with_struct_return_should_return_result_of_function()
        {
            const int expectedValue = 5;
            var value = new TestObject(expectedValue);

            value.Try(x => x.Value).Should().Be(expectedValue);
        }

        [Test]
        public void given_null_object_when_trying_to_call_function_with_struct_return_should_return_default_struct_value
            ()
        {
            TestObject value = null;

            value.Try(x => x.Value).Should().Be(default(int));
        }

        [Test]
        public void given_object_when_trying_to_call_function_with_reference_return_should_return_result_of_function()
        {
            var expectedValue = new TestObject(4);
            var value = new TestObject(5, expectedValue);

            value.Try(x => x.GetChild()).Should().Be(expectedValue);
        }

        [Test]
        public void given_null_object_when_trying_to_call_function_with_reference_return_should_return_null()
        {
            TestObject value = null;

            value.Try(x => x.GetChild()).Should().BeNull();
        }

        [Test]
        public void given_chain_of_nulls_when_trying_to_call_functions_should_return_default_or_null_values()
        {
            TestObject value = null;

            value.Try(x => x.GetChild()).Try(x => x.Value).Should().Be(default(int));
            value.Try(x => x.GetChild()).Try(x => x.GetChild()).Should().BeNull();
        }

        [Test]
        public void given_chain_of_methods_with_the_first_returning_null_when_trying_to_call_functions_should_return_default_or_null_values()
        {
            var names = new List<Person>() { new Person { FirstName = "Todd", LastName = "Meinershagen" } };
            var michaels = names.Where(x => x.FirstName == "Michael");

            michaels
                .Try(x => x.FirstOrDefault().FirstName.Capitalize().Reverse()).Should().BeNull();
        }

        class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }

    [TestFixture]
    public class given_function_with_output_variable
    {
        [Test]
        public void when_trying_to_call_function_with_struct_return_that_succeeds_should_return_true_and_set_output()
        {
            const int expectedValue = 5;
            var value = new TestObject(expectedValue);
            int output;

            value.Try(x => x.GetValue(), out output).Should().Be(true);
            output.Should().Be(expectedValue);
        }

        [Test]
        public void when_trying_to_call_function_with_struct_return_that_fails_should_return_false_and_set_output_to_default
            ()
        {
            var value = new TestObject(5);
            int output;

            value.Try(x => x.GetValue(true), out output).Should().Be(false);
            output.Should().Be(default(int));
        }

        [Test]
        public void when_trying_to_call_function_with_reference_return_that_succeeds_should_return_true_and_set_output()
        {
            var expectedValue = new TestObject(4);
            var value = new TestObject(5, expectedValue);
            TestObject output;

            value.Try(x => x.GetChild(), out output).Should().Be(true);
            output.Should().Be(expectedValue);
        }

        [Test]
        public void when_trying_to_call_function_with_reference_return_that_fails_should_return_false_and_set_output_to_null
            ()
        {
            var expectedValue = new TestObject(4);
            var value = new TestObject(5, expectedValue);
            TestObject output;

            value.Try(x => x.GetChild(true), out output).Should().Be(false);
            output.Should().BeNull();
        }

        [Test]
        public void when_trying_to_call_static_try_with_function_with_struct_return_that_succeeds_should_return_true_and_set_output()
        {
            int output;
            With.Try(() => int.Parse("1"), out output).Should().Be(true);
            output.Should().Be(1);
        }

        [Test]
        public void when_trying_to_call_static_try_with_function_with_struct_return_that_fails_should_return_false_and_set_output_to_default_value()
        {
            int output;
            With.Try(() => int.Parse("abc"), out output).Should().Be(false);
            output.Should().Be(default(int));
        }

        [Test]
        public void when_trying_to_call_static_try_with_function_with_reference_return_that_succeeds_should_return_true_and_set_output()
        {
            var expectedValue = new TestObject(4);
            TestObject output;

            With.Try(() => expectedValue, out output).Should().Be(true);
            output.Should().Be(expectedValue);
        }

        [Test]
        public void when_trying_to_call_static_try_with_function_with_reference_return_that_fails_should_return_false_and_set_output_to_null()
        {
            TestObject output;

            With.Try(() => {throw new ArgumentException();}, out output).Should().Be(false);
            output.Should().BeNull();
        }
    }
}
