using System;
using System.Text;
using Urho;
using Urho.Gui;
using Urho.Shapes;
using System.Diagnostics;




namespace _3Dlandscape
{
	public class Room : Component
	{
		Node roomNode;
		string titleString;

		public Room(Vector3 position, Vector3 size, Vector3 rotation, string title, Node myPlotNode)
		{
			this.roomNode = myPlotNode.CreateChild();
			this.titleString = title;

			Debug.WriteLine(this.titleString + " is created!");
			Debug.WriteLine("position:" + position + " size:" + size + " rotation:" + rotation);

			CreateCorner(position.X,          position.Y + size.Y, position.Z, 0, 0,   0);
			CreateCorner(position.X + size.X, position.Y + size.Y, position.Z, 0, 0, -90);
			CreateCorner(position.X,          position.Y         , position.Z, 0, 0,  90);
			CreateCorner(position.X + size.X, position.Y         , position.Z, 0, 0, 180);

			CreateCorner(position.X,          position.Y + size.Y, position.Z + size.Z,  90, 0,   0);
			CreateCorner(position.X + size.X, position.Y + size.Y, position.Z + size.Z,  90, 0, -90);
			CreateCorner(position.X,          position.Y,          position.Z + size.Z, -90, 0,  90);
			CreateCorner(position.X + size.X, position.Y,          position.Z + size.Z, -90, 0, 180);

			this.roomNode.Rotate(new Quaternion(rotation.X, rotation.Y, rotation.Z));

			ReceiveSceneUpdates = true;
		}


		private void CreateCorner(float x, float y, float z, float rx, float ry, float rz)
		{
			var cache = this.Application.ResourceCache;

			var corner = roomNode.CreateChild("Corner");
			corner.Position = new Vector3 (x,y,z);
			corner.Rotate(new Quaternion(rx, ry, rz), TransformSpace.World);
			corner.SetScale(0.1f);
			var cornerObject = corner.CreateComponent<StaticModel>();
			cornerObject.Model = cache.GetModel("corner.mdl");


			var i = cache.GetImage("world.png");
			var m = Material.FromImage(i);
			cornerObject.SetMaterial(m);

			//cornerObject.SetAttribute("Color", new Color(0.5f, 0.0f, 0.5f));

		}



		public string Touch()
		{
			Debug.WriteLine(this.titleString + " is touched!");
			return (this.titleString);
		}

	}
}
