using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static DataBoundApp1.ViewModels.Colors;

namespace DataBoundApp1.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private const int _articleTitleLenght = 70;

        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        private string _articleTitle;
        public string ArticleTitle
        {
            get
            {
                if (_articleTitle.Length > _articleTitleLenght)
                {
                    _articleTitle = _articleTitle.Substring(0, _articleTitleLenght);
                    _articleTitle = _articleTitle + "...";
                }

                return _articleTitle;
            }
            set
            {
                if (value != _articleTitle)
                {
                    _articleTitle = value;
                    NotifyPropertyChanged("ArticleTitle");
                }
            }
        }

        private DateTime _date;
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (value != _date)
                {
                    _date = value;
                    NotifyPropertyChanged("ArticleDate");
                }
            }
        }


        private string _url;
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                if (value != _url)
                {
                    _url = value;
                    NotifyPropertyChanged("WebsiteUrl");
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}