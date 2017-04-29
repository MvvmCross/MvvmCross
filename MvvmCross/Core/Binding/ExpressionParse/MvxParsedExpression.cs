// MvxParsedExpression.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Text;

namespace MvvmCross.Binding.ExpressionParse
{
    public class MvxParsedExpression : IMvxParsedExpression
    {
        protected LinkedList<INode> Nodes { get; } = new LinkedList<INode>();

        public string Print()
        {
            var output = new StringBuilder();
            foreach (var node in Nodes)
                node.AppendPrintTo(output);
            return output.ToString();
        }

        protected void Prepend(INode node)
        {
            Nodes.AddFirst(node);
        }

        public void PrependProperty(string propertyName)
        {
            Prepend(new PropertyNode(propertyName));
        }

        public void PrependIndexed(string indexedValue)
        {
            Prepend(new IndexedNode(indexedValue));
        }

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
    }
}