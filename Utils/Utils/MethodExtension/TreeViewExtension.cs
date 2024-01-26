using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.MethodExtension
{
    /// <summary>
    /// Diese statische Klasse enthält erweiterte Methoden für <see cref="TreeView"/>.
    /// </summary>
    public static class TreeViewExtension
    {
        /// <summary>
        /// Erweitert einen Knoten bis zu einem bestimmtem Pfad
        /// </summary>
        /// <param name="tv"></param>
        /// <param name="fullPath">Pfad, bis zu der die Knoten geöffnet werden sollen</param>
        public static void Expand(this TreeView tv, string fullPath)
        {
            // fullPath = @"A\1\2";
            List<string> path_list = fullPath.Split('\\').ToList();
            foreach (TreeNode node in tv.Nodes)
            {
                if (node.Text == path_list[0])
                {
                    ExpandNode(node, path_list);
                }
            }
        }

        private static void ExpandNode(TreeNode node, List<string> path)
        {
            path.RemoveAt(0);
            node.Expand();

            if (path.Count == 0) return;

            foreach (TreeNode myNode in node.Nodes)
            {
                if (myNode.Text == path[0])
                {
                    ExpandNode(myNode, path); // rekuriver Aufruf
                    break;
                }
            }
        }

    }
}
