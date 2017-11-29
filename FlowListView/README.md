## ![](http://res.cloudinary.com/dqeaiomo8/image/upload/c_scale,w_50/v1444578527/DLToolkit/Forms-Controls-128.png) FlowListView for Xamarin.Forms [![PayPal donate button](http://img.shields.io/paypal/donate.png?color=green)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=VPZ4KHKHXXHR2 "Donate to this project using Paypal")

ListView derivative with flowing, grid-like columns support.

NuGet: https://www.nuget.org/packages/DLToolkit.Forms.Controls.FlowListView/

## Features: 
- `DataTemplate` and `DataTemplateSelector` support
- Fixed or automatic column count
- Grouping support
- Columns can expand to empty space (configurable)
- Infinite loading, empty group cell, customzed number of columns per group support. (thanks to @rudacs)
- **ANY** View can be used as a cell
- **All** Xamarin.Forms platforms supported

<img src="https://raw.githubusercontent.com/daniel-luberda/DLToolkit.Forms.Controls/master/FlowListView/Screenshots/flowlistview5.png" width="340"/> <img src="https://raw.githubusercontent.com/daniel-luberda/DLToolkit.Forms.Controls/master/FlowListView/Screenshots/flowlistview6.png" width="110"/> 

<img src="https://raw.githubusercontent.com/daniel-luberda/DLToolkit.Forms.Controls/master/FlowListView/Screenshots/flowlistview1.png" width="150"/> <img src="https://raw.githubusercontent.com/daniel-luberda/DLToolkit.Forms.Controls/master/FlowListView/Screenshots/flowlistview3.png" width="150"/> <img src="https://raw.githubusercontent.com/daniel-luberda/DLToolkit.Forms.Controls/master/FlowListView/Screenshots/flowlistview4.png" width="150"/>

<img src="https://raw.githubusercontent.com/daniel-luberda/DLToolkit.Forms.Controls/master/FlowListView/Screenshots/flowlistview_ios1.png" width="150"/> <img src="https://raw.githubusercontent.com/daniel-luberda/DLToolkit.Forms.Controls/master/FlowListView/Screenshots/flowlistview_ios2.png" width="150"/> <img src="https://raw.githubusercontent.com/daniel-luberda/DLToolkit.Forms.Controls/master/FlowListView/Screenshots/flowlistview_ios3.png" width="150"/>

## Simple Example:

### Init

Add the following to your App.xaml.cs

```C#
public App()
{
    InitializeComponent();
    FlowListView.Init(); 
}
```

### Sample

```XML
<flv:FlowListView FlowColumnCount="3" SeparatorVisibility="None" HasUnevenRows="false"
	FlowItemTappedCommand="{Binding ItemTappedCommand}" FlowLastTappedItem="{Binding LastTappedItem}"
	FlowItemsSource="{Binding Items}" >

	<flv:FlowListView.FlowColumnTemplate>
		<DataTemplate>
			<Label HorizontalOptions="Fill" VerticalOptions="Fill" 
				XAlign="Center" YAlign="Center" Text="{Binding Title}"/>
		</DataTemplate>
	</flv:FlowListView.FlowColumnTemplate>

</flv:FlowListView>
```


For other examples see sample app: [FlowListView Examples](https://github.com/daniel-luberda/DLToolkit.Forms.Controls/tree/master/Samples/DLToolkitControlsSamples/SamplesFlowListView) *(TIP: Clone repo, open the solution, build it and run sample app.)*

## FAQ

#### How can I disable entire row highlighting when tapped? 

Make a custom renderers for `FlowListViewInternalCell` in platforms specific projects which disable ListView row highlighting. Examples: [Android](https://github.com/daniel-luberda/DLToolkit.Forms.Controls/blob/master/Samples/Droid/Renderers/FlowListViewInternalCellRenderer.cs) [iOS](https://github.com/daniel-luberda/DLToolkit.Forms.Controls/blob/master/Samples/iOS/Renderers/FlowListViewInternalCellRenderer.cs) [Windows](https://github.com/daniel-luberda/DLToolkit.Forms.Controls/blob/master/Samples/Windows/CustomListViewRenderer.cs)

#### How can I have variable row height? (basing on content, different sizes for header and items)

Set `HasUnevenRows` property to `true`.

#### Why FlowListView isn't working in Release mode?

Sometimes (eg. if you're using XAML only views) linker may remove dlls needed by FlowListView. To avoid that use: `FlowListView.Init()` somewhere in your code.

#### Android: Getting exceptions when using `Entry` views

See: https://github.com/daniel-luberda/DLToolkit.Forms.Controls/issues/61
