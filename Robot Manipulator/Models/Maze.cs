using System;
using System.Collections.Generic;
using System.Text;

namespace Robot_Manipulator.Models
{
    class Maze
    {
        public Cell[,] _cells;
        public int _width;
        public int _height;
        public Stack<Cell> _path = new Stack<Cell>();
        public List<Cell> _solve = new List<Cell>();
        public List<Cell> _visited = new List<Cell>();
        public List<Cell> _neighbours = new List<Cell>();
        public Random rnd = new Random();
        public Cell start;
        public Cell finish;

        public Maze(int width, int height)
        {
            start = new Cell(1, 1, true, true);
            finish = new Cell(width - 3, height - 3, true, true);


            _width = width;
            _height = height;
            _cells = new Cell[width, height];
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                    if ((i % 2 != 0 && j % 2 != 0) && (i < _width - 1 && j < _height - 1)) //если ячейка нечетная по х и по у и не выходит за пределы лабиринта
                    {
                        _cells[i, j] = new Cell(i, j); //то это клетка (по умолчанию)
                    }
                    else
                    {
                        _cells[i, j] = new Cell(i, j, false, false);
                    }
            _path.Push(start);
            _cells[start.X, start.Y] = start;
        }

        public void SolveMaze()
        {
            bool flag = false; //флаг достижения финиша
            foreach (Cell c in _cells)
            {
                if (_cells[c.X, c.Y]._isCell == true)
                {
                    _cells[c.X, c.Y]._isVisited = false;
                }
            }

            _path.Clear();
            _path.Push(start);

            while (_path.Count != 0) //пока в стеке есть клетки ищем соседей и строим путь
            {
                if (_path.Peek().X == finish.X && _path.Peek().Y == finish.Y)
                {
                    flag = true;
                }

                if (!flag)
                {
                    _neighbours.Clear();
                    GetNeighboursSolve(_path.Peek());
                    if (_neighbours.Count != 0)
                    {
                        Cell nextCell = ChooseNeighbour(_neighbours);
                        nextCell._isVisited = true; //делаем текущую клетку посещенной
                        _cells[nextCell.X, nextCell.Y]._isVisited = true; //и в общем массиве
                        _path.Push(nextCell); //затем добавляем её в стек
                        _visited.Add(_path.Peek());
                    }
                    else
                    {
                        _path.Pop();
                    }
                }
                else
                {
                    _solve.Add(_path.Peek());
                    _path.Pop();
                }
            }
        }
        public Maze(string buffer)
        {
            string[] mazeRows = buffer.Split("\n");
            int widht = mazeRows.Length;
            int height = mazeRows[0].Length;

            _cells = new Cell[widht, height];
            for (int i = 0; i < mazeRows.Length; i++)
            {
                for (int j = 0; i < mazeRows[i].Length; j++)
                {
                    foreach (char c in mazeRows[i])
                    {
                        if (c == 'S')
                        {
                            _cells[j, i].X = start.X = i;
                            _cells[j, i].Y = start.Y = j;
                            _cells[j, i]._isCell = true;
                            _cells[j, i]._isVisited = true;
                            continue;
                        }
                        if (c == 'F')
                        {
                            _cells[j, i].X = finish.X = i;
                            _cells[j, i].X = finish.Y = j;
                            _cells[j, i]._isCell = true;
                            _cells[j, i]._isVisited = true;
                            continue;
                        }
                        if (c == ' ')
                        {
                            _cells[j, i].X = i;
                            _cells[j, i].X = j;
                            _cells[j, i]._isCell = true;
                            _cells[j, i]._isVisited = true;
                            continue;
                        }
                        if (c == '#')
                        {
                            _cells[i, j].X = i;
                            _cells[i, j].X = j;
                            _cells[i, j]._isCell = false;
                            _cells[j, i]._isVisited = true;
                            continue;
                        }
                    }
                }
            }
        }

        public void CreateMaze()
        {
            _cells[start.X, start.Y] = start;
            while (_path.Count != 0) //пока в стеке есть клетки ищем соседей и строим путь
            {
                _neighbours.Clear();
                GetNeighbours(_path.Peek());
                if (_neighbours.Count != 0)
                {
                    Cell nextCell = ChooseNeighbour(_neighbours);
                    RemoveWall(_path.Peek(), nextCell);
                    nextCell._isVisited = true; //делаем текущую клетку посещенной
                    _cells[nextCell.X, nextCell.Y]._isVisited = true; //и в общем массиве
                    _path.Push(nextCell); //затем добавляем её в стек

                }
                else
                {
                    _path.Pop();
                }

            }
        }

        public void DrawGrid()
        {
            Console.CursorVisible = false;
            for (var i = 0; i < _cells.GetUpperBound(0); i++)
                for (var j = 0; j < _cells.GetUpperBound(1); j++)
                {
                    Console.SetCursorPosition(i, j);
                    if (_cells[i, j]._isCell)
                    {

                        Console.Write(" ");
                    }
                    else
                    {

                        Console.Write("#");
                    }
                }
            Console.SetCursorPosition(start.X, start.Y);
            Console.Write("i");
            Console.SetCursorPosition(finish.X, finish.Y);
            Console.Write("o");
        }

