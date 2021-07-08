using UnityEngine;

namespace Minigames.Lazers {
	public class Lazer : MonoBehaviour {
		public Transform Position;
		public Transform Target;
		public GameObject Shell;

		public Vector2 ShellMovementSpeedMinMax;

		private Material lineMaterial;
		private Vector2 initialPosition;
		private Rigidbody2D shellRigidbody2d;

		private void Start() {
			initialPosition = Shell.transform.position;
			shellRigidbody2d = Shell.GetComponent<Rigidbody2D>();
			launchFirstBall();
		}

		private void launchFirstBall()
		{
				transform.position = initialPosition;
				float x = Random.Range(0, 2) == 0 ? -1 : 1;
				float y = Random.Range(0, 2) == 0 ? -1 : 1;
				shellRigidbody2d.velocity = new Vector2(ShellMovementSpeedMinMax.x * x, ShellMovementSpeedMinMax.x * y);
		}

		private void createLineMaterial() {
			if (lineMaterial) return;

			// Unity has a built-in shader that is useful for drawing
			// simple colored things.
			Shader shader = Shader.Find("Hidden/Internal-Colored");
			lineMaterial = new Material(shader);
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			// Turn on alpha blending
			lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			// Turn backface culling off
			lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
			// Turn off depth writes
			lineMaterial.SetInt("_ZWrite", 0);
		}

		private void glDrawGradientCircle(float radius = 3.0f, float lineDensity = 100f) {
			GL.MultMatrix(transform.localToWorldMatrix);
			// Draw lines
			GL.Begin(GL.LINES);
			for (int i = 0; i < lineDensity; ++i)
			{
					float a = i / lineDensity;
					float angle = a * Mathf.PI * 2;
					// Vertex colors change from red to green
					GL.Color(new Color(a, 1 - a, 0, 0.8F));
					// One vertex at transform position
					GL.Vertex3(0, 0, 0);
					// Another vertex at edge of circle
					GL.Vertex3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
			}
			GL.End();
		}

		private void glDrawLine(Transform startPos, Vector3 endPos) {
			GL.MultMatrix(startPos.localToWorldMatrix);
			GL.Begin(GL.LINES);
			GL.Color(Color.red);
			GL.Vertex3(0, 0, 0);
			GL.Vertex3(endPos.x, endPos.y, endPos.y);
			GL.End();
		}

		private void OnRenderObject() {
			// createLineMaterial();
			// // Apply the line material
			// lineMaterial.SetPass(0);

			// GL.PushMatrix();
			// // Set transformation matrix for drawing to
			// // match our transform

			// glDrawGradientCircle(3.0f, 360);

			// glDrawLine(Position, Target.position);
			// GL.PopMatrix();
		}
	}
}
