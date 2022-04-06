using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpCodeSamples
{
    // Class: Tree
    //----------------------------------------------------------------------------------------------------------------------
    /// @brief Tree core functions
    //----------------------------------------------------------------------------------------------------------------------
    public partial class Tree
    {
        // Constant: DIMS
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief The dimensionality of the state vector 
        //----------------------------------------------------------------------------------------------------------------------
        public const int DIMS = 3;

        // Constant: MAX SIZE
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief The max size of the tree
        //----------------------------------------------------------------------------------------------------------------------
        public const int MAX_SIZE = 50000;

        // Member: Nodes
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief Nodes in the graph
        //----------------------------------------------------------------------------------------------------------------------
        public Dictionary<int, Node> Nodes;

        // Member: Origin
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief The ID of the origin node for the tree
        //----------------------------------------------------------------------------------------------------------------------
        public int origin;

        // Member: Queue
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief The list of node IDs that are currently contained in the priority queue
        //----------------------------------------------------------------------------------------------------------------------
        private List<int> queue;

        // Member: K-D Root
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief The ID of the root of the kd tree 
        //----------------------------------------------------------------------------------------------------------------------
        public int kdRoot;

        // Member: Nodes to Reinsert
        //-----------------------------------------------------------------------------------
        /// @brief The list of nodes that need to be reinserted into tree after deletion
        //-----------------------------------------------------------------------------------
        public List<int> nodesToReinsert;

        // Member: Nearby Nodes
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief The closest nodes found with a range search
        //----------------------------------------------------------------------------------------------------------------------
        public HashSet<int> nearbyNodes;

        // Member: Nearest Node
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief The nearest node found in a nearest node search
        //----------------------------------------------------------------------------------------------------------------------
        public int nearestNode;

        // Member: Nearest Distance
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief The distance to the nearest node
        //----------------------------------------------------------------------------------------------------------------------
        public double nearestDistance;

        //  Constructor: Tree
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief Tree data structure class
        //----------------------------------------------------------------------------------------------------------------------
        public Tree()
        {
            Nodes = new Dictionary<int, Node>();

            // priority queue 
            queue = new List<int>();

            // kd tree
            nearbyNodes = new HashSet<int>();
            nearestNode = -1;
            nearestDistance = Double.PositiveInfinity;
        }

        //  Function: RemoveBranch
        //-----------------------------------------------------------------------------------
        /// @brief Recursively removes a branch of the tree starting at the given node
        /// 
        /// @param node The node to remove
        //-----------------------------------------------------------------------------------
        void RemoveBranch(int node)
        {
            if (node < 0) return;

            if (Nodes[node].ID == origin)
            {
                Nodes.Clear();
                return;
            }

            // Recursively delete children 
            HashSet<int> children = new HashSet<int>(Nodes[node].children);
            foreach (int c in children)
            {
                RemoveBranch(c);
            }

            // Having removed children - now delete this node
            RemoveKD(node);
            //ReconstructKD(node);
            Nodes[Nodes[node].parent].children.Remove(node);
            Nodes.Remove(node);
        }

        // Function: Reset
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief Resets the graph
        //----------------------------------------------------------------------------------------------------------------------
        public void Reset()
        {
            Nodes.Clear();
            queue.Clear();
            nearestNode = -1;
            nearestDistance = Double.PositiveInfinity;
            nearbyNodes.Clear();
        }

        // Function: Size
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief Size
        /// 
        /// @return The # of nodes in the tree
        //----------------------------------------------------------------------------------------------------------------------
        public int Size()
        {
            return Nodes.Count;
        }
    }
}
