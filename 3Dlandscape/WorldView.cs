﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Urho;
using Urho.Actions;
using Urho.Gui;
using Urho.Shapes;
using System.IO;
using System.Reflection;

namespace _3Dlandscape
{
	public class WorldView : Application
	{
		bool movementsEnabled;
		Scene scene;
		Node plotNode;
		Node staticNode;
		Node cameraNode;
		Camera camera;
		Octree octree;

		Double pinchStart = 0.0;

		public Bar SelectedBar { get; private set; }

		//		public IEnumerable<Bar> Bars => bars;

		[Preserve]
		public WorldView(ApplicationOptions options = null) : base(options) { }

		static WorldView()
		{
			/*		UnhandledException += (s, e) =>
					{
						if (Debugger.IsAttached)
							Debugger.Break();
						e.Handled = true;
					};*/
		}

		protected override void Start()
		{
			base.Start();
			CreateScene();
			SetupViewport();
		}

		void CreateScene()
		{
			var cache = ResourceCache;
			Input.TouchEnd += OnTouched;

			scene = new Scene();
			octree = scene.CreateComponent<Octree>();

			plotNode = scene.CreateChild();
			var baseNode = plotNode.CreateChild().CreateChild();
			var plane = baseNode.CreateComponent<StaticModel>();
			plane.Model = CoreAssets.Models.Plane;


			// Create a Zone component for ambient lighting & fog control
			var zoneNode = scene.CreateChild("Zone");
			var zone = zoneNode.CreateComponent<Zone>();

			// Set same volume as the Octree, set a close bluish fog and some ambient light
			zone.SetBoundingBox(new BoundingBox(-1000.0f, 1000.0f));
			zone.AmbientColor = new Color(0.15f, 0.15f, 0.15f);
			zone.FogColor = new Color(0.5f, 0.5f, 0.8f);
			zone.FogStart = 50;
			zone.FogEnd = 300;

			// Create a directional light to the world. Enable cascaded shadows on it
			var DirlightNode = scene.CreateChild("DirectionalLight");
			DirlightNode.SetDirection(new Vector3(0.6f, -1.0f, 0.8f));
			var Dirlight = DirlightNode.CreateComponent<Light>();
			Dirlight.LightType = LightType.Directional;
			Dirlight.CastShadows = true;
			Dirlight.ShadowBias = new BiasParameters(0.00025f, 0.5f);

			// Set cascade splits at 10, 50 and 200 world units, fade shadows out at 80% of maximum shadow distance
			Dirlight.ShadowCascade = new CascadeParameters(10.0f, 50.0f, 200.0f, 0.0f, 0.8f);


			cameraNode = scene.CreateChild();
			camera = cameraNode.CreateComponent<Camera>();
			cameraNode.Position = new Vector3(0, 3, -13) / 1.75f;

			cameraNode.SetDirection(new Vector3(0, -3, 10)); // looking towards what point (x,y,z)

			Node lightNode = cameraNode.CreateChild();
			var light = lightNode.CreateComponent<Light>();
			light.LightType = LightType.Point;
			light.Range = 100;
			light.Brightness = 1.3f;


			// Draw a 3d reference 
			New3Dtext(1.0f, 0.0f, 0.0f, 0.0f,   0.0f,  0.0f, "x...", plotNode, "3DlabeelX");
			New3Dtext(0.0f, 1.0f, 0.0f, 0.0f,   0.0f, 90.0f, "y...", plotNode, "3DlabeelY");
			New3Dtext(0.0f, 0.0f, 1.0f, 0.0f, -90.0f,  0.0f, "z...", plotNode, "3DlabeelZ");

			// Write a text on a static place

			var staticObject = baseNode.CreateComponent<StaticModel>();
			plane.Model = CoreAssets.Models.Plane;


			// defining static cube
			staticNode = scene.CreateChild();
			New3Dtext(-2.0f,  2.0f, -2.0f, 0.0f, 0.0f, 0.0f, "*", staticNode, "ful");
			New3Dtext( 2.0f,  2.0f, -2.0f, 0.0f, 0.0f, 0.0f, "*", staticNode, "fur");
			New3Dtext(-2.0f, -2.0f, -2.0f, 0.0f, 0.0f, 0.0f, "*", staticNode, "fll");
			New3Dtext( 2.0f, -2.0f, -2.0f, 0.0f, 0.0f, 0.0f, "*", staticNode, "flr");
			New3Dtext(-2.0f,  2.0f,  2.0f, 0.0f, 0.0f, 0.0f, "*", staticNode, "bul");
			New3Dtext( 2.0f,  2.0f,  2.0f, 0.0f, 0.0f, 0.0f, "*", staticNode, "bur");
			New3Dtext(-2.0f, -2.0f,  2.0f, 0.0f, 0.0f, 0.0f, "*", staticNode, "bll");
			New3Dtext( 2.0f, -2.0f,  2.0f, 0.0f, 0.0f, 0.0f, "*", staticNode, "blr");

			var sphere = plotNode.CreateComponent<Sphere>();
			sphere.Color = new Color(0.0f, 0.0f, 1.0f);

			var i = cache.GetImage("world.png");
			var m = Material.FromImage(i);
			sphere.SetMaterial(m);



			movementsEnabled = true;
		}

