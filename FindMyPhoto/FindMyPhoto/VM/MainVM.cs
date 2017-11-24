using FindMyPhoto.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ImageHashing;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.IO;

namespace FindMyPhoto.VM
{
    class MainVM : BaseVM
    {

        #region DeclaringPrivateVariables

        private BitmapSource _image1;
        private BitmapSource _image2;
        private BitmapSource _folderSearchImage;
        private BitmapSource _matchedImage;
        private ICommand _changeImagePath1;
        private ICommand _changeImagePath2;
        private ICommand _compareImages;
        private ICommand _runFolderCompare;
        private ICommand _openImage;
        private ICommand _openFolder;
        private string EMPTY_IMAGE_PATH = "Resources\\NotFoundImage.png";
        private string imagePath1;
        private string imagePath2;
        private string folderSearchImagePath;
        private string folderSearchPath;
        private string _percentageText = "00.0";
        private string _selectedMatchedImage;
        private ObservableCollection<string> _matchingImages = new ObservableCollection<string>();

        #endregion


        #region PublicProperties
        public BitmapSource Image1
        {
            get
            {
                return _image1;
            }
            set
            {
                _image1 = value;
                RaisePropertyChanged(nameof(Image1));
            }
        }
        public BitmapSource Image2
        {
            get
            {
                return _image2;
            }
            set
            {
                _image2 = value;
                RaisePropertyChanged(nameof(Image2));
            }
        }
        public BitmapSource FolderSearchImage
        {
            get
            {
                return _folderSearchImage;
            }
            set
            {
                _folderSearchImage = value;
                RaisePropertyChanged(nameof(FolderSearchImage));
            }
        }
        public BitmapSource MatchedImage
        {
            get
            {
                return _matchedImage;
            }
            set
            {
                _matchedImage = value;
                RaisePropertyChanged(nameof(MatchedImage));
            }
        }
        public string PercentageText
        {
            get
            {
                return _percentageText + "%";
            }
            set
            {
                _percentageText = value;
                RaisePropertyChanged(nameof(PercentageText));
            }
        }
        public string Threshold
        {
            get;
            set;
        }
        public string SelectedMatchedImage
        {
            get
            {
                return _selectedMatchedImage;
            }
            set
            {
                _selectedMatchedImage = value;
                LoadImage(value);
            }
        }
        public ObservableCollection<string> MatchingImages
        {
            get
            {
                return _matchingImages;
            }
            set
            {
                _matchingImages = value;
                RaisePropertyChanged(nameof(MatchingImages));
            }
        }
        public ICommand ChangeImagePath1
        {
            get
            {
                return _changeImagePath1;
            }
            set
            {
                _changeImagePath1 = value;
            }
        }
        public ICommand ChangeImagePath2
        {
            get
            {
                return _changeImagePath2;
            }
            set
            {
                _changeImagePath2 = value;
            }
        }
        public ICommand CompareImages
        {
            get
            {
                return _compareImages;
            }
            set
            {
                _compareImages = value;
            }
        }
        public ICommand OpenImage
        {
            get
            {
                return _openImage;
            }
            set
            {
                _openImage = value;
            }
        }
        public ICommand OpenFolder
        {
            get
            {
                return _openFolder;
            }
            set
            {
                _openFolder = value;
            }
        }
        public ICommand RunFolderCompare
        {
            get
            {
                return _runFolderCompare;
            }
            set
            {
                _runFolderCompare = value;
            }
        }

        #endregion

        public MainVM()
        {
            ChangeImagePath1 = new DelegateCommand(OnChangeImagePath1);
            ChangeImagePath2 = new DelegateCommand(OnChangeImagePath2);
            CompareImages = new DelegateCommand(OnCompareImages);
            OpenImage = new DelegateCommand(OnOpenImage);
            OpenFolder = new DelegateCommand(OnOpenFolder);
            RunFolderCompare = new DelegateCommand(OnRunCompare);

            _image1 = GetBitmapImage();
            _image2 = GetBitmapImage();
            _folderSearchImage = GetBitmapImage();
        }

        private void OnChangeImagePath1(Object sender)
        {
            string filePath = GetFilePath();
            imagePath1 = filePath;
            Image1 = GetBitmapImage(filePath);
        }
        private void OnChangeImagePath2(Object sender)
        {
            string filePath = GetFilePath();
            imagePath2 = filePath;
            Image2 = GetBitmapImage(filePath);
        }
        private void OnOpenImage(Object sender)
        {
            string filePath = GetFilePath();
            folderSearchImagePath = filePath;
            FolderSearchImage = GetBitmapImage(filePath);
        }
        private void OnOpenFolder(Object sender)
        {
            folderSearchPath = GetFolderPath();
        }
        private void OnRunCompare(Object sender)
        {
            List<string> filesList = Directory.GetFiles(folderSearchPath, "*.*", SearchOption.AllDirectories).ToList();
            List<string> matchedFilesList = new List<string>();
            int threshold = Convert.ToInt32(Threshold);
            filesList.ForEach( s => {
                string extension = Path.GetExtension(s);
                if(extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                {
                    if(ImageHashing.ImageHashing.Similarity(s, folderSearchImagePath) >= threshold)
                    {
                        matchedFilesList.Add(s);
                    }
                }
            });
            MatchingImages = new ObservableCollection<string>(matchedFilesList);
        }
        private void OnCompareImages(Object sender)
        {
            if(!string.IsNullOrEmpty(imagePath1) && !string.IsNullOrEmpty(imagePath2))
                PercentageText = ImageHashing.ImageHashing.Similarity(imagePath1, imagePath2).ToString();
        }
        private void LoadImage(string FilePath)
        {
            MatchedImage = GetBitmapImage(FilePath);
        }
        private string GetFilePath()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
                return openFileDialog.FileName;

            return "";
        }
        private string GetFolderPath()
        {
            var folderBrowswerDialog = new FolderBrowserDialog();
            folderBrowswerDialog.ShowDialog();
            return folderBrowswerDialog.SelectedPath;
        }
        private BitmapImage GetBitmapImage(string filePath = "")
        {

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            
            if(!string.IsNullOrEmpty(filePath))
                bitmapImage.UriSource = new Uri(filePath, UriKind.Absolute);
            else
                bitmapImage.UriSource = new Uri(EMPTY_IMAGE_PATH, UriKind.Relative);

            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
