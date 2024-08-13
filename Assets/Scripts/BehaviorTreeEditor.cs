using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BehaviorTreeEditor : EditorWindow
{
	private CustomerBehavior customerBehavior;
	private Dictionary<Node, Vector2> nodePositions = new Dictionary<Node, Vector2>();
	private float nodeHeight = 20;
	private float nodeWidth = 100;
	private float verticalSpacing = 80;
	private float horizontalSpacing = 20;

	[MenuItem("Window/Behavior Tree Visualizer")]
	public static void ShowWindow()
	{
		GetWindow<BehaviorTreeEditor>("Behavior Tree Visualizer").Show();
	}

	void OnGUI()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Drag your Customer GameObject here: ");
		customerBehavior = (CustomerBehavior)EditorGUILayout.ObjectField(customerBehavior, typeof(CustomerBehavior), true);
		GUILayout.EndHorizontal();
		if (customerBehavior != null)
		{
			BehaviorTree tree = customerBehavior.GetBehaviorTree();
			if (tree != null)
			{
				DrawTree(tree);
			}
		}
	}

	void DrawTree(BehaviorTree tree)
	{
		nodePositions.Clear(); 
		CalculateNodePositions(tree, this.position.width / 2, 20, 0);
		foreach (var nodePos in nodePositions)
		{
			Node node = nodePos.Key;
			Vector2 position = nodePos.Value;
			Rect nodeRect = new Rect(position.x - nodeWidth / 2, position.y, nodeWidth, nodeHeight);
			GUI.color = GetNodeColor(node);  // Set the color based on the node type
			GUI.Box(nodeRect, node.name);
			GUI.color = Color.white;  // Reset the color to default
									  // Draw connections
			foreach (Node child in node.children)
			{
				Rect childRect = new Rect(nodePositions[child].x - nodeWidth / 2, nodePositions[child].y, nodeWidth, nodeHeight);
				DrawNodeCurve(nodeRect, childRect);
			}
		}
	}

	Color GetNodeColor(Node node)
	{
		if (node is RSelector)
			return Color.magenta;
		if (node is Sequence)
			return Color.green;
		if (node is Leaf)
			return Color.cyan;
		return Color.grey; 
	}

	void CalculateNodePositions(Node node, float x, float y, int level)
	{
		if (nodePositions.ContainsKey(node))
			return;

		float totalWidth = GetTotalWidth(node);
		float startX = x - totalWidth / 2;

		for (int i = 0; i < node.children.Count; i++)
		{
			Node child = node.children[i];
			float childWidth = GetTotalWidth(child);
			float childX = startX + childWidth / 2;
			CalculateNodePositions(child, childX, y + verticalSpacing + nodeHeight, level + 1);
			startX += childWidth + horizontalSpacing;
		}

		nodePositions[node] = new Vector2(x, y);
	}

	float GetTotalWidth(Node node)
	{
		float width = nodeWidth;
		if (node.children.Count == 0)
			return width;

		float childrenWidth = 0;
		foreach (Node child in node.children)
			childrenWidth += GetTotalWidth(child);

		childrenWidth += horizontalSpacing * (node.children.Count - 1);
		return Mathf.Max(width, childrenWidth);
	}

	void DrawNodeCurve(Rect start, Rect end)
	{
		Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height, 0);
		Vector3 endPos = new Vector3(end.x + end.width / 2, end.y, 0);
		Vector3 startTan = startPos + Vector3.up * 50;
		Vector3 endTan = endPos + Vector3.down * 50;
		Color shadowCol = new Color(0, 0, 0, 0.06f);

		for (int i = 0; i < 3; i++)
			Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
		Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
	}

	struct NodeLevel
	{
		public int level;
		public Node node;
		public Vector2 position;
	}
}