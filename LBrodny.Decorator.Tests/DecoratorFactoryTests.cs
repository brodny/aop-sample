using Moq;
using FluentAssertions;
using System;
using Xunit;
using Moq.Sequences;
using System.Reflection;

namespace LBrodny.Decorator.Tests
{
    public class DecoratorFactoryTests
    {
        private readonly Mock<IDecorateObject> _decoratorMock;

        public DecoratorFactoryTests()
        {
            _decoratorMock = new Mock<IDecorateObject>();
        }

        [Fact]
        public void Should_throw_when_class_provided()
        {
            Action action = () => DecoratorFactory<SampleClass>.Create(
                new SampleClass(),
                _decoratorMock.Object);

            action
                .Should()
                .Throw<TypeInitializationException>()
                .WithInnerException<NotSupportedException>();
        }

        [Fact]
        public void Should_throw_when_null_is_to_be_decorated()
        {
            Assert.Throws<ArgumentNullException>(
                () => DecoratorFactory<TSampleInterface>.Create(
                    null!,
                    _decoratorMock.Object));
        }

        [Fact]
        public void Should_throw_when_decorator_is_null()
        {
            var toBeDecoratedMock = new Mock<TSampleInterface>();
            TSampleInterface toBeDecorated = toBeDecoratedMock.Object;

            Assert.Throws<ArgumentNullException>(
                () => DecoratorFactory<TSampleInterface>.Create(
                    toBeDecorated,
                    null!));
        }

        [Fact]
        public void Should_create_a_decorating_object_that_implements_provided_interface()
        {
            var toBeDecoratedMock = new Mock<TSampleInterface>();
            TSampleInterface toBeDecorated = toBeDecoratedMock.Object;

            TSampleInterface decorated = DecoratorFactory<TSampleInterface>.Create(
                toBeDecorated,
                _decoratorMock.Object);

            Assert.NotSame(toBeDecorated, decorated);
        }

        [Fact]
        public void Should_execute_methods_of_decorated_object()
        {
            var toBeDecoratedMock = new Mock<TSampleInterface>();
            TSampleInterface toBeDecorated = toBeDecoratedMock.Object;

            TSampleInterface decorated = DecoratorFactory<TSampleInterface>.Create(
                toBeDecorated,
                _decoratorMock.Object);

            _ = decorated.Method();

            toBeDecoratedMock.Verify(m => m.Method(), Times.Once);
        }

        [Fact]
        public void Should_execute_MethodCalling_before_the_decorated_object_method_is_called()
        {
            var toBeDecoratedMock = new Mock<TSampleInterface>();

            using (Sequence.Create())
            {
                _decoratorMock
                    .Setup(_ => _.MethodCalling(It.IsAny<MethodInfo>(), It.IsAny<object[]>()))
                    .InSequence();
                toBeDecoratedMock
                    .Setup(_ => _.Method())
                    .InSequence();

                TSampleInterface decorated = DecoratorFactory<TSampleInterface>.Create(
                    toBeDecoratedMock.Object,
                    _decoratorMock.Object);

                _ = decorated.Method();
            }
        }

        public class SampleClass
        {
        }

        public interface TSampleInterface
        {
            object Method();
        }
    }
}
