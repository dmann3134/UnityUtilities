﻿using UnityEngine;
using System.Collections;

public enum ComparisonOperator
{
  LessThan,
  GreaterThan,
  EqualTo
}

public static class PhysicsUtilities
{
  private static int groundLayers = -1;
  public static int GroundLayers
  {
    get
    {
      if (groundLayers == -1)
      {
        groundLayers = LayerMask.NameToLayer("Ground");
      }
      return groundLayers;
    }
  }


  /// <summary>
  /// Pass in a rigidbody and what comparison to perform, returns true or false
  /// </summary>
  /// <param name="targetRigidbody"></param>
  /// <param name="comparisonOperator"></param>
  /// <param name="comparisonValue"></param>
  /// <returns>Bool for whther it passed that criteria</returns>
  public static bool CompareRigidbodyVelocity(Rigidbody targetRigidbody, ComparisonOperator comparisonOperator, float comparisonValue)
  {
    switch (comparisonOperator)
    {
      case ComparisonOperator.LessThan:
        if (targetRigidbody.velocity.magnitude < comparisonValue) return true;
        break;
      case ComparisonOperator.GreaterThan:
        if (targetRigidbody.velocity.magnitude > comparisonValue) return true;
        break;
      case ComparisonOperator.EqualTo:
        if (targetRigidbody.velocity.magnitude == comparisonValue) return true;
        break;
    }
    return false;
  }

  public static bool CompareTransformDistanceToGround(Transform targetTransform, ComparisonOperator comparisonOperator, float comparisonValue)
  {
    //add offset to y position
    RaycastHit[] hits = Physics.RaycastAll(targetTransform.position + Vector3.up, Vector3.down, groundLayers);
    foreach (RaycastHit hit in hits)
    {
      //remove the same offset you added for correct distance
      var distanceToGround = hit.distance - 1;
      switch (comparisonOperator)
      {
        case ComparisonOperator.LessThan:
          if (distanceToGround < comparisonValue) return true;
          break;
        case ComparisonOperator.GreaterThan:
          if (distanceToGround > comparisonValue) return true;
          break;
        case ComparisonOperator.EqualTo:
          if (distanceToGround == comparisonValue) return true;
          break;
      }
      return false;
    }
    return false;
  }

  public static Vector3 FindPointOnGround(Vector3 targetPosition, int maxDistance = 20, int overrideLayers = -1)
  {
    RaycastHit[] hits = Physics.RaycastAll(targetPosition + Vector3.up, Vector3.down, maxDistance, groundLayers);
    foreach (RaycastHit hit in hits)
    {
      return hit.point;
    }
    return targetPosition;
  }

  public static Transform FindPointOnGroundWithLayer(Vector3 position, LayerMask groundLayers, int maxDistance = 5)
  {
    //add offset to y position cuz unity physics
    RaycastHit[] hits = Physics.RaycastAll(position + Vector3.up, Vector3.down, maxDistance, groundLayers.value);
    foreach (RaycastHit hit in hits)
    {
      return hit.transform;
    }
    return null;
  }
}
