using System.Windows.Media; // Для работы с цветами
using System.Threading.Tasks; // Для асинхронных вызовов
using System.Diagnostics; // Для отладки. Debug.WriteLine( SomethingYouNeedToSee);
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using matrix.Resources;

namespace matrix
{
    public partial class MainPage : PhoneApplicationPage
    {
        readonly double _screenWidth = Application.Current.Host.Content.ActualWidth;
        readonly double _screenHeight = Application.Current.Host.Content.ActualHeight;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            CreateElement();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        private void CreateElement()
        {
            var countWidth = (int) Math.Round(this._screenWidth/50);
            var countHeight = (int) Math.Round(this._screenHeight/50);
            // Создаем сетку ячеек
            int i;
            for (i = 0; i < countWidth; i++)
            {
                int j;
                for (j = 0; j < countHeight; j++)
                {
                    // Создаем TextBlock
                    var element = new TextBlock {Name = "TB_" + i + "_" + j, Text = ""};
                    // Создаем Border
                    var elementBorder = new Border();
                    // Задаем имя элемента TextBlock

                    // Инициализируем начальный символ
                    //element.Text = char.ConvertFromUtf32(random.Next(0x4E00, 0x4FFF)); // Случайный символ из заданного диапазона

                    // Задаем смещение каждого нового элемента TextBlock
                    var wx = i * 50;
                    var wy = j * 50;
                    element.Margin = new Thickness(wx, wy, 0, 0);

                    // Задаем цвет символа
                    element.Foreground = new SolidColorBrush(Colors.Green);

                    // Задаем размер шрифта
                    element.FontSize = 36;

                    // Задаем смещение каждого нового элемента Border
                    elementBorder.Margin = new Thickness(wx, wy, 0, 0);

                    // Задаем толщину линии обводки
                    elementBorder.BorderThickness = new Thickness(1);

                    // Задаем цвет обводки
                    elementBorder.BorderBrush = new SolidColorBrush(Colors.Green);

                    // Добваляем TextBlock в Border
                    elementBorder.Child = element;     

                    // Добавляем созданный элемент в Grid
                    LayoutRoot.Children.Add(element);
                    LayoutRoot.Children.Add(elementBorder);
                }
            }
        }
        // Событие при нажатии на элемент Grid (на экран)


    }

    
}