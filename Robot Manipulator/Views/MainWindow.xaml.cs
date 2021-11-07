using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using Robot_Manipulator.Models;

namespace MazeBilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();            
            
            //renderingTimer.Tick += RenderingTimer_Tick;
            //renderingTimer.Start();

            
        }

        const float ScaleRate = 0.1f;
        private void CanvasMain_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                Node.scaleCoefficient -= ScaleRate;
                MazeGeometrycal mazeforDraw = new MazeGeometrycal(canvasMain, inMaze);
                canvasMain.Children.Clear();

                for (int i = 0; i < mazeforDraw.elements.Count; i++)
                    canvasMain.Children.Add(mazeforDraw.elements[i]);
            }
            else
            {
                Node.scaleCoefficient += ScaleRate;
                MazeGeometrycal mazeforDraw = new MazeGeometrycal(canvasMain, inMaze);
                canvasMain.Children.Clear();

                for (int i = 0; i < mazeforDraw.elements.Count; i++)
                    canvasMain.Children.Add(mazeforDraw.elements[i]);
            }
        }

        private void MenuItemHelpValues_Click(object sender, RoutedEventArgs e)
        {
            string outputMessage = "В крайнем правом блоке вы можете видеть информацию о лабиринте\n" +
                "В поля ширина и высота вводится количество клеток по вертикали и горизонтали\n" +
                "В графическом представлении можно видеть сам лабиринт, зеленым выделен стартовая клетка, красным выделен финиш.\n" +
                "Если нажата кнопка решить лабиринт\n" +
                "Синим выделяется правильная траектория достижения финиша\n" +
                "Розовым выделяются тупиковые траектории";

            MessageBox.Show(outputMessage);
        }

        private void MenuItemHelpSelectedItemChanging_Click(object sender, RoutedEventArgs e)
        {
            string outputMessage = "Редакитрование элемента не допилил, не успевал по времени, надо переписывать архитектуру";

            MessageBox.Show(outputMessage);
        }
        /*
                private void MenuItemHelpAddDelElement_Click(object sender, RoutedEventArgs e)
                {
                    string outputMessage = "Новый элемент добавляется к самому последнему элементу. Тип его выбирается автоматически.\n" +
                        "Удалять можно лишь выбранный элемент, все идущие от него элементы при этом удалятся тоже\n" +
                        "Нулевой элемент удалить нельзя\n";

                    MessageBox.Show(outputMessage);
                }*/

        private void MenuItemHelpScaleCenter_Click(object sender, RoutedEventArgs e)
        {
            string outputMessage = "Программа автоматически центрирует лабиринт прижимая его к левому верхнему углу области вывода, данная опция не настраивается, можно настроить лишь на этапе компиляции.\n" +
                "При выходе измении размеров масштаб лабиринта не будет меняться, при ручном масштабировании колесиком мыши. Следует быть аккуратным, т.к. можно вытянуть элемент до бесконечности.\n";

            MessageBox.Show(outputMessage);
        }

        private void MenuItemHelpFiles_Click(object sender, RoutedEventArgs e)
        {
            string outputMessage = "Для сохранение/загрузки лабиринта нужно воспользоваться пунктом верхнего меню \"Файл\"\n" +
                "Предварительная версия программы допускает сохранение только нерешенного лабринта.";

            MessageBox.Show(outputMessage);
        }

        Maze inMaze = new Maze(10, 10);


        private void MenuMazeSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.Filter = "Text files (*.txt)|*.txt";
                sf.FilterIndex = 2;
                sf.RestoreDirectory = true;
                sf.ShowDialog();

                if (sf.FileName != "")
                {
                    string kBasePath = sf.FileName;
                    inMaze.WriteMatrix(kBasePath);
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Что-то пошло не так");
            }
        }
        private void MenuMazeLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                var sr = new StreamReader(openFileDialog.FileName);

                string buffer = sr.ReadToEnd();
                
                inMaze = new Maze(buffer);
                sr.Close();
            }
            DrawSolve();
            void DrawSolve()
            {
                MazeGeometrycal mazeforDraw = new MazeGeometrycal(canvasMain, inMaze);
                canvasMain.Children.Clear();

                for (int i = 0; i < mazeforDraw.elements.Count; i++)
                    canvasMain.Children.Add(mazeforDraw.elements[i]);


            }
        }

        private void solveBtn_Click(object sender, EventArgs e)
        {
            inMaze.SolveMaze();
            DrawSolve();
            void DrawSolve()
            {
                MazeGeometrycal mazeforDraw = new MazeGeometrycal(canvasMain, inMaze);
                canvasMain.Children.Clear();

                for (int i = 0; i < mazeforDraw.elements.Count; i++)
                    canvasMain.Children.Add(mazeforDraw.elements[i]);

                
            }
        }
        
        private void createBtn_Click(object sender, EventArgs e)
        {
            //bool checkDim = Int32.TryParse(txtWidth.Text, out int result);
            int wid = 0;
            int hgt = 0;

            //Добавим проверку на корректность введенных размеров
            try
            {
                wid = int.Parse(textBoxWidht.Text);
                hgt = int.Parse(textBoxHeight.Text);

                if (wid == 0 || hgt == 0)
                {
                    throw new FormatException();
                }

            }
            catch (System.FormatException)
            {
                string message = "Размерность должна быть числом, больше 0.";
                MessageBox.Show(message);
                textBoxWidht.Text = "10";
                textBoxHeight.Text = "10";
                return;
            }


            int oddW = 0;
            int oddH = 0;

            //Обрабатываем случай с нечетными размерами
            if (wid % 2 != 0 && wid != 0)
            {
                oddW = 1;
            }
            if (hgt % 2 != 0 && hgt != 0)
            {
                oddH = 1;
            }


            Maze maze = new Maze(wid, hgt);


            //обрабатываем прорисовку финиша при нечетных размерах
            maze.finish.X = maze.finish.X + oddW;
            maze.finish.Y = maze.finish.Y + oddH;
            maze.CreateMaze();
            inMaze = maze;
            DrawMaze();
            inMaze.WriteMatrix("1.txt");
            
            void DrawMaze()
            {
                MazeGeometrycal mazeforDraw = new MazeGeometrycal(canvasMain, inMaze);
                canvasMain.Children.Clear();

                for (int i = 0; i < mazeforDraw.elements.Count; i++)
                    canvasMain.Children.Add(mazeforDraw.elements[i]);
                
            }
        }
    }
}
