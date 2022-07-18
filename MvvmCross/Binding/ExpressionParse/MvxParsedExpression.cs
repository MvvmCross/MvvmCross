// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text;

namespace MvvmCross.Binding.ExpressionParse
{
    public class MvxParsedExpression : IMvxParsedExpression
    {
        public interface INode
        {
            void AppendPrintTo(StringBuilder builder);
        }

        public class PropertyNode : INode
        {
            public PropertyNode(string propertyName)
            {
                PropertyName = propertyName;
            }

            public string PropertyName { get; }

            public void AppendPrintTo(StringBuilder builder)
            {
                if (builder.Length > 0)
                    builder.Append(".");

                builder.Append(PropertyName);
            }
        }

        public class IndexedNode : INode
        {
            public IndexedNode(string indexValue)
            {
                IndexValue = indexValue;
            }

            public string IndexValue { get; }

            public void AppendPrintTo(StringBuilder builder)
            {
                builder.AppendFormat("[{0}]", IndexValue);
            }
        }

        private readonly LinkedList<INode> _nodes = new LinkedList<INode>();

        protected LinkedList<INode> Nodes => _nodes;

        protected void Prepend(INode node)
        {
            _nodes.AddFirst(node);
        }

        public void PrependProperty(string propertyName)
        {
            Prepend(new PropertyNode(propertyName));
        }

        public void PrependIndexed(string indexedValue)
        {
            Prepend(new IndexedNode(indexedValue));
        }

        public string Print()
        {
            var output = new StringBuilder();
            foreach (var node in Nodes)
            {
                node.AppendPrintTo(output);
            }
            return output.ToString();
        }
    }
}
