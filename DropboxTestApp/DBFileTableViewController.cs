using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using DropNet.Models;

namespace DropboxTestApp
{
	[MonoTouch.Foundation.Register("DBFileTableViewController")]
	public class DBFileTableViewController : UITableViewController
	{
		MetaData _dropboxData;
		
		public DBFileTableViewController () 
		{
				
		}
		
		public DBFileTableViewController(IntPtr handle) : base(handle) {}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
		
		public MetaData DropboxData { get { return _dropboxData; } }
		
		public void DisplayContents(string path)
		{
			Console.WriteLine("Displaying the contents of : {0}",path);
			
			AppDelegate.Dropbox.GetMetaDataAsync(path, response => { 
				
				_dropboxData = response.Data; 
				InvokeOnMainThread(()=>{
					this.TableView.DataSource = new TableDataSource(this);
					this.TableView.Delegate = new TableDelegate(this);
					this.TableView.ReloadData();
				});
			});
		}
		
		class TableDataSource : UITableViewDataSource
		{
			static NSString kCellIdentifier = new NSString ("MyIdentifier");
		
			DBFileTableViewController _tvc;
			
			public TableDataSource(DBFileTableViewController tvc)
			{
				_tvc = tvc;
			}
			
			public override int RowsInSection (UITableView tableview, int section)
			{
				return _tvc.DropboxData.Contents.Count;
			}
			
			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell (kCellIdentifier);
				
				if (cell == null)
				{
					cell = new UITableViewCell(UITableViewCellStyle.Default, kCellIdentifier);
				}
				
				MetaData data = _tvc.DropboxData.Contents[indexPath.Row];
				cell.TextLabel.Text = data.Name;
				cell.Accessory = (data.Is_Dir) ? UITableViewCellAccessory.DisclosureIndicator : UITableViewCellAccessory.None;
				return cell;
			}
		}
		
		class TableDelegate : UITableViewDelegate
		{
			DBFileTableViewController _tvc;
			
			public TableDelegate(DBFileTableViewController tvc)
			{
				_tvc = tvc;	
			}
			
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				MetaData selected = _tvc.DropboxData.Contents[indexPath.Row];
				
				if(selected != null && selected.Is_Dir)
				{
					var vc = new DBFileTableViewController();
					vc.Title = selected.Name;
					_tvc.NavigationController.PushViewController(vc,true);
					vc.DisplayContents(selected.Path);
				}
			}
		}
	}
}