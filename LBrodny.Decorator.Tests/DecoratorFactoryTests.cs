using Moq;
using System;
using Xunit;

namespace LBrodny.Decorator.Tests
{
    public class DecoratorFactoryTests
    {
        [Fact]
        public void Should_throw_when_class_provided()
        {
            Assert.Throws<NotSupportedException>(
                () => DecoratorFactory<SampleClass>.Create(new SampleClass()));
        }

        [Fact]
        public void Should_throws_when_simple_type_provided()
        {
            Assert.Throws<NotSupportedException>(
                () => DecoratorFactory<int>.Create(5));
        }

        [Fact]
        public void Should_throw_when_null_is_to_be_decorated()
        {
            Assert.Throws<ArgumentNullException>(() => DecoratorFactory<TSampleInterface>.Create(null!));
        }

        [Fact]
        public void Should_create_a_decorating_object_that_implements_provided_interface()
        {
            var toBeDecoratedMock = new Mock<TSampleInterface>();
            TSampleInterface toBeDecorated = toBeDecoratedMock.Object;

            TSampleInterface decorated = DecoratorFactory<TSampleInterface>.Create(toBeDecorated);

            Assert.NotSame(toBeDecorated, decorated);
        }

        private class SampleClass
        {
        }

        private interface TSampleInterface
        {
            object Method();
        }
    }
}
