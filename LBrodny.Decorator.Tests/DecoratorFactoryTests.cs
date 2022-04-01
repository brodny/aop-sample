using Moq;
using FluentAssertions;
using System;
using Xunit;
using Moq.Sequences;
using System.Reflection;
using AutoFixture.Xunit2;

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
                    .Setup(_ => _.MethodCalling(It.IsAny<MethodInfo>(), It.IsAny<object?[]>()))
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

        [Fact]
        public void Should_execute_MethodCalled_after_the_decorated_object_method_successfully_executes()
        {
            var toBeDecoratedMock = new Mock<TSampleInterface>();

            using (Sequence.Create())
            {
                toBeDecoratedMock
                    .Setup(_ => _.Method())
                    .InSequence();

                _decoratorMock
                    .Setup(_ => _.MethodCalled(It.IsAny<MethodInfo>(), It.IsAny<object?[]>(), It.IsAny<object?>()))
                    .InSequence();

                TSampleInterface decorated = DecoratorFactory<TSampleInterface>.Create(
                    toBeDecoratedMock.Object,
                    _decoratorMock.Object);

                _ = decorated.Method();
            }
        }

        [Fact]
        public void Should_execute_MethodThrewException_after_the_decorated_object_method_throws()
        {
            var toBeDecoratedMock = new Mock<TSampleInterface>();

            using (Sequence.Create())
            {
                toBeDecoratedMock
                    .Setup(_ => _.Method())
                    .InSequence()
                    .Throws<Exception>();

                _decoratorMock
                    .Setup(_ => _.MethodCalled(It.IsAny<MethodInfo>(), It.IsAny<object?[]>(), It.IsAny<object?>()))
                    .InSequence(Times.Never());

                _decoratorMock
                    .Setup(_ => _.MethodThrewException(It.IsAny<MethodInfo>(), It.IsAny<object?[]>(), It.IsAny<Exception>()))
                    .InSequence();

                TSampleInterface decorated = DecoratorFactory<TSampleInterface>.Create(
                    toBeDecoratedMock.Object,
                    _decoratorMock.Object);

                try
                {
                    _ = decorated.Method();
                }
                catch { }
            }
        }

        [Theory]
        [AutoData]
        public void Should_pass_decorated_method_arguments_to_MethodCalling_method(
            int param1,
            string param2,
            SampleClass? param3)
        {
            var toBeDecoratedMock = new Mock<TSampleInterface>();

            object?[] actualArguments = null!;

            _decoratorMock
                .Setup(_ => _.MethodCalling(It.IsAny<MethodInfo>(), It.IsAny<object?[]>()))
                .Callback((MethodInfo _, object?[] args) => actualArguments = args);

            TSampleInterface decorated = DecoratorFactory<TSampleInterface>.Create(
                toBeDecoratedMock.Object,
                _decoratorMock.Object);

            _ = decorated.MethodWithParameters(param1, param2, param3);

            object?[] expectedArguments = new object?[] { param1, param2, param3 };

            actualArguments.Should().BeEquivalentTo(
                expectedArguments,
                config => config.WithStrictOrdering());
        }

        [Theory]
        [AutoData]
        public void Should_pass_decorated_method_arguments_to_MethodCalled_method(
            int param1,
            string param2,
            SampleClass? param3)
        {
            var toBeDecoratedMock = new Mock<TSampleInterface>();

            object?[] actualArguments = null!;

            _decoratorMock
                .Setup(_ => _.MethodCalled(It.IsAny<MethodInfo>(), It.IsAny<object?[]>(), It.IsAny<object?>()))
                .Callback((MethodInfo _, object?[] args, object? _) => actualArguments = args);

            TSampleInterface decorated = DecoratorFactory<TSampleInterface>.Create(
                toBeDecoratedMock.Object,
                _decoratorMock.Object);

            _ = decorated.MethodWithParameters(param1, param2, param3);

            object?[] expectedArguments = new object?[] { param1, param2, param3 };

            actualArguments.Should().BeEquivalentTo(
                expectedArguments,
                config => config.WithStrictOrdering());
        }

        [Theory]
        [AutoData]
        public void Should_pass_decorated_method_arguments_to_MethodThrewException_method(
            int param1,
            string param2,
            SampleClass? param3)
        {
            var toBeDecoratedMock = new Mock<TSampleInterface>();
            toBeDecoratedMock
                .Setup(_ => _.MethodWithParameters(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<SampleClass?>()))
                .Throws<Exception>();

            object?[] actualArguments = null!;

            _decoratorMock
                .Setup(_ => _.MethodThrewException(It.IsAny<MethodInfo>(), It.IsAny<object?[]>(), It.IsAny<Exception>()))
                .Callback((MethodInfo _, object?[] args, Exception _) => actualArguments = args);

            TSampleInterface decorated = DecoratorFactory<TSampleInterface>.Create(
                toBeDecoratedMock.Object,
                _decoratorMock.Object);

            try
            {
                _ = decorated.MethodWithParameters(param1, param2, param3);
            }
            catch { }

            object?[] expectedArguments = new object?[] { param1, param2, param3 };

            actualArguments.Should().BeEquivalentTo(
                expectedArguments,
                config => config.WithStrictOrdering());
        }

        [Fact]
        public void Should_pass_empty_array_to_MethodCalling_method_called_with_no_arguments()
        {
            var toBeDecoratedMock = new Mock<TSampleInterface>();

            object?[] actualArguments = null!;

            _decoratorMock
                .Setup(_ => _.MethodCalling(It.IsAny<MethodInfo>(), It.IsAny<object?[]>()))
                .Callback((MethodInfo _, object?[] args) => actualArguments = args);

            TSampleInterface decorated = DecoratorFactory<TSampleInterface>.Create(
                toBeDecoratedMock.Object,
                _decoratorMock.Object);

            _ = decorated.Method();

            object?[] expectedArguments = new object?[0];

            actualArguments.Should().BeEquivalentTo(
                expectedArguments,
                config => config.WithStrictOrdering());
        }

        [Fact]
        public void Should_pass_empty_array_to_MethodCalled_method_called_with_no_arguments()
        {
            var toBeDecoratedMock = new Mock<TSampleInterface>();

            object?[] actualArguments = null!;

            _decoratorMock
                .Setup(_ => _.MethodCalled(It.IsAny<MethodInfo>(), It.IsAny<object?[]>(), It.IsAny<object?>()))
                .Callback((MethodInfo _, object?[] args, object? _) => actualArguments = args);

            TSampleInterface decorated = DecoratorFactory<TSampleInterface>.Create(
                toBeDecoratedMock.Object,
                _decoratorMock.Object);

            _ = decorated.Method();

            object?[] expectedArguments = new object?[0];

            actualArguments.Should().BeEquivalentTo(
                expectedArguments,
                config => config.WithStrictOrdering());
        }

        [Fact]
        public void Should_pass_empty_array_to_MethodThrewException_method_called_with_no_arguments()
        {
            var toBeDecoratedMock = new Mock<TSampleInterface>();
            toBeDecoratedMock
                .Setup(_ => _.Method())
                .Throws<Exception>();

            object?[] actualArguments = null!;

            _decoratorMock
                .Setup(_ => _.MethodThrewException(It.IsAny<MethodInfo>(), It.IsAny<object?[]>(), It.IsAny<Exception>()))
                .Callback((MethodInfo _, object?[] args, Exception _) => actualArguments = args);

            TSampleInterface decorated = DecoratorFactory<TSampleInterface>.Create(
                toBeDecoratedMock.Object,
                _decoratorMock.Object);

            try
            {
                _ = decorated.Method();
            }
            catch { }

            object?[] expectedArguments = new object?[0];

            actualArguments.Should().BeEquivalentTo(
                expectedArguments,
                config => config.WithStrictOrdering());
        }

        public class SampleClass
        {
        }

        public interface TSampleInterface
        {
            object Method();
            object MethodWithParameters(int param1, string param2, SampleClass? param3);
            void VoidMethod();
        }
    }
}
