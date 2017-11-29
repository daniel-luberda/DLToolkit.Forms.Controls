using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamvvm;
using Xamarin.Forms;
using DLToolkitControlsSamples.SamplesFlowListView;
using DLToolkitControlsSamples.SamplesRecyclerView;
using DLToolkitControlsSamples.SamplesImageCropView;

namespace DLToolkitControlsSamples
{
	public class MainPageModel : BasePageModel
	{
		public MainPageModel()
		{
			ItemSelectedCommand = new BaseCommand<SelectedItemChangedEventArgs>((arg) =>
			{
				var item = arg?.SelectedItem as MenuItem;
				if (item != null)
				{
					SelectedItem = null;
					item.Command?.Execute(null);
				}
			});

			var menuItems = new List<MenuItem>() {

                new MenuItem() {
                    Section = "ImageCropView",
                    Title = "Simple example",
                    Detail = "Simplest example",
                    Command = new BaseCommand(async (param) =>
                    {
                        var page = this.GetPageFromCache<ImageCropViewExampleModel>();
                        await this.PushPageAsync(page);
                    }),
                },

                new MenuItem() {
                    Section = "RecyclerView",
                    Title = "Simple example",
                    Detail = "Simplest example",
                    Command = new BaseCommand(async (param) =>
                    {
                        var page = this.GetPageFromCache<SimpleRecyclerViewPageModel>();
                        await this.PushPageAsync(page, (model) => model.ReloadData());
                    }),
                },
				
				new MenuItem() {
					Section = "FlowListView",
					Title = "Simple example",
					Detail = "Simplest fixed column number example",
					Command = new BaseCommand(async (param) =>
					{
						var page = this.GetPageFromCache<SimplePageModel>();
						await this.PushPageAsync(page, (model) => model.ReloadData());
					}),
				},	


                new MenuItem() {
                    Section = "FlowListView",
                    Title = "Simple total example",
                    Detail = "Simplest total row example",
                    Command = new BaseCommand(async (param) =>
                    {
                        var page = this.GetPageFromCache<TotalRowSamplePageModel>();
                        await this.PushPageAsync(page, (model) => model.ReloadData());
                    }),
                },  

				new MenuItem() {
					Section = "FlowListView",
					Title = "Simple gallery example",
					Detail = "Simplest auto column number example",
					Command = new BaseCommand(async (param) =>
					{
						var page = this.GetPageFromCache<SimpleGalleryPageModel>();
						await this.PushPageAsync(page, (model) => model.ReloadData());
					}),
				},

				new MenuItem() {
					Section = "FlowListView",
					Title = "Template selector example",
					Detail = "Custom FlowDataTemplateSelector example",
					Command = new BaseCommand(async (param) =>
					{
						var page = this.GetPageFromCache<TemplateSelectorPageModel>();
						await this.PushPageAsync(page, (model) => model.ReloadData());
					}),
				},

				new MenuItem() {
					Section = "FlowListView",
					Title = "Infinite Loading example",
					Detail = "Infinite Loading example",
					Command = new BaseCommand(async (param) =>
					{
                        var page = this.GetPageFromCache<InfiniteLoadingPageModel>();
						await this.PushPageAsync(page, (model) => model.ReloadData());
					}),
				},

				new MenuItem() {
					Section = "FlowListView",
					Title = "Grouping example",
					Detail = "Grouping example",
					Command = new BaseCommand(async (param) =>
					{
						var page = this.GetPageFromCache<GroupingPageModel>();
						await this.PushPageAsync(page, (model) => model.ReloadData());
					}),
				},

				new MenuItem() {
					Section = "FlowListView",
					Title = "Advanced Grouping example",
					Detail = "Advanced Grouping example",
					Command = new BaseCommand(async (param) =>
					{
						var page = this.GetPageFromCache<GroupingAdvancedPageModel>();
						await this.PushPageAsync(page, (model) => model.ReloadData());
					}),
				},

				new MenuItem() {
					Section = "FlowListView",
					Title = "Update items example",
					Detail = "Update items example",
					Command = new BaseCommand(async (param) =>
					{
						var page = this.GetPageFromCache<UpdateItemsPageModel>();
						await this.PushPageAsync(page, (model) => model.ReloadData());
					}),
				},

				new MenuItem() {
					Section = "FlowListView",
					Title = "Update items grouped example",
					Detail = "Update items grouped example",
					Command = new BaseCommand(async (param) =>
					{
						var page = this.GetPageFromCache<UpdateItemsGroupedPageModel>();
						await this.PushPageAsync(page, (model) => model.ReloadData());
					}),
				},

				new MenuItem() {
					Section = "TagEntryView",
					Title = "TagEntryView example",
					Detail = "TagEntryView example",
					Command = new BaseCommand(async (param) =>
					{
						var page = this.GetPageFromCache<TagEntryViewExamplePageModel>();
						await this.PushPageAsync(page, (model) => model.ReloadTags());
					}),
				},
			};

			var sorted = menuItems
				.OrderBy(item => item.Section)
				.GroupBy(item => item.Section)
				.Select(itemGroup => new Grouping<string, MenuItem>(itemGroup.Key, itemGroup));

			Items = new ObservableCollection<Grouping<string, MenuItem>>(sorted);
		}

		public ICommand ItemSelectedCommand
		{
			get { return GetField<ICommand>(); }
			set { SetField(value); }
		}

		public object SelectedItem
		{
			get { return GetField<object>(); }
			set { SetField(value); }
		}

		public ObservableCollection<Grouping<string, MenuItem>> Items
		{
			get { return GetField<ObservableCollection<Grouping<string, MenuItem>>>(); }
			set { SetField(value); }
		}

		public class MenuItem : BaseModel
		{
			string section;
			public string Section
			{
				get { return section; }
				set { SetField(ref section, value); }
			}

			string title;
			public string Title
			{
				get { return title; }
				set { SetField(ref title, value); }
			}

			string detail;
			public string Detail
			{
				get { return detail; }
				set { SetField(ref detail, value); }
			}

			ICommand command;
			public ICommand Command
			{
				get { return command; }
				set { SetField(ref command, value); }
			}

			object commandParameter;
			public object CommandParameter
			{
				get { return commandParameter; }
				set { SetField(ref commandParameter, value); }
			}
		}
	}
}
