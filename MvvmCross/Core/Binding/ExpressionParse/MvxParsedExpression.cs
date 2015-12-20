// MvxParsedExpression.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.ExpressionParse
{
    using System.Collections.Generic;
    using System.Text;

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
                this.PropertyName = propertyName;
            }

            public string PropertyName { get; private set; }

            public void AppendPrintTo(StringBuilder builder)
            {
                if (builder.Length > 0)
                    builder.Append(".");

                builder.Append(this.PropertyName);
            }
        }

        public class IndexedNode : INode
        {
            public IndexedNode(string indexValue)
            {
                this.IndexValue = indexValue;
            }

            public string IndexValue { get; private set; }

            public void AppendPrintTo(StringBuilder builder)
            {
                builder.AppendFormat("[{0}]", this.IndexValue);
            }
        }

        private readonly LinkedList<INode> _nodes = new LinkedList<INode>();

        protected LinkedList<INode> Nodes => this._nodes;

        protected void Prepend(INode node)
        {
            this._nodes.AddFirst(node);
        }

        public void PrependProperty(string propertyName)
        {
            this.Prepend(new PropertyNode(propertyName));
        }

        public void PrependIndexed(string indexedValue)
        {
            this.Prepend(new IndexedNode(indexedValue));
        }

        public string Print()
        {
            var output = new StringBuilder();
            foreach (var node in this.Nodes)
            {
                node.AppendPrintTo(output);
            }
            return output.ToString();
        }
    }
}