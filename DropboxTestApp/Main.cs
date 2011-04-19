
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RestSharp;
using DropNet.Models;
using DropNet;

namespace DropboxTestApp
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args);
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate
	{
		const string USERNAME = "";
		const string PASSWORD = "";
		const string API_KEY = "";
		const string API_SECRET = "";
		
		static DropNetClient _client = new DropNet.DropNetClient(API_KEY,API_SECRET);
		DBFileTableViewController _tvc;
		MonoTouch.UIKit.UINavigationController navigationController;

		public static DropNetClient Dropbox
		{
			get { return _client; }
		}
		
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			_client.LoginAsync(USERNAME,PASSWORD, x => {
				FinishLaunching();
			});

            // Create the main view controller - the 'first' view in the app
            _tvc = new DBFileTableViewController();
			
			// Create a navigation controller, to which we'll add the view
			navigationController = new UINavigationController();
			navigationController.PushViewController(_tvc, false);
			navigationController.TopViewController.Title ="Dropbox Test";
			
			
            // Create the main window and add the navigation controller as a subview
            window = new UIWindow (UIScreen.MainScreen.Bounds);
            window.AddSubview(navigationController.View);
            window.MakeKeyAndVisible ();			
			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
		
		void FinishLaunching()
		{
			_tvc.DisplayContents(".");
		}
	}
}

