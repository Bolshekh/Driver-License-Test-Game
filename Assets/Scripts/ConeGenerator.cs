using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeGenerator : MonoBehaviour
{
	[SerializeField] LineRenderer[] lines;
	[SerializeField] GameObject obstacles;
	[SerializeField] GameObject cone;
	// Start is called before the first frame update
	void Start()
	{
		Generate(lines, obstacles, cone);
		foreach(LineRenderer line in lines)
		{
			line.gameObject.SetActive(false);
		}
	}
	void Generate(LineRenderer[] lines, GameObject Parent, GameObject SpawnObject)
	{
		foreach (LineRenderer line in lines)
		{
			Vector3[] points = new Vector3[line.positionCount];
			line.GetPositions(points);
			for (int i = 0; i < points.Length; i++)
			{
				if (i + 1 < points.Length && Vector3.Distance(points[i], points[i + 1]) > 0.5f)
				{
					Instantiate(SpawnObject, Vector3.Lerp(points[i], points[i + 1], 0.5f), Quaternion.identity, Parent.transform);
					Instantiate(SpawnObject, points[i], Quaternion.identity, Parent.transform);
				}
				else
				{
					Instantiate(SpawnObject, points[i], Quaternion.identity, Parent.transform);
				}
			}
		}
	}
}
