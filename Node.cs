using System;
using System.Collections.Generic;

namespace CSharpCodeSamples
{

    public class Node : IComparable
    {
        //  Property: ID
        //------------------------------------------------------------------------
        /// @brief ID number for the node
        //------------------------------------------------------------------------
        public int ID { get; private set; }

        //  Member: state
        //------------------------------------------------------------------------
        /// @brief State for the node
        //------------------------------------------------------------------------
        public Vector State { get; private set; }

        //  Member: cost
        //------------------------------------------------------------------------
        /// @brief Cost for the node
        //------------------------------------------------------------------------
        public double cost;

        //  Member: Parent
        //------------------------------------------------------------------------
        /// @brief ID of the parent node
        //------------------------------------------------------------------------
        public int parent;

        //  Member: children
        //------------------------------------------------------------------------
        /// @brief Children of the node
        //------------------------------------------------------------------------
        public List<int> children;

        //  Member: heapkey
        //------------------------------------------------------------------------
        /// @brief Heap key for priority queue of the node
        //------------------------------------------------------------------------
        public int heapkey;

        //  Member: Splitting dimension
        //------------------------------------------------------------------------
        /// @brief Splitting dimension for node in KD tree
        //------------------------------------------------------------------------
        public int splitDim;

        //  Member: Parent KD
        //------------------------------------------------------------------------
        /// @brief Parent of node in KD tree
        //------------------------------------------------------------------------
        public int parentKD;

        //  Member: Right KD
        //------------------------------------------------------------------------
        /// @brief Right child of node in KD tree
        //------------------------------------------------------------------------
        public int rightKD;

        //  Property: Left KD
        //------------------------------------------------------------------------
        /// @brief Left child of node in KD tree
        //------------------------------------------------------------------------
        public int leftKD;

        //  Constructor: Node
        //------------------------------------------------------------------------
        /// @brief Node class for search and graphing problems
        /// 
        /// @param id The identification number used for this node
        /// @param position The positional information for this node
        //------------------------------------------------------------------------
        public Node(int id, in List<double> position)
        {
            ID = id;
            State = new Vector(position);
            
            // node cost
            cost = 0;
            parent = -1;
            children = new List<int>();

            // priority queue 
            heapkey = -1;

            // kd tree
            splitDim = 0;
            parentKD = -1;
            rightKD = -1;
            leftKD = -1;
        }

        //  Constructor: Node
        //------------------------------------------------------------------------
        /// @brief Node class for search and graphing problems
        /// 
        /// @param id The identification number used for this node
        /// @param position The positional information for this node
        //------------------------------------------------------------------------
        public Node(int id, in Vector position)
        {
            ID = id;
            State = new Vector(position);

            // node cost
            cost = 0;
            children = new List<int>();

            // priority queue 
            heapkey = -1;

            // kd tree
            splitDim = 0;
            parentKD = -1;
            rightKD = -1;
            leftKD = -1;
        }

        //  Function: CompareTo
        //------------------------------------------------------------------------
        /// @brief Compares the costs of two Node objects
        /// 
        /// @param obj The other node object to compare
        /// 
        /// @return -1 if this node's cost is less than, 0 if equal, and 1 if 
        /// greater than
        //------------------------------------------------------------------------
        public int CompareTo(Object obj)
        {
            Node state = (Node)obj;

            if (Math.Abs(cost - state.cost) <= 1e-6) return 0;
            return cost < state.cost ? -1 : 1;
        }
    }
}
