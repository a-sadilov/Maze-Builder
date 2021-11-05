using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Robot_Manipulator.Models
{
    class Node : Shape
    {
        public static float scaleCoefficient = 1;
        Point _beginPosition = new Point();
        public Point BeginPosition
        {
            get { return _beginPosition; }
            set { _beginPosition = value; }
        }
        RectangleGeometry _nodeGeometry = new RectangleGeometry();
        Point _scaledBeginPoint = new Point();
        System.Windows.Size _size = new System.Windows.Size();
        public Node(Cell element, Canvas someCanvas, Maze someMaze)
        {
            int cellWidht = ((int)someCanvas.Width) / (someMaze._width + 2);
            int cellHeight = ((int)someCanvas.Height) / (someMaze._height + 2);

            //Установим минимальный размер ячейки
            int cellSizeMin = 10;
            if (cellWidht < cellSizeMin || cellHeight < cellSizeMin)
            {
                cellWidht = cellHeight = cellSizeMin;
            }
            if (cellWidht > cellHeight)
            {
                cellWidht = cellHeight;
            }
            else
            {
                cellHeight = cellWidht;
            }
            _size.Width = cellWidht / scaleCoefficient;
            _size.Height = cellHeight / scaleCoefficient;

            Fill = element._isCell ? Brushes.White : Brushes.Black;
            if (someMaze._visited.Contains(element))
            {
                Fill = Brushes.OrangeRed;
            }
            if (someMaze._solve.Contains(element))
            {
                Fill = Brushes.Blue;
            }
            if (element.X == someMaze.start.X && element.Y == someMaze.start.Y)
            {
                Fill = Brushes.Green;
            }
            if (element.X == someMaze.finish.X && element.Y == someMaze.finish.Y)
            {
                Fill = Brushes.Red;
            }


            _beginPosition.X = element.X * cellWidht;
            _beginPosition.Y = element.Y * cellHeight;
            //nodesList.Add(this);
            Stroke = Brushes.Gray;
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                _scaledBeginPoint.X = BeginPosition.X / scaleCoefficient;
                _scaledBeginPoint.Y = BeginPosition.Y / scaleCoefficient;

                _nodeGeometry.Rect = new Rect(_scaledBeginPoint, _size);
                
                return _nodeGeometry;
            }
        }
    }
}