		void New3Dtext(float x, float y, float z, float a, float b, float g, string myText, Node myPlotNode, string objectLabel)
		{// x,y,z is the coordinates, a(lpha) is the ccw rotation in degree around x, b(eta) around y, g(amma) around z. the upper left point of the text is the reference
			Node textNode;
			Text3D text3D;

			textNode = myPlotNode.CreateChild(objectLabel);
			textNode.Rotate(new Quaternion(a, b, g), TransformSpace.World);
			textNode.Position = new Vector3(x, y, z);
			text3D = textNode.CreateComponent<Text3D>();
			text3D.SetFont(CoreAssets.Fonts.AnonymousPro, 10);
			text3D.TextEffect = TextEffect.Shadow;
			text3D.Text = myText;
			text3D.SetColor(new Color(0.0f, 0.0f, 0.0f));

		}

		void Update3Dtext(Node myPlotNode, string objectLabel, string myText)
		{
			Node textNode = myPlotNode.GetChild(objectLabel);
			Text3D text3D = textNode.GetComponent<Text3D>();
			text3D.Text = myText;
		}

		void OnTouched(TouchEndEventArgs e)
		{
			uint NumFingers = Input.NumTouches;

			Debug.WriteLine("Ontouched! NumFingers=" + NumFingers);

			if (NumFingers == 2 /* PC:3  Andro:2  */ && movementsEnabled)
			{
				Debug.WriteLine("Ontouched! BEFORE pinchStart=" + pinchStart);
				pinchStart = 0.0;
				Debug.WriteLine("Ontouched! AFTER  pinchStart=" + pinchStart);
			}
		}

		protected override void OnUpdate(float timeStep)
		{
			uint NumFingers = Input.NumTouches;
			if (NumFingers >= 1 && movementsEnabled)
			{
				/*NumFingers--; /* remove if Andro, keep if WIN */

				if (NumFingers == 1)
				{
					var touch = Input.GetTouch(0);

					float rX = -touch.Delta.Y * 0.1f;
					float rY = -touch.Delta.X * 0.1f;
					float rZ = 0.0f;

					plotNode.Rotate(new Quaternion(rX, rY, rZ), TransformSpace.World);

					Update3Dtext(plotNode, "3DlabeelX", "x... rot:" + plotNode.Rotation.PitchAngle);
					Update3Dtext(plotNode, "3DlabeelY", "y... rot:" + plotNode.Rotation.YawAngle);
					Update3Dtext(plotNode, "3DlabeelZ", "z... rot:" + plotNode.Rotation.RollAngle);
				}
				if (NumFingers == 2) // pan and zoom
				{
					var touch0 = Input.GetTouch(0);
					var touch1 = Input.GetTouch(1);

					double pinch = Math.Sqrt(Math.Pow(touch0.Position.X - touch1.Position.X, 2) + Math.Pow(touch0.Position.Y - touch1.Position.Y, 2));

					cameraNode.Rotate(new Quaternion(-0.1f * ((touch0.Delta.Y + touch1.Delta.Y) / 2.0f), -0.1f * ((touch0.Delta.X + touch1.Delta.X) / 2.0f), 0), TransformSpace.Local);

					if (pinchStart != pinch)
					{
						if (pinchStart == 0) pinchStart = pinch;
						else
						{
							Debug.WriteLine("OnUpdate! pinch=" + pinch);
							cameraNode.Translate(new Vector3(0, 0, (float)(pinch - pinchStart) * 0.01f), TransformSpace.Local);
							pinchStart = pinch;
						}
					}
				}

			}
			base.OnUpdate(timeStep);
		}

		void SetupViewport()
		{
			var renderer = Renderer;
			var vp = new Viewport(Context, scene, camera, null);
			renderer.SetViewport(0, vp);
		}
	}
}