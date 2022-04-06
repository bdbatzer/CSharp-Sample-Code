using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpCodeSamples
{
    // Class: Tree
    //----------------------------------------------------------------------------------------------------------------------
    /// @brief Tree priority queue functions
    //----------------------------------------------------------------------------------------------------------------------
    public partial class Tree
    {
        // Function: SiftUp
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief Compares element at "child" with its parent in the priority queue and swaps them if the parent's cost is 
        /// greater and repeats until queue is in order and no more swaps are needed
        /// 
        /// @param child = Index of first element to check to make sure priority queue is in order
        //----------------------------------------------------------------------------------------------------------------------
        private void SiftUp(int child)
        {
            // Return early if first element in queue or invalid
            if (child <= 0) return;

            // Initialize 
            int parent = (child - 1) / 2;

            // Move up the queue swapping where appropriate
            while (child > 0 && Nodes[queue[parent]].CompareTo(Nodes[queue[child]]) > 0)
            {
                // Swap
                int tmp = queue[parent];
                queue[parent] = queue[child];
                queue[child] = tmp;

                // Keep nodes up to date
                Nodes[queue[child]].heapkey = child;
                Nodes[queue[parent]].heapkey = parent;

                // Next step
                child = parent;
                parent = (child - 1) / 2;
            }
        }

        // Function: SiftDown
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief Compares element at "parent" with its children in the priority queue and swaps them if the child's cost is 
        /// less and repeats until queue is in order
        /// 
        /// @param parent = Index of first element to check to make sure priority queue is in order
        //----------------------------------------------------------------------------------------------------------------------
        private void SiftDown(int parent)
        {
            // If invalid return early
            if (parent <= -1) return;

            // Initialize
            int child;
            if (2 * parent + 1 == queue.Count - 1)
            {
                child = 2 * parent + 1;
            }
            else if (2 * parent + 2 > queue.Count - 1)
            {
                return;
            }
            else if (Nodes[queue[2 * parent + 1]].CompareTo(Nodes[queue[2 * parent + 2]]) < 0)
            {
                child = 2 * parent + 1;
            }
            else
            {
                child = 2 * parent + 2;
            }

            // Move down the queue swapping where appropriate
            while (parent < queue.Count && Nodes[queue[child]].CompareTo(Nodes[queue[parent]]) < 0)
            {
                // Swap
                int tmp = queue[child];
                queue[child] = queue[parent];
                queue[parent] = tmp;

                // Keep nodes up to date
                Nodes[queue[child]].heapkey = child;
                Nodes[queue[parent]].heapkey = parent;

                // Next step
                parent = child;
                if (2 * parent + 1 == queue.Count - 1)
                {
                    child = 2 * parent + 1;
                }
                else if (2 * parent + 2 > queue.Count - 1)
                {
                    return;
                }
                else if (Nodes[queue[2 * parent + 1]].CompareTo(Nodes[queue[2 * parent + 2]]) < 0)
                {
                    child = 2 * parent + 1;
                }
                else
                {
                    child = 2 * parent + 2;
                }
            }
        }

        // Function: Push to Priority Queue 
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief Adds a node to the queue maintaining priority order
        /// 
        /// @param ID = ID # of node to be added to queue 
        //----------------------------------------------------------------------------------------------------------------------
        public void PushPQ(int ID)
        {
            queue.Add(ID);
            int last = queue.Count - 1;
            Nodes[ID].heapkey = last; // Give this node an index to start with in the queue
            SiftUp(last);
        }

        // Function: Pop from Priority Queue 
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief Pops the ID of the lowest cost element of the queue
        /// 
        /// @returns The ID of the lowest cost node from the queue
        //----------------------------------------------------------------------------------------------------------------------
        public int PopPQ()
        {
            int frontNode;
            // If only one element left in queue we don't want the usual step of setting the last element to the front of
            // the queue as this will set a node's priority queue index to 0 when it should be -1 signifying it is no longer
            // in the queue 
            if (queue.Count == 1)
            {
                Nodes[queue[0]].heapkey = -1;
                frontNode = queue[0];
                queue.RemoveAt(0);
            }
            // More than one element in the queue so proceed normally
            else
            {
                int last = queue.Count - 1;
                Nodes[queue[0]].heapkey = -1; // Removing means we need to reset priority queue index for this node to -1
                Nodes[queue[last]].heapkey = 0; // Set last element to front of queue and then bubble it back down
                frontNode = queue[0];
                queue[0] = queue[last];
                queue.RemoveAt(last);
                SiftDown(0);
            }

            return frontNode;
        }

        // Function: Remove from Priority Queue
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief Removes an element from the queue with the specified ID
        /// 
        /// @param ID = ID # of node to be removed from the queue
        //----------------------------------------------------------------------------------------------------------------------
        public void RemovePQ(int ID)
        {
            int last = queue.Count - 1;
            int idx = Nodes[ID].heapkey;
            queue[idx] = queue[last];
            Nodes[queue[idx]].heapkey = idx;
            Nodes[ID].heapkey = -1;
            ID = queue[idx];
            queue.RemoveAt(last);
            SiftUp(idx);
            idx = Nodes[ID].heapkey;
            SiftDown(idx);
        }

        // Function: Update in Priority Queue 
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief Updates an element's position in the queue
        /// 
        /// @param ID = Node ID of node to be updated in queue
        //----------------------------------------------------------------------------------------------------------------------
        public void UpdatePQ(int ID)
        {
            int idx = Nodes[ID].heapkey;
            SiftUp(idx);
            idx = Nodes[ID].heapkey;
            SiftDown(idx);
        }

        // Function: PrioQSize
        //----------------------------------------------------------------------------------------------------------------------
        /// @brief Priority Queue Size
        /// 
        /// @return The # of elements in the queue 
        //----------------------------------------------------------------------------------------------------------------------
        public int PrioQSize()
        {
            return queue.Count;
        }
    }
}
