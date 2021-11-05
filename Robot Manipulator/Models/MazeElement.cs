using System.Windows.Shapes;
using System.Windows;

namespace Robot_Manipulator.Models
{
    abstract class MazeElement : Shape
    {
        //ScaleCoefficient позволит масштабировать фигуры
        public static float scaleCoefficient = 1;

        public enum elementTypes
        {
            NULL_ELEMENT,
            SOLVE,
            CELL,
            WALL,
            START,
            FINISH,
            WRONGWAY,
        }
        abstract public Point BeginPosition
        {
            get; set;
        }

        //самотипизация
        protected elementTypes _elementType = elementTypes.NULL_ELEMENT;
        public elementTypes ElementType
        {
            get
            {
                return _elementType;
            }
        }
    }
}
