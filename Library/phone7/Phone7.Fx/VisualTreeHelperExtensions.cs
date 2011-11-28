using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Phone7.Fx
{
    public static class VisualTreeHelperExtensions
    {
        /// <summary>
        /// Equivalent of FindName, but works on the visual tree to go through templates, etc.
        /// </summary>
        /// <param name="root">The node to search from</param>
        /// <param name="name">The name to look for</param>
        /// <returns>The found node, or null if not found</returns>
        public static FrameworkElement FindVisualChild(this FrameworkElement root, string name)
        {
            FrameworkElement temp = root.FindName(name) as FrameworkElement;
            if (temp != null)
                return temp;

            foreach (FrameworkElement element in root.GetVisualChildren())
            {
                temp = element.FindName(name) as FrameworkElement;
                if (temp != null)
                    return temp;
            }

            return null;
        }

        /// <summary>
        /// Gets the visual parent of the element
        /// </summary>
        /// <param name="node">The element to check</param>
        /// <returns>The visual parent</returns>
        public static FrameworkElement GetVisualParent(this FrameworkElement node)
        {
            return VisualTreeHelper.GetParent(node) as FrameworkElement;
        }

        /// <summary>
        /// Gets a visual child of the element
        /// </summary>
        /// <param name="node">The element to check</param>
        /// <param name="index">The index of the child</param>
        /// <returns>The found child</returns>
        public static FrameworkElement GetVisualChild(this FrameworkElement node, int index)
        {
            return VisualTreeHelper.GetChild(node, index) as FrameworkElement;
        }

        /// <summary>
        /// Gets all the visual children of the element
        /// </summary>
        /// <param name="root">The element to get children of</param>
        /// <returns>An enumerator of the children</returns>
        public static IEnumerable<FrameworkElement> GetVisualChildren(this FrameworkElement root)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
                yield return VisualTreeHelper.GetChild(root, i) as FrameworkElement;
        }

        /// <summary>
        /// Gets the ancestors of the element, up to the root
        /// </summary>
        /// <param name="node">The element to start from</param>
        /// <returns>An enumerator of the ancestors</returns>
        public static IEnumerable<FrameworkElement> GetVisualAncestors(this FrameworkElement node)
        {
            FrameworkElement parent = node.GetVisualParent();
            while (parent != null)
            {
                yield return parent;
                parent = parent.GetVisualParent();
            }
        }

        /// <summary>
        /// Gets the VisualStateGroup with the given name, looking up the visual tree
        /// </summary>
        /// <param name="root">Element to start from</param>
        /// <param name="groupName">Name of the group to look for</param>
        /// <param name="searchAncestors">Whether or not to look up the tree</param>
        /// <returns>The group, if found</returns>
        public static VisualStateGroup GetVisualStateGroup(this FrameworkElement root, string groupName, bool searchAncestors)
        {
            IList groups = VisualStateManager.GetVisualStateGroups(root);
            foreach (object o in groups)
            {
                VisualStateGroup group = o as VisualStateGroup;
                if (group != null && group.Name == groupName)
                    return group;
            }

            if (searchAncestors)
            {
                FrameworkElement parent = root.GetVisualParent();
                if (parent != null)
                    return parent.GetVisualStateGroup(groupName, true);
            }

            return null;
        }

        /// <summary>
        /// Finds the VisualStateGroup with the given name
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static VisualStateGroup FindVisualState(this FrameworkElement root, string name)
        {
            if (root == null)
                return null;

            IList groups = VisualStateManager.GetVisualStateGroups(root);
            return groups.Cast<VisualStateGroup>().FirstOrDefault(group => group.Name == name);
        }

        /// <summary>
        /// Performs a breadth-first enumeration of all the descendents in the tree
        /// </summary>
        /// <param name="root">The root node</param>
        /// <returns>An enumerator of all the children</returns>
        public static IEnumerable<FrameworkElement> GetVisualDescendents(this FrameworkElement root)
        {
            Queue<IEnumerable<FrameworkElement>> toDo = new Queue<IEnumerable<FrameworkElement>>();

            toDo.Enqueue(root.GetVisualChildren());
            while (toDo.Count > 0)
            {
                IEnumerable<FrameworkElement> children = toDo.Dequeue();
                foreach (FrameworkElement child in children)
                {
                    yield return child;
                    toDo.Enqueue(child.GetVisualChildren());
                }
            }
        }

        /// <summary>
        /// Provides a debug string that represents the visual child tree
        /// </summary>
        /// <param name="root">The root node</param>
        /// <param name="result">StringBuilder into which the text is appended</param>
        /// <remarks>This method only works in DEBUG mode</remarks>
        [Conditional("DEBUG")]
        public static void GetVisualChildTreeDebugText(this FrameworkElement root, StringBuilder result)
        {
            List<string> results = new List<string>();
            root.GetChildTree("", "  ", results);
            foreach (string s in results)
                result.AppendLine(s);
        }

        private static void GetChildTree(this FrameworkElement root, string prefix, string addPrefix, List<string> results)
        {
            string thisElement = "";
            if (String.IsNullOrEmpty(root.Name))
                thisElement = "[Anonymous]";
            else
                thisElement = string.Format("[{0}]", root.Name);

            thisElement += string.Format(" : {0}", root.GetType().Name);

            results.Add(prefix + thisElement);
            foreach (FrameworkElement directChild in root.GetVisualChildren())
            {
                directChild.GetChildTree(prefix + addPrefix, addPrefix, results);
            }
        }

        /// <summary>
        /// Provides a debug string that represents the visual child tree
        /// </summary>
        /// <param name="node">The root node</param>
        /// <param name="result">StringBuilder into which the text is appended</param>
        /// <remarks>This method only works in DEBUG mode</remarks>
        [Conditional("DEBUG")]
        public static void GetAncestorVisualTreeDebugText(this FrameworkElement node, StringBuilder result)
        {
            List<string> tree = new List<string>();
            node.GetAncestorVisualTree(tree);
            string prefix = "";
            foreach (string s in tree)
            {
                result.AppendLine(prefix + s);
                prefix = prefix + "  ";
            }
        }

        private static void GetAncestorVisualTree(this FrameworkElement node, List<string> children)
        {
            string name = String.IsNullOrEmpty(node.Name) ? "[Anon]" : node.Name;
            string thisNode = name + ": " + node.GetType().Name;

            // Ensure list is in reverse order going up the tree
            children.Insert(0, thisNode);
            FrameworkElement parent = node.GetVisualParent();
            if (parent != null)
                GetAncestorVisualTree(parent, children);
        }

        /// <summary>
        /// Returns a render transform of the specified type from the element, creating it if necessary
        /// </summary>
        /// <typeparam name="TRequestedTransform">The type of transform (Rotate, Translate, etc)</typeparam>
        /// <param name="element">The element to check</param>
        /// <param name="mode">The mode to use for creating transforms, if not found</param>
        /// <returns>The specified transform, or null if not found and not created</returns>
        public static TRequestedTransform GetTransform<TRequestedTransform>(this UIElement element, TransformCreationMode mode) where TRequestedTransform : Transform, new()
        {
            Transform originalTransform = element.RenderTransform;
            TRequestedTransform requestedTransform = null;
            MatrixTransform matrixTransform = null;
            TransformGroup transformGroup = null;

            // Current transform is null -- create if necessary and return
            if (originalTransform == null)
            {
                if ((mode & TransformCreationMode.Create) == TransformCreationMode.Create)
                {
                    requestedTransform = new TRequestedTransform();
                    element.RenderTransform = requestedTransform;
                    return requestedTransform;
                }

                return null;
            }

            // Transform is exactly what we want -- return it
            requestedTransform = originalTransform as TRequestedTransform;
            if (requestedTransform != null)
                return requestedTransform;


            // The existing transform is matrix transform - overwrite if necessary and return
            matrixTransform = originalTransform as MatrixTransform;
            if (matrixTransform != null)
            {
                if (matrixTransform.Matrix.IsIdentity
                  && (mode & TransformCreationMode.Create) == TransformCreationMode.Create
                  && (mode & TransformCreationMode.IgnoreIdentityMatrix) == TransformCreationMode.IgnoreIdentityMatrix)
                {
                    requestedTransform = new TRequestedTransform();
                    element.RenderTransform = requestedTransform;
                    return requestedTransform;
                }

                return null;
            }

            // Transform is actually a group -- check for the requested type
            transformGroup = originalTransform as TransformGroup;
            if (transformGroup != null)
            {
                foreach (Transform child in transformGroup.Children)
                {
                    // Child is the right type -- return it
                    if (child is TRequestedTransform)
                        return child as TRequestedTransform;
                }

                // Right type was not found, but we are OK to add it
                if ((mode & TransformCreationMode.AddToGroup) == TransformCreationMode.AddToGroup)
                {
                    requestedTransform = new TRequestedTransform();
                    transformGroup.Children.Add(requestedTransform);
                    return requestedTransform;
                }

                return null;
            }

            // Current ransform is not a group and is not what we want;
            // create a new group containing the existing transform and the new one
            if ((mode & TransformCreationMode.CombineIntoGroup) == TransformCreationMode.CombineIntoGroup)
            {
                transformGroup = new TransformGroup();
                transformGroup.Children.Add(originalTransform);
                transformGroup.Children.Add(requestedTransform);
                element.RenderTransform = transformGroup;
                return requestedTransform;
            }

            Debug.Assert(false, "Shouldn't get here");
            return null;
        }

        /// <summary>
        /// Returns a string representation of a property path needed to update a Storyboard
        /// </summary>
        /// <param name="element">The element to get the path for</param>
        /// <param name="subProperty">The property of the transform</param>
        /// <typeparam name="TRequestedType">The type of transform to look fo</typeparam>
        /// <returns>A property path</returns>
        public static string GetTransformPropertyPath<TRequestedType>(this FrameworkElement element, string subProperty) where TRequestedType : Transform
        {
            Transform t = element.RenderTransform;
            if (t is TRequestedType)
                return String.Format("(RenderTransform).({0}.{1})", typeof(TRequestedType).Name, subProperty);

            else if (t is TransformGroup)
            {
                TransformGroup g = t as TransformGroup;
                for (int i = 0; i < g.Children.Count; i++)
                {
                    if (g.Children[i] is TRequestedType)
                        return String.Format("(RenderTransform).(TransformGroup.Children)[" + i + "].({0}.{1})",
                          typeof(TRequestedType).Name, subProperty);
                }
            }

            return "";
        }

        /// <summary>
        /// Returns a plane projection, creating it if necessary
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="create">Whether or not to create the projection if it doesn't already exist</param>
        /// <returns>The plane project, or null if not found / created</returns>
        public static PlaneProjection GetPlaneProjection(this UIElement element, bool create)
        {
            Projection originalProjection = element.Projection;
            PlaneProjection projection = null;

            // Projection is already a plane projection; return it
            if (originalProjection is PlaneProjection)
                return originalProjection as PlaneProjection;

            // Projection is null; create it if necessary
            if (originalProjection == null)
            {
                if (create)
                {
                    projection = new PlaneProjection();
                    element.Projection = projection;
                }
            }

            // Note that if the project is a Matrix projection, it will not be
            // changed and null will be returned.
            return projection;
        }
    }

    /// <summary>
    /// Possible modes for creating a transform
    /// </summary>
    [Flags]
    public enum TransformCreationMode
    {
        /// <summary>
        /// Don't try and create a transform if it doesn't already exist
        /// </summary>
        None = 0,

        /// <summary>
        /// Create a transform if none exists
        /// </summary>
        Create = 1,

        /// <summary>
        /// Create and add to an existing group
        /// </summary>
        AddToGroup = 2,

        /// <summary>
        /// Create a group and combine with existing transform; may break existing animations
        /// </summary>
        CombineIntoGroup = 4,

        /// <summary>
        /// Treat identity matrix as if it wasn't there; may break existing animations
        /// </summary>
        IgnoreIdentityMatrix = 8,

        /// <summary>
        /// Create a new transform or add to group
        /// </summary>
        CreateOrAddAndIgnoreMatrix = Create | AddToGroup | IgnoreIdentityMatrix,

        /// <summary>
        /// Default behaviour, equivalent to CreateOrAddAndIgnoreMatrix
        /// </summary>
        Default = CreateOrAddAndIgnoreMatrix,
    }
}