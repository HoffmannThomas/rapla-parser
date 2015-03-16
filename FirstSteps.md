# Visual Studio or Mono #

  * VS: Simply install, patch, reboot and configure git
  * Mono: Maybe GTK# and runtime needed, git integrated

# Git Part 1 #
  * Best integration of VS2010 so far: https://code.google.com/p/gitextensions/
  * Install and select both msysgit and kdiff3

# Git - Part 2 #

  1. Open the command prompt.in admin mode
  1. Type "set HOME=%HOMEPATH%"
  1. press enter to create a new environment variable telling git where to find your netrc file
  1. Type "echo machine code.google.com login yourEmail@gmail.com password > %HOME%\`_`netrc"
  1. press enter to create the `_`netrc file (`_`netrc, not .netrc) with your login information

# Git - Part 3 #
  1. Open the GitExtension-GUI
  1. Select clone repository
  1. fill in requested information
  1. clone

That's all. Now you schould be able to pull, commit and push without any further configuration. In VS2010 simply open the git-menu and access the options directly.