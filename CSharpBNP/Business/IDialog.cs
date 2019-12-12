namespace CSharpBNP.Business
{
    public interface IDialog
    {
        string FolderBrowserDialogShow(string rootFolder);

        string OpenFileDialogShow(string filter);
    }

}