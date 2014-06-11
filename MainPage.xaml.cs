using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace matrix
{
    public partial class MainPage
    {
        // Случайное число
        readonly Random _random = new Random();

        // Получаем расширение экрана
        readonly double _screenWidth = Application.Current.Host.Content.ActualWidth;
        readonly double _screenHeight = Application.Current.Host.Content.ActualHeight;

        // Конструктор
        public MainPage()
        {
            InitializeComponent();
            CreateElement();
        }
        // Создание сетки элементов, в которой будет сыпаться матрица
        private void CreateElement()
        {
            int i, j, countWidth, countHeight;

            // Количество строк и столбцов
            countWidth = (int)Math.Round(_screenWidth / 50);
            countHeight = (int)Math.Round(_screenHeight / 50);

            // Создаем сетку ячеек
            for (i = 0; i < countWidth; i++)
            {
                for (j = 0; j < countHeight; j++)
                {
                    // Создаем TextBlock
                    var element = new TextBlock {Name = "TB_" + i + "_" + j, Text = ""};

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

                    // Добавляем созданный элемент в Grid
                    LayoutRoot.Children.Add(element);
                }
            }
        }

        // Событие при нажатии на элемент Grid (на экран)
        private void Event_Grid_Tap_LayoutRoot(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Start();
        }

        // Метод запуска змейки
        private async void Start()
        {
            int count, iteration;

            // Количество змеек после нажатия на экран в очереди
            iteration = 1;

            count = 0;

            // Количество змеек после нажатия на экран в очереди
            while (count < iteration)
            {
                // Начало змейки по горизонтали случайным образом
                int ranX = _random.Next(0, 10);

                // Начало змейки по вертикали случайным образом
                int ranY = _random.Next(0, 20);

                // Длина змейки случайным образом
                int length = _random.Next(3, 7);

                // Скорость смены символов в змейке случайным образом в мс
                int time = _random.Next(30, 70);

                await Task.Delay(1);

                // Обработка змейки
                await RandomElementQ_Async(ranX, ranY, length, time);

                count++;
            }
        }

        // Определяю элемент, в котором нужно менять символы
        private async Task RandomElementQ_Async(int x, int y, int length, int timeOut)
        {
            // Словарь для хранения идентификаторов ячеек, которые вызывались на предыдущем этапе.
            var dicElem = new Dictionary<int, TextBlock>();

            // Счетчик, нужен для обработки случаев, когда не выполняется условие if ((y + i) < countHeight && (y + i) >= 0). Смотри на 4 строчки вниз.
            var count = 0;

            // Цикл формирует змейку заданной длины length
            for (var i = 0; i < length; i++)
            {
                // Нужно для обработки случаев, когда змейка растет за пределы области вниз 
                // Просто ничего не делаем
                var countHeight = (int)Math.Round(_screenHeight / 50);

                // Проверяем, что б змейка отображалась только в координатах, которые существуют в нашей сетке
                if ((y + i) >= countHeight) continue;
                // Формируем имя элемента, в котором будут меняться символы
                var elementName = "TB_" + x + "_" + (y + i);

                // Получаем элемент по его имени
                var wantedNode = LayoutRoot.FindName(elementName);
                var element = (TextBlock)wantedNode;

                // Отправляем элемент в словарь, из которого он будет извлекаться для эффекта "падения" и "затухания" змейки
                dicElem[count] = (element);

                // Определяем коеффициент для подсчета яркости. Первый элемент(который падает) -  всега самый яркий, последний - самый темный.
                // Отнимаем 1, потому, что последний элемент в итоге получается больше 255 и становится ярким.
                var rf = (int)Math.Round(255 / (double)(i + 1)) - 1;

                // Вызываем на прорисовку первый, самый яркий падающий элемент. Асинхронно.
                await Change(element, timeOut, 255);

                // Перебираем все элементы, составляющие змейку на данном этапе. С каждым циклом она увеличивается, пока не достигнет своей длины.
                for (var k = 0; k <= i; k++)
                {
                    // Если змейка начинается "выше" начальных координат (например, если y = -5)
                    if (!dicElem.ContainsKey(k)) continue;
                    //Извлекаем элементы, которые должны следовать за самым ярким. Создаем эффект "затухания" цвета
                    var previousElement = dicElem[k];

                    // Вызываем извлеченные элементы
                    // (rf * (k + 1)) - 20 Высчитываем яркость так, что б разница между первым и последним была на всех змейках одинаковая
                    // и равномерно распределялась независимо от ее длины(количества элементов)
                    
                    await 
                        Change(previousElement, timeOut, (rf * (k + 1)) - 20);
                }
                count++;
            }
        }

        // Метод изменения символов в заданном элеменете
        private async Task Change(TextBlock element, int timeOut, int opacity)
        {
            // Формируем нужный цвет с заданной яркостью
            var newColor = new SolidColorBrush(new Color
            {
                A = 255 /*opacity*/,
                R = 0 /*Red*/,
                G = (byte)(opacity) /*Green*/,
                B = 0 /*Blue*/
            });

            // При каждом "падении" на 1 клеточку равномерно "затухает"
            element.Foreground = newColor;

            // Количество смены символов в каждой ячейке          
            for (var i = 0; i < 5; i++)
            {
                // Каждый раз разный символ
                element.Text = char.ConvertFromUtf32(_random.Next(0x4E00, 0x4FFF));

                // Скорость смены символов в ячейке
                await Task.Delay(timeOut);
            }
        }

        // Пример кода для построения локализованной панели ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Установка в качестве ApplicationBar страницы нового экземпляра ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Создание новой кнопки и установка текстового значения равным локализованной строке из AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Создание нового пункта меню с локализованной строкой из AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}