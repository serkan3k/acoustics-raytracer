using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RayTracer : MonoBehaviour
{
    // TODO: direct path
    class RayTracerPathInfo
    {
        public Vector3 Origin;
        public Vector3 Direction;
        public Single PathLength;
        public int ReflectionCount;
        public Single Pressure;
        public bool IsDone;
    }
    private List<RayTracerPathInfo> rays;

	void Start ()
	{
		rays = new List<RayTracerPathInfo>();
	    for (int i = 0; i < 100000; ++i)
	    {
            RayTracerPathInfo rtInfo = new RayTracerPathInfo();
	        rtInfo.Origin = this.transform.position;
            rtInfo.Direction = (UnityEngine.Random.insideUnitSphere + this.transform.position) - this.transform.position;
	        rtInfo.PathLength = 0.0f;
	        rtInfo.ReflectionCount = 0;
	        rtInfo.Pressure = 1.0f;
            rays.Add(rtInfo);
	    }

        CalculateRays();
	}

    void CalculateRays()
    {

        for (int i = 0; i < rays.Count; ++i)
        {
            if(rays[i].IsDone) continue;
            
            //RaycastHit hit;// ray = new Ray();
            while (!rays[i].IsDone)
            {
                RaycastHit hit; // ray = new Ray();

                if (Physics.Raycast(rays[i].Origin, rays[i].Direction, out hit, Single.MaxValue))
                {
                    if (hit.transform.gameObject.layer == 11)
                    {
                        rays[i].IsDone = true;
                        rays[i].PathLength += hit.distance;
                        rays[i].ReflectionCount += 1;
                        //rays[i].Origin = hit.point;
                        //rays[i].Direction = Vector3.Reflect(rays[i].Direction, hit.normal);
                        rays[i].Pressure = rays[i].Pressure * 0.98f;
                    }
                    else
                    {
                        rays[i].PathLength += hit.distance;
                        rays[i].ReflectionCount += 1;
                        rays[i].Origin = hit.point;
                        rays[i].Direction = Vector3.Reflect(rays[i].Direction, hit.normal);
                        rays[i].Pressure = rays[i].Pressure * 0.98f;
                    }
                }
                else
                {
                    rays[i].IsDone = true;
                }
                if (rays[i].Pressure <= 0.001f)
                {
                    rays[i].IsDone = true;
                }
            }
        }
        using (StreamWriter writer = new StreamWriter("important2.txt"))
        {
            for (int i = 0; i < rays.Count; ++i)
            {
                writer.WriteLine(rays[i].Pressure);
            }
        }

        using (StreamWriter writer = new StreamWriter("important.txt"))
        {
            for (int i = 0; i < rays.Count; ++i)
            {
                writer.WriteLine(rays[i].Pressure + " " + rays[i].Direction + " " + rays[i].IsDone + " " + rays[i].Origin + " " + rays[i].PathLength + " " + rays[i].ReflectionCount);
            }
        }
    }

	void Update ()
	{
	    
	}
}
