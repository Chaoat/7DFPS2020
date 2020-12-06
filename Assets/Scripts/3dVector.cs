using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorFunctions 
{
	public static Vector3 parallelVectorToNormal(Vector3 vector, Vector3 normal) {
		Vector3 vectorInNormal = findVectorInDirection(vector, normal);

		if (vectorInNormal.magnitude == 0) {
			return vector;
		} else if (vector == vectorInNormal) {
			return Vector3.zero;
		} else {
			Vector3 hypotVector = vector/vectorInNormal.magnitude;

			Vector3 straightAdd = normal + hypotVector;
			if (straightAdd.magnitude == 1) {
				return straightAdd;
			} else {
				return hypotVector - normal;
			}
		}
	}

	public static Vector3 findVectorInDirection(Vector3 vector, Vector3 direction) {
		Vector3 normal = direction.normalized;
		return normal*Vector3.Dot(vector, normal);
	}
}