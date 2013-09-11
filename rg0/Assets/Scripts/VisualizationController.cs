using UnityEngine;
using System.Collections;

public class VisualizationController : MonoBehaviour
{
	public ParticleAnimator[] m_particleAnimators;

	public void InitializeColors (Color color)
	{
		foreach (ParticleAnimator animator in m_particleAnimators)
		{
			Color[] colors = new Color[animator.colorAnimation.Length];
			for (int i = 0; i < animator.colorAnimation.Length; i++)
			{
				colors[i] = color;
			}
			animator.colorAnimation = colors;
		}
	}

}
