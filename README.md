# DrawOverMyScreen

Since Windows 8, the Bubbles screensaver and every other screensavers that draws over your screen (ScreenSmelter and Hypnogenic Rain, for example) can't do it anymore. Instead, they draw over a single-colored background.

DrawOverMyScreen is a little tool that simplifies the workaround described [here](http://www.eightforums.com/customization/42258-solved-bubbles-scrn-svr-issue-restore-float-desktop.html).

It also does a bit more:

- Respects your settings, such as startup delay and lock at screensaver exit.
- Provides a simple GUI to change and configure the screensaver.
- Renders the preview of your screensaver in the settings.
- Easy to setup.

## Compiling

No prebuilt binaries are given. It is required to compile them. To do so, simply run `make.cmd`.

## Installing

To install, you can use `install.cmd` or proceed with the following steps:

1. Copy `DrawOverMyScreen.scr` in `C:\Windows`.
2. Copy `DrawOverMyScreenClient.exe` wherever you want. Please note that the file needs to remain there until you uninstall DrawOverMyScreen.
3. Run `DrawOverMyScreenClient.exe /register`
4. Set your screensaver as DrawOverMyScreen and open the screensaver configuration to set the real screensaver to run.
5. Enjoy!

## Uninstalling

Run `DrawOverMyScreenClient.exe /unregister` and delete the files `DrawOverMyScreen.scr` and `DrawOverMyScreenClient.exe`