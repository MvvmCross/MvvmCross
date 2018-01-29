// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Reflection;
using Xunit;

namespace MvvmCross.Platform.Test
{
    
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

        [Fact]
        public void TestGetStaticFields()
        {
            // Act
            var fields = typeof(TestClass).GetFields(BindingFlags.Static);

            // Assert
            Assert.Equal(1, fields.Count());
        }

        [Fact]
        public void TestGetPublicInstanceFields()
        {
            // Act
            var fields = typeof(TestClass).GetFields(BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.Equal(2, fields.Count());
        }

        [Fact]
        public void TestGetAllInstanceFields()
        {
            // Act
            var fields = typeof(TestClass).GetFields(BindingFlags.Instance);

            // Assert
            Assert.Equal(3, fields.Count());
        }

        [Fact]
        public void TestGetAllFields()
        {
            // Act
            var fields = typeof(TestClass).GetFields();

            // Assert
            Assert.Equal(3, fields.Count());
        }

        [Fact]
        public void TestGetInstanceFieldByNameStaticFieldSpecified()
        {
            // Act
            var field = typeof(TestClass).GetField("PublicStaticField", BindingFlags.Public | BindingFlags.Static);

            // Assert
            Assert.IsNotNull(field);
        }

        [Fact]
        public void TestGetInstanceFieldByNameReturnsNullIfStaticFieldSpecified()
        {
            // Act
            var field = typeof(TestClass).GetField("PublicStaticField", BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.IsNull(field);
        }

        [Fact]
        public void TestGetInstanceFieldByNameInstanceFieldSpecified()
        {
            // Act
            var field = typeof(TestClass).GetField("PublicInstanceField1", BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.IsNotNull(field);
        }

        [Fact]
        public void TestGetStaticFieldByNameReturnsNullIfInstanceFieldSpecified()
        {
            // Act
            var field = typeof(TestClass).GetField("PublicInstanceField1", BindingFlags.Public | BindingFlags.Static);

            // Assert
            Assert.IsNull(field);
        }

        [Fact]
        public void TestFlattenHierarchyGetsAllFields()
        {
            // Act
            var fields = typeof(TestSubClass).GetFields(BindingFlags.Public | BindingFlags.FlattenHierarchy);

            // Assert
            Assert.Equal(3, fields.Count());
        }

        [Fact]
        public void TestFlattenHierarchyGetsOnlyDeclaredFields()
        {
            // Act
            var fields = typeof(TestSubClass).GetFields(BindingFlags.Public);

            // Assert
            Assert.Equal(1, fields.Count());
        }

        #endregion Fields

        [Fact]
        public void TestGetConstructors()
        {
            // Act
            var ctors = typeof(TestClass).GetConstructors();

            // Assert
            Assert.Equal(2, ctors.Count());
        }

        [Fact]
        public void TestGetBaseConstructor()
        {
            // Act
            var ctors = typeof(TestSubClass).GetConstructors();

            // Assert
            Assert.Equal(1, ctors.Count());
        }
    }
}
