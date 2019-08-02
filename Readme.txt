This is for showing the windows 10 share UI in electron apps

First thing to do is to get a copy of visual studio 2019, then install the windows 10 sdk

Next, load this app in visual studio by going to the share_app folder and clicking on the WindowsFormsApps2.sln or WindowsFormsApps2.csproj file

The app is loaded in visual studio

Check on the right side of visual studio, You will find a mini window called Solution Explorer and a drop down called References

Right click it and select "Add Reference"

Add two references by clicking browse

The first one is a file you will find in:

C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Runtime.WindowsRuntime.dll (Please note, it might not be 4.0.30319 on your system. Just choose whichever number looks like the latest)

The second you will find in:

C:\Program Files (x86)\Windows Kits\10\UnionMetadata\10.0.17763.0\Windows.winmd (Also note, the 10.0.17763.0 might differ depending on the version of windows sdk you have)

Check both of these to add them



Now you can build by clicking on the top menu (at the top of the app), Build -> Build Solution

Before this you can select (Release and Any CPU) instead of Debug and Any CPU

If successful, go back to Solution Explorer, Now right-click on WindowsFormsApps2, then select Open Folder in File Explorer

In this new folder, go to bin/Release

In your electron app (at the top most directory), create folder with path static/exe/sharing and copy all of the files in bin/Release into it

Now, in your app.js, add the following:


function sharePhotos() {
	try {
		let cmd = app.getAppPath() + '\\' + 'static\\exe\\sharing\\WindowsFormsApp2.exe ';
		// add images to be shared to list
		for (i in selectedImages) {
			cmd += '"' + selectedImages[i] + '" ';
		}
		// trim to remove whitespace at the end
		cmd = cmd.trim();
		// launch process
		cp.exec(cmd, (error, stdout, stderr) => {});
	} catch (e) {
	}
}

So, you will need to explain an array called selectedImages or selectedFiles if you like. This array will contain the full path of files you want to share. So it could be:

selectedImages = ["C:\\Images\\man.jpg", "C:\\Images\\woman.jpg"]

The sharing app works using this format:

WindowsFormsApp2.exe "C:\Projects\Javascript\photos\celeste.png" "C:\Projects\Javascript\photos\Silvercoins.jpg"

So, the name of the exe, followed by each file to be shared

When the user clicks outside of the sharing dialog, our c# app is closed and stops running saving us memory

Thanks




