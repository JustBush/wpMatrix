using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Color = Windows.UI.Color;

namespace matrix
{
    partial class MainPage : PhoneApplicationPage
    {


        public void Event_Grid_Tap_LayoutRoot(object sender, GestureEventArgs e)
        {
            Start();
        }

        // Событие при нажатии на элемент Grid (на экран)


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
                var random = new Random();
                // Начало змейки по горизонтали случайным образом
                var ranX = random.Next(0, 10);

                // Начало змейки по вертикали случайным образом
                var ranY = random.Next(0, 20);

                // Длина змейки случайным образом
                var length = random.Next(3, 7);

                // Скорость смены символов в змейке случайным образом в мс
                var time = random.Next(30, 70);

                await Task.Delay(1);

                // Обработка змейки
                await RandomElementQ_Async(ranX, ranY, length, time);

                count++;
            }
        }


        // Определяю элемент, в котором нужно менять символы
        private async Task
            RandomElementQ_Async(int
                x,
                int y,
                int length,
                int timeOut)
        {
            // Словарь для хранения идентификаторов ячеек, которые вызывались на предыдущем этапе.
            var dicElem = new Dictionary<int, TextBlock>();

            // Счетчик, нужен для обработки случаев, когда не выполняется условие if ((y + i) < countHeight && (y + i) >= 0). Смотри на 4 строчки вниз.
            var count = 0;

            // Цикл формирует змейку заданной длины length
            int i;
            for (i = 0; i < length; i++)
            {
                // Нужно для обработки случаев, когда змейка растет за пределы области вниз 
                // Просто ничего не делаем
                var countHeight = (int) Math.Round(_screenHeight/50);

                // Проверяем, что б змейка отображалась только в координатах, которые существуют в нашей сетке
                if ((y + i) < countHeight)
                {
                    // Формируем имя элемента, в котором будут меняться символы
                    var elementName = "TB_" + x + "_" + (y + i);

                    // Получаем элемент по его имени
                    object wantedNode = LayoutRoot.FindName(elementName);
                    var element = (TextBlock) wantedNode;

                    // Отправляем элемент в словарь, из которого он будет извлекаться для эффекта "падения" и "затухания" змейки
                    dicElem[count] = (element);

                    // Определяем коеффициент для подсчета яркости. Первый элемент(который падает) -  всега самый яркий, последний - самый темный.
                    // Отнимаем 1, потому, что последний элемент в итоге получается больше 255 и становится ярким.
                    var rf = (int) Math.Round(255/(double) (i + 1)) - 1;

                    // Вызываем на прорисовку первый, самый яркий падающий элемент. Асинхронно.
                    await Change(element, timeOut, 255);

                    // Перебираем все элементы, составляющие змейку на данном этапе. С каждым циклом она увеличивается, пока не достигнет своей длины.
                    for (var k = 0; k <= i; k++)
                    {
                        // Если змейка начинается "выше" начальных координат (например, если y = -5)
                        if (dicElem.ContainsKey(k))
                        {
                            //Извлекаем элементы, которые должны следовать за самым ярким. Создаем эффект "затухания" цвета
                            var previousElement = dicElem[k];

                            // Вызываем извлеченные элементы
                            // (rf * (k + 1)) - 20 Высчитываем яркость так, что б разница между первым и последним была на всех змейках одинаковая
                            // и равномерно распределялась независимо от ее длины(количества элементов)
                            var dsvv = Change(previousElement, timeOut, (rf*(k + 1)) - 20);
                        }
                    }
                    count++;
                }
            }
        }
        // Метод изменения символов в заданном элеменете
        public async Task Change(TextBlock txt, int timeOut, int Opacity)
        {
            // Формируем нужный цвет с заданной яркостью
            var newColor = new SolidColorBrush(new Color()
            {
                A = (byte)(255) /*Opacity*/,
                R = (byte)(0) /*Red*/,
                G = (byte)(Opacity) /*Green*/,
                B = (byte)(0) /*Blue*/
            });

            // При каждом "падении" на 1 клеточку равномерно "затухает"
            txt.Foreground = newColor;

            // Количество смены символов в каждой ячейке          
            for (int i = 0; i < 5; i++)
            {
                // Каждый раз разный символ
                txt.Text = char.ConvertFromUtf32(random.Next(0x4E00, 0x4FFF));

                // Скорость смены символов в ячейке
                await Task.Delay(timeOut);
            }
        }

        // Метод изменения символов в заданном элеменете

    }
}