﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Configuration;
using System.Xml.Linq;
using System.IO;
using ClassRoom.Views;

namespace ClassRoom.Views
{
    public partial class PictureView : Window
    {
        #region Global Varibales
        int count = 0;
        string DBPath = @"F:\My Folder\MovieDB\MovieDB.xml";
        string ImageFolderPath = @"C:\Users\Public\Pictures\Sample Pictures";
        List<Movies> oldList;
        List<Movies> slideShowList = new List<Movies>();
        string FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        List<string> files;
        //MainWindow mainWindow;
        #endregion

        #region Constructor
        public PictureView()
        {
            InitializeComponent();
            files = new List<string>(Directory.GetFiles(ImageFolderPath, "*.jpg"));
            LoadData();
            lbImages.DataContext = slideShowList;

            //if (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings["ImagePath"]))
            //{
            //    ImageFolderPath = ConfigurationSettings.AppSettings["ImagePath"];

            //    if (files != null && files.Count > 0)
            //    {
            //        LoadData();
            //        lbImages.DataContext = slideShowList;
            //    }
            //}
        } 
        #endregion        
        
        #region LoadData
        public void LoadData()
        {
            oldList = (from movie in files
                       let photo = new Photo(movie)
                       select new Movies
                       {
                           Name = photo.Name,
                           Genre = "喜爱",
                           Year = photo.DateTime.Year.ToString(),
                           Cast = "未知",
                           Language = "中文",
                           ImageUri = photo.FullPath,
                           StoreType = "图片"
                       }).Take(10).ToList();

            foreach (Movies item in oldList)
            {
                if (item.ImageUri.Contains("NOIMAGE"))
                {
                    item.ImageUri = "Assets/noimage.png";
                    slideShowList.Add(item);
                }
                else
                {
                    //item.ImageUri = ImageFolderPath + item.ImageUri;
                    slideShowList.Add(item);
                }
            }
            lbImages.DataContext = slideShowList;
            lbImages.SelectedIndex = -1;
        }
        #endregion

        #region Image-MouseLeftButtonDown-imgMain
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            imgMain.Source = img.Source;
        }

        private void lbImages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbImages.SelectedIndex != -1)
            {
                Movies movie = lbImages.SelectedItem as Movies;
                txtTitle.Text = movie.Name + " [" + movie.Year + "]";
                if (!string.IsNullOrEmpty(movie.ImageUri))
                {
                    //movie.ImageUri = "D:\\Chrysanthemum.jpg";
                    Uri uri = new Uri(movie.ImageUri, UriKind.Absolute);
                    BitmapImage bmpImage = new BitmapImage(uri);
                    //bmpImage.BeginInit();
                    //if (!string.IsNullOrEmpty(movie.ImageUri))
                    //{
                    //    if (movie.ImageUri.Contains("Assets"))
                    //    {
                    //        movie.ImageUri = string.Empty;
                    //        movie.ImageUri = "NOIMAGE";
                    //    }

                    //    if (movie.ImageUri.Contains("NOIMAGE"))
                    //    {
                    //        bmpImage.UriSource = new Uri("Assets/noimage.png", UriKind.RelativeOrAbsolute);
                    //    }
                    //    else
                    //    {
                    //        using (FileStream stream = new FileStream(movie.ImageUri, FileMode.Open))
                    //        {
                    //            bmpImage.StreamSource = stream;
                    //        }
                    //        //bmpImage.UriSource = new Uri(movie.ImageUri, UriKind.RelativeOrAbsolute);
                    //    }
                    //}
                    //bmpImage.EndInit();
                    imgMain.Source = bmpImage;
                }
            }
        }
        #endregion

        #region Button-Click-Close
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            lbImages.SelectedIndex = -1;
            this.Close();
        } 
        #endregion

        #region Button-Click-Previous
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if ((count * 10) > 0)
            {
                count--;
                MakeTensList();
            }
        } 
        #endregion

        #region Buton-Click-Next
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if ((count * 10) < files.Count)
            {
                count++;
                MakeTensList();
            }
        } 
        #endregion

        #region MakeTensList()
        private void MakeTensList()
        {
            List<Movies> nextList = new List<Movies>();

            var nextTenItems = (from movie in files
                                let photo = new Photo(movie)
                                select new Movies
                                {
                                    Name = photo.Name,
                                    Genre = "喜爱",
                                    Year = photo.DateTime.Year.ToString(),
                                    Cast = "未知",
                                    Language = "中文",
                                    ImageUri = photo.FullPath,
                                    StoreType = "图片"
                                }).Skip(count * 10).Take(10).ToList();

            foreach (Movies item in nextTenItems)
            {
                if (item.ImageUri.Contains("NOIMAGE"))
                {
                    item.ImageUri = "Assets/noimage.png";
                    nextList.Add(item);
                }
                else
                {
                    //item.ImageUri = ImageFolderPath + item.ImageUri;
                    nextList.Add(item);
                }
            }

            lbImages.DataContext = nextList;
            lbImages.SelectedIndex = -1;
        } 
        #endregion       
    }

    public class Movies
    {
        public string Name { get; set; }
        public string Year { get; set; }
        public string Genre { get; set; }
        public string Cast { get; set; }
        public string Language { get; set; }
        public string ImageUri { get; set; }
        public string StoreType { get; set; }
    }
}