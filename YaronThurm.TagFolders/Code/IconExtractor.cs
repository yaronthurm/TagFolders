using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace YaronThurm.TagFolders
{
    public class IconExtractor
    {
        private ImageList smallImages;
        private ImageList largImages;
        private int fileNotExistIndex;
        private Dictionary<string /*file extension*/, int /*image index*/> fileExtensionToImageIndex;        

        /// <summary>
		/// Options to specify the size of icons to return.
		/// </summary>
		public enum IconSize
		{
			/// <summary>
			/// Specify large icon - 32 pixels by 32 pixels.
			/// </summary>
			Large = 0,
			/// <summary>
			/// Specify small icon - 16 pixels by 16 pixels.
			/// </summary>
			Small = 1
		}

        public IconExtractor(ImageList smallImages, ImageList largImages, int fileNotExistIndex)
        {
            this.smallImages = smallImages;
            this.largImages = largImages;
            this.fileNotExistIndex = fileNotExistIndex;

            this.fileExtensionToImageIndex = new Dictionary<string,int>();
        }

        public int GetIndexByFileName(string fileName)
        {
            int index = this.fileNotExistIndex;

            FileInfo file = new FileInfo(fileName);
            if (file.Exists)            
            {
                // Extract file extension
                string fileExtension = file.Extension;

                int i;
                if (this.fileExtensionToImageIndex.TryGetValue(fileExtension, out i))
                    index = i;
                else                
                {
                    // Load icon for it and create an entry
                    Icon small = null;
                    Icon large = null;
                    this.RetrieveFileIcons(fileName, ref small, ref large);

                    if (small != null && large != null)
                    {
                        this.smallImages.Images.Add(small);
                        this.largImages.Images.Add(large);
                    
                        index = this.smallImages.Images.Count - 1;
                        this.fileExtensionToImageIndex[fileExtension] =  index;
                    }
                    else
                        index = this.fileNotExistIndex;
                }
            }

            return index;
        }

        private Icon GetFileIcon(string fileName, IconSize size)
        {
            IntPtr iconPtr;
            Icon ret = null;
            SHFILEINFO shinfo = new SHFILEINFO();
                        
            uint flags = Win32.SHGFI_ICON;
            /* Check the size specified for return. */
            if (size == IconSize.Small)
                flags += Win32.SHGFI_SMALLICON;            
            else            
                flags += Win32.SHGFI_LARGEICON;

            
            iconPtr = Win32.SHGetFileInfo(fileName, 0,
                ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);

            if (iconPtr != IntPtr.Zero)
            {
                // Copy (clone) the returned icon to a new object, thus allowing us to clean-up properly
                ret = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(shinfo.hIcon).Clone();
                // Cleanup
                Win32.DestroyIcon(shinfo.hIcon);
            }

            return ret;
        }

        
        private void RetrieveFileIcons(string fileName, ref Icon smallIcon, ref Icon largeIcon)
        {
            smallIcon = this.GetFileIcon(fileName, IconSize.Small);
            largeIcon = this.GetFileIcon(fileName, IconSize.Large);
        }        
    }
    
    public class Win32
    {
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        /// <summary>
        /// Provides access to function required to delete handle. This method is used internally
        /// and is not required to be called separately.
        /// </summary>
        /// <param name="hIcon">Pointer to icon handle.</param>
        /// <returns>N/A</returns>
        [DllImport("user32.dll")]
        public static extern int DestroyIcon(IntPtr hIcon);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }
}