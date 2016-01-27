## ![](http://res.cloudinary.com/dqeaiomo8/image/upload/c_scale,w_50/v1444578527/DLToolkit/Forms-Controls-128.png) FlowListView for Xamarin.Forms [![PayPal donate button](http://img.shields.io/paypal/donate.png?color=green)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=VPZ4KHKHXXHR2 "Donate to this project using Paypal") [![Bitcoin donate button](http://img.shields.io/bitcoin/donate.png?color=green)](https://blockchain.info/address/16CvewT3QyAc5ATTVNHQ2EomxLQPXxyKQ7 "Donate to this project using Bitcoin")

ListView derivative with flowing, grid-like columns support.

NuGet: https://www.nuget.org/packages/DLToolkit.Forms.Controls.FlowListView/

## Features: 
- Different template for any column or cell (column template is returned basing on current item BindingContext
- Fixed or automatic column count
- Grouping support
- Columns can expand to empty space (configurable)
- **ANY** View can be used as a cell
- **All** Xamarin.Forms platforms supported

<img src="https://raw.githubusercontent.com/daniel-luberda/DLToolkit.Forms.Controls/master/FlowListView/Screenshots/flowlistview1.png" width="150"/> <img src="https://raw.githubusercontent.com/daniel-luberda/DLToolkit.Forms.Controls/master/FlowListView/Screenshots/flowlistview3.png" width="150"/> <img src="https://raw.githubusercontent.com/daniel-luberda/DLToolkit.Forms.Controls/master/FlowListView/Screenshots/flowlistview4.png" width="150"/>

<img src="https://raw.githubusercontent.com/daniel-luberda/DLToolkit.Forms.Controls/master/FlowListView/Screenshots/flowlistview_ios1.png" width="150"/> <img src="https://raw.githubusercontent.com/daniel-luberda/DLToolkit.Forms.Controls/master/FlowListView/Screenshots/flowlistview_ios2.png" width="150"/> <img src="https://raw.githubusercontent.com/daniel-luberda/DLToolkit.Forms.Controls/master/FlowListView/Screenshots/flowlistview_ios3.png" width="150"/>

## Simple Demo:

### C# 
- [SimpleExamplePage.cs](https://github.com/daniel-luberda/DLToolkit.Forms.Controls/blob/master/Examples/ExamplesFlowListView/Pages/SimpleExamplePage.cs)
- [SimpleExampleViewModel.cs](https://github.com/daniel-luberda/DLToolkit.Forms.Controls/blob/master/Examples/ExamplesFlowListView/ViewModels/SimpleExampleViewModel.cs)

### XAML
- [SimpleExampleXamlPage.xaml](https://github.com/daniel-luberda/DLToolkit.Forms.Controls/blob/master/Examples/ExamplesFlowListView/Pages/SimpleExampleXamlPage.xaml)
- [SimpleExampleXamlPage.xaml.cs](https://github.com/daniel-luberda/DLToolkit.Forms.Controls/blob/master/Examples/ExamplesFlowListView/Pages/SimpleExampleXamlPage.xaml.cs)
- [SimpleExampleXamlViewModel.cs](https://github.com/daniel-luberda/DLToolkit.Forms.Controls/blob/master/Examples/ExamplesFlowListView/ViewModels/SimpleExampleXamlViewModel.cs)

For other scenarios see sample app: [ExamplesFlowListView](https://github.com/daniel-luberda/DLToolkit.Forms.Controls/tree/master/Examples/ExamplesFlowListView) *(TIP: Clone repo, open the solution, build it and run sample app.)*

## FAQ

#### How can I disable entire row highlight on long tap&hold? 

Make a custom renderers for `FlowListViewInternalCell` in platforms specific projects which disable ListView row highlighting. More info here: http://stackoverflow.com/a/25887514/5064986

#### How can I have variable row height? (basing on content, different sizes for header and items)

Set `HasUnevenRows` property to `true`.

#### Why FlowListView isn't working in Release mode?

Sometimes (eg. if you're using XAML only views) linker may remove dlls needed by FlowListView. To avoid that use: `FlowListView.Init()` somewhere in your code.