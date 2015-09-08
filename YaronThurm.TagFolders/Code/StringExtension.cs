
namespace YaronThurm.TagFolders
{
    // Works only on .Net framework ver 3.5 and above
    public static class StringExtensions
    {
        public static bool IsFile(this string fullPath)
        {
            // Check that it is a file
            System.IO.FileInfo file = new System.IO.FileInfo(fullPath);
            return (file.Exists);
        }
        
        public static bool IsDirectory(this string fullPath)
        {
            // Check that it is a directory
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(fullPath);
            return (dir.Exists);
        }

        public static string GetFileName(this string fullPath)
        {
            // Find the index of the lase directory seperator. e.g: "c:\foo\bar" returns 6
            int i = fullPath.LastIndexOf("\\");
            string ret = "";
            if (i >= 0 && i < fullPath.Length)
                // Return the last part of the string, i.e the file name
                ret = fullPath.Substring(i + 1);
            else
                ret = fullPath;

            return ret;
        }

        public static string GetDirectoryName(this string fullPath)
        {
            // Find the index of the lase directory seperator. e.g: "c:\foo\bar" returns 6
            int i = fullPath.LastIndexOf("\\");
            string ret = "";
            if (i >= 0 && i < fullPath.Length)
                // Return the first part of the string, i.e the directory name
                ret = fullPath.Substring(0, i - 1);
            else
                ret = fullPath;

            return ret;
        }

        public static string GetFileExtension(this string fullPath)
        {                     
            // Find the index of the lase dot seperator. e.g: "c:\foo\bar.txt.exe" returns 13
            int i = fullPath.LastIndexOf(".");
            string ret = "";
            if (i >= 0 && i < fullPath.Length)
                // Return the first part of the string, i.e the directory name
                ret = fullPath.Substring(i + 1);
            else
                ret = "";

            return ret;
        }
    }
}