        public void WriteMatrix(string filename) 
        {
            using (System.IO.StreamWriter record = new System.IO.StreamWriter(filename)) //"1.txt"
            {
                for (var i = 0; i < _cells.GetUpperBound(0); i++)
                {
                    for (var j = 0; j < _cells.GetUpperBound(0); j++)
                    {
                        if (_cells[j, i]._isCell)
                        {
                            if (_cells[j, i].X == start.X && _cells[j, i].Y == start.Y)
                            {
                                record.Write("S");
                                continue;
                            }
                            if (_cells[j, i].X == finish.X && _cells[j, i].Y == finish.Y)
                            {
                                record.Write("F");
                            }
                            else
                            {
                                record.Write(" ");
                            }
                        }
                        else
                        {
                            record.Write("#");
                        }
                        
                    }
                    if (i < _cells.GetUpperBound(0) - 1)
                    {
                        record.Write("\n");
                    }
                        
                }
            }

            
        }

        public Maze ReadMatrix(string filename)
        {
            using (System.IO.StreamReader record = new System.IO.StreamReader(filename)) //"1.txt"
            {
                string buffer = record.ReadToEnd();
                string[] mazeRows = buffer.Split("\n");
                int width = mazeRows.Length;
                int heigth = mazeRows[0].Length;
                Maze createdMaze = new Maze(width, heigth);
                for (int i = 0; i < mazeRows.Length ; i++)
                {
                    for (int j = 0; i < mazeRows[i].Length; j++)
                    {
                        foreach (char c in mazeRows[i])
                        {
                            if (c == 'S')
                            {
                                createdMaze._cells[j, i].X = createdMaze.start.X = i;
                                createdMaze._cells[j, i].Y = createdMaze.start.Y = j;
                                createdMaze._cells[j, i]._isCell = true;
                                continue;
                            }
                            if (c == 'F')
                            {
                                createdMaze._cells[j, i].X = createdMaze.finish.X = i;
                                createdMaze._cells[j, i].X = createdMaze.finish.Y = j;
                                createdMaze._cells[j, i]._isCell = true;
                                continue;
                            }
                            if (c == ' ')
                            {
                                createdMaze._cells[j, i].X = i;
                                createdMaze._cells[i, j].X = j;
                                createdMaze._cells[i, j]._isCell = true;
                                continue;
                            }
                            if (c == '#')
                            {
                                createdMaze._cells[i, j].X = i;
                                createdMaze._cells[i, j].X = j;
                                createdMaze._cells[i, j]._isCell = false;
                                continue;
                            }
                        }
                    }
                }
                return createdMaze;
            }
        }

        private void GetNeighbours(Cell localcell) // Получаем соседа текущей клетки
        {

            int x = localcell.X;
            int y = localcell.Y;
            const int distance = 2;
            Cell[] possibleNeighbours = new[] // Список всех возможных соседeй
            {
                new Cell(x, y - distance), // Up
                new Cell(x + distance, y), // Right
                new Cell(x, y + distance), // Down
                new Cell(x - distance, y) // Left
            };
            for (int i = 0; i < 4; i++) // Проверяем все 4 направления
            {
                Cell curNeighbour = possibleNeighbours[i];
                if (curNeighbour.X > 0 && curNeighbour.X < _width && curNeighbour.Y > 0 && curNeighbour.Y < _height)
                {// Если сосед не выходит за стенки лабиринта
                    if (_cells[curNeighbour.X, curNeighbour.Y]._isCell && !_cells[curNeighbour.X, curNeighbour.Y]._isVisited)
                    { // А также является клеткой и непосещен
                        _neighbours.Add(curNeighbour);
                    }// добавляем соседа в Лист соседей
                }
            }

        }

        private void GetNeighboursSolve(Cell localcell) // Получаем соседа текущей клетки
        {

            int x = localcell.X;
            int y = localcell.Y;
            const int distance = 1;
            Cell[] possibleNeighbours = new[] // Список всех возможных соседeй
            {
                new Cell(x, y - distance), // Up
                new Cell(x + distance, y), // Right
                new Cell(x, y + distance), // Down
                new Cell(x - distance, y) // Left
            };
            for (int i = 0; i < 4; i++) // Проверяем все 4 направления
            {
                Cell curNeighbour = possibleNeighbours[i];
                if (curNeighbour.X > 0 && curNeighbour.X < _width && curNeighbour.Y > 0 && curNeighbour.Y < _height)
                {// Если сосед не выходит за стенки лабиринта
                    if (_cells[curNeighbour.X, curNeighbour.Y]._isCell && !_cells[curNeighbour.X, curNeighbour.Y]._isVisited)
                    { // А также является клеткой и непосещен
                        _neighbours.Add(curNeighbour);
                    }// добавляем соседа в Лист соседей
                }
            }

        }

        private Cell ChooseNeighbour(List<Cell> neighbours) //выбор случайного соседа
        {

            int r = rnd.Next(neighbours.Count);
            return neighbours[r];

        }

        private void RemoveWall(Cell first, Cell second)
        {
            int xDiff = second.X - first.X;
            int yDiff = second.Y - first.Y;
            int addX = (xDiff != 0) ? xDiff / Math.Abs(xDiff) : 0; // Узнаем направление удаления стены
            int addY = (yDiff != 0) ? yDiff / Math.Abs(yDiff) : 0;

            // Координаты удаленной стены
            _cells[first.X + addX, first.Y + addY]._isCell = true; //обращаем стену в клетку
            _cells[first.X + addX, first.Y + addY]._isVisited = true; //и делаем ее посещенной
            second._isVisited = true; //делаем клетку посещенной
            _cells[second.X, second.Y] = second;

        }
    }
}
