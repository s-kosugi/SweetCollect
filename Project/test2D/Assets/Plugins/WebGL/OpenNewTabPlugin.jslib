var OpenNewTabPlugin = 
{
  OpenNewTab: function(URL) 
  {
    var url = Pointer_stringify(URL);
    window.open(url);
  }
}
 
mergeInto(LibraryManager.library, OpenNewTabPlugin);