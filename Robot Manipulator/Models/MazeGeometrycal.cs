using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace Robot_Manipulator.Models
{
    class MazeGeometrycal : INotifyPropertyChanged
    {
        public ObservableCollection<Node> elements;

        public Maze mazeForGeometrical;

        public event PropertyChangedEventHandler PropertyChanged;

        private Node _selectedElement;

        public Node SelectedItem
        {
            get { return _selectedElement; }
            set
            {
                void UpdateLinksColor(Node value)
                {
                    for (int i = 0; i < elements.Count(); i++)
                    {
                        if (elements[i] != value)
                        {
                            //Все невыбранные приводим к голубому цвету
                            elements[i].Stroke = System.Windows.Media.Brushes.Blue;
                        }
                        else
                        {
                            //по идее функция будет вызываться уже для элементов массива, т.е. можно было бы напрямую у value цвет менять.
                            elements[i].Stroke = System.Windows.Media.Brushes.Black;
                        }
                    }
                }

                UpdateLinksColor(value);
                _selectedElement = value;

                OnPropertyChanged("SelectedItem");
            }
        }

        
/*
        public bool UpdateElementsAfterChanges()
        {
            bool isElementsUpdated = false;
            for (int i = 1; i < elements.Count(); i++)
            {
                if (elements[i].ElementType == ManipulatorElement.elementTypes.LINK)
                {
                    Link currentLink = (Link)elements[i];
                    Joint previousJoint = (Joint)elements[i - 1];

                    if (currentLink.BeginPosition != previousJoint.BeginPosition)
                    {
                        currentLink.BeginPosition = previousJoint.BeginPosition;
                        isElementsUpdated = true;
                    }
                }
                if (elements[i].ElementType == ManipulatorElement.elementTypes.JOINT)
                {
                    Joint currentJoint = (Joint)elements[i];
                    Link previousLink = (Link)elements[i - 1];

                    if (previousLink.EndPosition != currentJoint.BeginPosition)
                    {
                        currentJoint.BeginPosition = previousLink.EndPosition;
                        isElementsUpdated = true;
                    }
                }
            }
            return isElementsUpdated;

        }*/
        public MazeGeometrycal(Canvas someCanvas, Maze mazeinput)
        {
            elements = new ObservableCollection<Node>();

            mazeForGeometrical = mazeinput;

            foreach(Cell c in mazeinput._cells)
            {
                Node node = new Node(c, someCanvas, mazeinput);
                elements.Add(node);
            }

            OnPropertyChanged("MazeGeometrycal");
        }

        /*public void LoadManipulatorFromJson(ManipulatorSerialized serializedManipulator)
        {
            elements.Clear();

            foreach (var element in serializedManipulator.elements)
            {
                if (element.ElementType == ManipulatorElement.elementTypes.JOINT)
                {
                    Joint joint = new Joint()
                    {
                        BeginPosition = element.BeginPosition,
                        Weight = element.Weight
                    };

                    elements.Add(joint);
                }
                else if (element.ElementType == ManipulatorElement.elementTypes.LINK)
                {
                    Link link = new Link()
                    {
                        BeginPosition = element.BeginPosition,
                        EndPosition = element.EndPosition,
                        Weight = element.Weight
                    };

                    elements.Add(link);
                }
            }

            OnPropertyChanged("LoadManipulatorFromJson");
        }

       

       
        public void ChangeSelectedElementViaNewEndPoint(Point newPosition)
        {
            if (SelectedItem != null)
            {
                newPosition.X *= Node.scaleCoefficient;
                newPosition.Y *= Node.scaleCoefficient;
                switch (SelectedItem.ElementType)
                {
                    case ManipulatorElement.elementTypes.NULL_ELEMENT:
                        break;
                    case ManipulatorElement.elementTypes.LINK:
                        {
                            Node SelectedLink = (Node)SelectedItem;

                            SelectedLink.EndPosition = newPosition;
                            UpdateElementsAfterChanges();
                            OnPropertyChanged("ChangeLinkViaEndPoint");
                            break;
                        }
                    case ManipulatorElement.elementTypes.JOINT:
                        {
                            Joint SelectedJoint = (Joint)SelectedItem;

                            SelectedJoint.BeginPosition = newPosition;
                            UpdateElementsAfterChanges();
                            OnPropertyChanged("ChangeLinkViaEndPoint");
                            break;
                        }

                    case ManipulatorElement.elementTypes.INT_COORDINATES:
                        break;
                    default:
                        break;
                }



            }
        }

        public bool IsShapesOutOfCanvas(double canvasActualHeight, double cavasActualWidth)
        {
            foreach (var element in elements)
            {
                if (element.ElementType == ManipulatorElement.elementTypes.LINK)
                {
                    Link currentLink = (Link)element;
                    if (currentLink.BeginPosition.X > cavasActualWidth || currentLink.EndPosition.X > cavasActualWidth ||
                        currentLink.BeginPosition.X < 0 || currentLink.EndPosition.X < 0)
                        return true;
                    if (currentLink.BeginPosition.Y > canvasActualHeight || currentLink.EndPosition.Y > canvasActualHeight ||
                        currentLink.BeginPosition.Y < 0 || currentLink.EndPosition.Y < 0)
                        return true;
                }
                else if (element.ElementType == ManipulatorElement.elementTypes.JOINT)
                {
                    Joint currentJoint = (Joint)element;

                    if (currentJoint.BeginPosition.X > cavasActualWidth || currentJoint.BeginPosition.Y > canvasActualHeight)
                        return true;
                }
            }
            return false;
        }*/

        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
