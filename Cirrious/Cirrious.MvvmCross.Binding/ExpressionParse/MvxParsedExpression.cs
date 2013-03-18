using System.Collections.Generic;
using System.Text;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public class MvxParsedExpression : IMvxParsedExpression
    {
        public interface IBaseNode
        {
            void AppendPrintTo(StringBuilder builder);
        }

        public class PropertyNode : IBaseNode
        {
            public PropertyNode(string propertyName)
            {
                PropertyName = propertyName;
            }

            public string PropertyName { get; private set; }

            public void AppendPrintTo(StringBuilder builder)
            {
                if (builder.Length > 0)
                    builder.Append(".");

                builder.Append(PropertyName);
            }
        }

        public class IndexedNode : IBaseNode
        {
            public IndexedNode(string indexValue)
            {
                IndexValue = indexValue;
            }

            public string IndexValue { get; private set; }

            public void AppendPrintTo(StringBuilder builder)
            {
                builder.AppendFormat("[{0}]", IndexValue);
            }
        }

        private readonly LinkedList<IBaseNode> _nodes = new LinkedList<IBaseNode>();

        protected LinkedList<IBaseNode> Nodes
        {
            get { return _nodes; }
        }

        protected void Prepend(IBaseNode node)
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