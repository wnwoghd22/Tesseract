using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam
{
    Vector3 pos, dir;

    GameObject laserObj;
    LineRenderer laser;

    List<Vector3> laserIndices = new List<Vector3>();
    LayerMask mirror;

    public LaserBeam(Vector3 pos, Vector3 dir, Material material, LayerMask mirror)
    {
        this.laser = new LineRenderer();
        this.laserObj = new GameObject();
        this.laserObj.name = "Laser Beam";
        this.pos = pos;
        this.dir = dir;
        this.mirror = mirror;

        this.laser = this.laserObj.AddComponent(typeof(LineRenderer)) as LineRenderer;
        this.laser.startWidth = 0.5f;
        this.laser.endWidth = 0.3f;
        this.laser.material = material;
        this.laser.startColor = Color.yellow;
        this.laser.endColor = Color.yellow;

        CastRay(pos, dir, laser);
    }

    private void CastRay(Vector3 pos, Vector3 dir, LineRenderer laser)
    {
        laserIndices.Add(pos);

        Ray2D ray = new Ray2D(pos, dir);
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, float.PositiveInfinity, mirror);

        if (hit)
        {
            CheckHit(hit, dir, laser);
        }
        else
        {
            laserIndices.Add(ray.GetPoint(30));
            UpdateLaser();
        }
    }

    void UpdateLaser()
    {
        int count = 0;
        laser.positionCount = laserIndices.Count;

        foreach (Vector3 idx in laserIndices)
        {
            laser.SetPosition(count, idx);
            ++count;
        }
    }

    private void CheckHit(RaycastHit2D hitInfo, Vector3 direction, LineRenderer laser)
    {
        if (hitInfo.collider.gameObject.tag == "Mirror")
        {
            Vector3 pos = hitInfo.point;
            Vector3 dir = Vector3.Reflect(direction, hitInfo.normal);

            CastRay(pos + dir * 0.1f, dir, laser);
        } 
        else
        {
            laserIndices.Add(hitInfo.point);
            UpdateLaser();
        }
    }
}
