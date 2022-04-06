using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpCodeSamples
{
	// Class: Tree
	//----------------------------------------------------------------------------------------------------------------------
	/// @brief Tree kd tree functions
	//----------------------------------------------------------------------------------------------------------------------
	public partial class Tree
    {
		//  Function: Nearest
		//-----------------------------------------------------------------------------------
		/// @brief Brute force method of finding nearest node to state
		/// 
		/// @param state The state that we're trying to find the nearest node to
		//-----------------------------------------------------------------------------------
		public void Nearest(in Vector state)
		{
			nearestDistance = Double.PositiveInfinity;
			foreach (var node in Nodes)
			{
				double dist = (state - node.Value.State).Norm();
				if (dist < nearestDistance)
				{
					nearestNode = node.Key;
					nearestDistance = dist;
				}
			}
		}

		//  Function: RangeSearch
		//-----------------------------------------------------------------------------------
		/// @brief Brute force method of finding all nodes within a given range
		/// 
		/// @param state The state that we're trying to find the nodes around
		/// @param range The range to add nodes around state
		//-----------------------------------------------------------------------------------
		public void RangeSearch(in Vector state, double range)
		{
			nearbyNodes.Clear();
			foreach (var node in Nodes)
			{
				double dist = (node.Value.State - state).Norm();
				if (dist <= range) nearbyNodes.Add(node.Key);
			}
		}

		//  Function: InsertKD
		//-----------------------------------------------------------------------------------
		/// @brief Insert a node into the KD tree
		/// 
		/// @param node The node we are trying to add
		/// @param root The root of the subtree that we're inserting into
		//-----------------------------------------------------------------------------------
		public void InsertKD(int node, int root)
		{
			if (Nodes.Count == 1)
			{
				kdRoot = node;
				Nodes[node].splitDim = 0;
				return;
			}

			int count = 0;
			int currDim = 0;
			while (count < MAX_SIZE)
			{
				currDim = Nodes[root].splitDim;
				if (Nodes[node].State[currDim] < Nodes[root].State[currDim])
				{
					if (Nodes[root].leftKD == -1)
					{
						Nodes[root].leftKD = node;
						break;
					}
					root = Nodes[root].leftKD;
				}
				else
				{
					if (Nodes[root].rightKD == -1)
					{
						Nodes[root].rightKD = node;
						break;
					}
					root = Nodes[root].rightKD;
				}
				count++;
			}
			Nodes[node].parentKD = root;
			currDim = (currDim + 1) % DIMS;
			Nodes[node].splitDim = currDim;
		}

		//  Function: ReconstructKD
		//-----------------------------------------------------------------------------------
		/// @brief Remove a node from the KD tree
		/// 
		/// @param root The node to be removed
		//-----------------------------------------------------------------------------------
		public void ReconstructKD(int root)
		{
			nodesToReinsert.Clear();
			AddNodesInSubtree(Nodes[root].rightKD);
			AddNodesInSubtree(Nodes[root].leftKD);

			int newRoot = Nodes[root].parentKD;

			// If true then we deleted the root of the kd tree
			if (newRoot == -1)
			{
				newRoot = Nodes[nodesToReinsert[0]].ID;
				nodesToReinsert.RemoveAt(0);
				Nodes[newRoot].splitDim = 0;
				kdRoot = newRoot;
			}
			else
			{
				if (Nodes[newRoot].leftKD != -1 && Nodes[newRoot].leftKD == root) Nodes[newRoot].leftKD = -1;
				else if (Nodes[newRoot].rightKD != -1 && Nodes[newRoot].rightKD == root) Nodes[newRoot].rightKD = -1;
			}

			foreach (int ID in nodesToReinsert) InsertKD(ID, newRoot);
		}

		//  Function: RemoveKD
		//-----------------------------------------------------------------------------------
		/// @brief Remove a node from the KD tree
		/// 
		/// @param root The node to be removed
		//-----------------------------------------------------------------------------------
		public void RemoveKD(int root)
		{
			if (Nodes[root].parentKD != -1)
			{
				if (Nodes[Nodes[root].parentKD].rightKD != -1 && Nodes[Nodes[root].parentKD].rightKD == root)
				{
					Node n = Nodes[Nodes[root].parentKD];
					n.rightKD = DeleteInSubtree(root, ref n, root);
					Nodes[Nodes[root].parentKD] = n;
				}
				else if (Nodes[Nodes[root].parentKD].leftKD != -1 && Nodes[Nodes[root].parentKD].leftKD == root)
				{
					Node n = Nodes[Nodes[root].parentKD];
					n.leftKD = DeleteInSubtree(root, ref n, root);
					Nodes[Nodes[root].parentKD] = n;
				}
			}
			else
			{
				Node n = new Node(-1, new Vector(0));
				kdRoot = DeleteInSubtree(root, ref n, root);
			}
		}

		//  Function: NearestKD
		//-----------------------------------------------------------------------------------
		/// @brief KD tree method of finding nearest node to state
		/// 
		/// @param state The state that we're trying to find the nearest node to
		/// @param root The root of the subtree that we're searching within
		//-----------------------------------------------------------------------------------
		public void NearestKD(in Vector state, int root)
		{
			nearestNode = root;
			nearestDistance = Double.PositiveInfinity;
			FindNearestInSubtree(state, root);
		}

		//  Function: RangeSearchKD
		//-----------------------------------------------------------------------------------
		/// @brief KD tree method of finding all nodes within a given range
		/// 
		/// @param state The state that we're trying to find the nodes around
		/// @param range The range to add nodes around state
		/// @param root The root of the subtree that we're searching within
		//-----------------------------------------------------------------------------------
		public void RangeSearchKD(in Vector state, double range, int root)
		{
			nearbyNodes.Clear();
			FindInRange(state, range, root);
		}

		//  Function: MinAlongDim
		//-----------------------------------------------------------------------------------
		/// @brief Finds the minimum node from the 3 options along the given dimension
		/// 
		/// @param x The first node to compare
		/// @param y The second node to compare
		/// @param z The third node to compare
		/// @param dim The dimension to compare along
		/// 
		/// @return The minimum of the three nodes along the provided dimension
		//-----------------------------------------------------------------------------------
		private int MinAlongDim(int x, int y, int z, int dim)
		{
			int res = x;
			if (y != -1 && Nodes[y].State[dim] < Nodes[res].State[dim]) res = y;
			if (z != -1 && Nodes[z].State[dim] < Nodes[res].State[dim]) res = z;
			return res;
		}

		//  Function: MaxAlongDim
		//-----------------------------------------------------------------------------------
		/// @brief Finds the maximum node from the 3 options along the given dimension
		/// 
		/// @param x The first node to compare
		/// @param y The second node to compare
		/// @param z The third node to compare
		/// @param dim The dimension to compare along
		/// 
		/// @return The maximum of the three nodes along the provided dimension
		//-----------------------------------------------------------------------------------
		private int MaxAlongDim(int x, int y, int z, int dim)
		{
			int res = x;
			if (y != -1 && Nodes[y].State[dim] > Nodes[res].State[dim]) res = y;
			if (z != -1 && Nodes[z].State[dim] > Nodes[res].State[dim]) res = z;
			return res;
		}

		//  Function: FindMinInSubtreeAlongDim
		//-----------------------------------------------------------------------------------
		/// @brief Finds the minimum node in a subtree along the given dimension
		/// 
		/// @param node The root of the subtree 
		/// @param dim The dimension to compare along
		//-----------------------------------------------------------------------------------
		private int FindMinInSubtreeAlongDim(int node, int dim)
		{
			if (node == -1) return -1; // empty subtree

			if (Nodes[node].splitDim == dim)
			{
				if (Nodes[node].leftKD == -1) return node;
				return FindMinInSubtreeAlongDim(Nodes[node].leftKD, dim);
			}

			return MinAlongDim(
				node,
				FindMinInSubtreeAlongDim(Nodes[node].leftKD, dim),
				FindMinInSubtreeAlongDim(Nodes[node].rightKD, dim),
				dim);
		}

		//  Function: FindMaxInSubtreeAlongDim
		//-----------------------------------------------------------------------------------
		/// @brief Finds the maximum node in a subtree along the given dimension
		/// 
		/// @param node The root of the subtree 
		/// @param dim The dimension to compare along
		//-----------------------------------------------------------------------------------
		private int FindMaxInSubtreeAlongDim(int node, int dim)
		{
			if (node == -1) return -1; // empty subtree

			if (Nodes[node].splitDim == dim)
			{
				if (Nodes[node].leftKD == -1) return node;
				return FindMaxInSubtreeAlongDim(Nodes[node].leftKD, dim);
			}

			return MaxAlongDim(
				node,
				FindMaxInSubtreeAlongDim(Nodes[node].leftKD, dim),
				FindMaxInSubtreeAlongDim(Nodes[node].rightKD, dim),
				dim);
		}

		//  Function: DeleteInSubtree
		//-----------------------------------------------------------------------------------
		/// @brief Recursively delete nodes in a subtree
		/// 
		/// @param node The root of the subtree 
		/// @param parent The parent we should make this node's parent
		/// @param nextMin The next min node that we recursively search for
		//-----------------------------------------------------------------------------------
		private int DeleteInSubtree(int node, ref Node parent, int currMin)
		{
			if (node == currMin)
			{
				// Copy "min"'s attributes except for KD positions which are inherited from "node"
				if (Nodes[node].rightKD != -1)
				{
					int min = FindMinInSubtreeAlongDim(Nodes[node].rightKD, Nodes[node].splitDim);
					Node copy = new Node(min, Nodes[min].State);
					copy.cost = Nodes[min].cost;
					copy.parent = Nodes[min].parent;
					copy.children = new List<int>(Nodes[min].children);
					copy.heapkey = Nodes[min].heapkey;
					copy.splitDim = Nodes[node].splitDim;
					copy.parentKD = parent.ID;
					copy.leftKD = Nodes[node].leftKD;
					//copy.rightKD = Nodes[node].rightKD;
					copy.rightKD = DeleteInSubtree(Nodes[node].rightKD, ref copy, min);
					Nodes[min] = copy;
					if (copy.leftKD != -1) Nodes[copy.leftKD].parentKD = min;
					return min;
				}
				else if (Nodes[node].leftKD != -1)
				{
                    int min = FindMinInSubtreeAlongDim(Nodes[node].leftKD, Nodes[node].splitDim);
                    Node copy = new Node(min, Nodes[min].State);
					copy.cost = Nodes[min].cost;
					copy.parent = Nodes[min].parent;
					copy.children = new List<int>(Nodes[min].children);
					copy.heapkey = Nodes[min].heapkey;
					copy.splitDim = Nodes[node].splitDim;
					copy.parentKD = parent.ID;
					copy.leftKD = -1;
					copy.rightKD = DeleteInSubtree(Nodes[node].leftKD, ref copy, min);
					Nodes[min] = copy;
					return min;
				}
				else
				{
					if (parent.leftKD != -1 && parent.leftKD == node) parent.leftKD = -1;
					else if (parent.rightKD != -1 && parent.rightKD == node) parent.rightKD = -1;
					return -1;
				}
			}

			if (Nodes[currMin].State[Nodes[node].splitDim] < Nodes[node].State[Nodes[node].splitDim])
			{
				Node n = Nodes[node];
				n.leftKD = DeleteInSubtree(Nodes[node].leftKD, ref n, currMin);
				Nodes[node] = n;
			}
			else
			{
				Node n = Nodes[node];
				n.rightKD = DeleteInSubtree(Nodes[node].rightKD, ref n, currMin);
				Nodes[node] = n;
			}
			Nodes[node].parentKD = parent.ID;
			return node;
		}

		//  Function: Add Nodes in Subtree
		//-----------------------------------------------------------------------------------
		/// @brief Recursively adds all nodes to list of nodes to reconstruct
		/// 
		/// @param root The root of the subtree
		//-----------------------------------------------------------------------------------
		private void AddNodesInSubtree(int node)
		{
			if (node == -1) return;

			nodesToReinsert.Add(node);
			AddNodesInSubtree(Nodes[node].rightKD);
			AddNodesInSubtree(Nodes[node].leftKD);
			Nodes[node].leftKD = -1;
			Nodes[node].rightKD = -1;
			Nodes[node].parentKD = -1;
			Nodes[node].splitDim = 0;
		}

		//  Function: FindNearestInSubtree
		//-----------------------------------------------------------------------------------
		/// @brief Finds the nearest node in a subtree to the given state
		/// 
		/// @param state The state that we're trying to find the nearest node to
		/// @param root The root of the subtree that we're searching within
		//-----------------------------------------------------------------------------------
		private void FindNearestInSubtree(in Vector state, int root)
		{
			if (root == -1) return;

			int parent = root;
			int count = 0;
			int currDim = 0;
			while (count < MAX_SIZE)
			{
				currDim = Nodes[parent].splitDim;
				if (state[currDim] < Nodes[parent].State[currDim])
				{
					if (Nodes[parent].leftKD == -1)
					{
						break;
					}
					parent = Nodes[parent].leftKD;
				}
				else
				{
					if (Nodes[parent].rightKD == -1)
					{
						break;
					}
					parent = Nodes[parent].rightKD;
				}
				count++;
			}

			double distance = (state - Nodes[parent].State).Norm();
			if (distance < nearestDistance)
			{
				nearestNode = parent;
				nearestDistance = distance;
			}

			count = 0;
			while (count < MAX_SIZE)
			{
				currDim = Nodes[parent].splitDim;
				double dimDistance = Math.Abs(state[currDim] - Nodes[parent].State[currDim]);

				if (dimDistance > nearestDistance)
				{
					if (parent == root)
					{
						return;
					}
					parent = Nodes[parent].parentKD;
					count++;
					continue;
				}

				distance = (state - Nodes[parent].State).Norm();
				if (distance < nearestDistance)
				{
					nearestNode = parent;
					nearestDistance = distance;
				}

				if (Nodes[parent].rightKD != -1 && state[currDim] < Nodes[parent].State[currDim])
				{
					FindNearestInSubtree(state, Nodes[parent].rightKD);
				}
				else if (Nodes[parent].leftKD != -1)
				{
					FindNearestInSubtree(state, Nodes[parent].leftKD);
				}

				if (parent == root)
				{
					return;
				}
				parent = Nodes[parent].parentKD;
				count++;
			}
		}

		//  Function: FindInRange
		//-----------------------------------------------------------------------------------
		/// @brief Finds all nodes within range of state inside the subtree
		/// 
		/// @param state The state that we're trying to find the nodes around
		/// @param range The range to add nodes around state
		/// @param root The root of the subtree that we're searching within
		//-----------------------------------------------------------------------------------
		private void FindInRange(in Vector state, double range, int root)
		{
			int parent = root;
			int count = 0;
			int currDim;
			while (count < MAX_SIZE)
			{
				currDim = Nodes[parent].splitDim;
				if (state[currDim] < Nodes[parent].State[currDim])
				{
					if (Nodes[parent].leftKD == -1)
					{
						break;
					}
					parent = Nodes[parent].leftKD;
				}
				else
				{
					if (Nodes[parent].rightKD == -1)
					{
						break;
					}
					parent = Nodes[parent].rightKD;
				}
				count++;
			}


			double distance = (state - Nodes[parent].State).Norm();
			if (distance <= range)
			{
				nearbyNodes.Add(parent);
			}

			count = 0;
			while (count < MAX_SIZE)
			{
				currDim = Nodes[parent].splitDim;
				double dimDistance = Math.Abs(state[currDim] - Nodes[parent].State[currDim]);

				if (dimDistance > range)
				{
					if (parent == root)
					{
						return;
					}
					parent = Nodes[parent].parentKD;
					count++;
					continue;
				}

				distance = (state - Nodes[parent].State).Norm();
				if (distance <= range)
				{
					nearbyNodes.Add(parent);
				}

				if (Nodes[parent].rightKD != -1 && state[currDim] < Nodes[parent].State[currDim])
				{
					FindInRange(state, range, Nodes[parent].rightKD);
				}

				else if (Nodes[parent].leftKD != -1)
				{
					FindInRange(state, range, Nodes[parent].leftKD);
				}

				if (parent == root)
				{
					return;
				}
				parent = Nodes[parent].parentKD;
				count++;
			}
		}
    }
}
