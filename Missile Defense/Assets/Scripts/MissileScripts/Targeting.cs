using UnityEngine;
using System;
class Targeting
{
    public Targeting()
    {
        Debug.Log("Targeting Object Created");
    }

    //x vector = pointer, y vector = normal pointer to the left, z vector = vertical vector
    public Vector3 SphericalPointer(Vector3 SiloPoint, Vector3 RelativePosition)
    {
        Vector3 SphericalPointer = new Vector3(0.0f, 0.0f, 0.0f);
        SphericalPointer.x = RelativePosition.magnitude;
        Vector2 PointTopDown = new Vector2(SiloPoint.x, SiloPoint.z);
        Vector2 RelativePositionTopDown = new Vector2(RelativePosition.x, RelativePosition.z);
        SphericalPointer.y = (float) Math.Acos(Vector2.Dot(PointTopDown, RelativePositionTopDown) / (RelativePositionTopDown.magnitude * PointTopDown.magnitude));
        SphericalPointer.z = (float)Math.Acos(RelativePosition.x/(SphericalPointer.x * Math.Sin(SphericalPointer.y)));
        return SphericalPointer;
    }
    
    public Vector3 SphericalVector(Vector3 Silo, Vector3 Missile)
    {
        Vector3 relative = new Vector3(Silo.x - Missile.x, Silo.y - Missile.y, Silo.z - Missile.z);
        Vector3 spherical = new Vector3(0.0f, 0.0f, 0.0f);
        spherical.x = (float) Math.Sqrt(Math.Pow(relative.x, 2) + Math.Pow(relative.y, 2) + Math.Pow(relative.z, 2)); //r
        spherical.z = (float) Math.Acos(relative.z / spherical.x); //phi
        spherical.y = (float) Math.Asin(relative.y / (spherical.x * Math.Sin(spherical.z))); //theta
        return spherical;
    }


    
}