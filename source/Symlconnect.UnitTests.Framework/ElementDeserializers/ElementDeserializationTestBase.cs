using FakeItEasy;
using NUnit.Framework;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.UnitTests.Framework.ElementDeserializers
{
    /// <summary>
    ///     Base class for element deserialization tests.
    /// </summary>
    /// <remarks>
    ///     Not hugely keen on class hierarchies in tests but this is simple enough and reduces a great deal of
    ///     duplication.
    /// </remarks>
    public abstract class ElementDeserializationTestBase<T, TRoot>
    {
        protected IElementDeserializer<TRoot> ElementDeserializer;

        protected IFactory<T> FakeFactory;

        protected virtual void ConfigureFakeFactory(IFactory<T> fakeFactory)
        {
        }

        protected abstract IElementDeserializer<TRoot> CreateElementDeserializerInstance();

        [SetUp]
        protected virtual void TestSetup()
        {
            FakeFactory = A.Fake<IFactory<T>>();
            ConfigureFakeFactory(FakeFactory);

            ElementDeserializer = CreateElementDeserializerInstance();
        }
    }
}