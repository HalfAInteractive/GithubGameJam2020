using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Easing
{
  public delegate float EaseFunction(float a, float b);
  public static float SmoothStartN(float X, float N = 2)
  {
    return Mathf.Pow(X, N);
  }
  
  public static float SmoothStopN(float X, float N = 2)
  {
    return 1-Mathf.Pow((1-X), N);
  }
  
  public static float SmoothStepN(float X, float N = 2)
  {
    return Mix(SmoothStartN(X, N), SmoothStopN(X, N), X);
  }
  
  public static float SmoothStartArchN(float X, float N = 2)
  {
    return Mathf.Pow(2,N-1)*Mathf.Pow(X, N-1)*(1-X);
  }
  
  public static float SmoothStopArchN(float X, float N = 2)
  {
    return Mathf.Pow(2,N-1)*Mathf.Pow(1-X, N-1)*(X);
  }
  
  public static float SmoothStepArchN(float X, float N = 2)
  {
    return Mix(SmoothStartArchN(X, N), SmoothStopArchN(X, N), X);
  }

  public static float Mix(float A, float B, float mix)
  {
    return A * (1-mix) + B * (mix);
  }
  
  public static Vector3 MixV(Vector3 v1, Vector3 v2, float mix)
  {
    return new Vector3(Mix(v1.x, v2.x, mix), Mix(v1.y, v2.y, mix), Mix(v1.z, v2.z, mix));
  }
  
  public static Color MixC(Color c1, Color c2, float mix)
  {
    return new Color(Easing.Mix(c1.r, c2.r, mix)
                    ,Easing.Mix(c1.g, c2.g, mix)
                    ,Easing.Mix(c1.b, c2.b, mix)
                    ,Easing.Mix(c1.a, c2.a, mix));
  }
  
  public static Color MixHSV(Color c1, Color c2, float mix)
  {
    float H1;
    float S1;
    float V1;
    float H2;
    float S2;
    float V2;
    
    Color.RGBToHSV(c1, out H1, out S1, out V1);
    Color.RGBToHSV(c2, out H2, out S2, out V2);
    
    return Color.HSVToRGB(Easing.Mix(H1, H2, mix)
                          ,Easing.Mix(S1, S2, mix)
                          ,Easing.Mix(V1, V2, mix));
  }
  
  public static List<Quaternion> MixKey(List<Quaternion> KeyFrame1, List<Quaternion> KeyFrame2, float mix)
  {
    List<Quaternion> inbetween = new List<Quaternion>();
    for(int index = 0; index < KeyFrame1.Count; index++)
    {
      float x = Easing.Mix(KeyFrame1[index].x, KeyFrame2[index].x, mix);
      float y = Easing.Mix(KeyFrame1[index].y, KeyFrame2[index].y, mix);
      float z = Easing.Mix(KeyFrame1[index].z, KeyFrame2[index].z, mix);
      float w = Easing.Mix(KeyFrame1[index].w, KeyFrame2[index].w, mix);
      inbetween.Add(new Quaternion(x,y,z,w));
    }
    return inbetween;
  }
}
