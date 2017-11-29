clean:
	rm -f *.nupkg
	rm -rf */bin
	rm -rf */obj
	rm -rf */*/bin
	rm -rf */*/obj
	rm -rf */*/*/bin
	rm -rf */*/*/obj

nuget:
	nuget pack FlowListView/DLToolkit.Forms.Controls.FlowListView.nuspec
nuget2:
	nuget pack ImageCropView/DLToolkit.Forms.Controls.ImageCropView.nuspec