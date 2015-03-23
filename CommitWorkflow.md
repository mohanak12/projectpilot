Commit Workflow:
  1. Save all files in the VisualStudio
  1. Update from SVN
  1. If there are any conflicts, resolve them (consult Igor Brejc if necessary)
  1. Run **Build.bat** from the command line.
  1. If the build fails, you have to integrate your changes to make the build run (that's why it's called "continuous integration" :). Then proceed back to step 1.
  1. If the build runs...
  1. In VisualStudio, open AnhkSVN's **Pending Changes** window and add all files with the **New** status to the Subversion.
  1. After you've done this, use TortoiseSVN to commit all files **from root of the project**.
  1. Don't forget to enter a (oneliner) commit message in the TortoiseSVN dialog. This will make tracking changes **much** easier.
  1. You're done - now get back to coding :)