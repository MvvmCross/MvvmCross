namespace MvvmCross.Platform.Test
{
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class ReflectionExtensionsTests
    {
        private class TestClass
        {
            public static readonly object PublicStaticField = new object();

            public object PublicInstanceField1 = new object();
            public object PublicInstanceField2 = new object();

            private object _privateInstanceField1 = new object();

            public TestClass()
            {
            }

            public TestClass(object param1)
            {
            }
        }

        private class TestSubClass : TestBaseClass
        {
            public object PublicSubInstanceField1 = new object();

            private object _privateInstanceField1 = new object();
        }

        private class TestBaseClass
        {
            public object PublicBaseInstanceField1 = new object();
            public object PublicBaseInstanceField2 = new object();

            private object _privateBaseInstanceField1 = new object();

            public TestBaseClass()
            {
            }
        }

        #region Fields

        [Test]
        public void TestGetStaticFields()
        {
            // Act
            var fields = typeof(TestClass).GetFields(BindingFlags.Static);

            // Assert
            Assert.AreEqual(1, fields.Count());
        }

        [Test]
        public void TestGetPublicInstanceFields()
        {
            // Act
            var fields = typeof(TestClass).GetFields(BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.AreEqual(2, fields.Count());
        }

        [Test]
        public void TestGetAllInstanceFields()
        {
            // Act
            var fields = typeof(TestClass).GetFields(BindingFlags.Instance);

            // Assert
            Assert.AreEqual(3, fields.Count());
        }

        [Test]
        public void TestGetAllFields()
        {
            // Act
            var fields = typeof(TestClass).GetFields();

            // Assert
            Assert.AreEqual(3, fields.Count());
        }

        [Test]
        public void TestGetInstanceFieldByNameStaticFieldSpecified()
        {
            // Act
            var field = typeof(TestClass).GetField("PublicStaticField", BindingFlags.Public | BindingFlags.Static);

            // Assert
            Assert.IsNotNull(field);
        }

        [Test]
        public void TestGetInstanceFieldByNameReturnsNullIfStaticFieldSpecified()
        {
            // Act
            var field = typeof(TestClass).GetField("PublicStaticField", BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.IsNull(field);
        }

        [Test]
        public void TestGetInstanceFieldByNameInstanceFieldSpecified()
        {
            // Act
            var field = typeof(TestClass).GetField("PublicInstanceField1", BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.IsNotNull(field);
        }

        [Test]
        public void TestGetStaticFieldByNameReturnsNullIfInstanceFieldSpecified()
        {
            // Act
            var field = typeof(TestClass).GetField("PublicInstanceField1", BindingFlags.Public | BindingFlags.Static);

            // Assert
            Assert.IsNull(field);
        }

        [Test]
        public void TestFlattenHierarchyGetsAllFields()
        {
            // Act
            var fields = typeof(TestSubClass).GetFields(BindingFlags.Public | BindingFlags.FlattenHierarchy);

            // Assert
            Assert.AreEqual(3, fields.Count());
        }

        [Test]
        public void TestFlattenHierarchyGetsOnlyDeclaredFields()
        {
            // Act
            var fields = typeof(TestSubClass).GetFields(BindingFlags.Public);

            // Assert
            Assert.AreEqual(1, fields.Count());
        }

        #endregion Fields

        [Test]
        public void TestGetConstructors()
        {
            // Act
            var ctors = typeof(TestClass).GetConstructors();

            // Assert
            Assert.AreEqual(2, ctors.Count());
        }

        [Test]
        public void TestGetBaseConstructor()
        {
            // Act
            var ctors = typeof(TestSubClass).GetConstructors();

            // Assert
            Assert.AreEqual(1, ctors.Count());
        }
    }
}