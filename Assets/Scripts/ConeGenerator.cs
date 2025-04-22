using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeGenerator : MonoBehaviour
{
	[SerializeField] LineRenderer[] lines;
	[SerializeField] GameObject obstacles;
	[SerializeField] GameObject cone;
	[SerializeField] float coneDensity = 0.5f;
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
				if (i + 1 < points.Length)
				{
					Vector3 currentPos = points[i];
					while (currentPos != points[i + 1])
					{
						Instantiate(SpawnObject, currentPos, Quaternion.identity, Parent.transform);
						currentPos = Vector3.MoveTowards(currentPos, points[i + 1], coneDensity);
					}
				}
			}
		}
	}
}
